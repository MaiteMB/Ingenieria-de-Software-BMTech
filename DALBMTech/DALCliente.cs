using BEBMTech.Usuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALBMTech
{
    public class DALCliente
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALCliente()
        {
            dbConnection = DAL_AccesoSQL.GetInstance();
        }

        public Cliente BuscarCliente(string dni)
        {
            string query = @"
                SELECT dni, nombre, apellido, telefono, correoElectronico
                FROM Cliente
                WHERE dni = @dni";

            try
            {
                using (var conexion = dbConnection.GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@dni", SqlDbType.VarChar).Value = dni;

                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.Read() ? MapCliente(reader) : null;
                    }
                }
            }
            catch (SqlException)
            {
                return null;
            }
        }

        public bool InsertarCliente(Cliente cliente)
        {
            string query = @"
                INSERT INTO Cliente
                (
                    dni,
                    nombre,
                    apellido,
                    telefono,
                    correoElectronico
                )
                VALUES
                (
                    @dni,
                    @nombre,
                    @apellido,
                    @telefono,
                    @correoElectronico
                )";

            try
            {
                using (var conexion = dbConnection.GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@dni", SqlDbType.VarChar).Value = cliente.dni;
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = cliente.nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = cliente.apellido;
                    command.Parameters.Add("@telefono", SqlDbType.VarChar).Value = cliente.telefono;
                    command.Parameters.Add("@correoElectronico", SqlDbType.VarChar).Value = cliente.correoElectronico;

                    conexion.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al insertar el cliente.", ex);
            }
        }

        private Cliente MapCliente(SqlDataReader reader)
        {
            return new Cliente
            {
                dni = reader.GetString(0),
                nombre = reader.GetString(1),
                apellido = reader.GetString(2),
                telefono = reader.GetString(3),
                correoElectronico = reader.GetString(4)
            };
        }
    }
}
