namespace mb506.BMTech
{
    partial class mb506FrmIdiomas
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
            this.cmbIdiomas = new System.Windows.Forms.ComboBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.dgvTextos = new System.Windows.Forms.DataGridView();
            this.colTextosEtiqueta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTextosTextoTraducido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTextos)).BeginInit();
            this.SuspendLayout();
            // cmbIdiomas
            this.cmbIdiomas.Location = new System.Drawing.Point(20, 20);
            this.cmbIdiomas.Size = new System.Drawing.Size(250, 30);
            this.cmbIdiomas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIdiomas.Name = "cmbIdiomas";
            this.cmbIdiomas.TabIndex = 0;
            // lblCodigo
            this.lblCodigo.Location = new System.Drawing.Point(300, 24);
            this.lblCodigo.Size = new System.Drawing.Size(140, 25);
            this.lblCodigo.Text = "Codigo de idioma";
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.TabIndex = 1;
            // txtCodigo
            this.txtCodigo.Location = new System.Drawing.Point(440, 20);
            this.txtCodigo.Size = new System.Drawing.Size(65, 30);
            this.txtCodigo.MaxLength = 2;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.TabIndex = 2;
            // lblNombre
            this.lblNombre.Location = new System.Drawing.Point(525, 24);
            this.lblNombre.Size = new System.Drawing.Size(75, 25);
            this.lblNombre.Text = "Nombre";
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.TabIndex = 3;
            // txtNombre
            this.txtNombre.Location = new System.Drawing.Point(605, 20);
            this.txtNombre.Size = new System.Drawing.Size(320, 30);
            this.txtNombre.MaxLength = 50;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.TabIndex = 4;
            // dgvTextos
            this.dgvTextos.Location = new System.Drawing.Point(20, 75);
            this.dgvTextos.Size = new System.Drawing.Size(905, 510);
            this.dgvTextos.AllowUserToAddRows = false;
            this.dgvTextos.AllowUserToDeleteRows = false;
            this.dgvTextos.AutoGenerateColumns = false;
            this.dgvTextos.ReadOnly = false;
            this.dgvTextos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTextos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvTextos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTextos.Name = "dgvTextos";
            this.dgvTextos.TabIndex = 5;
            // btnNuevo
            this.btnNuevo.Location = new System.Drawing.Point(20, 605);
            this.btnNuevo.Size = new System.Drawing.Size(135, 36);
            this.btnNuevo.BackColor = System.Drawing.Color.Teal;
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.TabIndex = 6;
            // btnGuardar
            this.btnGuardar.Location = new System.Drawing.Point(170, 605);
            this.btnGuardar.Size = new System.Drawing.Size(135, 36);
            this.btnGuardar.BackColor = System.Drawing.Color.Teal;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.TabIndex = 7;
            // btnEliminar
            this.btnEliminar.Location = new System.Drawing.Point(320, 605);
            this.btnEliminar.Size = new System.Drawing.Size(135, 36);
            this.btnEliminar.BackColor = System.Drawing.Color.Teal;
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.TabIndex = 8;
            this.colTextosEtiqueta.DataPropertyName = "etiqueta";
            this.colTextosEtiqueta.HeaderText = "Texto original";
            this.colTextosEtiqueta.Name = "etiqueta";
            this.colTextosEtiqueta.ReadOnly = true;
            this.colTextosTextoTraducido.DataPropertyName = "textoTraducido";
            this.colTextosTextoTraducido.HeaderText = "Traduccion";
            this.colTextosTextoTraducido.Name = "textoTraducido";
            this.colTextosTextoTraducido.ReadOnly = false;
            this.dgvTextos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colTextosEtiqueta, this.colTextosTextoTraducido });
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.cmbIdiomas, this.lblCodigo, this.txtCodigo, this.lblNombre, this.txtNombre, this.dgvTextos, this.btnNuevo, this.btnGuardar, this.btnEliminar });
            this.cmbIdiomas.SelectedIndexChanged += new System.EventHandler(this.mb506Seleccionar);
            this.btnNuevo.Click += new System.EventHandler(this.mb506Nuevo);
            this.btnGuardar.Click += new System.EventHandler(this.mb506Guardar);
            this.btnEliminar.Click += new System.EventHandler(this.mb506Eliminar);
            this.Load += new System.EventHandler(this.mb506FrmIdiomas_Load);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(950, 660);
            this.MinimumSize = new System.Drawing.Size(966, 699);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FrmIdiomas";
            this.Text = "Idiomas y traducciones";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTextos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbIdiomas;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.DataGridView dgvTextos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTextosEtiqueta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTextosTextoTraducido;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
    }
}
