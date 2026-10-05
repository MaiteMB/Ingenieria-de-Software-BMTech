using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Producto
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

        public Producto() { }

        public Producto(int idProducto, string codigo, string descripcion, string marca, string modelo, decimal precio, int stock)
        {
            this.idProducto = idProducto;
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.marca = marca;
            this.modelo = modelo;
            this.precio = precio;
            this.stock = stock;
            this.activo = activo;
        }
    }
}
