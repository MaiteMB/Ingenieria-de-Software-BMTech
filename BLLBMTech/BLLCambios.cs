using mb506.BEBMTech.Cambios;
using System;
using System.Collections.Generic;

namespace mb506.BLLBMTech
{
    public class BLLCambios
    {
        private readonly mb506.DALBMTech.DALCambios dalCambios;
        private readonly BLLLog bllLog;

        public BLLCambios()
        {
            dalCambios = new mb506.DALBMTech.DALCambios();
            bllLog = new BLLLog();
        }

        public List<VersionCambio> mb506ObtenerTodasLasVersionesProducto()
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            return dalCambios.mb506ObtenerTodasLasVersionesProducto();
        }

        public List<VersionCambio> mb506ObtenerVersionesProducto(int idProducto)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto.");
            }

            return dalCambios.mb506ObtenerVersionesProducto(idProducto);
        }

        public VersionCambio mb506ObtenerVersion(int idHistorial)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (idHistorial <= 0)
            {
                throw new Exception("Debe seleccionar una versión.");
            }

            return dalCambios.mb506ObtenerVersion(idHistorial);
        }

        public bool mb506RestaurarProducto(int idHistorial)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (idHistorial <= 0)
            {
                throw new Exception("Debe seleccionar una versión.");
            }

            bool restaurado = dalCambios.mb506RestaurarProducto(idHistorial);

            if (restaurado)
            {
                bllLog.mb506RegistrarEvento("Restauración de producto a versión anterior", "Control de cambios", 2);
            }

            return restaurado;
        }
    }
}
