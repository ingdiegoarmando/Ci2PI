using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Models
{
    public enum Autoria
    {
        TodosLosAutores,
        Propia
    }

    public enum EstadoTarea
    {
        Finalizadas,
        Pendientes,
        Todas
    }

    public enum OrdenarFechaCreacion
    {
        Asc,
        Des,
        NoOrdenar
    }

    public class ConsultarBindingModel
    {
        public Autoria Autoria { get; set; } = Autoria.Propia;

        public EstadoTarea Estado { get; set; } = EstadoTarea.Todas;

        public OrdenarFechaCreacion OrdenarFechaVencimiento { get; set; } = OrdenarFechaCreacion.NoOrdenar;

    }


    public class CrearBindingModel
    {
        public System.DateTime FechaCreacion { get; set; }
        public System.DateTime FechaVencimiento { get; set; }
        public string Descripcion { get; set; }
        public EstadoTareaVM EstadoTarea { get; set; }        
    }

    public class ActualizarBindingModel : CrearBindingModel
    {
        [Required]
        public long Id { get; set; }
    }

    public class BorrarBindingModel
    {
        public long Id { get; set; }
    }
}