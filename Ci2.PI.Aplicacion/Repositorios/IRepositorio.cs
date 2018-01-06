using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci2.PI.Aplicacion.Repositorios
{
    public interface IRepositorio<T>
    {
        void AgregarOActualizar(T entidad);        
        void Eliminar(params object[] id);        
        IEnumerable<T> Listar();
        T ConsultarPorId(params object[] id);
    }
}
