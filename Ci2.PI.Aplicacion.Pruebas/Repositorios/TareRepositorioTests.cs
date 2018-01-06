using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ci2.PI.Aplicacion.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ci2.PI.Persistencia.Modelo;

namespace Ci2.PI.Aplicacion.Repositorios.Tests
{
    [TestClass()]
    public class TareRepositorioTests
    {
        private TabTarea GenerarTarea()
        {
            var fecha = DateTime.Now;            

            return new TabTarea()
            {
                Ci2Descripcion = $"Algo interesante pasó el {fecha}",
                Ci2EstadoTareaId = 1,
                Ci2FechaCreacion = fecha,
                Ci2UsuarioId = "852c89ca-b7a8-4423-84af-2f6fac9a4004",
                //Ci2UsuarioId = "8579920e-0c6b-4a74-aa34-2ac1fdffca69",
            };
        }

        private void AgregarOActualizarTarea(TabTarea tarea)
        {
            using (var db = new Ci2PIBDEntidades())
            {
                var repositorio = new TareRepositorio(db);
                repositorio.AgregarOActualizar(tarea);
            }
        }

        private TabTarea ConsultarTareaPorId(long id)
        {
            using (var db = new Ci2PIBDEntidades())
            {
                var repositorio = new TareRepositorio(db);
                return repositorio.ConsultarPorId(id);
            }
        }


        [TestMethod()]
        public void AgregarOActualizarTest_Agregar_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            AgregarOActualizarTarea(tarea);

            Assert.IsTrue(tarea.Ci2TareaId > 0);
        }

        [TestMethod()]
        public void AgregarOActualizarTest_Actualizar_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            AgregarOActualizarTarea(tarea);

            TabTarea tareaDeBaseDeDatos = ConsultarTareaPorId(tarea.Ci2TareaId);

            var fecha = DateTime.Now;
            tareaDeBaseDeDatos.Ci2FechaCreacion = fecha;

            AgregarOActualizarTarea(tareaDeBaseDeDatos);

            tareaDeBaseDeDatos = ConsultarTareaPorId(tarea.Ci2TareaId);

            Assert.AreEqual(tarea.Ci2TareaId, tareaDeBaseDeDatos.Ci2TareaId);
            Assert.AreEqual(tarea.Ci2Descripcion, tareaDeBaseDeDatos.Ci2Descripcion);
            Assert.AreNotEqual(tarea.Ci2FechaCreacion, tareaDeBaseDeDatos.Ci2FechaCreacion);
        }

        [TestMethod()]
        public void ConsultarPorIdTest_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            AgregarOActualizarTarea(tarea);

            var tareaDeBaseDeDatos = ConsultarTareaPorId(tarea.Ci2TareaId);

            Assert.AreNotEqual(tareaDeBaseDeDatos, null);
            Assert.AreEqual(tareaDeBaseDeDatos.Ci2TareaId, tarea.Ci2TareaId);
            Assert.AreEqual(tareaDeBaseDeDatos.Ci2Descripcion, tarea.Ci2Descripcion);
        }

        [TestMethod()]
        public void EliminarTest_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            AgregarOActualizarTarea(tarea);

            using (var db = new Ci2PIBDEntidades())
            {
                var repositorio = new TareRepositorio(db);
                repositorio.Eliminar(tarea.Ci2TareaId);
            }

            var tareaDeBaseDeDatos = ConsultarTareaPorId(tarea.Ci2TareaId);

            Assert.AreEqual(tareaDeBaseDeDatos, null);
        }

        [TestMethod()]
        public void ListarTest_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            AgregarOActualizarTarea(tarea);

            using (var db = new Ci2PIBDEntidades())
            {
                var repositorio = new TareRepositorio(db);
                var tareas = repositorio.Listar();

                Assert.IsTrue(tareas.Any(item => item.Ci2TareaId == tarea.Ci2TareaId));
            }
        }
    }
}