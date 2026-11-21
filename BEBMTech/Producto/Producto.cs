namespace mb506.BEBMTech.Producto
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public bool activo { get; set; } = true;
        public int digitoVerificador { get; set; }

        public Producto() { }

        public mb506.BEBMTech.Cambios.VersionCambio mb506CrearVersion()
        {
            return new mb506.BEBMTech.Cambios.VersionCambio(this);
        }

        public void mb506RestaurarVersion(mb506.BEBMTech.Cambios.VersionCambio version)
        {
            if (version == null || version.idProducto != idProducto)
                throw new System.Exception("La version no corresponde al producto.");
            codigo = version.codigo;
            descripcion = version.descripcion;
            marca = version.marca;
            modelo = version.modelo;
            precio = version.precio;
            stock = version.stock;
            activo = version.activo;
        }

        public Producto(int idProducto, string codigo, string descripcion, string marca, string modelo, decimal precio, int stock)
        {
            this.idProducto = idProducto;
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.marca = marca;
            this.modelo = modelo;
            this.precio = precio;
            this.stock = stock;
            this.activo = true;
        }
    }
}
