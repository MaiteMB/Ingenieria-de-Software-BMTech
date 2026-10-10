using mb506.BEBMTech.Usuario;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;
using System;
using System.Collections.Generic;

namespace mb506.BLLBMTech
{
    public class BLLUsuario
    {
        private readonly DALUsuario dalUsuario;
        private readonly HashPassword hashPassword;
        private readonly BLLLog bllLog;

        public BLLUsuario()
        {
            dalUsuario = new DALUsuario();
            hashPassword = new HashPassword();
            bllLog = new BLLLog();
        }

        public Usuario mb506Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Debe ingresar email y contraseña.");
            }

            if (!new DALIntegridad().mb506VerificarTablas("Usuario", "Perfil", "Familia", "Patente", "FamiliaPatente", "PerfilFamilia"))
                throw new Exception("La integridad de Seguridad no esta preparada o presenta diferencias.");
            Usuario usuario = dalUsuario.mb506BuscarUsuario(email.Trim());

            if (usuario == null)
            {
                throw new Exception("Credenciales incorrectas.");
            }

            if (!usuario.activo)
            {
                throw new Exception("El usuario se encuentra bloqueado.");
            }

            bool passwordCorrecta = hashPassword.mb506VerificarPassword(password, usuario.mb506GetPassword());

            if (!passwordCorrecta)
            {
                usuario.intentos--;

                if (usuario.intentos <= 0)
                {
                    usuario.intentos = 0;
                    usuario.activo = false;
                    dalUsuario.mb506ActualizarIntentosYEstado(usuario);
                    bllLog.mb506RegistrarEvento(usuario.email, "Usuario bloqueado por intentos", "Acceso", 3);
                    throw new Exception("Usuario bloqueado por superar los intentos permitidos.");
                }

                dalUsuario.mb506ActualizarIntentosYEstado(usuario);
                throw new Exception("Contraseña incorrecta. Intentos restantes: " + usuario.intentos);
            }

            usuario.intentos = 3;
            usuario.activo = true;
            dalUsuario.mb506ActualizarIntentosYEstado(usuario);

