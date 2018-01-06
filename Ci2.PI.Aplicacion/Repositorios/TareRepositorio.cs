using Ci2.PI.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci2.PI.Aplicacion.Repositorios
{
    public class ResultadoTareaConsultarPorFiltro
    {
        public long Ci2TareaId { get; set; }
        public System.DateTime Ci2FechaCreacion { get; set; }
        public string Ci2Descripcion { get; set; }
        public long Ci2EstadoTareaId { get; set; }
        public string Ci2UsuarioId { get; set; }
        public string Ci2NombreUsuario { get; set; }
    }

    public class FiltroConsultarTarea
    {
        public string UsuarioId { get; set; }

        public long EstadoId { get; set; }
    }

    public class TareRepositorio : IRepositorio<TabTarea>
    {
        private Ci2PIBDEntidades ContextoBD { get; set; }

        #region Constructores
        public TareRepositorio(Ci2PIBDEntidades contextoBD)
        {
            this.ContextoBD = contextoBD;
        }
        #endregion

        #region Implementacion de IRepositorio        
        public void AgregarOActualizar(TabTarea entidad)
        {
            var idTareaCreada = ContextoBD.PraTabTareaAgregarOActualizar(entidad.Ci2TareaId, entidad.Ci2FechaCreacion, entidad.Ci2Descripcion, entidad.Ci2EstadoTareaId, entidad.Ci2UsuarioId).SingleOrDefault();

            if (entidad.Ci2TareaId == 0)
            {
                entidad.Ci2TareaId = Convert.ToInt64(idTareaCreada);
            }

        }

        public TabTarea ConsultarPorId(params object[] id)
        {
            var idComoLong = Convert.ToInt64(id[0]);
            var resultadoBD = ContextoBD.PraTabTareaConsultarPorId(idComoLong).SingleOrDefault();
            if (resultadoBD != null)
            {
                return new TabTarea()
                {
                    Ci2Descripcion = resultadoBD.Ci2Descripcion,
                    Ci2EstadoTareaId = resultadoBD.Ci2EstadoTareaId,
                    Ci2FechaCreacion = resultadoBD.Ci2FechaCreacion,
                    Ci2TareaId = resultadoBD.Ci2TareaId,
                    Ci2UsuarioId = resultadoBD.Ci2UsuarioId,
                };
            }
            else
            {
                return null;
            }

        }

        public void Eliminar(params object[] id)
        {
            var idComoLong = Convert.ToInt64(id[0]);
            ContextoBD.PraTabTareaEliminar(idComoLong);
        }

        public IEnumerable<TabTarea> Listar()
        {
            var resultadoBaseDeDatos = ContextoBD.PraTabTareaListar().ToList();
            var listado = new List<TabTarea>();
            if (resultadoBaseDeDatos != null && resultadoBaseDeDatos.Count > 0)
            {
                listado = resultadoBaseDeDatos.Select(item => new TabTarea()
                {
                    Ci2Descripcion = item.Ci2Descripcion,
                    Ci2EstadoTareaId = item.Ci2EstadoTareaId,
                    Ci2FechaCreacion = item.Ci2FechaCreacion,
                    Ci2TareaId = item.Ci2TareaId,
                    Ci2UsuarioId = item.Ci2UsuarioId,
                }).ToList();
            }

            return listado;
        }
        #endregion

        public IEnumerable<ResultadoTareaConsultarPorFiltro> ConsultarPorFiltro(FiltroConsultarTarea filtro)
        {
            var consultaBD = Listar();

            if (filtro.EstadoId != 0)
            {
                consultaBD = consultaBD.Where(item => item.Ci2EstadoTareaId == filtro.EstadoId);
            }

            if (filtro.UsuarioId != null)
            {
                consultaBD = consultaBD.Where(item => item.Ci2UsuarioId == filtro.UsuarioId);
            }


            var resultado = new List<ResultadoTareaConsultarPorFiltro>();


            foreach (var tarea in consultaBD)
            {
                resultado.Add(new ResultadoTareaConsultarPorFiltro()
                {
                    Ci2Descripcion = tarea.Ci2Descripcion,
                    Ci2EstadoTareaId = tarea.Ci2EstadoTareaId,
                    Ci2FechaCreacion = tarea.Ci2FechaCreacion,
                    Ci2NombreUsuario = tarea.Ci2UsuarioId,
                    Ci2TareaId = tarea.Ci2TareaId,
                    Ci2UsuarioId = tarea.Ci2UsuarioId,
                });                
                
            }

            return resultado;
        }


    }
}
