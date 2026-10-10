using mb506.BEBMTech.Usuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALUsuario
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALUsuario()
        {
            dbConnection = DAL_AccesoSQL.mb506GetInstance();
        }

        public Usuario mb506BuscarUsuario(string email)
        {
            string query = @"
                SELECT email, nombre, apellido, password, activo, intentos, idperfil
                FROM Usuario
                WHERE email = @email";

            try
            {
                using (SqlConnection conexion = dbConnection.mb506GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.Read() ? mb506MapUsuario(reader) : null;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al buscar el usuario.", ex);
            }
        }

        public bool mb506ActualizarIntentosYEstado(Usuario usuario)
        {
            string query = @"
                UPDATE Usuario
                SET intentos = @intentos,
                    activo = @activo
                WHERE email = @email";

            try
            {
                using (SqlConnection conexion = dbConnection.mb506GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = usuario.email;
                    command.Parameters.Add("@intentos", SqlDbType.Int).Value = usuario.intentos;
                    command.Parameters.Add("@activo", SqlDbType.Bit).Value = usuario.activo;

                    conexion.Open();

                    return new DALIntegridad().mb506EjecutarComando(command, "Usuario") > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al actualizar el usuario.", ex);
            }
        }

        public bool mb506InsertarUsuario(Usuario usuario)
        {
            string query = @"
                INSERT INTO Usuario
                (
                    email,
                    nombre,
                    apellido,
                    password,
                    activo,
                    intentos,
                    idperfil
                )
                VALUES
                (
                    @email,
                    @nombre,
                    @apellido,
                    @password,
                    @activo,
                    @intentos,
                    @idperfil
                )";

            try
            {
                using (SqlConnection conexion = dbConnection.mb506GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = usuario.email;
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = usuario.apellido;
                    command.Parameters.Add("@password", SqlDbType.VarChar).Value = usuario.mb506GetPassword();
                    command.Parameters.Add("@activo", SqlDbType.Bit).Value = usuario.activo;
                    command.Parameters.Add("@intentos", SqlDbType.Int).Value = usuario.intentos;
                    command.Parameters.Add("@idperfil", SqlDbType.VarChar).Value = usuario.perfil;

                    conexion.Open();

                    return new DALIntegridad().mb506EjecutarComando(command, "Usuario") > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al insertar el usuario.", ex);
            }
        }

        public List<Usuario> mb506ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string query = @"
                SELECT email, nombre, apellido, password, activo, intentos, idperfil
                FROM Usuario
                ORDER BY apellido, nombre";

            try
            {
                using (SqlConnection conexion = dbConnection.mb506GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(mb506MapUsuario(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al obtener usuarios.", ex);
            }

            return usuarios;
        }

        public bool mb506ModificarUsuario(Usuario usuario)
        {
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(
                "UPDATE Usuario SET nombre=@nombre, apellido=@apellido, idperfil=@perfil WHERE email=@email", conexion))
            {
                command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = usuario.nombre;
                command.Parameters.Add("@apellido", SqlDbType.VarChar, 100).Value = usuario.apellido;
                command.Parameters.Add("@perfil", SqlDbType.VarChar, 50).Value = usuario.perfil;
                command.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = usuario.email;
                conexion.Open();
                return new DALIntegridad().mb506EjecutarComando(command, "Usuario") == 1;
            }
        }

        public bool mb506CambiarPassword(string email, string password)
        {
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand("UPDATE Usuario SET password=@password WHERE email=@email", conexion))
            {
                command.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = password;
                command.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = email;
                conexion.Open();
                return new DALIntegridad().mb506EjecutarComando(command, "Usuario") == 1;
            }
        }

        public List<string> mb506ObtenerPerfiles()
        {
            List<string> perfiles = new List<string>();
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand("SELECT idperfil FROM Perfil ORDER BY idperfil", conexion))
            {
                conexion.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read()) perfiles.Add(reader.GetString(0));
            }
            return perfiles;
        }

        private Usuario mb506MapUsuario(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();

            usuario.email = reader.GetString(0);
            usuario.nombre = reader.GetString(1);
            usuario.apellido = reader.GetString(2);
            usuario.mb506SetPassword(reader.GetString(3));
            usuario.activo = reader.GetBoolean(4);
            usuario.intentos = reader.GetInt32(5);
            usuario.perfil = reader.GetString(6);

            return usuario;
        }
    }
}
