using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Models.Shared
{
    public class UsuarioPEVM
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
        [Display(Name = "PasswordHash")]
        public String PasswordHash { get; set; }

        [Required]
        [Display(Name = "SecurityStamp")]
        public String SecurityStamp { get; set; }

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