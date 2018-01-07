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

    public class UsuarioActualValueProvider : IValueProvider
    {
        public const string PREFIJO_USUARIO_ACTUAL = "UsuarioActual";
        public const string NOMBRE_CAMPO_USUARIO_ACTUAL = "NombreDeUsuarioActual";
        public const string ID_CAMPO_USUARIO_ACTUAL = "IdDeUsuarioActual";

        private string nombreDeUsuarioActual;
        private string idDeUsuarioActual;

        public UsuarioActualValueProvider(HttpActionContext actionContext)
        {
            //Este código es temporal y debe ser eliminado una vez se implemente sistema de autenticación.
            nombreDeUsuarioActual = "a@a.com";
            idDeUsuarioActual = "852c89ca-b7a8-4423-84af-2f6fac9a4004";
        }

        public bool EsUnCampoValido(string campo)
        {

            return (PREFIJO_USUARIO_ACTUAL.ToUpper() + "."+NOMBRE_CAMPO_USUARIO_ACTUAL.ToUpper()).Equals((campo ?? "").ToUpper()) ||
               (PREFIJO_USUARIO_ACTUAL.ToUpper() + "." + ID_CAMPO_USUARIO_ACTUAL.ToUpper()).Equals((campo ?? "").ToUpper());
        }

        public bool ContainsPrefix(string prefix)
        {
            return PREFIJO_USUARIO_ACTUAL.ToUpper()== (prefix??"").ToUpper();
        }

        public ValueProviderResult GetValue(string key)
        {
            if (EsUnCampoValido(key))
            {
                if (key.ToUpper() == (PREFIJO_USUARIO_ACTUAL.ToUpper() + "." + NOMBRE_CAMPO_USUARIO_ACTUAL.ToUpper()))
                {
                    return new ValueProviderResult(nombreDeUsuarioActual, nombreDeUsuarioActual, CultureInfo.InvariantCulture);
                }
                else if(key.ToUpper() == (PREFIJO_USUARIO_ACTUAL.ToUpper() + "." + ID_CAMPO_USUARIO_ACTUAL.ToUpper()))
                {
                    return new ValueProviderResult(idDeUsuarioActual, idDeUsuarioActual, CultureInfo.InvariantCulture);
                }
            }
            
            return null;
        }
    }

    public class UsuarioActualValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new UsuarioActualValueProvider(actionContext);
        }
    }
}