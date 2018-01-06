using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Models
{
    public enum EstadoTareaVM
    {
        Si,
        No
    }

    public class TareaVM
    {
        public long Id { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public System.DateTime FechaVencimiento { get; set; }
        public string Descripcion { get; set; }
        public EstadoTareaVM EstadoTarea { get; set; }
        public string Autor { get; set; }
    }
}