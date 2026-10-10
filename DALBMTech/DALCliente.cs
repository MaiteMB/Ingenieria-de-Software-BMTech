using mb506.BEBMTech.Cliente;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALCliente
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALCliente()
        {
            dbConnection = DAL_AccesoSQL.mb506GetInstance();
        }

        public Cliente mb506BuscarCliente(string dni)
        {
            string query = @"
                SELECT dni, nombre, apellido, telefono, correoElectronico, digitoVerificador
                FROM Cliente
                WHERE dni = @dni";

            try
            {
                using (var conexion = dbConnection.mb506GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@dni", SqlDbType.VarChar).Value = dni;

                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.Read() ? mb506MapCliente(reader) : null;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("No se pudo consultar el cliente.", ex);
            }
        }


        public List<Cliente> mb506BuscarClientes(string criterio)
        {
            List<Cliente> clientes = new List<Cliente>();

            string query = @"
                SELECT dni, nombre, apellido, telefono, correoElectronico, digitoVerificador
                FROM Cliente
                WHERE
                    dni LIKE @criterio OR
                    nombre LIKE @criterio OR
                    apellido LIKE @criterio
                ORDER BY apellido, nombre";

            using (var conexion = dbConnection.mb506GetConnection())
            using (var command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@criterio", SqlDbType.VarChar).Value = "%" + criterio + "%";

                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(mb506MapCliente(reader));
                    }
                }
            }

            return clientes;
        }
        public bool mb506InsertarCliente(Cliente cliente)
        {
            string query = @"
                INSERT INTO Cliente
                (
                    dni,
                    nombre,
                    apellido,
                    telefono,
                    correoElectronico,
                    digitoVerificador
                )
                VALUES
                (
                    @dni,
                    @nombre,
                    @apellido,
                    @telefono,
                    @correoElectronico,
                    @digitoVerificador
                )";

            try
            {
                using (var conexion = dbConnection.mb506GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@dni", SqlDbType.VarChar).Value = cliente.dni;
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = cliente.nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = cliente.apellido;
                    command.Parameters.Add("@telefono", SqlDbType.VarChar).Value = cliente.telefono;
                    command.Parameters.Add("@correoElectronico", SqlDbType.VarChar).Value = cliente.correoElectronico;
                    command.Parameters.Add("@digitoVerificador", SqlDbType.Int).Value = cliente.digitoVerificador;

                    conexion.Open();

                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        DALIntegridad integridad = new DALIntegridad();
                        integridad.mb506PrepararOperacion(conexion, transaccion, "Cliente");
                        command.Transaction = transaccion;
                        bool registrado = command.ExecuteNonQuery() > 0;
                        integridad.mb506ActualizarTablas(conexion, transaccion, "Cliente");
                        transaccion.Commit();
                        return registrado;
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) throw new Exception("El cliente ya se encuentra registrado.", ex);
                throw new Exception("Error en la base de datos al insertar el cliente.", ex);
            }
        }

        public void mb506ActualizarDigitoVerificador(string dni, int digito)
        {
            using (var conexion = dbConnection.mb506GetConnection())
            using (var command = new SqlCommand("UPDATE Cliente SET digitoVerificador = @digito WHERE dni = @dni", conexion))
            {
                command.Parameters.Add("@dni", SqlDbType.VarChar).Value = dni;
                command.Parameters.Add("@digito", SqlDbType.Int).Value = digito;
                conexion.Open();
                command.ExecuteNonQuery();
            }
        }

        private Cliente mb506MapCliente(SqlDataReader reader)
        {
            return new Cliente
            {
                dni = reader.GetString(0),
                nombre = reader.GetString(1),
                apellido = reader.GetString(2),
                telefono = reader.GetString(3),
                correoElectronico = reader.GetString(4),
                digitoVerificador = reader.GetInt32(5)
            };
        }
    }
}
