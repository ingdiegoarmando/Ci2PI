using Ci2.PI.Aplicacion.Excepciones;
using Ci2.PI.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci2.PI.Aplicacion.Repositorios
{
    public class UsuarioRepositorio : IRepositorio<TabUsuario>
    {
        private Ci2PIBDEntidades ContextoBD { get; set; }

        public UsuarioRepositorio(Ci2PIBDEntidades contextoBD)
        {
            this.ContextoBD = contextoBD;
        }
        #region Impelmentacion de IRepositorio<TabUsuario>
        public void AgregarOActualizar(TabUsuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Ci2UsuarioId))
            {
                if (Listar().Any(item => item.Ci2NombreUsuario == usuario.Ci2NombreUsuario))
                {
                    throw new NombreDeUsuarioEnUsoException() { NombreDeUsuarioEnUso = usuario.Ci2NombreUsuario };
                }
            }

            string id = ContextoBD.PraTabUsuarioAgregarOActualizar(usuario.Ci2UsuarioId, usuario.Ci2CorreoElectronico, usuario.Ci2CorreoElectronicoConfirmado, usuario.Ci2PasswordHash, usuario.Ci2SecurityStamp, usuario.Ci2NumeroTelefonico, usuario.Ci2NumeroTelefonicoConfirmado, usuario.Ci2AutenticacionDosEtapasActivado, usuario.Ci2FechaBloqueoUtc, usuario.Ci2BloqueoActivado, usuario.Ci2NumeroAccesosFallidos, usuario.Ci2NombreUsuario).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(usuario.Ci2UsuarioId))
            {
                usuario.Ci2UsuarioId = id;
            }
        }

        public TabUsuario ConsultarPorId(params object[] id)
        {
            string idComoString = Convert.ToString(id[0]);
            var resultadoBD = ContextoBD.PraTabUsuarioConsultarPorId(idComoString).SingleOrDefault();
            if (resultadoBD != null)
            {
                return new TabUsuario()
                {
                    Ci2AutenticacionDosEtapasActivado = resultadoBD.Ci2AutenticacionDosEtapasActivado,
                    Ci2BloqueoActivado = resultadoBD.Ci2BloqueoActivado,
                    Ci2CorreoElectronico = resultadoBD.Ci2CorreoElectronico,
                    Ci2CorreoElectronicoConfirmado = resultadoBD.Ci2CorreoElectronicoConfirmado,
                    Ci2FechaBloqueoUtc = resultadoBD.Ci2FechaBloqueoUtc,
                    Ci2NombreUsuario = resultadoBD.Ci2NombreUsuario,
                    Ci2NumeroAccesosFallidos = resultadoBD.Ci2NumeroAccesosFallidos,
                    Ci2NumeroTelefonico = resultadoBD.Ci2NumeroTelefonico,
                    Ci2NumeroTelefonicoConfirmado = resultadoBD.Ci2NumeroTelefonicoConfirmado,
                    Ci2PasswordHash = resultadoBD.Ci2PasswordHash,
                    Ci2SecurityStamp = resultadoBD.Ci2SecurityStamp,
                    Ci2UsuarioId = resultadoBD.Ci2UsuarioId,
                };
            }
            else
            {
                return null;
            }
        }

        public TabUsuario ConsultarPorNombre(string nombreDeUsuario)
        {
            var usuarios = Listar();

            return usuarios.FirstOrDefault(item => item.Ci2NombreUsuario.ToUpper() == (nombreDeUsuario ?? "").ToUpper());
        }

        public void Eliminar(params object[] id)
        {
            string idComoString = Convert.ToString(id[0]);
            ContextoBD.PraTabUsuarioEliminar(idComoString);
        }

        public IEnumerable<TabUsuario> Listar()
        {
            var resultadoBD = ContextoBD.PraTabUsuarioListar().ToList();
            var listado = new List<TabUsuario>();
            if (resultadoBD != null && resultadoBD.Count > 0)
            {
                listado = resultadoBD.Select(item => new TabUsuario()
                {
                    Ci2AutenticacionDosEtapasActivado = item.Ci2AutenticacionDosEtapasActivado,
                    Ci2BloqueoActivado = item.Ci2BloqueoActivado,
                    Ci2CorreoElectronico = item.Ci2CorreoElectronico,
                    Ci2CorreoElectronicoConfirmado = item.Ci2CorreoElectronicoConfirmado,
                    Ci2FechaBloqueoUtc = item.Ci2FechaBloqueoUtc,
                    Ci2NombreUsuario = item.Ci2NombreUsuario,
                    Ci2NumeroAccesosFallidos = item.Ci2NumeroAccesosFallidos,
                    Ci2NumeroTelefonico = item.Ci2NumeroTelefonico,
                    Ci2NumeroTelefonicoConfirmado = item.Ci2NumeroTelefonicoConfirmado,
                    Ci2PasswordHash = item.Ci2PasswordHash,
                    Ci2SecurityStamp = item.Ci2SecurityStamp,
                    Ci2UsuarioId = item.Ci2UsuarioId,
                }).ToList();
            }

            return listado;
        }


        #endregion
        public void CambiarPasswordHashPorIdUsuario(string usuarioId, string passwordHash)
        {
            var usuarioBD = ConsultarPorId(usuarioId);

            usuarioBD.Ci2PasswordHash = passwordHash;

            AgregarOActualizar(usuarioBD);
        }

        public void ActualizarSinModifcarContraseñaYSecurityStamp(TabUsuario usuario)
        {
            var usuarioActual = ConsultarPorId(usuario.Ci2UsuarioId);

            usuarioActual.Ci2AutenticacionDosEtapasActivado = usuario.Ci2AutenticacionDosEtapasActivado;
            usuarioActual.Ci2BloqueoActivado = usuario.Ci2BloqueoActivado;
            usuarioActual.Ci2CorreoElectronico = usuario.Ci2CorreoElectronico;
            usuarioActual.Ci2CorreoElectronicoConfirmado = usuario.Ci2CorreoElectronicoConfirmado;
            usuarioActual.Ci2FechaBloqueoUtc = usuario.Ci2FechaBloqueoUtc;            
            usuarioActual.Ci2NumeroAccesosFallidos = usuario.Ci2NumeroAccesosFallidos;
            usuarioActual.Ci2NumeroTelefonico = usuario.Ci2NumeroTelefonico;
            usuarioActual.Ci2NumeroTelefonicoConfirmado = usuario.Ci2NumeroTelefonicoConfirmado;            

            AgregarOActualizar(usuarioActual);
        }
    }
}
