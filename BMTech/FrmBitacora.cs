using System;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmBitacora : Form
    {
        public mb506FrmBitacora()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }

        private void mb506Cargar(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                dgvEventos.DataSource = new BLLLog().mb506ObtenerEventos(dtpDesde.Value, dtpHasta.Value, txtUsuario.Text, txtAccion.Text);

            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506FrmBitacora_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            mb506Cargar(sender, e);
        }
    }
}
