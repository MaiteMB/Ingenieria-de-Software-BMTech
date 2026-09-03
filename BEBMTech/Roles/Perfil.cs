using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Roles
{
    public class Perfil
    {
        public Perfil()
        {
            Roles = new List<Rol>();
        }

        public string id_perfil { get; set; }

        public List<Rol> Roles { get; set; }

        public override string ToString()
        {
            return id_perfil;
        }
    }
}
