using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mb506.BEBMTech.Roles
{
    public class Familia : Rol
    {
        public List<Rol> Hijos { get; private set; }

        public Familia(string id) : base(id)
        {
            Hijos = new List<Rol>();
        }

        public override string Tipo => "Familia";

        public void mb506Agregar(Rol rol)
        {
            if (rol == null) throw new ArgumentNullException("rol");
            Familia familia = rol as Familia;
            if (ReferenceEquals(rol, this) || (familia != null && familia.mb506Contiene(idRol)))
                throw new Exception("Una familia no puede contenerse a si misma.");
            if (!Hijos.Any(x => x.idRol == rol.idRol))
                Hijos.Add(rol);
        }

        public void mb506Quitar(Rol rol)
        {
            Hijos.RemoveAll(x => x.idRol == rol.idRol);
        }

        public bool mb506Contiene(string id)
        {
            foreach (Rol r in Hijos)
            {
                if (r.idRol == id)
                    return true;

                if (r is Familia rc)
                {
                    if (rc.mb506Contiene(id))
                        return true;
                }
            }

            return false;
        }

        public override List<Rol> mb506ObtenerTodos()
        {
            List<Rol> lista = new List<Rol>();

            foreach (Rol r in Hijos)
            {
                lista.Add(r);

                if (r is Familia rc)
                    lista.AddRange(rc.mb506ObtenerTodos());
            }

            return lista;
        }
    }
}
