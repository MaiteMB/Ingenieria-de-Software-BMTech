namespace mb506.BMTech
{
    partial class mb506FrmPago
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblCampo0 = new System.Windows.Forms.Label();
            this.lblCampo1 = new System.Windows.Forms.Label();
            this.lblCampo2 = new System.Windows.Forms.Label();
            this.lblCampo3 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.cmbMedio = new System.Windows.Forms.ComboBox();
            this.nudImporte = new System.Windows.Forms.NumericUpDown();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.chkVerificado = new System.Windows.Forms.CheckBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudImporte)).BeginInit();
            this.SuspendLayout();
            // lblCampo0
            this.lblCampo0.Location = new System.Drawing.Point(20, 25);
            this.lblCampo0.Size = new System.Drawing.Size(175, 25);
            this.lblCampo0.Text = "Total";
            this.lblCampo0.Name = "lblCampo0";
            this.lblCampo0.TabIndex = 0;
            // lblCampo1
            this.lblCampo1.Location = new System.Drawing.Point(20, 75);
            this.lblCampo1.Size = new System.Drawing.Size(175, 25);
            this.lblCampo1.Text = "Medio de pago";
            this.lblCampo1.Name = "lblCampo1";
            this.lblCampo1.TabIndex = 1;
            // lblCampo2
            this.lblCampo2.Location = new System.Drawing.Point(20, 125);
            this.lblCampo2.Size = new System.Drawing.Size(175, 25);
            this.lblCampo2.Text = "Importe";
            this.lblCampo2.Name = "lblCampo2";
            this.lblCampo2.TabIndex = 2;
            // lblCampo3
            this.lblCampo3.Location = new System.Drawing.Point(20, 175);
            this.lblCampo3.Size = new System.Drawing.Size(175, 25);
            this.lblCampo3.Text = "Numero de operacion";
            this.lblCampo3.Name = "lblCampo3";
            this.lblCampo3.TabIndex = 3;
            // lblTotal
            this.lblTotal.Location = new System.Drawing.Point(200, 25);
            this.lblTotal.Size = new System.Drawing.Size(240, 25);
            this.lblTotal.Text = "0,00";
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.TabIndex = 4;
            // cmbMedio
            this.cmbMedio.Location = new System.Drawing.Point(200, 70);
            this.cmbMedio.Size = new System.Drawing.Size(240, 28);
            this.cmbMedio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMedio.Items.AddRange(new object[] { "Efectivo", "Transferencia" });
            this.cmbMedio.SelectedIndex = 0;
            this.cmbMedio.Name = "cmbMedio";
            this.cmbMedio.TabIndex = 5;
            // nudImporte
            this.nudImporte.Location = new System.Drawing.Point(200, 120);
            this.nudImporte.Size = new System.Drawing.Size(240, 28);
            this.nudImporte.DecimalPlaces = 2;
            this.nudImporte.Maximum = 9999999999999999.99m;
            this.nudImporte.Name = "nudImporte";
            this.nudImporte.TabIndex = 6;
            // txtOperacion
            this.txtOperacion.Location = new System.Drawing.Point(200, 170);
            this.txtOperacion.Size = new System.Drawing.Size(240, 28);
            this.txtOperacion.MaxLength = 100;
            this.txtOperacion.Enabled = false;
            this.txtOperacion.Name = "txtOperacion";
            this.txtOperacion.TabIndex = 7;
            // chkVerificado
            this.chkVerificado.Location = new System.Drawing.Point(20, 220);
            this.chkVerificado.Size = new System.Drawing.Size(420, 32);
            this.chkVerificado.Text = "Pago verificado por el empleado";
            this.chkVerificado.Name = "chkVerificado";
            this.chkVerificado.TabIndex = 8;
            // btnAceptar
            this.btnAceptar.Location = new System.Drawing.Point(200, 285);
            this.btnAceptar.Size = new System.Drawing.Size(115, 38);
            this.btnAceptar.BackColor = System.Drawing.Color.Teal;
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Text = "Confirmar";
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.TabIndex = 9;
            // btnCancelar
            this.btnCancelar.Location = new System.Drawing.Point(330, 285);
            this.btnCancelar.Size = new System.Drawing.Size(110, 38);
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.TabIndex = 10;
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblCampo0, this.lblCampo1, this.lblCampo2, this.lblCampo3, this.lblTotal, this.cmbMedio, this.nudImporte, this.txtOperacion, this.chkVerificado, this.btnAceptar, this.btnCancelar });
            this.btnAceptar.Click += new System.EventHandler(this.mb506Aceptar);
            this.cmbMedio.SelectedIndexChanged += new System.EventHandler(this.mb506MedioCambiado);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(470, 350);
            this.MinimumSize = new System.Drawing.Size(486, 389);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AcceptButton = this.btnAceptar;
            this.CancelButton = this.btnCancelar;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPago";
            this.Text = "Pago de la venta";
            ((System.ComponentModel.ISupportInitialize)(this.nudImporte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblCampo0;
        private System.Windows.Forms.Label lblCampo1;
        private System.Windows.Forms.Label lblCampo2;
        private System.Windows.Forms.Label lblCampo3;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ComboBox cmbMedio;
        private System.Windows.Forms.NumericUpDown nudImporte;
        private System.Windows.Forms.TextBox txtOperacion;
        private System.Windows.Forms.CheckBox chkVerificado;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
