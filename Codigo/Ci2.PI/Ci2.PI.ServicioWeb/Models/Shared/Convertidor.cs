using Ci2.PI.Persistencia.Modelo;
using Ci2.PI.ServicioWeb.Excepciones;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Models.Shared
{
    public sealed class Convertidor
    {
        private Convertidor()
        {

        }

        public static UsuarioPEVM Obtener(TabUsuario usuarios)
        {
            return new UsuarioPEVM()
            {
                UsuarioId = usuarios.Ci2UsuarioId,
                AutenticacionDosEtapasActivado = usuarios.Ci2AutenticacionDosEtapasActivado,
                BloqueoActivado = usuarios.Ci2BloqueoActivado,
                CorreoElectronico = usuarios.Ci2CorreoElectronico,
                CorreoElectronicoConfirmado = usuarios.Ci2CorreoElectronicoConfirmado,
                FechaBloqueoUtc = usuarios.Ci2FechaBloqueoUtc,
                NombreUsuario = usuarios.Ci2NombreUsuario,
                NumeroAccesosFallidos = usuarios.Ci2NumeroAccesosFallidos,
                NumeroTelefonico = usuarios.Ci2NumeroTelefonico,
                NumeroTelefonicoConfirmado = usuarios.Ci2NumeroTelefonicoConfirmado,
                PasswordHash = usuarios.Ci2PasswordHash,
                SecurityStamp = usuarios.Ci2SecurityStamp,
            };
        }

        public static IEnumerable<UsuarioPEVM> Obtener(IEnumerable<TabUsuario> usuarios)
        {
            var listado = new List<UsuarioPEVM>();
            if (usuarios != null && usuarios.Count() > 0)
            {
                foreach (var item in usuarios)
                {
                    listado.Add(Obtener(item));
                }
            }
            return listado;
        }

        public static bool ContraseñaValida(string contraseña, out IPasswordHasher passwordHasherOut, out IdentityResult validadorOut)
        {
            IdentityResult validador = null;
            IPasswordHasher passwordHasher = null;

            if (HttpContext.Current != null && HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>() != null)
            {
                var man = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if (man.PasswordValidator != null)
                {
                    validador = man.PasswordValidator.ValidateAsync(contraseña).Result;
                }

                if (man.PasswordHasher != null)
                {
                    passwordHasher = man.PasswordHasher;
                }

            }

            if (validador == null)
            {
                validador = ApplicationUserManager.PasswordValidatorPorDefecto.ValidateAsync(contraseña).Result;

            }
            
            if (passwordHasher == null)
            {
                passwordHasher = new PasswordHasher();
            }

            passwordHasherOut = passwordHasher;
            validadorOut = validador;

            return validador.Succeeded;
        }

        public static TabUsuario Obtener(UsuarioRegistrarViewModel usuario)
        {
            IPasswordHasher passwordHasher = null;
            IdentityResult validador = null;

            if (!ContraseñaValida(usuario.Contraseña,out passwordHasher, out validador))
            {
                throw new ContraseñaInvalidaException(string.Join(",", validador.Errors));
            }

            if (passwordHasher == null)
            {
                passwordHasher = new PasswordHasher();
            }

            var passwordHash = passwordHasher.HashPassword(usuario.Contraseña);
            var hashCode = Guid.NewGuid().ToString();

            return new TabUsuario()
            {
                Ci2AutenticacionDosEtapasActivado = false,
                Ci2BloqueoActivado = false,
                Ci2CorreoElectronico = usuario.CorreoElectronico,
                Ci2CorreoElectronicoConfirmado = false,
                Ci2FechaBloqueoUtc = null,
                Ci2NombreUsuario = usuario.NombreUsuario,
                Ci2NumeroAccesosFallidos = 0,
                Ci2NumeroTelefonico = usuario.NumeroTelefonico,
                Ci2NumeroTelefonicoConfirmado = false,
                Ci2PasswordHash = passwordHash,
                Ci2SecurityStamp = hashCode,

            };
        }

        public static TabUsuario Obtener(UsuarioPEVM usuario)
        {
            return new TabUsuario()
            {
                Ci2UsuarioId = usuario.UsuarioId,
                Ci2AutenticacionDosEtapasActivado = usuario.AutenticacionDosEtapasActivado,
                Ci2BloqueoActivado = usuario.BloqueoActivado,
                Ci2CorreoElectronico = usuario.CorreoElectronico,
                Ci2CorreoElectronicoConfirmado = usuario.CorreoElectronicoConfirmado,
                Ci2FechaBloqueoUtc = usuario.FechaBloqueoUtc,
                Ci2NombreUsuario = usuario.NombreUsuario,
                Ci2NumeroAccesosFallidos = usuario.NumeroAccesosFallidos,
                Ci2NumeroTelefonico = usuario.NumeroTelefonico,
                Ci2NumeroTelefonicoConfirmado = usuario.NumeroTelefonicoConfirmado,
                Ci2PasswordHash = usuario.PasswordHash,
                Ci2SecurityStamp = usuario.SecurityStamp,

            };
        }

        public static TabUsuario Obtener(UsuarioEditarViewModel usuario)
        {                    
            return new TabUsuario()
            {
                Ci2UsuarioId = usuario.UsuarioId,
                Ci2AutenticacionDosEtapasActivado = usuario.AutenticacionDosEtapasActivado,
                Ci2BloqueoActivado = usuario.BloqueoActivado,
                Ci2CorreoElectronico = usuario.CorreoElectronico,
                Ci2CorreoElectronicoConfirmado = usuario.CorreoElectronicoConfirmado,
                Ci2FechaBloqueoUtc = usuario.FechaBloqueoUtc,
                Ci2NombreUsuario = usuario.NombreUsuario,
                Ci2NumeroAccesosFallidos = usuario.NumeroAccesosFallidos,
                Ci2NumeroTelefonico = usuario.NumeroTelefonico,
                Ci2NumeroTelefonicoConfirmado = usuario.NumeroTelefonicoConfirmado,                
            };
        }

        public static UsuarioEditarViewModel ObtenerUEVM(TabUsuario usuarios)
        {
            return new UsuarioEditarViewModel()
            {
                UsuarioId = usuarios.Ci2UsuarioId,
                AutenticacionDosEtapasActivado = usuarios.Ci2AutenticacionDosEtapasActivado,
                BloqueoActivado = usuarios.Ci2BloqueoActivado,
                CorreoElectronico = usuarios.Ci2CorreoElectronico,
                CorreoElectronicoConfirmado = usuarios.Ci2CorreoElectronicoConfirmado,
                FechaBloqueoUtc = usuarios.Ci2FechaBloqueoUtc,
                NombreUsuario = usuarios.Ci2NombreUsuario,
                NumeroAccesosFallidos = usuarios.Ci2NumeroAccesosFallidos,
                NumeroTelefonico = usuarios.Ci2NumeroTelefonico,
                NumeroTelefonicoConfirmado = usuarios.Ci2NumeroTelefonicoConfirmado,                
            };
        }
    }
}