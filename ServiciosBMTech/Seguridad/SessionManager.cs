using BEBMTech.Usuario;
using System;

namespace ServiciosBMTech.Seguridad
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

        public static void Login(Usuario usuario)
        {
            lock (lockObject)
            {
                if (getSession.Usuario != null)
                {
                    throw new Exception("Ya hay una sesión iniciada.");
                }

                getSession.Usuario = usuario;
            }
        }

        public static void Logout()
        {
            lock (lockObject)
            {
                getSession.Usuario = null;
            }
        }
    }
}
