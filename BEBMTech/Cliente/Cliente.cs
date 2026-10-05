using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Cliente
{
    public class Cliente
    {
        public string dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string correoElectronico { get; set; }

        public Cliente() { }

        public Cliente(string dni, string nombre, string apellido, string telefono, string correoElectronico)
        {
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = apellido;
            this.telefono = telefono;
            this.correoElectronico = correoElectronico;
        }
    }
}
