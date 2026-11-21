using System;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmCambiarPassword : Form
    {
        public mb506FrmCambiarPassword()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }

        private void mb506Guardar(object sender, EventArgs e)
        {
            try
            {
                if (txtNueva.Text != txtConfirmacion.Text) throw new Exception("Las claves no coinciden.");
                new BLLUsuario().mb506CambiarPassword(txtActual.Text, txtNueva.Text);
                Mensajes.mb506Mostrar("Clave modificada."); Close();
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
