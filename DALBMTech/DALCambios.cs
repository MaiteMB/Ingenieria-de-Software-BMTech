using mb506.BEBMTech.Cambios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALCambios
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALCambios()
        {
            dbConnection = DAL_AccesoSQL.mb506GetInstance();
        }

        public List<VersionCambio> mb506ObtenerVersionesProducto(int idProducto)
        {
            List<VersionCambio> versiones = new List<VersionCambio>();

            string query = @"
                SELECT idHistorial, idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador, usuario, fecha, accion
                FROM HistorialCambiosProducto
                WHERE idProducto = @idProducto
                ORDER BY fecha DESC";

            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto;
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        versiones.Add(mb506MapVersion(reader));
                    }
                }
            }

            return versiones;
        }

        public List<VersionCambio> mb506ObtenerTodasLasVersionesProducto()
        {
            List<VersionCambio> versiones = new List<VersionCambio>();

            string query = @"
                SELECT idHistorial, idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador, usuario, fecha, accion
                FROM HistorialCambiosProducto
                ORDER BY fecha DESC";

            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        versiones.Add(mb506MapVersion(reader));
                    }
                }
            }

            return versiones;
        }

        public VersionCambio mb506ObtenerVersion(int idHistorial)
        {
            string query = @"
                SELECT idHistorial, idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador, usuario, fecha, accion
                FROM HistorialCambiosProducto
                WHERE idHistorial = @idHistorial";

            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idHistorial", SqlDbType.Int).Value = idHistorial;
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? mb506MapVersion(reader) : null;
                }
            }
        }

        public bool mb506RestaurarProducto(int idHistorial)
        {
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

            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    DALIntegridad integridad = new DALIntegridad();
                    integridad.mb506PrepararOperacion(conexion, transaccion, "Producto", "HistorialCambiosProducto");
                    VersionCambio version;
                    using (SqlCommand lectura = new SqlCommand("SELECT idHistorial,idProducto,codigo,descripcion,marca,modelo,precio,stock,activo,digitoVerificador,usuario,fecha,accion FROM HistorialCambiosProducto WHERE idHistorial=@id", conexion, transaccion))
                    {
                        lectura.Parameters.Add("@id", SqlDbType.Int).Value = idHistorial;
                        using (SqlDataReader reader = lectura.ExecuteReader()) version = reader.Read() ? mb506MapVersion(reader) : null;
                    }
                    if (version == null) return false;
                    mb506.BEBMTech.Producto.Producto producto = new mb506.BEBMTech.Producto.Producto { idProducto = version.idProducto };
                    producto.mb506RestaurarVersion(version);
                    using (SqlCommand command = new SqlCommand(query, conexion, transaccion))
                    {
                        command.Parameters.Add("@idProducto", SqlDbType.Int).Value = producto.idProducto;
                        command.Parameters.Add("@codigo", SqlDbType.VarChar, 50).Value = producto.codigo;
                        command.Parameters.Add("@descripcion", SqlDbType.VarChar, 150).Value = producto.descripcion;
                        command.Parameters.Add("@marca", SqlDbType.VarChar, 100).Value = producto.marca;
                        command.Parameters.Add("@modelo", SqlDbType.VarChar, 100).Value = producto.modelo;
                        SqlParameter precio = command.Parameters.Add("@precio", SqlDbType.Decimal);
                        precio.Precision = 18;
                        precio.Scale = 2;
                        precio.Value = producto.precio;
                        command.Parameters.Add("@stock", SqlDbType.Int).Value = producto.stock;
                        command.Parameters.Add("@activo", SqlDbType.Bit).Value = producto.activo;
                        command.Parameters.Add("@digitoVerificador", SqlDbType.Int).Value = new mb506.ServiciosBMTech.Seguridad.DigitoVerificador().mb506CalcularProducto(producto);
                        if (command.ExecuteNonQuery() != 1) return false;
                    }
                    integridad.mb506ActualizarTablas(conexion, transaccion, "Producto", "HistorialCambiosProducto");
                    transaccion.Commit();
                    return true;
                }
            }
        }

        private VersionCambio mb506MapVersion(SqlDataReader reader)
        {
            mb506.BEBMTech.Producto.Producto producto = new mb506.BEBMTech.Producto.Producto
            {
                idProducto = reader.GetInt32(1),
                codigo = reader.GetString(2),
                descripcion = reader.GetString(3),
                marca = reader.GetString(4),
                modelo = reader.GetString(5),
                precio = reader.GetDecimal(6),
                stock = reader.GetInt32(7),
                activo = reader.GetBoolean(8),
                digitoVerificador = reader.GetInt32(9)
            };
            VersionCambio version = producto.mb506CrearVersion();
            version.idHistorial = reader.GetInt32(0);
            version.usuario = reader.GetString(10);
            version.fecha = reader.GetDateTime(11);
            version.accion = reader.GetString(12);
            return version;
        }
    }
}
