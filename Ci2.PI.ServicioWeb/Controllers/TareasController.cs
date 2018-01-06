using Ci2.PI.Aplicacion.Repositorios;
using Ci2.PI.ServicioWeb.Infraestructura.Binders;
using Ci2.PI.ServicioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace Ci2.PI.ServicioWeb.Controllers
{
    public class TareasController : Ci2PIApiController
    {
        public IEnumerable<TareaVM> GetConsultar([FromUri]ConsultarBindingModel filtro, [ValueProvider(typeof(NombreDeUsuarioActualValueProviderFactory))] string nombreDeUsuarioActual)
        {
            var filtroBD = new FiltroConsultarTarea() { };

            switch (filtro.Autoria)
            {
                case Autoria.TodosLosAutores:
                    filtroBD.UsuarioId = null;
                    break;
                case Autoria.Propia:
                    filtroBD.UsuarioId = nombreDeUsuarioActual;
                    break;
                default:
                    throw new NotSupportedException($"La autoria = {filtro.Autoria} no es soportado");
            }

            filtroBD.EstadoId = ConvertidosDeEntidades.ObtenerEstadoTareaBD(filtro.Estado);

            var datosDeBD = UnidadDeTrabajo.TareaRepositorio.ConsultarPorFiltro(filtroBD);
            var datosVista = ConvertidosDeEntidades.ObtenerTareaVM(datosDeBD);

            switch (filtro.OrdenarFechaCreacion)
            {
                case OrdenarFechaCreacion.Asc:
                    datosVista = datosVista.OrderBy(item => item.FechaCreacion);
                    break;
                case OrdenarFechaCreacion.Des:
                    datosVista = datosVista.OrderByDescending(item => item.FechaCreacion);
                    break;
                case OrdenarFechaCreacion.NoOrdenar:
                    //No hacer nada
                    break;
                default:
                    throw new NotSupportedException($"El orden = {filtro.OrdenarFechaCreacion} no es soportado");
            }

            return datosVista;
        }
    }
}
