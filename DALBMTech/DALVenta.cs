using BEBMTech.Venta;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALBMTech
{
    public class DALVenta
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALVenta()
        {
            dbConnection = DAL_AccesoSQL.GetInstance();
        }

        public int InsertarVenta(Venta venta)
        {
            string queryVenta = @"
                INSERT INTO Venta
                (
                    dniCliente,
                    emailUsuario,
                    fecha,
                    total
                )
                VALUES
                (
                    @dniCliente,
                    @emailUsuario,
                    @fecha,
                    @total
                );
                SELECT SCOPE_IDENTITY();";

            string queryDetalle = @"
                INSERT INTO DetalleVenta
                (
                    idVenta,
                    idProducto,
                    cantidad,
                    precioUnitario,
                    subtotal
                )
                VALUES
                (
                    @idVenta,
                    @idProducto,
                    @cantidad,
                    @precioUnitario,
                    @subtotal
                );";

            string queryStock = @"
                UPDATE Producto
                SET stock = stock - @cantidad
                WHERE idProducto = @idProducto;";

            try
            {
                using (SqlConnection conexion = dbConnection.GetConnection())
                {
                    conexion.Open();

                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        try
                        {
                            int idVenta;

                            using (SqlCommand commandVenta = new SqlCommand(queryVenta, conexion, transaccion))
                            {
                                commandVenta.Parameters.Add("@dniCliente", SqlDbType.VarChar).Value = venta.dniCliente;

                                if (string.IsNullOrWhiteSpace(venta.emailUsuario))
                                {
                                    commandVenta.Parameters.Add("@emailUsuario", SqlDbType.VarChar).Value = DBNull.Value;
                                }
                                else
                                {
                                    commandVenta.Parameters.Add("@emailUsuario", SqlDbType.VarChar).Value = venta.emailUsuario;
                                }

                                commandVenta.Parameters.Add("@fecha", SqlDbType.DateTime).Value = venta.fecha;
                                commandVenta.Parameters.Add("@total", SqlDbType.Decimal).Value = venta.total;

                                idVenta = Convert.ToInt32(commandVenta.ExecuteScalar());
                            }

                            foreach (DetalleVenta detalle in venta.detalles)
                            {
                                using (SqlCommand commandDetalle = new SqlCommand(queryDetalle, conexion, transaccion))
                                {
                                    commandDetalle.Parameters.Add("@idVenta", SqlDbType.Int).Value = idVenta;
                                    commandDetalle.Parameters.Add("@idProducto", SqlDbType.Int).Value = detalle.idProducto;
                                    commandDetalle.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad;
                                    commandDetalle.Parameters.Add("@precioUnitario", SqlDbType.Decimal).Value = detalle.precioUnitario;
                                    commandDetalle.Parameters.Add("@subtotal", SqlDbType.Decimal).Value = detalle.subtotal;

                                    commandDetalle.ExecuteNonQuery();
                                }

                                using (SqlCommand commandStock = new SqlCommand(queryStock, conexion, transaccion))
                                {
                                    commandStock.Parameters.Add("@idProducto", SqlDbType.Int).Value = detalle.idProducto;
                                    commandStock.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad;

                                    commandStock.ExecuteNonQuery();
                                }
                            }

                            transaccion.Commit();

                            return idVenta;
                        }
                        catch
                        {
                            transaccion.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al registrar la venta.", ex);
            }
        }
    }
}