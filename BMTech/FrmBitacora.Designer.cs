namespace mb506.BMTech
{
    partial class mb506FrmBitacora
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
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblAccion = new System.Windows.Forms.Label();
            this.txtAccion = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvEventos = new System.Windows.Forms.DataGridView();
            this.colEventosIdLog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventosEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventosFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventosAccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventosModulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventosCriticidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewCellStyle styleColEventosFecha = new System.Windows.Forms.DataGridViewCellStyle();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventos)).BeginInit();
            this.SuspendLayout();
            // lblDesde
            this.lblDesde.Location = new System.Drawing.Point(20, 15);
            this.lblDesde.Size = new System.Drawing.Size(150, 25);
            this.lblDesde.Text = "Desde";
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.TabIndex = 0;
            // dtpDesde
            this.dtpDesde.Location = new System.Drawing.Point(20, 45);
            this.dtpDesde.Size = new System.Drawing.Size(150, 28);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.TabIndex = 1;
            // lblHasta
            this.lblHasta.Location = new System.Drawing.Point(190, 15);
            this.lblHasta.Size = new System.Drawing.Size(150, 25);
            this.lblHasta.Text = "Hasta";
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.TabIndex = 2;
            // dtpHasta
            this.dtpHasta.Location = new System.Drawing.Point(190, 45);
            this.dtpHasta.Size = new System.Drawing.Size(150, 28);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.TabIndex = 3;
            // lblUsuario
            this.lblUsuario.Location = new System.Drawing.Point(360, 15);
            this.lblUsuario.Size = new System.Drawing.Size(245, 25);
            this.lblUsuario.Text = "Usuario";
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.TabIndex = 4;
            // txtUsuario
            this.txtUsuario.Location = new System.Drawing.Point(360, 45);
            this.txtUsuario.Size = new System.Drawing.Size(245, 28);
            this.txtUsuario.MaxLength = 150;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.TabIndex = 5;
            // lblAccion
            this.lblAccion.Location = new System.Drawing.Point(625, 15);
            this.lblAccion.Size = new System.Drawing.Size(245, 25);
            this.lblAccion.Text = "Actividad";
            this.lblAccion.Name = "lblAccion";
            this.lblAccion.TabIndex = 6;
            // txtAccion
            this.txtAccion.Location = new System.Drawing.Point(625, 45);
            this.txtAccion.Size = new System.Drawing.Size(245, 28);
            this.txtAccion.MaxLength = 255;
            this.txtAccion.Name = "txtAccion";
            this.txtAccion.TabIndex = 7;
            // btnBuscar
            this.btnBuscar.Location = new System.Drawing.Point(950, 40);
            this.btnBuscar.Size = new System.Drawing.Size(130, 36);
            this.btnBuscar.BackColor = System.Drawing.Color.Teal;
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.TabIndex = 8;
            // dgvEventos
            this.dgvEventos.Location = new System.Drawing.Point(20, 95);
            this.dgvEventos.Size = new System.Drawing.Size(1060, 440);
            this.dgvEventos.AllowUserToAddRows = false;
            this.dgvEventos.AllowUserToDeleteRows = false;
            this.dgvEventos.AutoGenerateColumns = false;
            this.dgvEventos.ReadOnly = true;
            this.dgvEventos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEventos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvEventos.Name = "dgvEventos";
            this.dgvEventos.TabIndex = 9;
            this.colEventosIdLog.DataPropertyName = "idLog";
            this.colEventosIdLog.HeaderText = "idLog";
            this.colEventosIdLog.Name = "idLog";
            this.colEventosEmail.DataPropertyName = "email";
            this.colEventosEmail.HeaderText = "email";
            this.colEventosEmail.Name = "email";
            this.colEventosFecha.DataPropertyName = "fecha";
            this.colEventosFecha.HeaderText = "fecha";
            this.colEventosFecha.Name = "fecha";
            this.colEventosAccion.DataPropertyName = "accion";
            this.colEventosAccion.HeaderText = "accion";
            this.colEventosAccion.Name = "accion";
            this.colEventosModulo.DataPropertyName = "modulo";
            this.colEventosModulo.HeaderText = "modulo";
            this.colEventosModulo.Name = "modulo";
            this.colEventosCriticidad.DataPropertyName = "criticidad";
            this.colEventosCriticidad.HeaderText = "criticidad";
            this.colEventosCriticidad.Name = "criticidad";
            styleColEventosFecha.Format = "dd/MM/yyyy HH:mm:ss";
            this.colEventosFecha.DefaultCellStyle = styleColEventosFecha;
            this.dgvEventos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colEventosIdLog, this.colEventosEmail, this.colEventosFecha, this.colEventosAccion, this.colEventosModulo, this.colEventosCriticidad });
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblDesde, this.dtpDesde, this.lblHasta, this.dtpHasta, this.lblUsuario, this.txtUsuario, this.lblAccion, this.txtAccion, this.btnBuscar, this.dgvEventos });
            this.btnBuscar.Click += new System.EventHandler(this.mb506Cargar);
            this.Load += new System.EventHandler(this.mb506FrmBitacora_Load);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1100, 560);
            this.MinimumSize = new System.Drawing.Size(1116, 599);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FrmBitacora";
            this.Text = "Bitacora";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblAccion;
        private System.Windows.Forms.TextBox txtAccion;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView dgvEventos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventosIdLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventosEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventosFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventosAccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventosModulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventosCriticidad;
    }
}
