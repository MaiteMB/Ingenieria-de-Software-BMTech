using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mb506.BEBMTech.Venta
{
    public class DetalleVenta
    {
        public int idDetalleVenta { get; set; }
        public int idVenta { get; set; }
        public int idProducto { get; set; }
        public string codigoProducto { get; set; }
        public string descripcionProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }

        public DetalleVenta()
        {

        }
    }
}
