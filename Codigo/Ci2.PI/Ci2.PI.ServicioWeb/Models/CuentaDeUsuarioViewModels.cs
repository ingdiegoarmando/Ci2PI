using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Models
{
    public class CuentaDeUsuarioIniciarSesionViewModel
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string NombreDeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Display(Name = "Recordar inicio de sesión")]
        public bool RecordarMe { get; set; }

        public string UrlARetornar { get; set; }
    }

    public class CuentaDeUsuarioCambiarContraseñaViewModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string ContrasenaAnterior { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string ContrasenaNueva { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        [Compare("ContrasenaNueva")]
        public string ContrasenaNuevaConfirmacion { get; set; }
    }

    public class ResultadoLLamadoAjax
    {
        public bool LLamadoExitoso { get; set; }

        public Dictionary<String, String> ValoresEnviados { get; set; }

        public bool HayCamposInvalidos
        {
            get
            {
                return CamposInvalidos.Count > 0;
            }
        }

        public List<CamposInvalidos> CamposInvalidos { get; set; } = new List<CamposInvalidos>();


        public string MensajeDeErrorGeneral { get; set; }
    }

    public class CamposInvalidos
    {
        public string NombreDelCampo { get; set; }

        public string MensajeDeError { get; set; }
    }

    public class CuentaDeUsuarioGestionarPerfilViewModel
    {
        public string NombreDeUsuario { get; set; }

        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

             

        public CuentaDeUsuarioCambiarContraseñaViewModel CambioDeContrasenaVM { get; set; } = new CuentaDeUsuarioCambiarContraseñaViewModel();        

    }
    
}