using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Roles
{
    public class Patente : Rol
    {
        public Patente(string id) : base(id) { }

        public override string Tipo => "Patente";

        public override List<Rol> ObtenerTodos()
        {
            return new List<Rol>()
            {
                this
            };
        }
    }
}
