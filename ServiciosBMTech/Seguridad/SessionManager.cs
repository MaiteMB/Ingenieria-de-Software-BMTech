using mb506.BEBMTech.Usuario;
using System;

namespace mb506.ServiciosBMTech.Seguridad
{
    public class SessionManager
    {
        private static readonly Lazy<SessionManager> instance = new Lazy<SessionManager>(() => new SessionManager());
        private static readonly object lockObject = new object();

        public Usuario Usuario { get; private set; }

        private SessionManager() { }

        public static SessionManager getSession
        {
            get { return instance.Value; }
        }

        public static bool IsSessionActive
        {
            get { return getSession.Usuario != null; }
        }

        public static void mb506Login(Usuario usuario)
        {
            if (usuario == null || !usuario.activo || string.IsNullOrWhiteSpace(usuario.email))
                throw new Exception("Debe indicar un usuario activo para iniciar sesion.");
            lock (lockObject)
            {
                if (getSession.Usuario != null)
                {
                    throw new Exception("Ya hay una sesión iniciada.");
                }

                getSession.Usuario = usuario;
            }
        }

        public static void mb506Logout()
        {
            lock (lockObject)
            {
                getSession.Usuario = null;
            }
        }
    }
}
