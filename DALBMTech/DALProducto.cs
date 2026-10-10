using mb506.BEBMTech.Producto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALProducto
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALProducto()
        {
            dbConnection = DAL_AccesoSQL.mb506GetInstance();
        }

        public List<Producto> mb506BuscarProducto(string criterio, bool incluirInactivos = false)
        {
            List<Producto> productos = new List<Producto>();

            string query = @"
                SELECT idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador
                FROM Producto
                WHERE
                    (@incluirInactivos = 1 OR activo = 1)
                    AND
                    (
                        codigo LIKE @criterio OR
                        descripcion LIKE @criterio OR
                        marca LIKE @criterio OR
                        modelo LIKE @criterio
                    )
                ORDER BY descripcion";

            try
            {
                using (var conexion = dbConnection.mb506GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@criterio", SqlDbType.VarChar).Value = "%" + criterio + "%";
                    command.Parameters.Add("@incluirInactivos", SqlDbType.Bit).Value = incluirInactivos;

                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(mb506MapProducto(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al buscar productos.", ex);
            }

            return productos;
        }

        public Producto mb506ObtenerProductoPorCodigo(string codigo)
        {
            string query = @"
                SELECT idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador
                FROM Producto
                WHERE codigo = @codigo";

            try
            {
                using (var conexion = dbConnection.mb506GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@codigo", SqlDbType.VarChar).Value = codigo;

                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.Read() ? mb506MapProducto(reader) : null;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al obtener el producto.", ex);
            }
        }

        public Producto mb506ObtenerProductoPorId(int idProducto)
        {
            string query = @"
                SELECT idProducto, codigo, descripcion, marca, modelo, precio, stock, activo, digitoVerificador
                FROM Producto
                WHERE idProducto = @idProducto";

            using (var conexion = dbConnection.mb506GetConnection())
            using (var command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto;
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? mb506MapProducto(reader) : null;
                }
            }
        }

        public bool mb506InsertarProducto(Producto producto)
        {
            string query = @"
                INSERT INTO Producto
                (
                    codigo,
                    descripcion,
                    marca,
                    modelo,
                    precio,
                    stock,
                    activo,
                    digitoVerificador
                )
                VALUES
                (
                    @codigo,
                    @descripcion,
                    @marca,
                    @modelo,
                    @precio,
                    @stock,
                    @activo,
                    @digitoVerificador
                )";

            try
            {
                using (var conexion = dbConnection.mb506GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    mb506CargarParametrosProducto(command, producto);

                    conexion.Open();
                    return new DALIntegridad().mb506EjecutarComando(command, "Producto", "HistorialCambiosProducto") > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al insertar el producto.", ex);
            }
        }

        public bool mb506ModificarProducto(Producto producto)
        {
            string query = @"
                UPDATE Producto
                SET
                    descripcion = @descripcion,
                    marca = @marca,
                    modelo = @modelo,
                    precio = @precio,
                    stock = @stock,
                    digitoVerificador = @digitoVerificador
                WHERE idProducto = @idProducto";

            try
            {
                using (var conexion = dbConnection.mb506GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@idProducto", SqlDbType.Int).Value = producto.idProducto;
                    command.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = producto.descripcion;
                    command.Parameters.Add("@marca", SqlDbType.VarChar).Value = producto.marca;
                    command.Parameters.Add("@modelo", SqlDbType.VarChar).Value = producto.modelo;
                    command.Parameters.Add("@precio", SqlDbType.Decimal).Value = producto.precio;
                    command.Parameters.Add("@stock", SqlDbType.Int).Value = producto.stock;
                    command.Parameters.Add("@digitoVerificador", SqlDbType.Int).Value = producto.digitoVerificador;

                    conexion.Open();
                    return new DALIntegridad().mb506EjecutarComando(command, "Producto", "HistorialCambiosProducto") > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al modificar el producto.", ex);
            }
        }

        public bool mb506CambiarEstadoProducto(Producto producto)
        {
            string query = @"
                UPDATE Producto
                SET activo = @activo,
                    digitoVerificador = @digitoVerificador
                WHERE idProducto = @idProducto";

            try
            {
                using (var conexion = dbConnection.mb506GetConnection())
                using (var command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@idProducto", SqlDbType.Int).Value = producto.idProducto;
                    command.Parameters.Add("@activo", SqlDbType.Bit).Value = producto.activo;
                    command.Parameters.Add("@digitoVerificador", SqlDbType.Int).Value = producto.digitoVerificador;

                    conexion.Open();
                    return new DALIntegridad().mb506EjecutarComando(command, "Producto", "HistorialCambiosProducto") > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al cambiar el estado del producto.", ex);
            }
        }


        public bool mb506ActualizarDigitoVerificador(int idProducto, int digitoVerificador)
        {
            string query = @"
                UPDATE Producto
                SET digitoVerificador = @digitoVerificador
                WHERE idProducto = @idProducto";

            using (var conexion = dbConnection.mb506GetConnection())
            using (var command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto;
                command.Parameters.Add("@digitoVerificador", SqlDbType.Int).Value = digitoVerificador;

                conexion.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
        private void mb506CargarParametrosProducto(SqlCommand command, Producto producto)
        {
            command.Parameters.Add("@codigo", SqlDbType.VarChar).Value = producto.codigo;
            command.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = producto.descripcion;
            command.Parameters.Add("@marca", SqlDbType.VarChar).Value = producto.marca;
            command.Parameters.Add("@modelo", SqlDbType.VarChar).Value = producto.modelo;
            command.Parameters.Add("@precio", SqlDbType.Decimal).Value = producto.precio;
            command.Parameters.Add("@stock", SqlDbType.Int).Value = producto.stock;
            command.Parameters.Add("@activo", SqlDbType.Bit).Value = producto.activo;
            command.Parameters.Add("@digitoVerificador", SqlDbType.Int).Value = producto.digitoVerificador;
        }

        private Producto mb506MapProducto(SqlDataReader reader)
        {
            return new Producto
            {
                idProducto = reader.GetInt32(0),
                codigo = reader.GetString(1),
                descripcion = reader.GetString(2),
                marca = reader.GetString(3),
                modelo = reader.GetString(4),
                precio = reader.GetDecimal(5),
                stock = reader.GetInt32(6),
                activo = reader.GetBoolean(7),
                digitoVerificador = reader.GetInt32(8)
            };
        }
    }
}
