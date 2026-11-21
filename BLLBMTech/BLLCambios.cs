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

        private void mb506ValidarAcceso()
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (!new mb506.DALBMTech.DALIntegridad().mb506VerificarTablas("Producto", "HistorialCambiosProducto"))
                throw new Exception("Revise la integridad de los productos y sus versiones.");
        }

        public List<VersionCambio> mb506ObtenerTodasLasVersionesProducto()
        {
            mb506ValidarAcceso();
            return dalCambios.mb506ObtenerTodasLasVersionesProducto();
        }

        public List<VersionCambio> mb506ObtenerVersionesProducto(int idProducto)
        {
            mb506ValidarAcceso();
            if (idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto.");
            }

            return dalCambios.mb506ObtenerVersionesProducto(idProducto);
        }

        public VersionCambio mb506ObtenerVersion(int idHistorial)
        {
            mb506ValidarAcceso();
            if (idHistorial <= 0)
            {
                throw new Exception("Debe seleccionar una versión.");
            }

            return dalCambios.mb506ObtenerVersion(idHistorial);
        }

        public bool mb506RestaurarProducto(int idHistorial)
        {
            mb506ValidarAcceso();
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
