using BEBMTech.Usuario;
using DALBMTech;
using ServiciosBMTech.Seguridad;
using System;
using System.Collections.Generic;

namespace BLLBMTech
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

        public Usuario Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Debe ingresar email y contraseña.");
            }

            Usuario usuario = dalUsuario.BuscarUsuario(email.Trim());

            if (usuario == null)
            {
                throw new Exception("Credenciales incorrectas.");
            }

            if (!usuario.activo)
            {
                throw new Exception("El usuario se encuentra bloqueado.");
            }

            bool passwordCorrecta = hashPassword.VerificarPassword(password, usuario.GetPassword());

            if (!passwordCorrecta)
            {
                usuario.intentos--;

                if (usuario.intentos <= 0)
                {
                    usuario.intentos = 0;
                    usuario.activo = false;
                    dalUsuario.ActualizarIntentosYEstado(usuario);
                    throw new Exception("Usuario bloqueado por superar los intentos permitidos.");
                }

                dalUsuario.ActualizarIntentosYEstado(usuario);
                throw new Exception("Contraseña incorrecta. Intentos restantes: " + usuario.intentos);
            }

            usuario.intentos = 3;
            usuario.activo = true;
            dalUsuario.ActualizarIntentosYEstado(usuario);

            return usuario;
        }

        public bool RegistrarUsuario(Usuario usuario, string password)
        {
            ValidarUsuario(usuario, password);

            Usuario usuarioExistente = dalUsuario.BuscarUsuario(usuario.email);

            if (usuarioExistente != null)
            {
                throw new Exception("Ya existe un usuario registrado con ese email.");
            }

            usuario.SetPassword(hashPassword.GenerarHash(password));
            usuario.activo = true;
            usuario.intentos = 3;

            return dalUsuario.InsertarUsuario(usuario);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return dalUsuario.ObtenerUsuarios();
        }

        private void ValidarUsuario(Usuario usuario, string password)
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
        }
    }
}

