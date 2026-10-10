using mb506.DALBMTech;
using System.Collections.Generic;

namespace mb506.BLLBMTech
{
    public class BLLRol
    {
        private void mb506ValidarPermiso()
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
        }

        public System.Data.DataTable mb506ObtenerElementos(string tipo)
        {
            mb506ValidarPermiso();
            return dalRol.mb506ObtenerElementos(tipo);
        }

        public void mb506GuardarElemento(string tipo, string id, string nombre, bool nuevo)
        {
            mb506ValidarPermiso();
            if (string.IsNullOrWhiteSpace(id) || id.Length > 50 ||
                string.IsNullOrWhiteSpace(nombre) || nombre.Length > 100)
                throw new System.Exception("Ingrese un identificador y nombre validos.");
            if (tipo == "Patente" && nuevo)
                throw new System.Exception("Las patentes corresponden a acciones del sistema. Se crean al agregar esas acciones.");
            dalRol.mb506GuardarElemento(tipo, id.Trim(), nombre.Trim(), nuevo);
            new BLLLog().mb506RegistrarEvento("Rol guardado: " + tipo + " " + id, "Seguridad", 2);
        }

        public mb506.BEBMTech.Roles.Perfil mb506ObtenerPerfil(string idPerfil)
        {
            mb506ValidarPermiso();
            return dalRol.mb506ObtenerPerfil(idPerfil);
        }

        public List<string> mb506ObtenerAsignaciones(string tipo, string id)
        {
            mb506ValidarPermiso();
            return dalRol.mb506ObtenerAsignaciones(tipo, id);
        }

        public void mb506GuardarAsignaciones(string tipo, string id, List<string> asignaciones)
        {
            mb506ValidarPermiso();
            dalRol.mb506GuardarAsignaciones(tipo, id, asignaciones,
                mb506.ServiciosBMTech.Seguridad.SessionManager.getSession.Usuario.perfil);
            new BLLLog().mb506RegistrarEvento("Permisos modificados: " + tipo + " " + id, "Seguridad", 2);
        }

        private readonly DALRol dalRol;

        public BLLRol()
        {
            dalRol = new DALRol();
        }

        public List<string> mb506ObtenerPatentesPorPerfil(string idPerfil)
        {
            if (string.IsNullOrWhiteSpace(idPerfil))
            {
                return new List<string>();
            }

            List<string> patentes = new List<string>();
            mb506.BEBMTech.Roles.Perfil perfil = dalRol.mb506ObtenerPerfil(idPerfil);
            foreach (mb506.BEBMTech.Roles.Rol rol in perfil.Roles)
                foreach (mb506.BEBMTech.Roles.Rol hijo in rol.mb506ObtenerTodos())
                    if (hijo is mb506.BEBMTech.Roles.Patente && !patentes.Contains(hijo.idRol))
                        patentes.Add(hijo.idRol);
            return patentes;
        }

        public void mb506EliminarElemento(string tipo, string id)
        {
            mb506ValidarPermiso();
            dalRol.mb506EliminarElemento(tipo, id);
            new BLLLog().mb506RegistrarEvento("Elimina " + tipo + ": " + id, "Seguridad", 2);
        }
    }
}
