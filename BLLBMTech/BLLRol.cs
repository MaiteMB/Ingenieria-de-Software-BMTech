using DALBMTech;
using System.Collections.Generic;

namespace BLLBMTech
{
    public class BLLRol
    {
        private readonly DALRol dalRol;

        public BLLRol()
        {
            dalRol = new DALRol();
        }

        public List<string> ObtenerPatentesPorPerfil(string idPerfil)
        {
            if (string.IsNullOrWhiteSpace(idPerfil))
            {
                return new List<string>();
            }

            return dalRol.ObtenerPatentesPorPerfil(idPerfil);
        }
    }
}
