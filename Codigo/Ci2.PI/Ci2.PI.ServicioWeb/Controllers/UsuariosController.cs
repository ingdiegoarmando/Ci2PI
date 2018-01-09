using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ci2.PI.ServicioWeb.Models;
using Ci2.PI.ServicioWeb.Models.Shared;
using Ci2.PI.Persistencia.Modelo;
using Ci2.PI.ServicioWeb.Excepciones;
using Ci2.PI.Aplicacion.Excepciones;
using Ci2.PI.ServicioWeb.Entidades;

namespace Ci2.PI.ServicioWeb.Controllers
{
    [RoutePrefix("Usuarios")]
    [Authorize]
    public class UsuariosController : Ci2PIMVCController
    {
        // GET: Usuarios
        [Route("Index")]
        public ActionResult Index(UsuarioActualMVC usuarioActual)
        {
            try
            {
                var usuariosBD = UnidadDeTrabajo.UsuarioRepositorio.Listar();

                var inicioViewModel = new UsuarioInicioViewModel();
                inicioViewModel.Usuarios = Convertidor.Obtener(usuariosBD);
                inicioViewModel.UsuarioActualId = usuarioActual.UsuarioId;
                return View(inicioViewModel);
            }
            catch (Exception e)
            {
                //GenerarNotificacionDeError("No fue posible cargar la pagina si el problem persiste por favor comuniquese con el administrador");

                return RedireccionLocal();
            }
        }

        // GET: Usuario/Registrar
        [Route("Registrar")]
        public ActionResult Registrar()
        {
            return View(new UsuarioRegistrarViewModel());
        }


        // POST: Usuario/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Registrar")]
        public ActionResult Registrar(UsuarioRegistrarViewModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TabUsuario usuarioBD = Convertidor.Obtener(usuario);
                    UnidadDeTrabajo.UsuarioRepositorio.AgregarOActualizar(usuarioBD);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(usuario);
                }


            }
            catch (ContraseñaInvalidaException ex)
            {
                ModelState.AddModelError("Contraseña", "La contraseña no es lo suficientemente segura. Por favor ingrese una contraseña de mayor complejidad");
                return View(usuario);
            }
            catch (NombreDeUsuarioEnUsoException ex)
            {
                ModelState.AddModelError("NombreUsuario", "El usuario [" + ex.NombreDeUsuarioEnUso + "] ya existe en el sistema. Por favor ingrese un nombre de usuario diferente.");

                return View(usuario);
            }
            catch (CorreoElectronicoInvalidoException ex)
            {
                ModelState.AddModelError("CorreoElectronico", "El correo electronico no tiene un formato valido.");

                return View(usuario);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Usuario/Editar/5
        public ActionResult Editar(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToAction("index");
            }

            try
            {
                var usuarioBD = UnidadDeTrabajo.UsuarioRepositorio.ConsultarPorId(Id);

                if (usuarioBD == null)
                {
                    return RedirectToAction("Inicio");
                }

                UsuarioEditarViewModel usuario = Convertidor.ObtenerUEVM(usuarioBD);

                return View(usuario);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Usuario/Editar/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UsuarioEditarViewModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TabUsuario usuarioDB = Convertidor.Obtener(usuario);
                    UnidadDeTrabajo.UsuarioRepositorio.ActualizarSinModifcarContraseñaYSecurityStamp(usuarioDB);                    

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(usuario);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Usuarios/Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(string id)
        {
            try
            {
                UnidadDeTrabajo.UsuarioRepositorio.Eliminar(id);                

                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}