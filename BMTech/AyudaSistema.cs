using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace mb506.BMTech
{
    public static class AyudaSistema
    {
        public static string mb506ObtenerRuta(string formulario)
        {
            string tema;
            switch ((formulario ?? "").Replace("mb506", ""))
            {
                case "FrmLogin": tema = "acceso"; break;
                case "FrmCliente": tema = "clientes"; break;
                case "FrmProducto": tema = "productos"; break;
                case "FrmVenta":
                case "FrmPago":
                case "FrmComprobante":
                case "FrmHistorialVentas": tema = "ventas"; break;
                case "FrmServicioTecnico": tema = "servicio"; break;
                case "FrmControlCambiosProducto": tema = "cambios"; break;
                case "FrmBackup": tema = "copias"; break;
                case "FrmInformes": tema = "informes"; break;
                case "FrmUsuarios":
                case "FrmPermisos":
                case "FrmCambiarPassword": tema = "seguridad"; break;
                case "FrmIdiomas": tema = "idiomas"; break;
                case "FrmBitacora": tema = "bitacora"; break;
                default: tema = "inicio"; break;
            }
            string archivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ayuda.html");
            if (!File.Exists(archivo)) throw new FileNotFoundException("No se encontro la ayuda de BMTech. Revise los archivos de instalacion.");
            return new Uri(archivo).AbsoluteUri + "#" + tema;
        }
        public static void mb506Mostrar(string formulario)
        {
            try { Process.Start(new ProcessStartInfo(mb506ObtenerRuta(formulario)) { UseShellExecute = true }); }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
        public static void mb506Vincular(Form formulario)
        {
            formulario.HelpRequested += delegate(object sender, HelpEventArgs e)
            {
                mb506Mostrar(formulario.Name);
                e.Handled = true;
            };
        }
    }
}
