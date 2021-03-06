﻿using Ci2.PI.Aplicacion.Repositorios;
using Ci2.PI.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Models
{
    public sealed class ConvertidosDeEntidades
    {
        private ConvertidosDeEntidades()
        {

        }

        public static EstadoTareaVM ObtenerEstadoTareaVM(long id)
        {
            if (id == 1)
            {
                return EstadoTareaVM.Si;
            }
            else if (id == 2)
            {
                return EstadoTareaVM.No;
            }
            else
            {
                throw new NotSupportedException($"El id = {id} no es soportado");
            }
        }

        public static long ObtenerEstadoTareaBD(EstadoTarea estado)
        {
            switch (estado)
            {
                case EstadoTarea.Finalizadas:
                    return 1;
                case EstadoTarea.Pendientes:
                    return 2;
                case EstadoTarea.Todas:
                    return 0;
                default:
                    throw new NotSupportedException($"El estado = {estado} no es soportado");
            }
        }

        public static long ObtenerEstadoTareaBD(EstadoTareaVM estado)
        {
            switch (estado)
            {
                case EstadoTareaVM.Si:
                    return 1;
                case EstadoTareaVM.No:
                    return 2;
                default:
                    throw new NotSupportedException($"El estado = {estado} no es soportado");
            }

        }

        public static TareaVM ObtenerTareaVM(TabTarea tarea)
        {
            string autor = null;

            using (var bd = new Ci2PIBDEntidades())
            {
                autor = bd.TabUsuario.Where(item => item.Ci2UsuarioId == tarea.Ci2UsuarioId).Select(item => item.Ci2NombreUsuario).SingleOrDefault();
            }

            EstadoTareaVM estadoTarea = ObtenerEstadoTareaVM(tarea.Ci2EstadoTareaId);
            return new TareaVM()
            {
                Id = tarea.Ci2TareaId,
                Descripcion = tarea.Ci2Descripcion,
                EstadoTarea = estadoTarea,
                FechaCreacion = tarea.Ci2FechaCreacion,
                FechaVencimiento = tarea.Ci2FechaVencimiento,
                Autor = autor,
            };
        }

        public static TareaVM ObtenerTareaVM(TabTarea tarea, string autor)
        {
            EstadoTareaVM estadoTarea = ObtenerEstadoTareaVM(tarea.Ci2EstadoTareaId);
            return new TareaVM()
            {
                Id = tarea.Ci2TareaId,
                Descripcion = tarea.Ci2Descripcion,
                EstadoTarea = estadoTarea,
                FechaCreacion = tarea.Ci2FechaCreacion,
                FechaVencimiento = tarea.Ci2FechaVencimiento,
                Autor = autor,
            };
        }

        public static IEnumerable<TareaVM> ObtenerTareaVM(IEnumerable<TabTarea> tareas)
        {
            var resultado = new List<TareaVM>();

            if (tareas != null)
            {
                foreach (var tarea in tareas)
                {
                    resultado.Add(ObtenerTareaVM(tarea));
                }
            }

            return resultado;

        }

        public static IEnumerable<TareaVM> ObtenerTareaVM(IEnumerable<TabTarea> tareas, string autor)
        {
            var resultado = new List<TareaVM>();

            if (tareas != null)
            {
                foreach (var tarea in tareas)
                {
                    resultado.Add(ObtenerTareaVM(tarea, autor));
                }
            }

            return resultado;

        }

        public static TareaVM ObtenerTareaVM(ResultadoTareaConsultarPorFiltro tarea)
        {
            string autor = tarea.Ci2UsuarioId;
            EstadoTareaVM estadoTarea = ObtenerEstadoTareaVM(tarea.Ci2EstadoTareaId);
            return new TareaVM()
            {
                Id = tarea.Ci2TareaId,
                Descripcion = tarea.Ci2Descripcion,
                EstadoTarea = estadoTarea,
                FechaCreacion = tarea.Ci2FechaCreacion,
                FechaVencimiento = tarea.Ci2FechaVencimiento,
                Autor = tarea.Ci2NombreUsuario,
            };
        }

        public static TabTarea ObtenerTareaBD(CrearBindingModel tarea)
        {
            long estadoTarea = ObtenerEstadoTareaBD(tarea.EstadoTarea);
            return new TabTarea()
            {
                Ci2Descripcion = tarea.Descripcion,
                Ci2EstadoTareaId = estadoTarea,
                Ci2FechaCreacion = tarea.FechaCreacion,
                Ci2FechaVencimiento = tarea.FechaVencimiento,
            };

        }

        public static TabTarea ObtenerTareaBD(ActualizarBindingModel tarea)
        {
            long estadoTarea = ObtenerEstadoTareaBD(tarea.EstadoTarea);
            return new TabTarea()
            {
                Ci2TareaId=tarea.Id,
                Ci2Descripcion = tarea.Descripcion,
                Ci2EstadoTareaId = estadoTarea,
                Ci2FechaCreacion = tarea.FechaCreacion,
                Ci2FechaVencimiento = tarea.FechaVencimiento,
            };

        }

        public static IEnumerable<TareaVM> ObtenerTareaVM(IEnumerable<ResultadoTareaConsultarPorFiltro> tareas)
        {
            var resultado = new List<TareaVM>();

            if (tareas != null)
            {
                foreach (var tarea in tareas)
                {
                    resultado.Add(ObtenerTareaVM(tarea));
                }
            }

            return resultado;

        }


    }
}