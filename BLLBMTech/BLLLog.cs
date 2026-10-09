using BEBMTech.Log;
using DALBMTech;
using ServiciosBMTech.Seguridad;
using System.Collections.Generic;

namespace BLLBMTech
{
    public class BLLLog
    {
        private readonly DALLog dalLog;

        public BLLLog()
        {
            dalLog = new DALLog();
        }

        public bool RegistrarEvento(string accion, string modulo, int criticidad)
        {
            string email = ObtenerUsuarioActual();
            return dalLog.RegistrarEvento(email, accion, modulo, criticidad);
        }

        public bool RegistrarEvento(string email, string accion, string modulo, int criticidad)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                email = "sistema@bmtech.com";
            }

            return dalLog.RegistrarEvento(email, accion, modulo, criticidad);
        }

        public List<Evento> ObtenerEventos()
        {
            return dalLog.ObtenerEventos();
        }

        private string ObtenerUsuarioActual()
        {
            if (SessionManager.getSession != null && SessionManager.getSession.Usuario != null)
            {
                return SessionManager.getSession.Usuario.email;
            }

            return "sistema@bmtech.com";
        }
    }
}
