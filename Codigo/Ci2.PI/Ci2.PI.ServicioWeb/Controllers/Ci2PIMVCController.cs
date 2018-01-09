using Ci2.PI.Aplicacion;
using Ci2.PI.ServicioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ci2.PI.ServicioWeb.Controllers
{
    public class Ci2PIMVCController : Controller
    {
        protected UnidadDeTrabajo UnidadDeTrabajo = new UnidadDeTrabajo();       

        protected ActionResult RedireccionLocal(string urlAnterior = "")
        {
            if (Url.IsLocalUrl(urlAnterior))
            {
                return Redirect(urlAnterior);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            finally
            {
                if (UnidadDeTrabajo != null)
                {
                    UnidadDeTrabajo.Dispose();
                }

            }
        }

        public List<CamposInvalidos> ErroresDelModeloComoCamposInvalidos
        {
            get
            {
                var resultado = new List<CamposInvalidos>();

                //var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var nombreDelCampo in ModelState.Keys)
                {
                    var mensajeDeErrorComoTexto = ModelState[nombreDelCampo].Errors;
                    foreach (var item in mensajeDeErrorComoTexto)
                    {
                        var mensajeDeError = new CamposInvalidos() { NombreDelCampo = nombreDelCampo, MensajeDeError = item.ErrorMessage };
                        resultado.Add(mensajeDeError);
                    }
                }

                return resultado;
            }
        }
    }
}