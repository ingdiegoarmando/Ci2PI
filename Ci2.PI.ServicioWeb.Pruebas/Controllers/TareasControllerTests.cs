using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ci2.PI.ServicioWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ci2.PI.ServicioWeb.Models;
using Ci2.PI.ServicioWeb.Entidades;
using System.Web.Http;

namespace Ci2.PI.ServicioWeb.Controllers.Tests
{
    [TestClass()]
    public class TareasControllerTests
    {
        private UsuarioActual ObtenerNombreDeUsuario()
        {
            return new UsuarioActual()
            {
                NombreDeUsuarioActual = "a@a.com",
                IdDeUsuarioActual = "852c89ca-b7a8-4423-84af-2f6fac9a4004",
            };
        }

        [TestMethod()]
        public void GetConsultarTest_FiltroPorDefecto_LlamadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var nombreDeUsuario = ObtenerNombreDeUsuario();

                var filtro = new ConsultarBindingModel();

                var tareas = controlador.GetConsultar(filtro, nombreDeUsuario);

                foreach (var tarea in tareas)
                {
                    //Por defecto solo se traen las tareas propias
                    Assert.IsTrue(nombreDeUsuario.Equals(tarea.Autor));
                }
            }
        }

        [TestMethod()]
        public void GetConsultarTest_FiltroConLasTareasDeTodosLoUsuarios_LlamadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var nombreDeUsuario = ObtenerNombreDeUsuario();

                var filtro1TodosLosAutores = new ConsultarBindingModel();
                filtro1TodosLosAutores.Autoria = Autoria.TodosLosAutores;

                var tareasFiltro1 = controlador.GetConsultar(filtro1TodosLosAutores, nombreDeUsuario);

                var filtro2Pripias = new ConsultarBindingModel();
                filtro2Pripias.Autoria = Autoria.Propia;

                var tareasFiltro2 = controlador.GetConsultar(filtro2Pripias, nombreDeUsuario);

                Assert.IsTrue(tareasFiltro1.Count() >= tareasFiltro2.Count());
            }
        }

        [TestMethod()]
        public void GetConsultarTest_FiltroConLasTareasFinalizadasDeUnUsuario_LlamadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var nombreDeUsuario = ObtenerNombreDeUsuario();

                var filtro1TodosLosAutores = new ConsultarBindingModel();
                filtro1TodosLosAutores.Autoria = Autoria.Propia;
                filtro1TodosLosAutores.Estado = EstadoTarea.Finalizadas;

                var tareas = controlador.GetConsultar(filtro1TodosLosAutores, nombreDeUsuario);

                foreach (var tarea in tareas)
                {
                    Assert.IsTrue(EstadoTareaVM.Si == tarea.EstadoTarea);
                }
            }
        }

        [TestMethod()]
        public void GetConsultarTest_FiltroConLasTareasSinFinalizadasDeUnUsuario_LlamadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var nombreDeUsuario = ObtenerNombreDeUsuario();

                var filtro1TodosLosAutores = new ConsultarBindingModel();
                filtro1TodosLosAutores.Autoria = Autoria.Propia;
                filtro1TodosLosAutores.Estado = EstadoTarea.Pendientes;

                var tareas = controlador.GetConsultar(filtro1TodosLosAutores, nombreDeUsuario);

                foreach (var tarea in tareas)
                {
                    Assert.IsTrue(EstadoTareaVM.No == tarea.EstadoTarea);
                }
            }
        }

        [TestMethod()]
        public void GetConsultarTest_FiltroOrdenamiento_LlamadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var nombreDeUsuario = ObtenerNombreDeUsuario();

                var filtro1 = new ConsultarBindingModel();
                filtro1.OrdenarFechaVencimiento = OrdenarFechaCreacion.Asc;

                var tareasFiltro1 = controlador.GetConsultar(filtro1, nombreDeUsuario);

                var filtro2 = new ConsultarBindingModel();
                filtro2.OrdenarFechaVencimiento = OrdenarFechaCreacion.Des;

                var tareasFiltro2 = controlador.GetConsultar(filtro2, nombreDeUsuario);

                Assert.AreEqual(tareasFiltro1.Count(), tareasFiltro2.Count());

                var tareasFiltro1ComoList = new List<TareaVM>(tareasFiltro1);
                var tareasFiltro2ComoList = new List<TareaVM>(tareasFiltro2);

                for (int indice = 0; indice < tareasFiltro1ComoList.Count(); indice++)
                {
                    int indiceInverso = tareasFiltro1ComoList.Count() - indice - 1;

                    Assert.AreEqual(tareasFiltro1ComoList[indice].FechaCreacion, tareasFiltro2ComoList[indiceInverso].FechaCreacion);

                }

            }
        }

        private CrearBindingModel GenerarCrearBindingModel()
        {
            var fecha = DateTime.Now;
            var fechaVencimiento = fecha.AddDays(7);

            return new CrearBindingModel()
            {
                Descripcion = $"Algo interesante pasó el {fecha}",
                EstadoTarea = EstadoTareaVM.Si,
                FechaCreacion = fecha,
                FechaVencimiento = fechaVencimiento,

            };
        }

        [TestMethod()]
        public void PostCrearTest_LlammadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var nombreDeUsuario = ObtenerNombreDeUsuario();

                var tarea = GenerarCrearBindingModel();

                var tareasVM = controlador.PostCrear(tarea, nombreDeUsuario);

                Assert.IsTrue(tareasVM.Id > 0, $"tareasVM.Id : {tareasVM.Id}");
                Assert.AreEqual(tarea.Descripcion, tareasVM.Descripcion);
                Assert.AreEqual(tarea.FechaVencimiento, tareasVM.FechaVencimiento);
                Assert.AreEqual(tareasVM.Autor, nombreDeUsuario.NombreDeUsuarioActual);

            }
        }

        private ActualizarBindingModel GenerarActualizarBindingModel()
        {
            var fecha = DateTime.Now;
            var fechaVencimiento = fecha.AddDays(7);

            return new ActualizarBindingModel()
            {
                Id = 1,
                Descripcion = $"Algo interesante pasó el {fecha}",
                EstadoTarea = EstadoTareaVM.Si,
                FechaCreacion = fecha,
                FechaVencimiento = fechaVencimiento,

            };
        }

        private ActualizarBindingModel GenerarActualizarBindingModelDesdeVM(TareaVM tarea)
        {

            return new ActualizarBindingModel()
            {
                Id = tarea.Id,
                Descripcion = tarea.Descripcion,
                EstadoTarea = tarea.EstadoTarea,
                FechaCreacion = tarea.FechaCreacion,
                FechaVencimiento = tarea.FechaVencimiento,
            };
        }

        [TestMethod()]
        public void PostActualizarTest_LlammadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var nombreDeUsuario = ObtenerNombreDeUsuario();


                var tareaCBM = GenerarCrearBindingModel();

                var tareasVM = controlador.PostCrear(tareaCBM, nombreDeUsuario);

                var tareaABM = GenerarActualizarBindingModelDesdeVM(tareasVM);

                var fecha = DateTime.Now;

                tareaABM.FechaVencimiento = fecha;

                var tareasVM2 = controlador.PostActualizar(tareaABM, nombreDeUsuario);

                Assert.IsTrue(tareasVM.Id > 0, $"tareasVM.Id : {tareasVM.Id}");
                Assert.AreEqual(tareasVM.Id, tareasVM2.Id);
                Assert.AreEqual(tareaCBM.Descripcion, tareasVM2.Descripcion);
                Assert.AreNotEqual(tareaCBM.FechaVencimiento, tareasVM2.FechaVencimiento);
                Assert.AreEqual(tareasVM.Autor, nombreDeUsuario.NombreDeUsuarioActual);

            }
        }

        [TestMethod()]
        [ExpectedException(typeof(HttpResponseException))]
        public void PostActualizarTest_LlammadoFallido()
        {
            using (var controlador = new TareasController())
            {
                var usuarioAutor = ObtenerNombreDeUsuario();

                var tareaCBM = GenerarCrearBindingModel();

                var tareasVM = controlador.PostCrear(tareaCBM, usuarioAutor);

                var tareaABM = GenerarActualizarBindingModelDesdeVM(tareasVM);

                var fecha = DateTime.Now;

                tareaABM.FechaVencimiento = fecha;

                var usuarioDiferenteAutor = new UsuarioActual()
                {
                    IdDeUsuarioActual = "123",
                    NombreDeUsuarioActual = usuarioAutor.NombreDeUsuarioActual + "1"
                };

                var tareasVM2 = controlador.PostActualizar(tareaABM, usuarioDiferenteAutor);

                Assert.Fail();

            }
        }

        [TestMethod()]
        public void PostBorrarTest_LlammadoExitoso()
        {
            using (var controlador = new TareasController())
            {
                var usuarioAutor = ObtenerNombreDeUsuario();

                var tarea = GenerarCrearBindingModel();

                var tareasVM = controlador.PostCrear(tarea, usuarioAutor);

                var tareaBBM = new BorrarBindingModel()
                {
                    Id = tareasVM.Id
                };

                controlador.PostBorrar(tareaBBM, usuarioAutor);

            }
        }

        [TestMethod()]
        [ExpectedException(typeof(HttpResponseException))]
        public void PostBorrarTest_LlammadoFallido()
        {
            using (var controlador = new TareasController())
            {
                var usuarioAutor = ObtenerNombreDeUsuario();

                var tarea = GenerarCrearBindingModel();

                var tareasVM = controlador.PostCrear(tarea, usuarioAutor);

                var tareaBBM = new BorrarBindingModel()
                {
                    Id = tareasVM.Id
                };

                var usuarioDiferenteAutor = new UsuarioActual()
                {
                    IdDeUsuarioActual = "123",
                    NombreDeUsuarioActual = usuarioAutor.NombreDeUsuarioActual + "1"
                };

                controlador.PostBorrar(tareaBBM, usuarioDiferenteAutor);

            }
        }
    }
}