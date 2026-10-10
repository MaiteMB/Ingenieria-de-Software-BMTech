using mb506.BEBMTech.Venta;
using mb506.DALBMTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mb506.BLLBMTech
{
    public class BLLVenta
    {
        private readonly DALVenta dalVenta;

        public BLLVenta()
        {
            dalVenta = new DALVenta();
        }

        public int mb506RegistrarVenta(Venta venta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            mb506ValidarVenta(venta);

            if (!mb506.ServiciosBMTech.Seguridad.SessionManager.IsSessionActive)
                throw new Exception("Debe iniciar sesion para registrar una venta.");
            venta.emailUsuario = mb506.ServiciosBMTech.Seguridad.SessionManager.getSession.Usuario.email;

            venta.total = mb506CalcularTotal(venta.detalles);
            if (venta.total > 9999999999999999.99m) throw new Exception("El total supera el importe permitido.");
            mb506ValidarPago(venta.pago, venta.total, false);
            int idVenta = dalVenta.mb506InsertarVenta(venta);
            new BLLLog().mb506RegistrarEvento("Venta registrada: " + idVenta, "Ventas", 1);
            return idVenta;
        }

        public void mb506ValidarPago(Pago pago, decimal total, bool exigirVerificado)
        {
            if (pago == null) throw new Exception("Debe indicar el pago.");
            if (pago.medioPago != "Efectivo" && pago.medioPago != "Transferencia") throw new Exception("Seleccione un medio de pago.");
            if (pago.importe < 0 || pago.importe > 9999999999999999.99m || decimal.Round(pago.importe, 2) != pago.importe)
                throw new Exception("El importe del pago no es valido.");
            if ((pago.numeroOperacion ?? "").Length > 100) throw new Exception("El numero de operacion es demasiado largo.");
            if (pago.medioPago == "Transferencia" && string.IsNullOrWhiteSpace(pago.numeroOperacion))
                throw new Exception("Indique el numero de operacion de la transferencia.");
            if (pago.importe != total) pago.verificado = false;
            if (exigirVerificado && !pago.verificado) throw new Exception("Verifique el pago por el importe exacto antes de continuar.");
        }

        public decimal mb506ObtenerTotal(int idVenta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            return dalVenta.mb506ObtenerTotal(idVenta);
        }

        public void mb506ConfirmarPago(int idVenta, Pago pago)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            mb506ValidarPago(pago, dalVenta.mb506ObtenerTotal(idVenta), true);
            dalVenta.mb506ConfirmarPago(idVenta, pago);
            new BLLLog().mb506RegistrarEvento("Pago verificado de venta: " + idVenta, "Ventas", 1);
        }

        public int mb506GenerarComprobante(int idVenta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            int id = dalVenta.mb506GenerarComprobante(idVenta);
            new BLLLog().mb506RegistrarEvento("Comprobante emitido de venta: " + idVenta, "Ventas", 1);
            return id;
        }

        public Comprobante mb506ObtenerComprobante(int idVenta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            Comprobante comprobante = dalVenta.mb506ObtenerComprobante(idVenta);
            foreach (System.Data.DataRow fila in dalVenta.mb506ObtenerDetalle(idVenta).Rows)
                comprobante.detalles.Add(new DetalleVenta { idProducto = Convert.ToInt32(fila["idProducto"]),
                    codigoProducto = Convert.ToString(fila["codigo"]), descripcionProducto = Convert.ToString(fila["descripcion"]),
                    cantidad = Convert.ToInt32(fila["cantidad"]), precioUnitario = Convert.ToDecimal(fila["precioUnitario"]),
                    subtotal = Convert.ToDecimal(fila["subtotal"]) });
            return comprobante;
        }

        public void mb506EntregarVenta(int idVenta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            dalVenta.mb506CambiarEstado(idVenta, true);
            new BLLLog().mb506RegistrarEvento("Entrega de venta: " + idVenta, "Ventas", 1);
        }

        public void mb506CancelarVenta(int idVenta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            dalVenta.mb506CambiarEstado(idVenta, false);
            new BLLLog().mb506RegistrarEvento("Venta pendiente cancelada: " + idVenta, "Ventas", 1);
        }

        public System.Data.DataTable mb506ObtenerVentas(DateTime desde, DateTime hasta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            if (desde.Date > hasta.Date) throw new Exception("El rango de fechas es incorrecto.");
            return dalVenta.mb506ObtenerVentas(desde, hasta);
        }

        public System.Data.DataTable mb506ObtenerDetalle(int idVenta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            if (idVenta <= 0) throw new Exception("Seleccione una venta.");
            return dalVenta.mb506ObtenerDetalle(idVenta);
        }

        public System.Data.DataTable mb506ObtenerPagos(int idVenta)
        {
            new BLLPermiso().mb506Validar("VENTAS");
            if (!new DALIntegridad().mb506VerificarTablas("Pago")) throw new Exception("Revise la integridad de los pagos.");
            return dalVenta.mb506ObtenerPagos(idVenta);
        }

        private void mb506ValidarVenta(Venta venta)
        {
            if (venta == null)
            {
                throw new Exception("Debe ingresar los datos de la venta.");
            }

            if (string.IsNullOrWhiteSpace(venta.dniCliente))
            {
                throw new Exception("Debe seleccionar un cliente.");
            }

            if (venta.detalles == null || venta.detalles.Count == 0)
            {
                throw new Exception("Debe agregar al menos un producto a la venta.");
            }

            foreach (DetalleVenta detalle in venta.detalles)
            {
                if (detalle == null) throw new Exception("El detalle no puede ser nulo.");
                if (detalle.idProducto <= 0)
                {
                    throw new Exception("Debe seleccionar un producto válido.");
                }

                if (detalle.cantidad <= 0)
                {
                    throw new Exception("La cantidad debe ser mayor a cero.");
                }

                if (detalle.precioUnitario <= 0)
                {
                    throw new Exception("El precio del producto debe ser mayor a cero.");
                }
                if (decimal.Round(detalle.precioUnitario, 2) != detalle.precioUnitario)
                    throw new Exception("El precio debe tener como maximo dos decimales.");

                detalle.subtotal = detalle.cantidad * detalle.precioUnitario;
            }
        }

        private decimal mb506CalcularTotal(List<DetalleVenta> detalles)
        {
            decimal total = 0;

            foreach (DetalleVenta detalle in detalles)
            {
                total += detalle.subtotal;
            }

            return total;
        }
    }
}
