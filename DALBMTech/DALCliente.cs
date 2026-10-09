using BEBMTech.Cliente;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

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
                SELECT dni, nombre, apellido, telefono, correoElectronico, digitoVerificador
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


        public List<Cliente> BuscarClientes(string criterio)
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

            using (var conexion = dbConnection.GetConnection())
            using (var command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@criterio", SqlDbType.VarChar).Value = "%" + criterio + "%";

                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(MapCliente(reader));
                    }
                }
            }

            return clientes;
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
                using (var conexion = dbConnection.GetConnection())
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
                        command.Transaction = transaccion;
                        bool registrado = command.ExecuteNonQuery() > 0;
                        using (SqlCommand actualizar = new SqlCommand(@"
                            UPDATE DigitoVerificador SET digitoVertical =
                                (SELECT ISNULL(SUM(digitoVerificador), 0) FROM Cliente)
                            WHERE tabla = 'Cliente';
                            IF @@ROWCOUNT = 0
                                INSERT INTO DigitoVerificador (tabla, digitoVertical)
                                SELECT 'Cliente', ISNULL(SUM(digitoVerificador), 0) FROM Cliente;", conexion, transaccion))
                        {
                            actualizar.ExecuteNonQuery();
                        }
                        transaccion.Commit();
                        return registrado;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al insertar el cliente.", ex);
            }
        }

        public void ActualizarDigitoVerificador(string dni, int digito)
        {
            using (var conexion = dbConnection.GetConnection())
            using (var command = new SqlCommand("UPDATE Cliente SET digitoVerificador = @digito WHERE dni = @dni", conexion))
            {
                command.Parameters.Add("@dni", SqlDbType.VarChar).Value = dni;
                command.Parameters.Add("@digito", SqlDbType.Int).Value = digito;
                conexion.Open();
                command.ExecuteNonQuery();
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
                correoElectronico = reader.GetString(4),
                digitoVerificador = reader.GetInt32(5)
            };
        }
    }
}

