namespace mb506.BMTech
{
    partial class mb506FrmUsuarios
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
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.cmbPerfil = new System.Windows.Forms.ComboBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnBloquear = new System.Windows.Forms.Button();
            this.btnDesbloquear = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.colUsuariosEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuariosNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuariosApellido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuariosPerfil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuariosActivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuariosIntentos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();
            // lblEmail
            this.lblEmail.Location = new System.Drawing.Point(20, 20);
            this.lblEmail.Size = new System.Drawing.Size(130, 30);
            this.lblEmail.Text = "Email";
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.TabIndex = 0;
            // txtEmail
            this.txtEmail.Location = new System.Drawing.Point(160, 20);
            this.txtEmail.Size = new System.Drawing.Size(350, 30);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.TabIndex = 1;
            // lblNombre
            this.lblNombre.Location = new System.Drawing.Point(20, 60);
            this.lblNombre.Size = new System.Drawing.Size(130, 30);
            this.lblNombre.Text = "Nombre";
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.TabIndex = 2;
            // txtNombre
            this.txtNombre.Location = new System.Drawing.Point(160, 60);
            this.txtNombre.Size = new System.Drawing.Size(350, 30);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.TabIndex = 3;
            // lblApellido
            this.lblApellido.Location = new System.Drawing.Point(20, 100);
            this.lblApellido.Size = new System.Drawing.Size(130, 30);
            this.lblApellido.Text = "Apellido";
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.TabIndex = 4;
            // txtApellido
            this.txtApellido.Location = new System.Drawing.Point(160, 100);
            this.txtApellido.Size = new System.Drawing.Size(350, 30);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.TabIndex = 5;
            // lblPassword
            this.lblPassword.Location = new System.Drawing.Point(20, 140);
            this.lblPassword.Size = new System.Drawing.Size(130, 30);
            this.lblPassword.Text = "Clave inicial";
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.TabIndex = 6;
            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(160, 140);
            this.txtPassword.Size = new System.Drawing.Size(350, 30);
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.TabIndex = 7;
            // lblPerfil
            this.lblPerfil.Location = new System.Drawing.Point(20, 180);
            this.lblPerfil.Size = new System.Drawing.Size(130, 30);
            this.lblPerfil.Text = "Perfil";
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.TabIndex = 8;
            // cmbPerfil
            this.cmbPerfil.Location = new System.Drawing.Point(160, 180);
            this.cmbPerfil.Size = new System.Drawing.Size(350, 30);
            this.cmbPerfil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPerfil.Name = "cmbPerfil";
            this.cmbPerfil.TabIndex = 9;
            // btnRegistrar
            this.btnRegistrar.Location = new System.Drawing.Point(20, 235);
            this.btnRegistrar.Size = new System.Drawing.Size(145, 36);
            this.btnRegistrar.BackColor = System.Drawing.Color.Teal;
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.UseVisualStyleBackColor = false;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.TabIndex = 10;
            // btnModificar
            this.btnModificar.Location = new System.Drawing.Point(180, 235);
            this.btnModificar.Size = new System.Drawing.Size(145, 36);
            this.btnModificar.BackColor = System.Drawing.Color.Teal;
            this.btnModificar.ForeColor = System.Drawing.Color.White;
            this.btnModificar.UseVisualStyleBackColor = false;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.TabIndex = 11;
            // btnBloquear
            this.btnBloquear.Location = new System.Drawing.Point(340, 235);
            this.btnBloquear.Size = new System.Drawing.Size(145, 36);
            this.btnBloquear.BackColor = System.Drawing.Color.Teal;
            this.btnBloquear.ForeColor = System.Drawing.Color.White;
            this.btnBloquear.UseVisualStyleBackColor = false;
            this.btnBloquear.Text = "Bloquear";
            this.btnBloquear.Name = "btnBloquear";
            this.btnBloquear.TabIndex = 12;
            // btnDesbloquear
            this.btnDesbloquear.Location = new System.Drawing.Point(500, 235);
            this.btnDesbloquear.Size = new System.Drawing.Size(145, 36);
            this.btnDesbloquear.BackColor = System.Drawing.Color.Teal;
            this.btnDesbloquear.ForeColor = System.Drawing.Color.White;
            this.btnDesbloquear.UseVisualStyleBackColor = false;
            this.btnDesbloquear.Text = "Desbloquear";
            this.btnDesbloquear.Name = "btnDesbloquear";
            this.btnDesbloquear.TabIndex = 13;
            // btnNuevo
            this.btnNuevo.Location = new System.Drawing.Point(660, 235);
            this.btnNuevo.Size = new System.Drawing.Size(145, 36);
            this.btnNuevo.BackColor = System.Drawing.Color.Teal;
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.TabIndex = 14;
            // dgvUsuarios
            this.dgvUsuarios.Location = new System.Drawing.Point(20, 290);
            this.dgvUsuarios.Size = new System.Drawing.Size(810, 200);
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoGenerateColumns = false;
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.TabIndex = 15;
            this.colUsuariosEmail.DataPropertyName = "email";
            this.colUsuariosEmail.HeaderText = "email";
            this.colUsuariosEmail.Name = "email";
            this.colUsuariosNombre.DataPropertyName = "nombre";
            this.colUsuariosNombre.HeaderText = "nombre";
            this.colUsuariosNombre.Name = "nombre";
            this.colUsuariosApellido.DataPropertyName = "apellido";
            this.colUsuariosApellido.HeaderText = "apellido";
            this.colUsuariosApellido.Name = "apellido";
            this.colUsuariosPerfil.DataPropertyName = "perfil";
            this.colUsuariosPerfil.HeaderText = "perfil";
            this.colUsuariosPerfil.Name = "perfil";
            this.colUsuariosActivo.DataPropertyName = "activo";
            this.colUsuariosActivo.HeaderText = "activo";
            this.colUsuariosActivo.Name = "activo";
            this.colUsuariosIntentos.DataPropertyName = "intentos";
            this.colUsuariosIntentos.HeaderText = "intentos";
            this.colUsuariosIntentos.Name = "intentos";
            this.dgvUsuarios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colUsuariosEmail, this.colUsuariosNombre, this.colUsuariosApellido, this.colUsuariosPerfil, this.colUsuariosActivo, this.colUsuariosIntentos });
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblEmail, this.txtEmail, this.lblNombre, this.txtNombre, this.lblApellido, this.txtApellido, this.lblPassword, this.txtPassword, this.lblPerfil, this.cmbPerfil, this.btnRegistrar, this.btnModificar, this.btnBloquear, this.btnDesbloquear, this.btnNuevo, this.dgvUsuarios });
            this.btnRegistrar.Click += new System.EventHandler(this.mb506Registrar);
            this.btnModificar.Click += new System.EventHandler(this.mb506Modificar);
            this.btnBloquear.Click += new System.EventHandler(this.mb506Bloquear);
            this.btnDesbloquear.Click += new System.EventHandler(this.mb506Desbloquear);
            this.btnNuevo.Click += new System.EventHandler(this.mb506Nuevo);
            this.dgvUsuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mb506Seleccionar);
            this.Load += new System.EventHandler(this.mb506FrmUsuarios_Load);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(850, 510);
            this.MinimumSize = new System.Drawing.Size(866, 549);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FrmUsuarios";
            this.Text = "Usuarios";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPerfil;
        private System.Windows.Forms.ComboBox cmbPerfil;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnBloquear;
        private System.Windows.Forms.Button btnDesbloquear;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuariosEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuariosNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuariosApellido;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuariosPerfil;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuariosActivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuariosIntentos;
    }
}
