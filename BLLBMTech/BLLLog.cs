using mb506.BEBMTech.Log;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;
using System.Collections.Generic;

namespace mb506.BLLBMTech
{
    public class BLLLog
    {
        public static event System.Action<string> errorRegistro;
        private readonly DALLog dalLog;

        public BLLLog()
        {
            dalLog = new DALLog();
        }

        public bool mb506RegistrarEvento(string accion, string modulo, int criticidad)
        {
            string email = mb506ObtenerUsuarioActual();
            return mb506RegistrarEvento(email, accion, modulo, criticidad);
        }

        public bool mb506RegistrarEvento(string email, string accion, string modulo, int criticidad)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                email = "sistema@bmtech.com";
            }

            try { return dalLog.mb506RegistrarEvento(email, accion, modulo, criticidad); }
            catch (System.Exception ex)
            {
                if (errorRegistro != null) errorRegistro("La operacion puede haberse guardado, pero fallo la bitacora: " + ex.Message);
                return false;
            }
        }

        public List<Evento> mb506ObtenerEventos()
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            return dalLog.mb506ObtenerEventos();
        }

        public List<Evento> mb506ObtenerEventos(System.DateTime desde, System.DateTime hasta, string usuario, string accion)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (desde.Date > hasta.Date) throw new System.Exception("El rango de fechas es incorrecto.");
            return dalLog.mb506ObtenerEventos(desde, hasta, usuario.Trim(), accion.Trim());
        }

        private string mb506ObtenerUsuarioActual()
        {
            if (SessionManager.getSession != null && SessionManager.getSession.Usuario != null)
            {
                return SessionManager.getSession.Usuario.email;
            }

            return "sistema@bmtech.com";
        }
    }
}
