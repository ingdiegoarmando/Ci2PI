using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace Ci2.PI.ServicioWeb.Infraestructura.Binders
{

    public class NombreDeUsuarioActualValueProvider : IValueProvider
    {
        public const string NOMBRE_CAMPO_USUARIO_ACTUAL = "NombreDeUsuarioActual";

        private string nombreDeUsuarioActual;

        public NombreDeUsuarioActualValueProvider(HttpActionContext actionContext)
        {
            //Este código es temporal y debe ser eliminado una vez se implemente sistema de autenticación.
            nombreDeUsuarioActual = "a@a.com";
        }

        public bool EsUnCampoValido(string campo) {

            return NOMBRE_CAMPO_USUARIO_ACTUAL.ToUpper().Equals((campo ?? "").ToUpper());
        }

        public bool ContainsPrefix(string prefix)
        {
            return EsUnCampoValido(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            if (EsUnCampoValido(key))
            {
                return new ValueProviderResult(nombreDeUsuarioActual, nombreDeUsuarioActual, CultureInfo.InvariantCulture);
            }

            return null;
        }
    }

    public class NombreDeUsuarioActualValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new NombreDeUsuarioActualValueProvider(actionContext);
        }
    }
}