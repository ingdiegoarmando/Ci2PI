using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Ci2.PI.ServicioWeb.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Ci2.PI.ServicioWeb.Entidades;
using Ci2.PI.ServicioWeb.Excepciones;
using Ci2.PI.ServicioWeb.Models.Shared;
using Ci2.PI.Persistencia.Modelo;

namespace Ci2.PI.ServicioWeb.Controllers
{
    [RoutePrefix("CuentaDeUsuario")]
    [Authorize]
    public class CuentaDeUsuarioController : Ci2PIMVCController
    {


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

        }

        // GET: CuentaDeUsuario/IniciarSesion
        [AllowAnonymous]
        public ActionResult IniciarSesion(string urlARetornar)
        {
            if (Request.IsAuthenticated)
            {
                return RedireccionLocal();
            }

            var vm = new CuentaDeUsuarioIniciarSesionViewModel()
            {
                UrlARetornar = urlARetornar
            };

            return View(vm);
        }



        // Post: CuentaDeUsuario/IniciarSesion
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult IniciarSesion(CuentaDeUsuarioIniciarSesionViewModel modelo, string urlARetornar)
        {
            if (ModelState.IsValid)
            {              
                var resultado = SignInManager.PasswordSignInAsync(modelo.NombreDeUsuario, modelo.Contrasena, modelo.RecordarMe, true).Result;
                switch (resultado)
                {
                    case SignInStatus.Success:
                        return RedireccionLocal(urlARetornar);
                    case SignInStatus.LockedOut:
                        ModelState.AddModelError("", "El usuario se encuentra bloqueado por favor contacte al administrador");
                        return View(modelo);
                    case SignInStatus.RequiresVerification:
                        throw new NotSupportedException(resultado.ToString());
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Inicio de sesión fallido. Si el problema persiste por favor comuniquese con el administrador.");
                        return View(modelo);
                }
            }
            else
            {
                return View(modelo);
            }
        }

        // Get: CuentaDeUsuario/GestionarPerfil
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioActual"> Este campo se manejado por el Binder UsuarioActualModelBinder</param>
        /// <returns></returns>
        public ActionResult GestionarPerfil(UsuarioActualMVC usuarioActual)
        {
            if (usuarioActual.ExisteUnUsuarioActual)
            {
                var modelo = new CuentaDeUsuarioGestionarPerfilViewModel();
                modelo.NombreDeUsuario = usuarioActual.NombreUsuario;
                return View(modelo);
            }
            else
            {
                return RedireccionLocal();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasena(UsuarioActualMVC usuarioActual, [Bind(Prefix = "CambioDeContrasenaVM")] CuentaDeUsuarioCambiarContraseñaViewModel modelo)
        {
            var resultado = new ResultadoLLamadoAjax();
            if (ModelState.IsValid)
            {
                try
                {
                    IPasswordHasher passwordHasher = null;
                    IdentityResult validador = null;

                    if (!Convertidor.ContraseñaValida(modelo.ContrasenaNueva, out passwordHasher, out validador))
                    {
                        throw new ContraseñaInvalidaException(string.Join(",", validador.Errors));
                    }

                    var passwordHash = passwordHasher.HashPassword(modelo.ContrasenaNueva);

                    UnidadDeTrabajo.UsuarioRepositorio.CambiarPasswordHashPorIdUsuario(usuarioActual.UsuarioId, passwordHash);

                    resultado.LLamadoExitoso = true;
                }
                catch (ContraseñaInvalidaException ex)
                {
                    //ModelState.AddModelError("Contraseña", "La contraseña no es lo suficientemente segura. Por favor ingrese una contraseña de mayor complejidad");

                    var mensajeDeError = new CamposInvalidos() { NombreDelCampo = "CambioDeContrasenaVM.ContrasenaNueva", MensajeDeError = "La contraseña no es lo suficientemente segura. Por favor ingrese una contraseña de mayor complejidad" };
                    resultado.CamposInvalidos.Add(mensajeDeError);
                }
                catch (Exception ex)
                {
                    resultado.MensajeDeErrorGeneral = "Ocurrio Un error";
                }

                return Json(resultado);
            }
            else
            {

                resultado.CamposInvalidos = ErroresDelModeloComoCamposInvalidos;

                return Json(resultado);
            }


        }        

        // Get: CuentaDeUsuario/CerrarSesion
        public ActionResult CerrarSesion()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "Home");
        }

    }
}