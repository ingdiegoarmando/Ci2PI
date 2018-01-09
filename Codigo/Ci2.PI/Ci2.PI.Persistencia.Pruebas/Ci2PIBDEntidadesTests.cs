using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ci2.PI.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Ci2.PI.Persistencia.Modelo.Tests
{
    [TestClass()]
    public class Ci2PIBDEntidadesTests
    {
        #region CRUD de Tareas
        private TabTarea GenerarTarea()
        {
            var fecha = DateTime.Now;
            var fechaVencimiento = fecha.AddDays(7);
            return new TabTarea()
            {
                Ci2Descripcion = $"Algo interesante paso el {fecha}",
                Ci2EstadoTareaId = 1,
                Ci2FechaCreacion = fecha,
                Ci2FechaVencimiento = fechaVencimiento,
                Ci2UsuarioId = "852c89ca-b7a8-4423-84af-2f6fac9a4004",
            };
        }

        private long AgregarOActualizarTarea(TabTarea tarea)
        {
            using (var db = new Ci2PIBDEntidades())
            {
                var id = db.PraTabTareaAgregarOActualizar(0, tarea.Ci2FechaCreacion, tarea.Ci2Descripcion, tarea.Ci2EstadoTareaId, tarea.Ci2UsuarioId, tarea.Ci2FechaVencimiento).SingleOrDefault();

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
        #endregion

        #region CRUD de Usuario
        private TabUsuario GenerarUsuario()
        {
            var fecha = DateTime.Now;
            string telefono = fecha.ToString("yyyy-mm-dd");
            string correoElectronico = "c@c.com";
            string nombreDeUsuario = "Tester-"+ fecha.ToString();
            string contraseña = "!Qaz12";

            var passwordHasher = new PasswordHasher();
            var passwordHash = passwordHasher.HashPassword(contraseña);
            var hashCode = Guid.NewGuid().ToString();

            return new TabUsuario()
            {
                Ci2AutenticacionDosEtapasActivado = false,
                Ci2BloqueoActivado = false,
                Ci2CorreoElectronico = correoElectronico,
                Ci2CorreoElectronicoConfirmado = false,
                Ci2FechaBloqueoUtc = null,
                Ci2NombreUsuario = nombreDeUsuario,
                Ci2NumeroAccesosFallidos = 0,
                Ci2NumeroTelefonico = telefono,
                Ci2NumeroTelefonicoConfirmado = false,
                Ci2PasswordHash = passwordHash,
                Ci2SecurityStamp = hashCode,
            };
        }

        private string AgregarOActualizarUsuario(TabUsuario usuario)
        {
            using (var db = new Ci2PIBDEntidades())
            {
                string id = db.PraTabUsuarioAgregarOActualizar( usuario.Ci2UsuarioId,  usuario.Ci2CorreoElectronico, usuario.Ci2CorreoElectronicoConfirmado,  usuario.Ci2PasswordHash,  usuario.Ci2SecurityStamp,  usuario.Ci2NumeroTelefonico,  usuario.Ci2NumeroTelefonicoConfirmado,  usuario.Ci2AutenticacionDosEtapasActivado, usuario.Ci2FechaBloqueoUtc,usuario.Ci2BloqueoActivado, usuario.Ci2NumeroAccesosFallidos,  usuario.Ci2NombreUsuario).SingleOrDefault();

                return id;
            }
        }

        [TestMethod()]
        public void PraTabUsuarioAgregarOActualizarTest_Agregar_LlamadoExitoso()
        {
            var usuario = GenerarUsuario();

            string idUsuarioCreada = AgregarOActualizarUsuario(usuario);

            Assert.AreNotEqual(idUsuarioCreada, null);
            Assert.AreNotEqual(idUsuarioCreada,"");
        }

        [TestMethod()]
        public void PraTabUsuarioConsultarPorIdTest_LlamadoExitoso()
        {
            var usuario = GenerarUsuario();

            string id = AgregarOActualizarUsuario(usuario);

            using (var db = new Ci2PIBDEntidades())
            {
                var usuarioDeBaseDeDatos = db.PraTabUsuarioConsultarPorId(id).SingleOrDefault();

                Assert.AreNotEqual(usuarioDeBaseDeDatos, null);
                Assert.AreEqual(usuarioDeBaseDeDatos.Ci2UsuarioId, id);
                Assert.AreEqual(usuarioDeBaseDeDatos.Ci2NombreUsuario, usuario.Ci2NombreUsuario);
                Assert.AreEqual(usuarioDeBaseDeDatos.Ci2CorreoElectronico, usuario.Ci2CorreoElectronico);
            }
        }

        [TestMethod()]
        public void PraTabUsuarioEliminarTest_LlamadoExitoso()
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
        public void PraTabUsuarioListarTest_LlamadoExitoso()
        {
            var tarea = GenerarTarea();

            var idTareaCreada = AgregarOActualizarTarea(tarea);

            using (var db = new Ci2PIBDEntidades())
            {
                var tareas = db.PraTabTareaListar().ToList();

                Assert.IsTrue(tareas.Any(item => item.Ci2TareaId == idTareaCreada));
            }
        }
        #endregion
    }
}