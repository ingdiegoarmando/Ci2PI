using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ci2.PI.ServicioWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ci2.PI.ServicioWeb.Models;

namespace Ci2.PI.ServicioWeb.Controllers.Tests
{
    [TestClass()]
    public class TareasControllerTests
    {
        private string ObtenerNombreDeUsuario()
        {
            return "a@a.com";
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
                filtro1.OrdenarFechaCreacion = OrdenarFechaCreacion.Asc;

                var tareasFiltro1 = controlador.GetConsultar(filtro1, nombreDeUsuario);

                var filtro2 = new ConsultarBindingModel();
                filtro2.OrdenarFechaCreacion = OrdenarFechaCreacion.Des;

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
    }
}