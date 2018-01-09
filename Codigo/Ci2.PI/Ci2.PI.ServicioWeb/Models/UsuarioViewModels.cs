using Ci2.PI.ServicioWeb.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Models
{
    public class InformacionParaGenerarListado
    {
        public int RegistrosPorPagina { get; set; } = 10;
    }

    public class BaseInicioViewModel
    {
        public InformacionParaGenerarListado InformacionParaGenerarListado { get; set; } = new InformacionParaGenerarListado();
    }

    public class UsuarioInicioViewModel : BaseInicioViewModel
    {
        public IEnumerable<UsuarioPEVM> Usuarios { get; set; }

        public string UsuarioActualId { get; set; }
    }

    public class UsuarioRegistrarViewModel
    {

        [Required]
        [Display(Name = "Nombre de Usuario")]
        [RegularExpression(@"^([a-zA-Z][\w.]+|[0-9][0-9_.]*[a-zA-Z]+[\w.]*)$")]
        public String NombreUsuario { get; set; }

        [Required]
        [Display(Name = "Correo Electronico")]
        [EmailAddress]
        public String CorreoElectronico { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public String Contraseña { get; set; }

        [Required]
        [Compare("Contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public String ConfirmacionDeContraseña { get; set; }

        [Required]
        [Display(Name = "Numero Telefonico")]
        public String NumeroTelefonico { get; set; }


    }

    public class UsuarioEditarViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public String UsuarioId { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        public String NombreUsuario { get; set; }

        [Required]
        [Display(Name = "Correo Electronico")]
        public String CorreoElectronico { get; set; }

        [Required]
        [Display(Name = "Correo Electronico de Confirmación")]
        public Boolean CorreoElectronicoConfirmado { get; set; }

        [Required]
        [Display(Name = "Numero Telefonico")]
        public String NumeroTelefonico { get; set; }

        [Required]
        [Display(Name = "Numero Telefonico de Confirmación")]
        public Boolean NumeroTelefonicoConfirmado { get; set; }

        [Required]
        [Display(Name = "Autenticacion Dos Etapas Activado")]
        public Boolean AutenticacionDosEtapasActivado { get; set; }

        [Display(Name = "Fecha de Bloqueo")]
        public Nullable<DateTime> FechaBloqueoUtc { get; set; }

        [Display(Name = "Bloqueo por intentos fallidos")]
        public Boolean BloqueoActivado { get; set; }

        [Required]
        [Display(Name = "Numero de Intento de Acceso Fallidos")]
        public Int32 NumeroAccesosFallidos { get; set; }

    }
}