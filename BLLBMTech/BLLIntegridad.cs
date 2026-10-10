using System;
using System.Collections.Generic;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;

namespace mb506.BLLBMTech
{
    public class BLLIntegridad
    {
        private DALIntegridad dalIntegridad = new DALIntegridad();

        public List<string> mb506VerificarSistema()
        {
            return dalIntegridad.mb506VerificarSistema();
        }

        public void mb506RegenerarSistema()
        {
            if (!SessionManager.IsSessionActive ||
                !new BLLRol().mb506ObtenerPatentesPorPerfil(SessionManager.getSession.Usuario.perfil).Contains("SEGURIDAD"))
                throw new Exception("Solo un administrador puede regenerar integridad.");
            dalIntegridad.mb506RegenerarSistema();
            new BLLLog().mb506RegistrarEvento("Integridad regenerada", "Seguridad", 3);
        }

        public bool mb506VerificarIntegridadProducto()
        {
            return !mb506VerificarSistema().Contains("Producto");
        }

        public bool mb506VerificarIntegridadCliente()
        {
            return !mb506VerificarSistema().Contains("Cliente");
        }

        public void mb506ActualizarDigitoVerticalProducto()
        {
            dalIntegridad.mb506GuardarDigitoVertical("Producto", dalIntegridad.mb506CalcularDigitoVerticalProducto());
        }

        public void mb506RegenerarIntegridadProducto() { mb506RegenerarSistema(); }
        public void mb506RegenerarIntegridadCliente() { mb506RegenerarSistema(); }
    }
}
