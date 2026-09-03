using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Roles
{
    public class RolRelacion
    {
        public RolRelacion()
        {
        }

        public RolRelacion(Rol padre, Rol hijo)
        {
            RolPadre = padre;
            RolHijo = hijo;
        }

        public Rol RolPadre { get; set; }

        public Rol RolHijo { get; set; }
    }
}
