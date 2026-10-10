using System;

namespace mb506.BEBMTech.Venta
{
    public class Pago
    {
        public int idPago { get; set; }
        public int idVenta { get; set; }
        public string medioPago { get; set; }
        public decimal importe { get; set; }
        public string numeroOperacion { get; set; }
        public DateTime fecha { get; set; } = DateTime.Now;
        public bool verificado { get; set; }
    }
}
