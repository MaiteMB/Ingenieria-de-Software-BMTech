namespace mb506.BMTech
{
    partial class mb506FrmCambiarPassword
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
            this.lblActual = new System.Windows.Forms.Label();
            this.txtActual = new System.Windows.Forms.TextBox();
            this.lblNueva = new System.Windows.Forms.Label();
            this.txtNueva = new System.Windows.Forms.TextBox();
            this.lblConfirmacion = new System.Windows.Forms.Label();
            this.txtConfirmacion = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // lblActual
            this.lblActual.Location = new System.Drawing.Point(20, 25);
            this.lblActual.Size = new System.Drawing.Size(130, 25);
            this.lblActual.Text = "Clave actual";
            this.lblActual.Name = "lblActual";
            this.lblActual.TabIndex = 0;
            // txtActual
            this.txtActual.Location = new System.Drawing.Point(160, 25);
            this.txtActual.Size = new System.Drawing.Size(250, 25);
            this.txtActual.UseSystemPasswordChar = true;
            this.txtActual.Name = "txtActual";
            this.txtActual.TabIndex = 1;
            // lblNueva
            this.lblNueva.Location = new System.Drawing.Point(20, 70);
            this.lblNueva.Size = new System.Drawing.Size(130, 25);
            this.lblNueva.Text = "Nueva clave";
            this.lblNueva.Name = "lblNueva";
            this.lblNueva.TabIndex = 2;
            // txtNueva
            this.txtNueva.Location = new System.Drawing.Point(160, 70);
            this.txtNueva.Size = new System.Drawing.Size(250, 25);
            this.txtNueva.UseSystemPasswordChar = true;
            this.txtNueva.Name = "txtNueva";
            this.txtNueva.TabIndex = 3;
            // lblConfirmacion
            this.lblConfirmacion.Location = new System.Drawing.Point(20, 115);
            this.lblConfirmacion.Size = new System.Drawing.Size(130, 25);
            this.lblConfirmacion.Text = "Confirmar clave";
            this.lblConfirmacion.Name = "lblConfirmacion";
            this.lblConfirmacion.TabIndex = 4;
            // txtConfirmacion
            this.txtConfirmacion.Location = new System.Drawing.Point(160, 115);
            this.txtConfirmacion.Size = new System.Drawing.Size(250, 25);
            this.txtConfirmacion.UseSystemPasswordChar = true;
            this.txtConfirmacion.Name = "txtConfirmacion";
            this.txtConfirmacion.TabIndex = 5;
            // btnGuardar
            this.btnGuardar.Location = new System.Drawing.Point(270, 170);
            this.btnGuardar.Size = new System.Drawing.Size(140, 35);
            this.btnGuardar.BackColor = System.Drawing.Color.Teal;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.TabIndex = 6;
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblActual, this.txtActual, this.lblNueva, this.txtNueva, this.lblConfirmacion, this.txtConfirmacion, this.btnGuardar });
            this.btnGuardar.Click += new System.EventHandler(this.mb506Guardar);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(440, 230);
            this.MinimumSize = new System.Drawing.Size(456, 269);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.AcceptButton = this.btnGuardar;
            this.Name = "FrmCambiarPassword";
            this.Text = "Cambiar contraseña";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblActual;
        private System.Windows.Forms.TextBox txtActual;
        private System.Windows.Forms.Label lblNueva;
        private System.Windows.Forms.TextBox txtNueva;
        private System.Windows.Forms.Label lblConfirmacion;
        private System.Windows.Forms.TextBox txtConfirmacion;
        private System.Windows.Forms.Button btnGuardar;
    }
}
