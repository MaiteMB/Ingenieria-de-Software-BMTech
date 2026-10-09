using BEBMTech.Cambios;
using System;
using System.Collections.Generic;

namespace BLLBMTech
{
    public class BLLCambios
    {
        private readonly DALBMTech.DALCambios dalCambios;
        private readonly BLLLog bllLog;
        private readonly BLLIntegridad bllIntegridad;

        public BLLCambios()
        {
            dalCambios = new DALBMTech.DALCambios();
            bllLog = new BLLLog();
            bllIntegridad = new BLLIntegridad();
        }

        public List<VersionCambio> ObtenerTodasLasVersionesProducto()
        {
            return dalCambios.ObtenerTodasLasVersionesProducto();
        }

        public List<VersionCambio> ObtenerVersionesProducto(int idProducto)
        {
            if (idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto.");
            }

            return dalCambios.ObtenerVersionesProducto(idProducto);
        }

        public VersionCambio ObtenerVersion(int idHistorial)
        {
            if (idHistorial <= 0)
            {
                throw new Exception("Debe seleccionar una versión.");
            }

            return dalCambios.ObtenerVersion(idHistorial);
        }

        public bool RestaurarProducto(int idHistorial)
        {
            if (idHistorial <= 0)
            {
                throw new Exception("Debe seleccionar una versión.");
            }

            bool restaurado = dalCambios.RestaurarProducto(idHistorial);

            if (restaurado)
            {
                bllIntegridad.ActualizarDigitoVerticalProducto();
                bllLog.RegistrarEvento("Restauración de producto a versión anterior", "Control de cambios", 2);
            }

            return restaurado;
        }
    }
}
