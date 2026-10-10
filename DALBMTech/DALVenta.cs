using mb506.BEBMTech.Venta;
using System;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALVenta
    {
        private readonly string[] tablas = { "Cliente", "Usuario", "Venta", "DetalleVenta", "Producto", "HistorialCambiosProducto", "Pago", "Comprobante" };

        private void mb506Decimal(SqlCommand command, string nombre, decimal valor)
        {
            SqlParameter parametro = command.Parameters.Add(nombre, SqlDbType.Decimal);
            parametro.Precision = 18;
            parametro.Scale = 2;
            parametro.Value = valor;
        }

        private void mb506GuardarPago(SqlConnection conexion, SqlTransaction transaccion, int idVenta, Pago pago)
        {
            using (SqlCommand command = new SqlCommand(@"
                INSERT INTO Pago(idVenta,medioPago,importe,numeroOperacion,fecha,verificado)
                VALUES(@venta,@medio,@importe,@operacion,@fecha,@verificado)", conexion, transaccion))
            {
                command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                command.Parameters.Add("@medio", SqlDbType.VarChar, 20).Value = pago.medioPago;
                mb506Decimal(command, "@importe", pago.importe);
                command.Parameters.Add("@operacion", SqlDbType.NVarChar, 500).Value =
                    mb506.ServiciosBMTech.Seguridad.Cifrado.mb506Cifrar(pago.numeroOperacion ?? "");
                command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = pago.fecha;
                command.Parameters.Add("@verificado", SqlDbType.Bit).Value = pago.verificado;
                command.ExecuteNonQuery();
            }
        }

        private void mb506DescontarStock(SqlConnection conexion, SqlTransaction transaccion, int idVenta)
        {
            DataTable detalles = new DataTable();
            using (SqlCommand command = new SqlCommand(
                "SELECT idProducto,SUM(cantidad) AS cantidad,MIN(precioUnitario) AS precio,MAX(precioUnitario) AS precioMax FROM DetalleVenta WHERE idVenta=@venta GROUP BY idProducto",
                conexion, transaccion))
            {
                command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(detalles);
            }
            foreach (DataRow fila in detalles.Rows)
            {
                if (Convert.ToDecimal(fila["precio"]) != Convert.ToDecimal(fila["precioMax"]))
                    throw new Exception("Un producto no puede tener dos precios en la misma venta.");
                using (SqlCommand command = new SqlCommand(@"
                    UPDATE Producto SET stock=stock-@cantidad
                    WHERE idProducto=@id AND activo=1 AND stock>=@cantidad AND precio=@precio", conexion, transaccion))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = fila["idProducto"];
                    command.Parameters.Add("@cantidad", SqlDbType.Int).Value = fila["cantidad"];
                    mb506Decimal(command, "@precio", Convert.ToDecimal(fila["precio"]));
                    if (command.ExecuteNonQuery() != 1)
                        throw new Exception("El producto cambio de precio, no esta activo o no tiene stock suficiente.");
                }
            }
        }

        public int mb506InsertarVenta(Venta venta)
        {
            DALIntegridad integridad = new DALIntegridad();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    integridad.mb506PrepararOperacion(conexion, transaccion, tablas);
                    using (SqlCommand usuario = new SqlCommand(
                        "SELECT COUNT(*) FROM Usuario WHERE email=@email AND activo=1", conexion, transaccion))
                    {
                        usuario.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = venta.emailUsuario;
                        if (Convert.ToInt32(usuario.ExecuteScalar()) != 1) throw new Exception("El vendedor no esta activo.");
                    }
                    int idVenta;
                    using (SqlCommand command = new SqlCommand(@"
                        INSERT INTO Venta(dniCliente,emailUsuario,fecha,total,estado)
                        VALUES(@dni,@email,@fecha,@total,@estado); SELECT SCOPE_IDENTITY();", conexion, transaccion))
                    {
                        command.Parameters.Add("@dni", SqlDbType.VarChar, 20).Value = venta.dniCliente;
                        command.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = venta.emailUsuario;
                        command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = venta.fecha;
                        command.Parameters.Add("@estado", SqlDbType.VarChar, 20).Value = venta.pago.verificado ? "PAGADA" : "PENDIENTE";
                        mb506Decimal(command, "@total", venta.total);
                        idVenta = Convert.ToInt32(command.ExecuteScalar());
                    }
                    foreach (DetalleVenta detalle in venta.detalles)
                    {
                        using (SqlCommand command = new SqlCommand(@"
                            INSERT INTO DetalleVenta(idVenta,idProducto,cantidad,precioUnitario,subtotal,codigoProducto,descripcionProducto)
                            SELECT @venta,idProducto,@cantidad,@precio,@subtotal,codigo,descripcion FROM Producto
                            WHERE idProducto=@producto AND activo=1 AND stock>=@cantidad AND precio=@precio", conexion, transaccion))
                        {
                            command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                            command.Parameters.Add("@producto", SqlDbType.Int).Value = detalle.idProducto;
                            command.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad;
                            mb506Decimal(command, "@precio", detalle.precioUnitario);
                            mb506Decimal(command, "@subtotal", detalle.subtotal);
                            if (command.ExecuteNonQuery() != 1) throw new Exception("Revise el precio y el stock del producto.");
                        }
                    }
                    // Una venta pendiente no descuenta stock hasta verificar el pago.
                    using (SqlCommand stock = new SqlCommand(@"SELECT COUNT(*) FROM
                        (SELECT idProducto,SUM(cantidad) AS cantidad FROM DetalleVenta WHERE idVenta=@venta GROUP BY idProducto) d
                        JOIN Producto p ON p.idProducto=d.idProducto WHERE d.cantidad>p.stock", conexion, transaccion))
                    {
                        stock.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                        if (Convert.ToInt32(stock.ExecuteScalar()) > 0) throw new Exception("No hay stock suficiente.");
                    }
                    mb506GuardarPago(conexion, transaccion, idVenta, venta.pago);
                    if (venta.pago.verificado) mb506DescontarStock(conexion, transaccion, idVenta);
                    integridad.mb506ActualizarTablas(conexion, transaccion, tablas);
                    transaccion.Commit();
                    return idVenta;
                }
            }
        }

        public decimal mb506ObtenerTotal(int idVenta)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand("SELECT total FROM Venta WHERE idVenta=@venta", conexion))
            {
                conexion.Open();
                command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                object valor = command.ExecuteScalar();
                if (valor == null) throw new Exception("La venta no existe.");
                return Convert.ToDecimal(valor);
            }
        }

        public void mb506ConfirmarPago(int idVenta, Pago pago)
        {
            DALIntegridad integridad = new DALIntegridad();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    integridad.mb506PrepararOperacion(conexion, transaccion, tablas);
                    using (SqlCommand command = new SqlCommand(
                        "UPDATE Venta SET estado='PAGADA' WHERE idVenta=@venta AND estado='PENDIENTE' AND total=@importe", conexion, transaccion))
                    {
                        command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                        mb506Decimal(command, "@importe", pago.importe);
                        if (command.ExecuteNonQuery() != 1) throw new Exception("La venta no esta pendiente o el importe no coincide.");
                    }
                    mb506DescontarStock(conexion, transaccion, idVenta);
                    mb506GuardarPago(conexion, transaccion, idVenta, pago);
                    integridad.mb506ActualizarTablas(conexion, transaccion, tablas);
                    transaccion.Commit();
                }
            }
        }

        public int mb506GenerarComprobante(int idVenta)
        {
            DALIntegridad integridad = new DALIntegridad();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    integridad.mb506PrepararOperacion(conexion, transaccion, tablas);
                    using (SqlCommand command = new SqlCommand(@"
                        IF NOT EXISTS(SELECT 1 FROM Comprobante WHERE idVenta=@venta)
                            INSERT INTO Comprobante(idVenta,fecha,cliente,dniCliente,vendedor,total)
                            SELECT v.idVenta,GETDATE(),c.nombre+' '+c.apellido,v.dniCliente,
                                COALESCE(u.nombre+' '+u.apellido,'Vendedor no registrado'),v.total
                            FROM Venta v JOIN Cliente c ON c.dni=v.dniCliente
                            LEFT JOIN Usuario u ON u.email=v.emailUsuario
                            WHERE v.idVenta=@venta AND v.estado IN ('PAGADA','FINALIZADA');
                        SELECT idComprobante FROM Comprobante WHERE idVenta=@venta;", conexion, transaccion))
                    {
                        command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                        object resultado = command.ExecuteScalar();
                        if (resultado == null) throw new Exception("Solo se emite comprobante de una venta pagada.");
                        integridad.mb506ActualizarTablas(conexion, transaccion, tablas);
                        transaccion.Commit();
                        return Convert.ToInt32(resultado);
                    }
                }
            }
        }

        public void mb506CambiarEstado(int idVenta, bool entregar)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand(entregar
                ? @"UPDATE Venta SET estado='FINALIZADA',fechaEntrega=GETDATE()
                    WHERE idVenta=@venta AND estado='PAGADA'
                    AND EXISTS(SELECT 1 FROM Comprobante c WHERE c.idVenta=Venta.idVenta)"
                : "UPDATE Venta SET estado='CANCELADA' WHERE idVenta=@venta AND estado='PENDIENTE'", conexion))
            {
                conexion.Open();
                command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                if (new DALIntegridad().mb506EjecutarComando(command, "Venta", "Comprobante") != 1)
                    throw new Exception(entregar ? "La venta debe estar pagada y tener comprobante antes de entregar." : "Solo se cancela una venta pendiente.");
            }
        }

        public Comprobante mb506ObtenerComprobante(int idVenta)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand("SELECT * FROM Comprobante WHERE idVenta=@venta", conexion))
            {
                conexion.Open();
                command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) throw new Exception("Todavia no hay comprobante.");
                    return new Comprobante { idComprobante = (int)reader["idComprobante"], idVenta = idVenta,
                        fecha = (DateTime)reader["fecha"], cliente = (string)reader["cliente"],
                        dniCliente = (string)reader["dniCliente"], vendedor = (string)reader["vendedor"], total = (decimal)reader["total"] };
                }
            }
        }

        public DataTable mb506ObtenerVentas(DateTime desde, DateTime hasta)
        {
            DataTable ventas = new DataTable();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                SELECT v.idVenta,v.fecha,v.dniCliente,c.nombre+' '+c.apellido AS cliente,
                    v.emailUsuario AS vendedor,v.total,v.estado,v.fechaEntrega
                FROM Venta v JOIN Cliente c ON c.dni=v.dniCliente
                WHERE v.fecha>=@desde AND v.fecha<@hasta ORDER BY v.fecha DESC,v.idVenta DESC", conexion))
            {
                command.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde.Date;
                command.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta.Date.AddDays(1);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(ventas);
            }
            return ventas;
        }

        public DataTable mb506ObtenerDetalle(int idVenta)
        {
            DataTable detalles = new DataTable();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                SELECT idProducto,codigoProducto AS codigo,descripcionProducto AS descripcion,cantidad,precioUnitario,subtotal
                FROM DetalleVenta WHERE idVenta=@venta ORDER BY idDetalleVenta", conexion))
            {
                command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(detalles);
            }
            return detalles;
        }

        public DataTable mb506ObtenerPagos(int idVenta)
        {
            DataTable pagos = new DataTable();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand(@"SELECT fecha,medioPago,importe,numeroOperacion,verificado
                FROM Pago WHERE idVenta=@venta ORDER BY idPago DESC", conexion))
            {
                command.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(pagos);
            }
            foreach (DataRow fila in pagos.Rows)
                fila["numeroOperacion"] = mb506.ServiciosBMTech.Seguridad.Cifrado.mb506Descifrar(Convert.ToString(fila["numeroOperacion"]));
            return pagos;
        }
    }
}
