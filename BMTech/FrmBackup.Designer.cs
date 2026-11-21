namespace mb506.BMTech
{
    partial class mb506FrmBackup
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
            this.dgvBackups = new System.Windows.Forms.DataGridView();
            this.colBackupsIdBackup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBackupsFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBackupsUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBackupsRuta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            System.Windows.Forms.DataGridViewCellStyle styleColBackupsFecha = new System.Windows.Forms.DataGridViewCellStyle();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).BeginInit();
            this.SuspendLayout();
            // dgvBackups
            this.dgvBackups.Location = new System.Drawing.Point(20, 20);
            this.dgvBackups.Size = new System.Drawing.Size(910, 405);
            this.dgvBackups.AllowUserToAddRows = false;
            this.dgvBackups.AllowUserToDeleteRows = false;
            this.dgvBackups.AutoGenerateColumns = false;
            this.dgvBackups.ReadOnly = true;
            this.dgvBackups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBackups.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvBackups.Name = "dgvBackups";
            this.dgvBackups.TabIndex = 0;
            // btnCrear
            this.btnCrear.Location = new System.Drawing.Point(20, 450);
            this.btnCrear.Size = new System.Drawing.Size(160, 36);
            this.btnCrear.BackColor = System.Drawing.Color.Teal;
            this.btnCrear.ForeColor = System.Drawing.Color.White;
            this.btnCrear.UseVisualStyleBackColor = false;
            this.btnCrear.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCrear.Text = "Crear copia";
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.TabIndex = 1;
            // btnRestaurar
            this.btnRestaurar.Location = new System.Drawing.Point(200, 450);
            this.btnRestaurar.Size = new System.Drawing.Size(160, 36);
            this.btnRestaurar.BackColor = System.Drawing.Color.Teal;
            this.btnRestaurar.ForeColor = System.Drawing.Color.White;
            this.btnRestaurar.UseVisualStyleBackColor = false;
            this.btnRestaurar.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRestaurar.Text = "Restaurar copia";
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.TabIndex = 2;
            // btnActualizar
            this.btnActualizar.Location = new System.Drawing.Point(380, 450);
            this.btnActualizar.Size = new System.Drawing.Size(160, 36);
            this.btnActualizar.BackColor = System.Drawing.Color.Teal;
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.TabIndex = 3;
            this.colBackupsIdBackup.DataPropertyName = "idBackup";
            this.colBackupsIdBackup.HeaderText = "idBackup";
            this.colBackupsIdBackup.Name = "idBackup";
            this.colBackupsFecha.DataPropertyName = "fecha";
            this.colBackupsFecha.HeaderText = "fecha";
            this.colBackupsFecha.Name = "fecha";
            this.colBackupsUsuario.DataPropertyName = "usuario";
            this.colBackupsUsuario.HeaderText = "usuario";
            this.colBackupsUsuario.Name = "usuario";
            this.colBackupsRuta.DataPropertyName = "ruta";
            this.colBackupsRuta.HeaderText = "ruta";
            this.colBackupsRuta.Name = "ruta";
            styleColBackupsFecha.Format = "dd/MM/yyyy HH:mm:ss";
            this.colBackupsFecha.DefaultCellStyle = styleColBackupsFecha;
            this.dgvBackups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colBackupsIdBackup, this.colBackupsFecha, this.colBackupsUsuario, this.colBackupsRuta });
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.dgvBackups, this.btnCrear, this.btnRestaurar, this.btnActualizar });
            this.btnCrear.Click += new System.EventHandler(this.mb506Crear);
            this.btnRestaurar.Click += new System.EventHandler(this.mb506Restaurar);
            this.btnActualizar.Click += new System.EventHandler(this.mb506Actualizar);
            this.Load += new System.EventHandler(this.mb506FrmBackup_Load);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(950, 510);
            this.MinimumSize = new System.Drawing.Size(966, 549);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FrmBackup";
            this.Text = "Copias de seguridad";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBackups;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBackupsIdBackup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBackupsFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBackupsUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBackupsRuta;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnActualizar;
    }
}
