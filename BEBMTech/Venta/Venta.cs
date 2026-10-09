using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Venta
{
    public class Venta
    {
        public int idVenta { get; set; }
        public string dniCliente { get; set; }
        public string emailUsuario { get; set; }
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public List<DetalleVenta> detalles { get; set; }

        public Venta()
        {
            fecha = DateTime.Now;
            detalles = new List<DetalleVenta>();
        }
    }
}
