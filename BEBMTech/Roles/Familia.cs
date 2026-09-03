using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Roles
{
    public class Familia : Rol
    {
        public List<Rol> Hijos { get; private set; }

        public Familia(string id) : base(id)
        {
            Hijos = new List<Rol>();
        }

        public override string Tipo => "Familia";

        public void Agregar(Rol rol)
        {
            if (!Hijos.Any(x => x.idRol == rol.idRol))
                Hijos.Add(rol);
        }

        public void Quitar(Rol rol)
        {
            Hijos.RemoveAll(x => x.idRol == rol.idRol);
        }

        public bool Contiene(string id)
        {
            foreach (Rol r in Hijos)
            {
                if (r.idRol == id)
                    return true;

                if (r is Familia rc)
                {
                    if (rc.Contiene(id))
                        return true;
                }
            }

            return false;
        }

        public override List<Rol> ObtenerTodos()
        {
            List<Rol> lista = new List<Rol>();

            foreach (Rol r in Hijos)
            {
                lista.Add(r);

                if (r is Familia rc)
                    lista.AddRange(rc.ObtenerTodos());
            }

            return lista;
        }
    }
}
