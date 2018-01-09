using Ci2.PI.ServicioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Entidades
{
    public class UsuarioActualMVC
    {
        public bool ExisteUnUsuarioActual
        {
            get
            {
                return !string.IsNullOrEmpty(UsuarioId) && InformacionDeUsuarioComoApplicationUser != null;
            }
        }

        public string UsuarioId { get; set; }

        public string NombreUsuario
        {
            get
            {
                return InformacionDeUsuarioComoApplicationUser.UserName;
            }
        }

        public ApplicationUser InformacionDeUsuarioComoApplicationUser { get; set; }
    }
}