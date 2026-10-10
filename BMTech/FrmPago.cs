using System;
using System.Drawing;
using System.Windows.Forms;
using mb506.BEBMTech.Venta;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmPago : Form
    {
        private ComboBox cmbMedio = new ComboBox();
        private NumericUpDown nudImporte = new NumericUpDown();
        private TextBox txtOperacion = new TextBox();
        private CheckBox chkVerificado = new CheckBox();
        private decimal total;
        private bool exigirVerificado;
        public Pago pago { get; private set; }

        public mb506FrmPago() : this(0, false) { }

        public mb506FrmPago(decimal importe, bool confirmarPendiente)
        {
            IdiomasFormulario.mb506Vincular(this);
            total = importe;
            exigirVerificado = confirmarPendiente;
            Text = "Pago de la venta";
            ClientSize = new Size(470, 350);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);
            BackColor = Color.White;
            string[] nombres = { "Total", "Medio de pago", "Importe", "Numero de operacion" };
            for (int i = 0; i < nombres.Length; i++)
                Controls.Add(new Label { Text = nombres[i], AutoSize = true, Left = 20, Top = 25 + i * 50 });
            Controls.Add(new Label { Name = "lblTotal", Text = importe.ToString("N2"), Left = 200, Top = 25, Width = 240 });
            cmbMedio.SetBounds(200, 70, 240, 28);
            cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMedio.Items.AddRange(new object[] { "Efectivo", "Transferencia" });
            cmbMedio.SelectedIndex = 0;
            nudImporte.SetBounds(200, 120, 240, 28);
            nudImporte.DecimalPlaces = 2;
            nudImporte.Maximum = 9999999999999999.99m;
            nudImporte.Value = importe;
            txtOperacion.SetBounds(200, 170, 240, 28);
            txtOperacion.MaxLength = 100;
            txtOperacion.Enabled = false;
            chkVerificado.Text = "Pago verificado por el empleado";
            chkVerificado.SetBounds(20, 220, 420, 32);
            cmbMedio.SelectedIndexChanged += delegate { txtOperacion.Enabled = cmbMedio.SelectedIndex == 1; };
            Button aceptar = new Button { Text = "Confirmar", BackColor = Color.Teal, ForeColor = Color.White };
            aceptar.SetBounds(200, 285, 115, 38);
            aceptar.Click += mb506Aceptar;
            Button cancelar = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel };
            cancelar.SetBounds(330, 285, 110, 38);
            Controls.AddRange(new Control[] { cmbMedio, nudImporte, txtOperacion, chkVerificado, aceptar, cancelar });
            AcceptButton = aceptar;
            CancelButton = cancelar;
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
    }
}