            return usuario;
        }

        public bool mb506RegistrarUsuario(Usuario usuario, string password)
        {
            mb506ValidarAdministrador();
            mb506ValidarUsuario(usuario, password);

            Usuario usuarioExistente = dalUsuario.mb506BuscarUsuario(usuario.email);

            if (usuarioExistente != null)
            {
                throw new Exception("Ya existe un usuario registrado con ese email.");
            }

            usuario.mb506SetPassword(hashPassword.mb506GenerarHash(password));
            usuario.activo = true;
            usuario.intentos = 3;

            bool registrado = dalUsuario.mb506InsertarUsuario(usuario);
            if (registrado) bllLog.mb506RegistrarEvento("Usuario registrado: " + usuario.email, "Usuarios", 2);
            return registrado;
        }

        public List<Usuario> mb506ObtenerUsuarios()
        {
            mb506ValidarAdministrador();
            return dalUsuario.mb506ObtenerUsuarios();
        }

        private void mb506ValidarAdministrador()
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
        }

        public void mb506InicializarIntegridad(string email, string password)
        {
            if (SessionManager.IsSessionActive) throw new Exception("Cierre la sesion antes de preparar integridad.");
            if (!new DALIntegridad().mb506NecesitaInicializacion())
                throw new Exception("La integridad ya fue preparada. Para regenerarla ingrese como administrador.");
            Usuario usuario = dalUsuario.mb506BuscarUsuario(email.Trim());
            if (usuario == null || !usuario.activo ||
                !hashPassword.mb506VerificarPassword(password, usuario.mb506GetPassword()) ||
                !new BLLRol().mb506ObtenerPatentesPorPerfil(usuario.perfil).Contains("SEGURIDAD"))
                throw new Exception("Ingrese las credenciales de un administrador activo.");
            new DALIntegridad().mb506RegenerarSistema();
            bllLog.mb506RegistrarEvento(usuario.email, "Preparacion de integridad", "Seguridad", 3);
        }

        public List<string> mb506ObtenerPerfiles()
        {
            mb506ValidarAdministrador();
            return dalUsuario.mb506ObtenerPerfiles();
        }

        public bool mb506ModificarUsuario(Usuario usuario)
        {
            mb506ValidarAdministrador();
            mb506ValidarUsuario(usuario, "sin cambio");
            if (string.Equals(usuario.email, SessionManager.getSession.Usuario.email, StringComparison.OrdinalIgnoreCase) &&
                usuario.perfil != SessionManager.getSession.Usuario.perfil)
                throw new Exception("No puede cambiar su propio perfil durante la sesion.");
            bool modificado = dalUsuario.mb506ModificarUsuario(usuario);
            if (modificado) bllLog.mb506RegistrarEvento("Usuario modificado: " + usuario.email, "Usuarios", 2);
            return modificado;
        }

        public void mb506CambiarEstado(string email, bool activo)
        {
            mb506ValidarAdministrador();
            if (string.Equals(email, SessionManager.getSession.Usuario.email, StringComparison.OrdinalIgnoreCase))
                throw new Exception("No puede bloquear su propia cuenta.");
            Usuario usuario = dalUsuario.mb506BuscarUsuario(email);
            if (usuario == null) throw new Exception("Usuario no encontrado.");
            usuario.activo = activo;
            usuario.intentos = activo ? 3 : 0;
            if (!dalUsuario.mb506ActualizarIntentosYEstado(usuario))
                throw new Exception("No se pudo actualizar el usuario.");
            bllLog.mb506RegistrarEvento((activo ? "Desbloqueo: " : "Bloqueo: ") + email, "Usuarios", 2);
        }

        public void mb506CambiarPassword(string actual, string nueva)
        {
            if (!SessionManager.IsSessionActive) throw new Exception("Debe iniciar sesion.");
            Usuario usuario = dalUsuario.mb506BuscarUsuario(SessionManager.getSession.Usuario.email);
            if (!hashPassword.mb506VerificarPassword(actual, usuario.mb506GetPassword()))
                throw new Exception("La clave actual es incorrecta.");
            if (string.IsNullOrWhiteSpace(nueva)) throw new Exception("Ingrese la nueva clave.");
            if (System.Text.Encoding.UTF8.GetByteCount(nueva) > 72) throw new Exception("La clave es demasiado larga.");
            new BLLPermiso().mb506ValidarUsuarioActivo();
            if (!dalUsuario.mb506CambiarPassword(usuario.email, hashPassword.mb506GenerarHash(nueva)))
                throw new Exception("No se pudo cambiar la clave.");
            bllLog.mb506RegistrarEvento("Cambio de contraseña", "Usuarios", 2);
        }

        private void mb506ValidarUsuario(Usuario usuario, string password)
        {
            if (usuario == null)
            {
                throw new Exception("Debe ingresar los datos del usuario.");
            }

            if (string.IsNullOrWhiteSpace(usuario.email))
            {
                throw new Exception("Debe ingresar el email del usuario.");
            }

            if (string.IsNullOrWhiteSpace(usuario.nombre))
            {
                throw new Exception("Debe ingresar el nombre del usuario.");
            }

            if (string.IsNullOrWhiteSpace(usuario.apellido))
            {
                throw new Exception("Debe ingresar el apellido del usuario.");
            }

            if (string.IsNullOrWhiteSpace(usuario.perfil))
            {
                throw new Exception("Debe ingresar el perfil del usuario.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Debe ingresar la contraseña del usuario.");
            }
            if (System.Text.Encoding.UTF8.GetByteCount(password) > 72)
                throw new Exception("La clave es demasiado larga.");
            if (usuario.email.Length > 150 || usuario.nombre.Length > 100 ||
                usuario.apellido.Length > 100 || usuario.perfil.Length > 50)
                throw new Exception("Los datos del usuario superan el largo permitido.");
            if (!dalUsuario.mb506ObtenerPerfiles().Contains(usuario.perfil))
                throw new Exception("Seleccione un perfil existente.");
        }
    }
}

