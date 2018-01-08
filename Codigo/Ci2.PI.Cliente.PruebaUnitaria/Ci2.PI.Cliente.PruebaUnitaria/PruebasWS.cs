using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace Ci2.PI.Cliente.PruebaUnitaria
{
    public class ResultadoWSConsultar
    {
        public long Id { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public System.DateTime FechaVencimiento { get; set; }
        public string Descripcion { get; set; }
        public string EstadoTarea { get; set; }
        public string Autor { get; set; }
    }

    [TestClass]
    public class PruebasWS
    {
        public const string URL_BASE = "http://localhost:50089/tareas";

        public const string URL_BASE_CONSULTAR = URL_BASE + "/consultar";

        public const string URL_BASE_CREAR = URL_BASE + "/crear";

        public const string URL_BASE_ACTUALIZAR = URL_BASE + "/actualizar";

        public const string URL_BASE_BORRAR = URL_BASE + "/borrar";

        public string ObtenerTextoFormatoJsonDeLaUrl(string url)
        {
            string respuesta = null;

            using (var cliente = new WebClient())
            {
                cliente.Headers.Add("Content-Type", "application/json; charset=utf-8");
                respuesta = cliente.DownloadString(url);
            }

            return respuesta;
        }

        public T ObtenerObjetoDesdeUnTextoConFormatoJson<T>(string respuestaFormatoJson)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(respuestaFormatoJson);
        }

        [TestMethod]
        public void TestWSConsultar_SinFiltros_LlamadoExitoso()
        {
            string nombreUsuario = "a@a.com";

            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}");
            var tareas = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJson);
            foreach (var tarea in tareas)
            {
                //Por defecto solo se traen las tareas propias
                Assert.IsTrue(nombreUsuario.Equals(tarea.Autor));
            }
        }

        [TestMethod]
        public void TestWSConsultar_ConFiltroAutorPropioEstadoTodosOrdenarFechaCreacionNoOrdenar_LlamadoExitoso()
        {
            string respuestaFormatoJsonConsulta1 = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}");
            var tareasConsulta1 = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJsonConsulta1);

            string respuestaFormatoJsonConsulta2 = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}?Autoria=TodosLosAutores");
            var tareasConsulta2 = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJsonConsulta2);

            Assert.IsTrue(tareasConsulta1.Count() <= tareasConsulta2.Count());
        }

        [TestMethod]
        public void TestWSConsultar_ConFiltrosTareasFinalizadasDeUnUsuario_LlamadoExitoso()
        {
            string nombreUsuario = "a@a.com";

            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}?Autoria=Propia&Estado=Finalizadas");
            var tareas = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJson);
            foreach (var tarea in tareas)
            {
                //Por defecto solo se traen las tareas propias
                Assert.IsTrue(tarea.EstadoTarea == "Si");
            }
        }

        [TestMethod]
        public void TestWSConsultar_ConFiltrosTareasSinFinalizarDeUnUsuario_LlamadoExitoso()
        {
            string nombreUsuario = "a@a.com";

            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}?Autoria=Propia&Estado=Pendientes");
            var tareas = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJson);
            foreach (var tarea in tareas)
            {
                //Por defecto solo se traen las tareas propias
                Assert.IsTrue(tarea.EstadoTarea == "No");
            }
        }

        [TestMethod]
        public void TestWSConsultar_ConFiltrosTareasOrdenadas_LlamadoExitoso()
        {
            string nombreUsuario = "a@a.com";

            string respuestaFormatoJsonConsulta1 = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}?Autoria=Propia&OrdenarFechaVencimiento=Asc");
            var tareasConsulta1 = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJsonConsulta1);

            string respuestaFormatoJsonConsulta2 = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}?Autoria=Propia&OrdenarFechaVencimiento=Des");
            var tareasConsulta2 = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJsonConsulta2);

            Assert.AreEqual(respuestaFormatoJsonConsulta1.Count(), respuestaFormatoJsonConsulta2.Count());

            var tareasFiltro1ComoList = new List<ResultadoWSConsultar>(tareasConsulta1);
            var tareasFiltro2ComoList = new List<ResultadoWSConsultar>(tareasConsulta2);

            for (int indice = 0; indice < tareasFiltro1ComoList.Count(); indice++)
            {
                int indiceInverso = tareasFiltro1ComoList.Count() - indice - 1;

                Assert.AreEqual(tareasFiltro1ComoList[indice].FechaVencimiento, tareasFiltro2ComoList[indiceInverso].FechaVencimiento);

            }
        }

        public string ObtenerTextoFormatoJsonDeAccederALaUrlConPost(string url, Dictionary<string, string> parametros = null)
        {
            string respuesta = null;

            using (var cliente = new WebClient())
            {
                cliente.Encoding = System.Text.Encoding.UTF8;
                cliente.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                cliente.Headers.Add("Accept:application/json");

                string parametrosComoString = "";

                if (parametros != null && parametros.Count > 0)
                {
                    foreach (var item in parametros)
                    {
                        parametrosComoString += $"{item.Key}={item.Value}&";
                    }

                    parametrosComoString = parametrosComoString.Substring(0, parametrosComoString.Length - 1);
                }

                respuesta = cliente.UploadString(url, "POST", parametrosComoString);
            }

            return respuesta;
        }

        public string ObtenerFechaAjustadaComoString(DateTime fecha)
        {
            return fecha.ToUniversalTime()
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        [TestMethod]
        public void TestWSCrear_LlamadoExitoso()
        {
            string nombreUsuario = "a@a.com";

            string estado = "Si";
            var fechaCreacion = DateTime.Now;
            var fechaVencimiento = fechaCreacion.AddDays(7);
            string descripcion = $"Algo interesante pasó el {fechaCreacion}";

            string fechaCreacionComoString = ObtenerFechaAjustadaComoString(fechaCreacion);
            string fechaVencimientoComoString = ObtenerFechaAjustadaComoString(fechaVencimiento);

            var parametros = new Dictionary<string, string>();
            parametros.Add("Descripcion", descripcion);
            parametros.Add("EstadoTarea", estado);
            parametros.Add("FechaCreacion", fechaCreacionComoString);
            parametros.Add("FechaVencimiento", fechaVencimientoComoString);

            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_CREAR, parametros);
            var tarea = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJson);

            Assert.IsTrue(tarea.Id > 0, $"tarea.Id : {tarea.Id}");
            Assert.AreEqual(tarea.Descripcion, descripcion);
            Assert.AreEqual(ObtenerFechaAjustadaComoString(tarea.FechaVencimiento), ObtenerFechaAjustadaComoString(fechaVencimiento));
            Assert.AreEqual(tarea.Autor, nombreUsuario);

        }

        [TestMethod]
        public void TestWSActualizar_LlamadoExitoso()
        {
            string nombreUsuario = "a@a.com";

            string estado = "Si";
            var fechaCreacion = DateTime.Now;
            var fechaVencimiento = fechaCreacion.AddDays(7);
            string descripcion = $"Algo interesante pasó el {fechaCreacion}";

            string fechaCreacionComoString = ObtenerFechaAjustadaComoString(fechaCreacion);
            string fechaVencimientoComoString = ObtenerFechaAjustadaComoString(fechaVencimiento);

            var parametrosCrear = new Dictionary<string, string>();
            parametrosCrear.Add("Descripcion", descripcion);
            parametrosCrear.Add("EstadoTarea", estado);
            parametrosCrear.Add("FechaCreacion", fechaCreacionComoString);
            parametrosCrear.Add("FechaVencimiento", fechaVencimientoComoString);

            string respuestaFormatoJsonCrear = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_CREAR, parametrosCrear);
            var tareaCreada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonCrear);

            var nuevaFechaVencimiento = DateTime.Now;

            var parametrosActualizar = new Dictionary<string, string>();
            parametrosActualizar.Add("Id", Convert.ToString(tareaCreada.Id));
            parametrosActualizar.Add("Descripcion", tareaCreada.Descripcion);
            parametrosActualizar.Add("EstadoTarea", tareaCreada.EstadoTarea);
            parametrosActualizar.Add("FechaCreacion", ObtenerFechaAjustadaComoString(tareaCreada.FechaCreacion));
            parametrosActualizar.Add("FechaVencimiento", ObtenerFechaAjustadaComoString(nuevaFechaVencimiento));

            string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_ACTUALIZAR, parametrosActualizar);
            var tareaActualizada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonActualizar);

            Assert.IsTrue(tareaCreada.Id > 0, $"tareasVM.Id : {tareaCreada.Id}");
            Assert.AreEqual(tareaCreada.Id, tareaActualizada.Id);
            Assert.AreEqual(tareaCreada.Descripcion, tareaActualizada.Descripcion);
            Assert.AreNotEqual(tareaCreada.FechaVencimiento, tareaActualizada.FechaVencimiento);
            Assert.AreEqual(tareaCreada.Autor, nombreUsuario);

        }

        [TestMethod]
        public void TestWSBorrar_LlamadoExitoso()
        {
            string nombreUsuario = "a@a.com";

            string estado = "Si";
            var fechaCreacion = DateTime.Now;
            var fechaVencimiento = fechaCreacion.AddDays(7);
            string descripcion = $"Algo interesante pasó el {fechaCreacion}";

            string fechaCreacionComoString = ObtenerFechaAjustadaComoString(fechaCreacion);
            string fechaVencimientoComoString = ObtenerFechaAjustadaComoString(fechaVencimiento);

            var parametrosCrear = new Dictionary<string, string>();
            parametrosCrear.Add("Descripcion", descripcion);
            parametrosCrear.Add("EstadoTarea", estado);
            parametrosCrear.Add("FechaCreacion", fechaCreacionComoString);
            parametrosCrear.Add("FechaVencimiento", fechaVencimientoComoString);

            string respuestaFormatoJsonCrear = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_CREAR, parametrosCrear);
            var tareaCreada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonCrear);

            var parametrosBorrar = new Dictionary<string, string>();
            parametrosBorrar.Add("Id", Convert.ToString(tareaCreada.Id));
            string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_BORRAR, parametrosBorrar);

            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}");
            var tareas = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJson);

            Assert.IsFalse(tareas.Any(item=>item.Id == tareaCreada.Id));
        }
    }
}
