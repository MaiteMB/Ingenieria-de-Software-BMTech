namespace BMTech
{
    partial class FrmControlCambiosProducto
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvVersiones = new System.Windows.Forms.DataGridView();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.lblVersiones = new System.Windows.Forms.Label();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVersiones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Teal;
            this.pnlHeader.Controls.Add(this.lblSubtitulo);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1050, 95);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.White;
            this.lblSubtitulo.Location = new System.Drawing.Point(38, 58);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(333, 19);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Historial de modificaciones y restauración de producto";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(35, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(398, 37);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Control de cambios de producto";
            // 
            // dgvVersiones
            // 
            this.dgvVersiones.AllowUserToAddRows = false;
            this.dgvVersiones.AllowUserToDeleteRows = false;
            this.dgvVersiones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVersiones.BackgroundColor = System.Drawing.Color.White;
            this.dgvVersiones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVersiones.Location = new System.Drawing.Point(35, 150);
            this.dgvVersiones.MultiSelect = false;
            this.dgvVersiones.Name = "dgvVersiones";
            this.dgvVersiones.ReadOnly = true;
            this.dgvVersiones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVersiones.Size = new System.Drawing.Size(960, 210);
            this.dgvVersiones.TabIndex = 1;
            this.dgvVersiones.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVersiones_CellClick);
            // 
            // dgvDetalle
            // 
            this.dgvDetalle.AllowUserToAddRows = false;
            this.dgvDetalle.AllowUserToDeleteRows = false;
            this.dgvDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetalle.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Location = new System.Drawing.Point(35, 410);
            this.dgvDetalle.MultiSelect = false;
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.ReadOnly = true;
            this.dgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalle.Size = new System.Drawing.Size(960, 160);
            this.dgvDetalle.TabIndex = 2;
            // 
            // lblVersiones
            // 
            this.lblVersiones.AutoSize = true;
            this.lblVersiones.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblVersiones.ForeColor = System.Drawing.Color.Teal;
            this.lblVersiones.Location = new System.Drawing.Point(35, 120);
            this.lblVersiones.Name = "lblVersiones";
            this.lblVersiones.Size = new System.Drawing.Size(189, 25);
            this.lblVersiones.TabIndex = 3;
            this.lblVersiones.Text = "Versiones registradas";
            // 
            // lblDetalle
            // 
            this.lblDetalle.AutoSize = true;
            this.lblDetalle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblDetalle.ForeColor = System.Drawing.Color.Teal;
            this.lblDetalle.Location = new System.Drawing.Point(35, 380);
            this.lblDetalle.Name = "lblDetalle";
            this.lblDetalle.Size = new System.Drawing.Size(173, 25);
            this.lblDetalle.TabIndex = 4;
            this.lblDetalle.Text = "Detalle del cambio";
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.Teal;
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.Location = new System.Drawing.Point(35, 595);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(145, 38);
            this.btnActualizar.TabIndex = 5;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnRestaurar.FlatAppearance.BorderSize = 0;
            this.btnRestaurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestaurar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRestaurar.ForeColor = System.Drawing.Color.White;
            this.btnRestaurar.Location = new System.Drawing.Point(200, 595);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(175, 38);
            this.btnRestaurar.TabIndex = 6;
            this.btnRestaurar.Text = "Restaurar versión";
            this.btnRestaurar.UseVisualStyleBackColor = false;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMensaje.ForeColor = System.Drawing.Color.Teal;
            this.lblMensaje.Location = new System.Drawing.Point(400, 604);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(0, 19);
            this.lblMensaje.TabIndex = 7;
            // 
            // FrmControlCambiosProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnRestaurar);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.lblDetalle);
            this.Controls.Add(this.lblVersiones);
            this.Controls.Add(this.dgvDetalle);
            this.Controls.Add(this.dgvVersiones);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FrmControlCambiosProducto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control de cambios de producto";
            this.Load += new System.EventHandler(this.FrmControlCambiosProducto_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVersiones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvVersiones;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Label lblVersiones;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Label lblMensaje;
    }
}
