using BEBMTech.Cambios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DALBMTech
{
    public class DALCambios
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALCambios()
        {
            dbConnection = DAL_AccesoSQL.GetInstance();
        }

        public List<VersionCambio> ObtenerVersionesProducto(int idProducto)
        {
            List<VersionCambio> versiones = new List<VersionCambio>();

            string query = @"
                SELECT idHistorial, idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador, usuario, fecha, accion
                FROM HistorialCambiosProducto
                WHERE idProducto = @idProducto
                ORDER BY fecha DESC";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto;
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        versiones.Add(MapVersion(reader));
                    }
                }
            }

            return versiones;
        }

        public List<VersionCambio> ObtenerTodasLasVersionesProducto()
        {
            List<VersionCambio> versiones = new List<VersionCambio>();

            string query = @"
                SELECT idHistorial, idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador, usuario, fecha, accion
                FROM HistorialCambiosProducto
                ORDER BY fecha DESC";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        versiones.Add(MapVersion(reader));
                    }
                }
            }

            return versiones;
        }

        public VersionCambio ObtenerVersion(int idHistorial)
        {
            string query = @"
                SELECT idHistorial, idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador, usuario, fecha, accion
                FROM HistorialCambiosProducto
                WHERE idHistorial = @idHistorial";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idHistorial", SqlDbType.Int).Value = idHistorial;
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapVersion(reader) : null;
                }
            }
        }

        public bool RestaurarProducto(int idHistorial)
        {
            VersionCambio version = ObtenerVersion(idHistorial);

            if (version == null)
            {
                return false;
            }

            string query = @"
                UPDATE Producto
                SET codigo = @codigo,
                    descripcion = @descripcion,
                    marca = @marca,
                    modelo = @modelo,
                    precio = @precio,
                    stock = @stock,
                    activo = @activo,
                    digitoVerificador = @digitoVerificador
                WHERE idProducto = @idProducto";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idProducto", SqlDbType.Int).Value = version.idProducto;
                command.Parameters.Add("@codigo", SqlDbType.VarChar).Value = version.codigo;
                command.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = version.descripcion;
                command.Parameters.Add("@marca", SqlDbType.VarChar).Value = version.marca;
                command.Parameters.Add("@modelo", SqlDbType.VarChar).Value = version.modelo;
                command.Parameters.Add("@precio", SqlDbType.Decimal).Value = version.precio;
                command.Parameters.Add("@stock", SqlDbType.Int).Value = version.stock;
                command.Parameters.Add("@activo", SqlDbType.Bit).Value = version.activo;
                command.Parameters.Add("@digitoVerificador", SqlDbType.Int).Value = version.digitoVerificador;

                conexion.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        private VersionCambio MapVersion(SqlDataReader reader)
        {
            return new VersionCambio
            {
                idHistorial = reader.GetInt32(0),
                idProducto = reader.GetInt32(1),
                codigo = reader.GetString(2),
                descripcion = reader.GetString(3),
                marca = reader.GetString(4),
                modelo = reader.GetString(5),
                precio = reader.GetDecimal(6),
                stock = reader.GetInt32(7),
                activo = reader.GetBoolean(8),
                digitoVerificador = reader.GetInt32(9),
                usuario = reader.GetString(10),
                fecha = reader.GetDateTime(11),
                accion = reader.GetString(12)
            };
        }
    }
}
