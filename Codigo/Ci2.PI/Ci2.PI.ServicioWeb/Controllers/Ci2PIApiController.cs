using Ci2.PI.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Ci2.PI.ServicioWeb.Controllers
{
    public class Ci2PIApiController: ApiController
    {
        protected UnidadDeTrabajo UnidadDeTrabajo = new UnidadDeTrabajo();

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
    }
}