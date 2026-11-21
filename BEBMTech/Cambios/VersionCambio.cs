using System;

namespace mb506.BEBMTech.Cambios
{
    public class VersionCambio
    {
        private readonly mb506.BEBMTech.Producto.Producto estado;

        public VersionCambio(mb506.BEBMTech.Producto.Producto producto)
        {
            if (producto == null) throw new ArgumentNullException("producto");
            estado = new mb506.BEBMTech.Producto.Producto {
                idProducto = producto.idProducto, codigo = producto.codigo, descripcion = producto.descripcion,
                marca = producto.marca, modelo = producto.modelo, precio = producto.precio,
                stock = producto.stock, activo = producto.activo, digitoVerificador = producto.digitoVerificador
            };
        }
        public int idHistorial { get; set; }
        public int idProducto { get { return estado.idProducto; } }
        public string codigo { get { return estado.codigo; } }
        public string descripcion { get { return estado.descripcion; } }
        public string marca { get { return estado.marca; } }
        public string modelo { get { return estado.modelo; } }
        public decimal precio { get { return estado.precio; } }
        public int stock { get { return estado.stock; } }
        public bool activo { get { return estado.activo; } }
        public int digitoVerificador { get { return estado.digitoVerificador; } }
        public string usuario { get; set; }
        public DateTime fecha { get; set; }
        public string accion { get; set; }
    }
}
