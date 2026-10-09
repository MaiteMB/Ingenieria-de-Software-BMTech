using BEBMTech.Venta;
using DALBMTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLBMTech
{
    public class BLLVenta
    {
        private readonly DALVenta dalVenta;

        public BLLVenta()
        {
            dalVenta = new DALVenta();
        }

        public int RegistrarVenta(Venta venta)
        {
            ValidarVenta(venta);

            venta.total = CalcularTotal(venta.detalles);

            return dalVenta.InsertarVenta(venta);
        }

        private void ValidarVenta(Venta venta)
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

                detalle.subtotal = detalle.cantidad * detalle.precioUnitario;
            }
        }

        private decimal CalcularTotal(List<DetalleVenta> detalles)
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