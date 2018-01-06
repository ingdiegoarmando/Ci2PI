using System;
using System.Collections.Generic;
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

        public OrdenarFechaCreacion OrdenarFechaCreacion { get; set; } = OrdenarFechaCreacion.NoOrdenar;

    }
}