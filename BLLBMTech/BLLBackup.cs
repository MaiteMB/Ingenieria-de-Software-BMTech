using System;
using System.Data;
using System.IO;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;

namespace mb506.BLLBMTech
{
    public class BLLBackup
    {
        public DataTable mb506ObtenerBackups()
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            return new DALBackup().mb506ObtenerBackups();
        }

        public string mb506Crear(string carpeta)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (new BLLIntegridad().mb506VerificarSistema().Count > 0) throw new Exception("Revise la integridad antes de hacer una copia.");
            if (!Directory.Exists(carpeta)) throw new Exception("La carpeta no existe.");
            string ruta = Path.Combine(carpeta, "BMTech_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".bak");
            if (ruta.Length > 450 || File.Exists(ruta)) throw new Exception("La ruta de backup no es valida.");
            Cifrado.mb506ExportarClave(ruta + ".key");
            new BLLLog().mb506RegistrarEvento("Inicio de backup: " + Path.GetFileName(ruta), "Backup", 1);
            new DALBackup().mb506Crear(ruta, SessionManager.getSession.Usuario.email);
            new BLLLog().mb506RegistrarEvento("Backup creado: " + Path.GetFileName(ruta), "Backup", 1);
            return ruta;
        }

        public void mb506Restaurar(string ruta)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            string usuario = SessionManager.getSession.Usuario.email;
            if (!File.Exists(ruta) || !File.Exists(ruta + ".key")) throw new Exception("Faltan el archivo .bak o su archivo .key.");
            if (new FileInfo(ruta + ".key").Length != 32) throw new Exception("La clave de la copia no es valida.");
            new BLLLog().mb506RegistrarEvento("Restauracion solicitada: " + Path.GetFileName(ruta), "Backup", 3);
            new DALBackup().mb506Restaurar(ruta);
            try
            {
                Cifrado.mb506ImportarClave(ruta + ".key");
                new BLLLog().mb506RegistrarEvento(usuario, "Backup restaurado: " + Path.GetFileName(ruta), "Backup", 3);
            }
            finally { SessionManager.mb506Logout(); }
        }
    }
}
