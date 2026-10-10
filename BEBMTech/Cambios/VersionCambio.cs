using System;

namespace mb506.BEBMTech.Cambios
{
    public class VersionCambio
    {
        public int idHistorial { get; set; }
        public int idProducto { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public bool activo { get; set; }
        public int digitoVerificador { get; set; }
        public string usuario { get; set; }
        public DateTime fecha { get; set; }
        public string accion { get; set; }
    }
}
