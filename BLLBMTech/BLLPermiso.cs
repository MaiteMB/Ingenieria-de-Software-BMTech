using System;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;

namespace mb506.BLLBMTech
{
    public class BLLPermiso
    {
        public void mb506ValidarUsuarioActivo()
        {
            if (!SessionManager.IsSessionActive) throw new Exception("Debe iniciar sesion.");
            if (!new DALIntegridad().mb506VerificarTablas("Usuario"))
                throw new Exception("La integridad de Usuario presenta diferencias.");
            mb506.BEBMTech.Usuario.Usuario usuario = new DALUsuario().mb506BuscarUsuario(SessionManager.getSession.Usuario.email);
            if (usuario == null || !usuario.activo) throw new Exception("El usuario no esta activo.");
        }
        public void mb506Validar(string patente)
        {
            if (!SessionManager.IsSessionActive) throw new Exception("Debe iniciar sesion.");
            if (!new DALIntegridad().mb506VerificarTablas("Usuario", "Perfil", "Familia", "Patente", "FamiliaPatente", "PerfilFamilia"))
                throw new Exception("La integridad de Seguridad presenta diferencias.");
            mb506.BEBMTech.Usuario.Usuario usuario = new DALUsuario().mb506BuscarUsuario(SessionManager.getSession.Usuario.email);
            if (usuario == null || !usuario.activo) throw new Exception("El usuario no esta activo.");
            SessionManager.getSession.Usuario.perfil = usuario.perfil;
            if (!new BLLRol().mb506ObtenerPatentesPorPerfil(usuario.perfil).Contains(patente))
                throw new Exception("No tiene permiso para esta operacion.");
        }
    }
}
