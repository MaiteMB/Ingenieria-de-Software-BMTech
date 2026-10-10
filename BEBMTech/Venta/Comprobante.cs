using System;

namespace mb506.BEBMTech.Venta
{
    public class Comprobante
    {
        public int idComprobante { get; set; }
        public int idVenta { get; set; }
        public DateTime fecha { get; set; }
        public string cliente { get; set; }
        public string dniCliente { get; set; }
        public string vendedor { get; set; }
        public decimal total { get; set; }
        public System.Collections.Generic.List<DetalleVenta> detalles { get; set; } = new System.Collections.Generic.List<DetalleVenta>();
    }
}
