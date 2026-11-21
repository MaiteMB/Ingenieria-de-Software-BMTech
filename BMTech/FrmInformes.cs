using System;
using System.ComponentModel;
using System.Windows.Forms;
using mb506.BEBMTech.Informes;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmInformes : Form
    {
        private readonly BLLInformes bllInformes = new BLLInformes();
        private InformeOperaciones actual;
        public mb506FrmInformes()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }
        private void mb506FrmInformes_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;
        }
        private void mb506Cargar(string tipo, DateTime desde, DateTime hasta)
        {
            actual = null;
            dgvInforme.DataSource = null;
            txtResumen.Clear();
            actual = bllInformes.mb506Generar(tipo, desde, hasta);
            dgvInforme.DataSource = actual.datos;
            txtResumen.Text = actual.cantidad + " operaciones | Cobrado: " + actual.totalCobrado.ToString("N2") +
                " | Pendiente: " + actual.totalPendiente.ToString("N2");
        }
        private void mb506Buscar(object sender, EventArgs e)
        {
            try { mb506Cargar(Convert.ToString(cmbTipo.SelectedItem), dtpDesde.Value, dtpHasta.Value); }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
        private void mb506Exportar(object sender, EventArgs e)
        {
            try
            {
                if (actual == null) throw new Exception("Genere el informe primero.");
                string tipo = actual.tipo;
                DateTime desde = actual.desde, hasta = actual.hasta;
                mb506Cargar(tipo, desde, hasta);
                dlgInformePdf.FileName = "BMTech_Informe_" + desde.ToString("yyyyMMdd") + "_" + hasta.ToString("yyyyMMdd") + ".pdf";
                if (dlgInformePdf.ShowDialog(this) == DialogResult.OK)
                {
                    ExportacionComprobante.mb506GuardarPdf(bllInformes.mb506PrepararTexto(actual), dlgInformePdf.FileName, "BMTech - Informe de operaciones");
                    new BLLLog().mb506RegistrarEvento("Informe exportado: " + tipo, "Informes", 1);
                }
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
        private void mb506Cerrar(object sender, EventArgs e) { Close(); }
    }
}
