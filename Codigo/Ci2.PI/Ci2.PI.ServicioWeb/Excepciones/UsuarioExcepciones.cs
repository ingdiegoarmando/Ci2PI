using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ci2.PI.ServicioWeb.Excepciones
{
    [Serializable]
    public class ContraseñaInvalidaException : Exception
    {
        public ContraseñaInvalidaException() { }
        public ContraseñaInvalidaException(string message) : base(message) { }
        public ContraseñaInvalidaException(string message, Exception inner) : base(message, inner) { }
        protected ContraseñaInvalidaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class CorreoElectronicoInvalidoException : Exception
    {
        public CorreoElectronicoInvalidoException() { }
        public CorreoElectronicoInvalidoException(string message) : base(message) { }
        public CorreoElectronicoInvalidoException(string message, Exception inner) : base(message, inner) { }
        protected CorreoElectronicoInvalidoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}