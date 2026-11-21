using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mb506.BEBMTech.Roles
{
    public class Perfil : Rol
    {
        public Perfil() : base("")
        {
            Roles = new List<Rol>();
        }

        public string id_perfil { get { return idRol; } set { idRol = value; } }
        public override string Tipo { get { return "Perfil"; } }

        public List<Rol> Roles { get; set; }

        public override List<Rol> mb506ObtenerTodos()
        {
            return mb506ObtenerTodos(new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        }

        private List<Rol> mb506ObtenerTodos(HashSet<string> camino)
        {
            if (!camino.Add(id_perfil)) throw new Exception("Los perfiles no pueden formar ciclos.");
            List<Rol> resultado = new List<Rol>();
            foreach (Rol rol in Roles)
            {
                resultado.Add(rol);
                Perfil perfil = rol as Perfil;
                resultado.AddRange(perfil == null ? rol.mb506ObtenerTodos() : perfil.mb506ObtenerTodos(camino));
            }
            camino.Remove(id_perfil);
            return resultado;
        }

        public override string ToString()
        {
            return id_perfil;
        }
    }
}
