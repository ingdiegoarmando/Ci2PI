using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ci2.PI.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci2.PI.Persistencia.Modelo.Tests
{
    [TestClass()]
    public class Ci2PIBDEntidadesTests
    {
        private TabTarea GenerarTarea()
        {
            var fecha = DateTime.Now;

            return new TabTarea()
            {
                Ci2Descripcion = $"Algo interesante paso el {fecha}",
                Ci2EstadoTareaId = 1,
                Ci2FechaCreacion = fecha,
                Ci2UsuarioId = "852c89ca-b7a8-4423-84af-2f6fac9a4004",
            };
        }

        private long AgregarOActualizarTarea(TabTarea tarea)
        {
            using (var db = new Ci2PIBDEntidades())
            {
                var id = db.PraTabTareaAgregarOActualizar(0, tarea.Ci2FechaCreacion, tarea.Ci2Descripcion, tarea.Ci2EstadoTareaId, tarea.Ci2UsuarioId).SingleOrDefault();

                return Convert.ToInt64(id);
            }
        }

        [TestMethod()]
        public void PraTabTareaAgregarOActualizarTest_Agregar_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            var idTareaCreada = AgregarOActualizarTarea(tarea);

            Assert.IsTrue(idTareaCreada > 0);
        }

        [TestMethod()]
        public void PraTabTareaConsultarPorIdTest_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            var idTareaCreada = AgregarOActualizarTarea(tarea);

            using (var db = new Ci2PIBDEntidades())
            {
                var tareaDeBaseDeDatos = db.PraTabTareaConsultarPorId(idTareaCreada).SingleOrDefault();

                Assert.AreNotEqual(tareaDeBaseDeDatos, null);
                Assert.AreEqual(tareaDeBaseDeDatos.Ci2TareaId, idTareaCreada);
                Assert.AreEqual(tareaDeBaseDeDatos.Ci2Descripcion, tarea.Ci2Descripcion);
            }
        }

        [TestMethod()]
        public void PraTabTareaEliminarTest_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            var idTareaCreada = AgregarOActualizarTarea(tarea);

            using (var db = new Ci2PIBDEntidades())
            {
                db.PraTabTareaEliminar(idTareaCreada);
            }

            using (var db = new Ci2PIBDEntidades())
            {
                var tareaDeBaseDeDatos = db.PraTabTareaConsultarPorId(idTareaCreada).SingleOrDefault();
                Assert.AreEqual(tareaDeBaseDeDatos, null);
            }
        }

        [TestMethod()]
        public void PraTabTareaListarTest_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            var idTareaCreada = AgregarOActualizarTarea(tarea);

            using (var db = new Ci2PIBDEntidades())
            {
                var tareas = db.PraTabTareaListar().ToList();

                Assert.IsTrue(tareas.Any(item => item.Ci2TareaId == idTareaCreada));
            }
        }
    }
}