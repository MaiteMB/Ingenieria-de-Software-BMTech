using System;
using System.Drawing;
using System.Windows.Forms;
using mb506.BEBMTech.Usuario;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmUsuarios : Form
    {
        private readonly BLLUsuario bllUsuario = new BLLUsuario();
        private readonly DataGridView dgvUsuarios = new DataGridView();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtApellido = new TextBox();
        private readonly TextBox txtPassword = new TextBox();
        private readonly ComboBox cmbPerfil = new ComboBox();
        private string emailSeleccionado;

        public mb506FrmUsuarios()
        {
            IdiomasFormulario.mb506Vincular(this);
            Text = "Usuarios";
            ClientSize = new Size(850, 510);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);
            mb506AgregarCampo("Email", txtEmail, 20);
            mb506AgregarCampo("Nombre", txtNombre, 60);
            mb506AgregarCampo("Apellido", txtApellido, 100);
            mb506AgregarCampo("Clave inicial", txtPassword, 140);
            txtPassword.UseSystemPasswordChar = true;
            mb506AgregarCampo("Perfil", cmbPerfil, 180);
            cmbPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            mb506AgregarBoton("Registrar", 20, mb506Registrar);
            mb506AgregarBoton("Modificar", 180, mb506Modificar);
            mb506AgregarBoton("Bloquear", 340, mb506Bloquear);
            mb506AgregarBoton("Desbloquear", 500, mb506Desbloquear);
            mb506AgregarBoton("Nuevo", 660, mb506Nuevo);
            dgvUsuarios.SetBounds(20, 290, 810, 200);
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AutoGenerateColumns = false;
            foreach (string campo in new[] { "email", "nombre", "apellido", "perfil", "activo", "intentos" })
                dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                { HeaderText = campo, DataPropertyName = campo, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvUsuarios.CellClick += mb506Seleccionar;
            Controls.Add(dgvUsuarios);
            Load += delegate
            {
                try { cmbPerfil.DataSource = bllUsuario.mb506ObtenerPerfiles(); mb506Cargar(); }
                catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); Close(); }
            };
        }

        private void mb506AgregarCampo(string texto, Control control, int y)
        {
            Label label = new Label { Text = texto };
            label.SetBounds(20, y, 130, 30);
            control.SetBounds(160, y, 350, 30);
            Controls.Add(label);
            Controls.Add(control);
        }

        private void mb506AgregarBoton(string texto, int x, EventHandler evento)
        {
            Button boton = new Button { Text = texto, BackColor = Color.Teal, ForeColor = Color.White };
            boton.SetBounds(x, 235, 145, 36);
            boton.Click += evento;
            Controls.Add(boton);
        }

        private void mb506Cargar()
        {
            dgvUsuarios.DataSource = bllUsuario.mb506ObtenerUsuarios();
            dgvUsuarios.ClearSelection();
        }

        private Usuario mb506Leer()
        {
            return new Usuario
            {
                email = txtEmail.Text.Trim(), nombre = txtNombre.Text.Trim(),
                apellido = txtApellido.Text.Trim(), perfil = Convert.ToString(cmbPerfil.SelectedItem)
            };
        }

        private void mb506Registrar(object sender, EventArgs e)
        {
            try
            {
                if (emailSeleccionado != null) throw new Exception("Pulse Nuevo para registrar otro usuario.");
                if (!bllUsuario.mb506RegistrarUsuario(mb506Leer(), txtPassword.Text))
                    throw new Exception("No se pudo registrar.");
                mb506Cargar(); mb506Nuevo(sender, e);
                Mensajes.mb506Mostrar("Usuario registrado.");
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Modificar(object sender, EventArgs e)
        {
            try
            {
                if (emailSeleccionado == null) throw new Exception("Seleccione un usuario.");
                if (!bllUsuario.mb506ModificarUsuario(mb506Leer())) throw new Exception("No se pudo modificar.");
                mb506Cargar(); mb506Nuevo(sender, e);
                Mensajes.mb506Mostrar("Usuario modificado.");
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Bloquear(object sender, EventArgs e) { mb506CambiarEstado(false); }
        private void mb506Desbloquear(object sender, EventArgs e) { mb506CambiarEstado(true); }

        private void mb506CambiarEstado(bool activo)
        {
            try
            {
                if (emailSeleccionado == null) throw new Exception("Seleccione un usuario.");
                if (Mensajes.mb506Mostrar("Confirmar cambio de estado de " + emailSeleccionado + "?",
                    "Usuarios", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                bllUsuario.mb506CambiarEstado(emailSeleccionado, activo);
                mb506Cargar(); mb506Nuevo(this, EventArgs.Empty);
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Seleccionar(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Usuario usuario = dgvUsuarios.Rows[e.RowIndex].DataBoundItem as Usuario;
            if (usuario == null) return;
            emailSeleccionado = usuario.email;
            txtEmail.Text = usuario.email; txtEmail.ReadOnly = true;
            txtNombre.Text = usuario.nombre; txtApellido.Text = usuario.apellido;
            cmbPerfil.SelectedItem = usuario.perfil;
            txtPassword.Clear(); txtPassword.Enabled = false;
        }

        private void mb506Nuevo(object sender, EventArgs e)
        {
            emailSeleccionado = null;
            txtEmail.Clear(); txtEmail.ReadOnly = false;
            txtNombre.Clear(); txtApellido.Clear();
            txtPassword.Clear(); txtPassword.Enabled = true;
            dgvUsuarios.ClearSelection(); txtEmail.Focus();
        }
    }
}
