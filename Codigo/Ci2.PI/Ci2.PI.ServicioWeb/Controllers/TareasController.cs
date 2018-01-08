using Ci2.PI.Aplicacion.Excepciones;
using Ci2.PI.Aplicacion.Repositorios;
using Ci2.PI.ServicioWeb.Entidades;
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
    [RoutePrefix("Tareas")]
    [Authorize]
    public class TareasController : Ci2PIApiController
    {
        [Route("Consultar")]
        public IEnumerable<TareaVM> GetConsultar([FromUri]ConsultarBindingModel filtro, [ValueProvider(typeof(UsuarioActualValueProviderFactory))] UsuarioActual usuarioActual)
        {
            var filtroBD = new FiltroConsultarTarea() { };

            if (filtro == null)
            {
                filtro = new ConsultarBindingModel();
            }

            switch (filtro.Autoria)
            {
                case Autoria.TodosLosAutores:
                    filtroBD.NombreUsuario = null;
                    break;
                case Autoria.Propia:
                    filtroBD.NombreUsuario = usuarioActual.NombreDeUsuarioActual;
                    break;
                default:
                    throw new NotSupportedException($"La autoria = {filtro.Autoria} no es soportado");
            }

            filtroBD.EstadoId = ConvertidosDeEntidades.ObtenerEstadoTareaBD(filtro.Estado);

            var datosDeBD = UnidadDeTrabajo.TareaRepositorio.ConsultarPorFiltro(filtroBD);
            var datosVista = ConvertidosDeEntidades.ObtenerTareaVM(datosDeBD);

            switch (filtro.OrdenarFechaVencimiento)
            {
                case OrdenarFechaCreacion.Asc:
                    datosVista = datosVista.OrderBy(item => item.FechaVencimiento);
                    break;
                case OrdenarFechaCreacion.Des:
                    datosVista = datosVista.OrderByDescending(item => item.FechaVencimiento);
                    break;
                case OrdenarFechaCreacion.NoOrdenar:
                    //No hacer nada
                    break;
                default:
                    throw new NotSupportedException($"El orden = {filtro.OrdenarFechaVencimiento} no es soportado");
            }

            return datosVista;
        }

        [Route("Crear")]
        public TareaVM PostCrear([FromBody]CrearBindingModel tarea, [ValueProvider(typeof(UsuarioActualValueProviderFactory))] UsuarioActual usuarioActual)
        {
            var tareaBD = ConvertidosDeEntidades.ObtenerTareaBD(tarea);
            tareaBD.Ci2UsuarioId = usuarioActual.IdDeUsuarioActual;

            UnidadDeTrabajo.TareaRepositorio.AgregarOActualizar(tareaBD);

            var tareaResultante = ConvertidosDeEntidades.ObtenerTareaVM(tareaBD, usuarioActual.NombreDeUsuarioActual);
            //tareaResultante.Autor = usuarioActual.NombreDeUsuarioActual;

            return tareaResultante;
        }

        [Route("Actualizar")]
        public TareaVM PostActualizar([FromBody]ActualizarBindingModel tarea, [ValueProvider(typeof(UsuarioActualValueProviderFactory))] UsuarioActual usuarioActual)
        {
            try
            {
                var tareaBD = ConvertidosDeEntidades.ObtenerTareaBD(tarea);
                tareaBD.Ci2UsuarioId = usuarioActual.IdDeUsuarioActual;

                UnidadDeTrabajo.TareaRepositorio.AgregarOActualizar(tareaBD);

                var tareaResultante = ConvertidosDeEntidades.ObtenerTareaVM(tareaBD, usuarioActual.NombreDeUsuarioActual);

                return tareaResultante;
            }
            catch (TareaNoAutorizadaException ex)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = ex.Message };
                throw new HttpResponseException(msg);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Route("Borrar")]
        public void PostBorrar([FromBody]BorrarBindingModel tarea, [ValueProvider(typeof(UsuarioActualValueProviderFactory))] UsuarioActual usuarioActual)
        {

            try
            {
                UnidadDeTrabajo.TareaRepositorio.EliminarVerificandoAutoria(tarea.Id, usuarioActual.IdDeUsuarioActual);

            }
            catch (TareaNoAutorizadaException ex)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = ex.Message };
                throw new HttpResponseException(msg);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
