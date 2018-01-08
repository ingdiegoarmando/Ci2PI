using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci2.PI.Aplicacion.Excepciones
{
    [System.Serializable]
    public class TareaNoAutorizadaException : Exception
    {
        public TareaNoAutorizadaException() { }

        public TareaNoAutorizadaException(string message) : base(message) { }

        public TareaNoAutorizadaException(string message, Exception inner) : base(message, inner) { }

        protected TareaNoAutorizadaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
