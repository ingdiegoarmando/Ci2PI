using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ci2.PI.Aplicacion.Excepciones
{
    [Serializable]
    public class NombreDeUsuarioEnUsoException : Exception
    {
        public string NombreDeUsuarioEnUso { get; set; }

        public NombreDeUsuarioEnUsoException() { }
        public NombreDeUsuarioEnUsoException(string message) : base(message) { }
        public NombreDeUsuarioEnUsoException(string message, Exception inner) : base(message, inner) { }
        protected NombreDeUsuarioEnUsoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}