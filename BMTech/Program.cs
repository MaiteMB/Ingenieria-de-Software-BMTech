using mb506.ServiciosBMTech.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mb506.BMTech
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
            mb506.BLLBMTech.BLLLog.errorRegistro += mensaje => Mensajes.mb506Mostrar(mensaje, "Bitacora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Application.Run(new mb506FrmLogin());
        }
    }
}

