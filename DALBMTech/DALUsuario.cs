using BEBMTech.Usuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DALBMTech
{
    public class DALUsuario
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALUsuario()
        {
            dbConnection = DAL_AccesoSQL.GetInstance();
        }

        public Usuario BuscarUsuario(string email)
        {
            string query = @"
                SELECT email, nombre, apellido, password, activo, intentos, idperfil
                FROM Usuario
                WHERE email = @email";

            try
            {
                using (SqlConnection conexion = dbConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.Read() ? MapUsuario(reader) : null;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al buscar el usuario.", ex);
            }
        }

        public bool ActualizarIntentosYEstado(Usuario usuario)
        {
            string query = @"
                UPDATE Usuario
                SET intentos = @intentos,
                    activo = @activo
                WHERE email = @email";

            try
            {
                using (SqlConnection conexion = dbConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = usuario.email;
                    command.Parameters.Add("@intentos", SqlDbType.Int).Value = usuario.intentos;
                    command.Parameters.Add("@activo", SqlDbType.Bit).Value = usuario.activo;

                    conexion.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al actualizar el usuario.", ex);
            }
        }

        public bool InsertarUsuario(Usuario usuario)
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
                using (SqlConnection conexion = dbConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = usuario.email;
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = usuario.apellido;
                    command.Parameters.Add("@password", SqlDbType.VarChar).Value = usuario.GetPassword();
                    command.Parameters.Add("@activo", SqlDbType.Bit).Value = usuario.activo;
                    command.Parameters.Add("@intentos", SqlDbType.Int).Value = usuario.intentos;
                    command.Parameters.Add("@idperfil", SqlDbType.VarChar).Value = usuario.perfil;

                    conexion.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al insertar el usuario.", ex);
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string query = @"
                SELECT email, nombre, apellido, password, activo, intentos, idperfil
                FROM Usuario
                ORDER BY apellido, nombre";

            try
            {
                using (SqlConnection conexion = dbConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(MapUsuario(reader));
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

        private Usuario MapUsuario(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();

            usuario.email = reader.GetString(0);
            usuario.nombre = reader.GetString(1);
            usuario.apellido = reader.GetString(2);
            usuario.SetPassword(reader.GetString(3));
            usuario.activo = reader.GetBoolean(4);
            usuario.intentos = reader.GetInt32(5);
            usuario.perfil = reader.GetString(6);

            return usuario;
        }
    }
}
