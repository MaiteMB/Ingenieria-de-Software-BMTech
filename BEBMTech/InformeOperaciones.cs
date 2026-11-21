using System;
using System.Data;

namespace mb506.BEBMTech.Informes
{
    public class InformeOperaciones
    {
        public string tipo { get; set; }
        public DateTime desde { get; set; }
        public DateTime hasta { get; set; }
        public DataTable datos { get; set; } = new DataTable();
        public decimal totalCobrado { get; set; }
        public decimal totalPendiente { get; set; }
        public int cantidad { get { return datos.Rows.Count; } }
    }
}
