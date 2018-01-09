using Ci2.PI.Aplicacion.Repositorios;
using Ci2.PI.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci2.PI.Aplicacion
{
    public class UnidadDeTrabajo : IDisposable
    {
        private Ci2PIBDEntidades contextoBD;

        #region Propiedades y atributos de repositorio
        private TareRepositorio tareaRepositorio;

        public TareRepositorio TareaRepositorio
        {
            get { return tareaRepositorio; }
            set { tareaRepositorio = value; }
        }

        public UsuarioRepositorio UsuarioRepositorio { get; set; }
        #endregion

        #region Constructores
        public UnidadDeTrabajo(Ci2PIBDEntidades contextoBD)
        {
            if (contextoBD == null)
            {
                throw new ArgumentNullException("contextoBD no puede ser nulo");
            }
            this.contextoBD = contextoBD;
            TareaRepositorio = new TareRepositorio(contextoBD);
            UsuarioRepositorio = new UsuarioRepositorio(contextoBD);

        }

        public UnidadDeTrabajo() : this(new Ci2PIBDEntidades())
        {
        }
        #endregion

        #region transacciones        
        public void BeginTransaction()
        {

            if (contextoBD.Database.CurrentTransaction == null)
            {
                contextoBD.Database.BeginTransaction();
            }

        }

        private void Commit()
        {
            if (contextoBD.Database.CurrentTransaction != null)
            {
                contextoBD.Database.CurrentTransaction.Commit();
            }
        }

        public void Rollback()
        {
            if (contextoBD.Database.CurrentTransaction != null)
            {
                contextoBD.Database.CurrentTransaction.Rollback();
            }
        }
        #endregion

        public void Guardar()
        {
            contextoBD.SaveChanges();
            Commit();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (contextoBD.Database.CurrentTransaction != null)
                    {
                        contextoBD.Database.CurrentTransaction.Dispose();
                    }
                    contextoBD.Dispose();                    
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnidadDeTrabajo() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
