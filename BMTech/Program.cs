using ServiciosBMTech.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMTech
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //  HashPassword hash = new HashPassword();
          //  Clipboard.SetText(hash.GenerarHash("1234"));
           // MessageBox.Show("Hash copiado al portapapeles");
            Application.Run(new FrmLogin());
        }
    }
}

