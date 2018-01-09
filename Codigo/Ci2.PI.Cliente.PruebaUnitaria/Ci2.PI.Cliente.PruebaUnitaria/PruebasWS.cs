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
        public const string URL_WS_BASE = "http://localhost:50089";

        public const string URL_WS_TAREA_BASE = URL_WS_BASE + "/tareas";

        public const string URL_BASE_TOKEN = URL_WS_BASE + "/token";

        public const string URL_BASE_CONSULTAR = URL_WS_TAREA_BASE + "/consultar";

        public const string URL_BASE_CREAR = URL_WS_TAREA_BASE + "/crear";

        public const string URL_BASE_ACTUALIZAR = URL_WS_TAREA_BASE + "/actualizar";

        public const string URL_BASE_BORRAR = URL_WS_TAREA_BASE + "/borrar";

        public const string NOMBRE_USUARIO_POR_DEFECTO_1 = "aa";

        public const string NOMBRE_USUARIO_POR_DEFECTO_ALTERNATIVO = "bb";

        public string ObtenerTokenUsuarioPorDefecto()
        {
            return ObtenerToken(NOMBRE_USUARIO_POR_DEFECTO_1, "!Qaz12");
        }

        public string ObtenerToken(string nombreUsuario, string contraseña)
        {
            string resultado = null;

            using (var cliente = new WebClient())
            {
                cliente.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                string tokenComoString = cliente.UploadString(URL_BASE_TOKEN, "POST", $"grant_type=password&username={nombreUsuario}&password={contraseña}");

                var tokenComoDictionary = ObtenerObjetoDesdeUnTextoConFormatoJson<Dictionary<string, string>>(tokenComoString);

                if (tokenComoDictionary != null && tokenComoDictionary.ContainsKey("access_token"))
                {
                    resultado = tokenComoDictionary["access_token"];
                }
            }

            return resultado;
        }

        public string ObtenerTextoFormatoJsonDeLaUrl(string url, string token = null)
        {
            string respuesta = null;

            using (var cliente = new WebClient())
            {
                cliente.Headers.Add("Content-Type", "application/json; charset=utf-8");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    cliente.Headers.Add("Authorization", "Bearer " + token);
                }

                respuesta = cliente.DownloadString(url);
            }

            return respuesta;
        }

        private string ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto(string url)
        {
            return ObtenerTextoFormatoJsonDeLaUrl(url, ObtenerTokenUsuarioPorDefecto());
        }

        public T ObtenerObjetoDesdeUnTextoConFormatoJson<T>(string respuestaFormatoJson)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(respuestaFormatoJson);
        }

        [TestMethod]
        public void TestWSToken_LlamadoExitoso()
        {
            string token = ObtenerTokenUsuarioPorDefecto();
            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }

        [TestMethod]
        public void TestWSConsultar_SinFiltrosSinAutenticacion_LlamadoFallido()
        {
            try
            {
                string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrl($"{URL_BASE_CONSULTAR}");
                Assert.Fail();
            }
            catch (System.Net.WebException ex)
            {
                var respuesta = ex.Response as HttpWebResponse;
                if (respuesta == null || respuesta.StatusCode != HttpStatusCode.Unauthorized)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestWSConsultar_SinFiltros_LlamadoExitoso()
        {
            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}");
            var tareas = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJson);
            foreach (var tarea in tareas)
            {
                //Por defecto solo se traen las tareas propias
                Assert.IsTrue(NOMBRE_USUARIO_POR_DEFECTO_1.Equals(tarea.Autor), tarea.Autor);
            }
        }

        [TestMethod]
        public void TestWSConsultar_ConFiltroAutorPropioEstadoTodosOrdenarFechaCreacionNoOrdenar_LlamadoExitoso()
        {
            string respuestaFormatoJsonConsulta1 = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}");
            var tareasConsulta1 = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJsonConsulta1);

            string respuestaFormatoJsonConsulta2 = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}?Autoria=TodosLosAutores");
            var tareasConsulta2 = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJsonConsulta2);

            Assert.IsTrue(tareasConsulta1.Count() <= tareasConsulta2.Count());
        }

        [TestMethod]
        public void TestWSConsultar_ConFiltrosTareasFinalizadasDeUnUsuario_LlamadoExitoso()
        {
            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}?Autoria=Propia&Estado=Finalizadas");
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
            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}?Autoria=Propia&Estado=Pendientes");
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
            string respuestaFormatoJsonConsulta1 = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}?Autoria=Propia&OrdenarFechaVencimiento=Asc");
            var tareasConsulta1 = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJsonConsulta1);

            string respuestaFormatoJsonConsulta2 = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}?Autoria=Propia&OrdenarFechaVencimiento=Des");
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

        public string ObtenerTextoFormatoJsonDeAccederALaUrlConPost(string url, Dictionary<string, string> parametros = null, string token = null)
        {
            string respuesta = null;

            using (var cliente = new WebClient())
            {
                cliente.Encoding = System.Text.Encoding.UTF8;
                cliente.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                cliente.Headers.Add("Accept:application/json");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    cliente.Headers.Add("Authorization", "Bearer " + token);
                }

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

        public string ObtenerTextoFormatoJsonDeAccederALaUrlConPostTokenPorDefecto(string url, Dictionary<string, string> parametros = null)
        {
            return ObtenerTextoFormatoJsonDeAccederALaUrlConPost(url, parametros, ObtenerTokenUsuarioPorDefecto());
        }

        public string ObtenerFechaAjustadaComoString(DateTime fecha)
        {
            return fecha.ToUniversalTime()
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        [TestMethod]
        public void TestWSCrear_LlamadoExitoso()
        {
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

            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeAccederALaUrlConPostTokenPorDefecto(URL_BASE_CREAR, parametros);
            var tarea = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJson);

            Assert.IsTrue(tarea.Id > 0, $"tarea.Id : {tarea.Id}");
            Assert.AreEqual(tarea.Descripcion, descripcion);
            Assert.AreEqual(ObtenerFechaAjustadaComoString(tarea.FechaVencimiento), ObtenerFechaAjustadaComoString(fechaVencimiento));
            Assert.AreEqual(tarea.Autor, NOMBRE_USUARIO_POR_DEFECTO_1);

        }

        [TestMethod]
        public void TestWSCrear_SinAutenticacion_LlamadoFallido()
        {
            try
            {
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
                Assert.Fail();

            }
            catch (System.Net.WebException ex)
            {
                var respuesta = ex.Response as HttpWebResponse;
                if (respuesta == null || respuesta.StatusCode != HttpStatusCode.Unauthorized)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestWSActualizar_LlamadoExitoso()
        {
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

            string respuestaFormatoJsonCrear = ObtenerTextoFormatoJsonDeAccederALaUrlConPostTokenPorDefecto(URL_BASE_CREAR, parametrosCrear);
            var tareaCreada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonCrear);

            var nuevaFechaVencimiento = DateTime.Now;

            var parametrosActualizar = new Dictionary<string, string>();
            parametrosActualizar.Add("Id", Convert.ToString(tareaCreada.Id));
            parametrosActualizar.Add("Descripcion", tareaCreada.Descripcion);
            parametrosActualizar.Add("EstadoTarea", tareaCreada.EstadoTarea);
            parametrosActualizar.Add("FechaCreacion", ObtenerFechaAjustadaComoString(tareaCreada.FechaCreacion));
            parametrosActualizar.Add("FechaVencimiento", ObtenerFechaAjustadaComoString(nuevaFechaVencimiento));

            string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPostTokenPorDefecto(URL_BASE_ACTUALIZAR, parametrosActualizar);
            var tareaActualizada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonActualizar);

            Assert.IsTrue(tareaCreada.Id > 0, $"tareasVM.Id : {tareaCreada.Id}");
            Assert.AreEqual(tareaCreada.Id, tareaActualizada.Id);
            Assert.AreEqual(tareaCreada.Descripcion, tareaActualizada.Descripcion);
            Assert.AreNotEqual(tareaCreada.FechaVencimiento, tareaActualizada.FechaVencimiento);
            Assert.AreEqual(tareaCreada.Autor, NOMBRE_USUARIO_POR_DEFECTO_1);

        }

        [TestMethod]
        public void TestWSActualizar_ActualizarTareaDeOtroUsuario_LlamadoFallido()
        {
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

            string nombreUsuario1 = NOMBRE_USUARIO_POR_DEFECTO_1;
            string token1 = ObtenerToken(nombreUsuario1, "!Qaz12");
            string respuestaFormatoJsonCrear = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_CREAR, parametrosCrear, token1);
            var tareaCreada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonCrear);

            Assert.AreEqual(tareaCreada.Autor, nombreUsuario1);

            var nuevaFechaVencimiento = DateTime.Now;

            var parametrosActualizar = new Dictionary<string, string>();
            parametrosActualizar.Add("Id", Convert.ToString(tareaCreada.Id));
            parametrosActualizar.Add("Descripcion", tareaCreada.Descripcion);
            parametrosActualizar.Add("EstadoTarea", tareaCreada.EstadoTarea);
            parametrosActualizar.Add("FechaCreacion", ObtenerFechaAjustadaComoString(tareaCreada.FechaCreacion));
            parametrosActualizar.Add("FechaVencimiento", ObtenerFechaAjustadaComoString(nuevaFechaVencimiento));

            string nombreUsuario2 = NOMBRE_USUARIO_POR_DEFECTO_ALTERNATIVO;
            string token2 = ObtenerToken(nombreUsuario2, "!Qaz12");
            try
            {
                string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_ACTUALIZAR, parametrosActualizar, token2);

                Assert.Fail();

            }
            catch (System.Net.WebException ex)
            {
                var respuesta = ex.Response as HttpWebResponse;
                if (respuesta == null || respuesta.StatusCode != HttpStatusCode.Unauthorized)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestWSActualizar_SinAutenticacion_LlamadoFallido()
        {
            try
            {
                string estado = "Si";
                var fechaCreacion = DateTime.Now;
                var fechaVencimiento = fechaCreacion.AddDays(7);
                string descripcion = $"Algo interesante pasó el {fechaCreacion}";

                string fechaCreacionComoString = ObtenerFechaAjustadaComoString(fechaCreacion);
                string fechaVencimientoComoString = ObtenerFechaAjustadaComoString(fechaVencimiento);

                var parametros = new Dictionary<string, string>();
                parametros.Add("Id", "1");
                parametros.Add("Descripcion", descripcion);
                parametros.Add("EstadoTarea", estado);
                parametros.Add("FechaCreacion", fechaCreacionComoString);
                parametros.Add("FechaVencimiento", fechaVencimientoComoString);

                string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_ACTUALIZAR, parametros);
                Assert.Fail();

            }
            catch (System.Net.WebException ex)
            {
                var respuesta = ex.Response as HttpWebResponse;
                if (respuesta == null || respuesta.StatusCode != HttpStatusCode.Unauthorized)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestWSBorrar_LlamadoExitoso()
        {
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

            string respuestaFormatoJsonCrear = ObtenerTextoFormatoJsonDeAccederALaUrlConPostTokenPorDefecto(URL_BASE_CREAR, parametrosCrear);
            var tareaCreada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonCrear);

            var parametrosBorrar = new Dictionary<string, string>();
            parametrosBorrar.Add("Id", Convert.ToString(tareaCreada.Id));
            string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPostTokenPorDefecto(URL_BASE_BORRAR, parametrosBorrar);

            string respuestaFormatoJson = ObtenerTextoFormatoJsonDeLaUrlConTokenPorDefecto($"{URL_BASE_CONSULTAR}");
            var tareas = ObtenerObjetoDesdeUnTextoConFormatoJson<IEnumerable<ResultadoWSConsultar>>(respuestaFormatoJson);

            Assert.IsFalse(tareas.Any(item => item.Id == tareaCreada.Id));
        }

        [TestMethod]
        public void TestWSBorrar_BorrarTareaDeOtroUsuario_LlamadoFallido()
        {
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

            string nombreUsuario1 = NOMBRE_USUARIO_POR_DEFECTO_1;
            string token1 = ObtenerToken(nombreUsuario1, "!Qaz12");

            string respuestaFormatoJsonCrear = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_CREAR, parametrosCrear, token1);
            var tareaCreada = ObtenerObjetoDesdeUnTextoConFormatoJson<ResultadoWSConsultar>(respuestaFormatoJsonCrear);

            string nombreUsuario2 = NOMBRE_USUARIO_POR_DEFECTO_ALTERNATIVO;
            string token2 = ObtenerToken(nombreUsuario2, "!Qaz12");

            var parametrosBorrar = new Dictionary<string, string>();
            parametrosBorrar.Add("Id", Convert.ToString(tareaCreada.Id));

            try
            {
                string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_BORRAR, parametrosBorrar, token2);

                Assert.Fail();

            }
            catch (System.Net.WebException ex)
            {
                var respuesta = ex.Response as HttpWebResponse;
                if (respuesta == null || respuesta.StatusCode != HttpStatusCode.Unauthorized)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void TestWSBorrar_SinAutenticacion_LlamadoFallido()
        {
            try
            {
                var parametros = new Dictionary<string, string>();
                parametros.Add("Id", "1");

                string respuestaFormatoJsonActualizar = ObtenerTextoFormatoJsonDeAccederALaUrlConPost(URL_BASE_BORRAR, parametros);
                Assert.Fail();

            }
            catch (System.Net.WebException ex)
            {
                var respuesta = ex.Response as HttpWebResponse;
                if (respuesta == null || respuesta.StatusCode != HttpStatusCode.Unauthorized)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
