namespace mb506.BMTech
{
    partial class mb506FrmPermisos
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
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.dgvElementos = new System.Windows.Forms.DataGridView();
            this.colElementosId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colElementosNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblId = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnAsignar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.lstAsignaciones = new System.Windows.Forms.CheckedListBox();
            this.lblEstructura = new System.Windows.Forms.Label();
            this.tvPermisos = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvElementos)).BeginInit();
            this.SuspendLayout();
            // cmbTipo
            this.cmbTipo.Location = new System.Drawing.Point(20, 20);
            this.cmbTipo.Size = new System.Drawing.Size(260, 30);
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.Items.AddRange(new object[] { "Perfil", "Familia", "Patente" });
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.TabIndex = 0;
            // dgvElementos
            this.dgvElementos.Location = new System.Drawing.Point(20, 70);
            this.dgvElementos.Size = new System.Drawing.Size(390, 400);
            this.dgvElementos.AllowUserToAddRows = false;
            this.dgvElementos.AllowUserToDeleteRows = false;
            this.dgvElementos.AutoGenerateColumns = false;
            this.dgvElementos.ReadOnly = true;
            this.dgvElementos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvElementos.MultiSelect = false;
            this.dgvElementos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvElementos.Name = "dgvElementos";
            this.dgvElementos.TabIndex = 1;
            // lblId
            this.lblId.Location = new System.Drawing.Point(440, 70);
            this.lblId.Size = new System.Drawing.Size(140, 25);
            this.lblId.Text = "Identificador";
            this.lblId.Name = "lblId";
            this.lblId.TabIndex = 2;
            // txtId
            this.txtId.Location = new System.Drawing.Point(580, 70);
            this.txtId.Size = new System.Drawing.Size(240, 28);
            this.txtId.Name = "txtId";
            this.txtId.TabIndex = 3;
            // lblNombre
            this.lblNombre.Location = new System.Drawing.Point(440, 110);
            this.lblNombre.Size = new System.Drawing.Size(140, 25);
            this.lblNombre.Text = "Nombre";
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.TabIndex = 4;
            // txtNombre
            this.txtNombre.Location = new System.Drawing.Point(580, 110);
            this.txtNombre.Size = new System.Drawing.Size(240, 28);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.TabIndex = 5;
            // btnNuevo
            this.btnNuevo.Location = new System.Drawing.Point(440, 160);
            this.btnNuevo.Size = new System.Drawing.Size(180, 36);
            this.btnNuevo.BackColor = System.Drawing.Color.Teal;
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.TabIndex = 6;
            // btnGuardar
            this.btnGuardar.Location = new System.Drawing.Point(630, 160);
            this.btnGuardar.Size = new System.Drawing.Size(180, 36);
            this.btnGuardar.BackColor = System.Drawing.Color.Teal;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Text = "Guardar datos";
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.TabIndex = 7;
            // btnAsignar
            this.btnAsignar.Location = new System.Drawing.Point(440, 435);
            this.btnAsignar.Size = new System.Drawing.Size(250, 36);
            this.btnAsignar.BackColor = System.Drawing.Color.Teal;
            this.btnAsignar.ForeColor = System.Drawing.Color.White;
            this.btnAsignar.UseVisualStyleBackColor = false;
            this.btnAsignar.Text = "Guardar asignaciones";
            this.btnAsignar.Name = "btnAsignar";
            this.btnAsignar.TabIndex = 8;
            // btnEliminar
            this.btnEliminar.Location = new System.Drawing.Point(300, 20);
            this.btnEliminar.Size = new System.Drawing.Size(110, 36);
            this.btnEliminar.BackColor = System.Drawing.Color.Teal;
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.TabIndex = 9;
            // lstAsignaciones
            this.lstAsignaciones.Location = new System.Drawing.Point(440, 220);
            this.lstAsignaciones.Size = new System.Drawing.Size(380, 190);
            this.lstAsignaciones.CheckOnClick = true;
            this.lstAsignaciones.Name = "lstAsignaciones";
            this.lstAsignaciones.TabIndex = 10;
            // lblEstructura
            this.lblEstructura.Location = new System.Drawing.Point(850, 25);
            this.lblEstructura.Size = new System.Drawing.Size(295, 25);
            this.lblEstructura.Text = "Estructura de permisos";
            this.lblEstructura.Name = "lblEstructura";
            this.lblEstructura.TabIndex = 11;
            // tvPermisos
            this.tvPermisos.Location = new System.Drawing.Point(850, 70);
            this.tvPermisos.Size = new System.Drawing.Size(295, 400);
            this.tvPermisos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.tvPermisos.Name = "tvPermisos";
            this.tvPermisos.TabIndex = 12;
            this.colElementosId.DataPropertyName = "id";
            this.colElementosId.HeaderText = "id";
            this.colElementosId.Name = "id";
            this.colElementosNombre.DataPropertyName = "nombre";
            this.colElementosNombre.HeaderText = "nombre";
            this.colElementosNombre.Name = "nombre";
            this.dgvElementos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colElementosId, this.colElementosNombre });
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.cmbTipo, this.dgvElementos, this.lblId, this.txtId, this.lblNombre, this.txtNombre, this.btnNuevo, this.btnGuardar, this.btnAsignar, this.btnEliminar, this.lstAsignaciones, this.lblEstructura, this.tvPermisos });
            this.dgvElementos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mb506Seleccionar);
            this.btnNuevo.Click += new System.EventHandler(this.mb506Nuevo);
            this.btnGuardar.Click += new System.EventHandler(this.mb506GuardarDatos);
            this.btnAsignar.Click += new System.EventHandler(this.mb506GuardarAsignaciones);
            this.btnEliminar.Click += new System.EventHandler(this.mb506Eliminar);
            this.cmbTipo.SelectedIndexChanged += new System.EventHandler(this.mb506TipoCambiado);
            this.Load += new System.EventHandler(this.mb506FrmPermisos_Load);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1170, 500);
            this.MinimumSize = new System.Drawing.Size(1186, 539);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FrmPermisos";
            this.Text = "Perfiles, familias y patentes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvElementos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.DataGridView dgvElementos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElementosId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElementosNombre;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnAsignar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.CheckedListBox lstAsignaciones;
        private System.Windows.Forms.Label lblEstructura;
        private System.Windows.Forms.TreeView tvPermisos;
    }
}
