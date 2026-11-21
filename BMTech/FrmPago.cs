using System;
using System.Windows.Forms;
using mb506.BEBMTech.Venta;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmPago : Form
    {
        private decimal total;
        private bool exigirVerificado;
        public Pago pago { get; private set; }

        public mb506FrmPago()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }

        public mb506FrmPago(decimal importe, bool confirmarPendiente) : this()
        {
            total = importe;
            exigirVerificado = confirmarPendiente;
            lblTotal.Text = importe.ToString("N2");
            nudImporte.Value = importe;
        }

        private void mb506Aceptar(object sender, EventArgs e)
        {
            try
            {
                Pago nuevo = new Pago { medioPago = cmbMedio.SelectedIndex == 0 ? "Efectivo" : "Transferencia",
                    importe = nudImporte.Value, numeroOperacion = txtOperacion.Enabled ? txtOperacion.Text.Trim() : "",
                    verificado = chkVerificado.Checked };
                new BLLVenta().mb506ValidarPago(nuevo, total, exigirVerificado);
                if (!nuevo.verificado && Mensajes.mb506Mostrar("El pago no esta verificado o el importe no coincide. Guardar venta pendiente?",
                    "Pago pendiente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                pago = nuevo;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506MedioCambiado(object sender, EventArgs e)
        {
            txtOperacion.Enabled = cmbMedio.SelectedIndex == 1;
        }
    }
}
