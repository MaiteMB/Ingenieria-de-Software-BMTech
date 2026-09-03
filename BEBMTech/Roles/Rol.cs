using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Roles
{
    public abstract class Rol
    {
        protected Rol(string id)
        {
            idRol = id;
        }

        public string idRol { get; set; }
        public string Vista { get; set; }
        public abstract string Tipo { get; }

        public abstract List<Rol> ObtenerTodos();

        public override string ToString()
        {
            return idRol;
        }
    }
}
