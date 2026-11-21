using System;
using System.Windows.Forms;
using mb506.BEBMTech.Usuario;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmUsuarios : Form
    {
        private readonly BLLUsuario bllUsuario = new BLLUsuario();

        private string emailSeleccionado;

        public mb506FrmUsuarios()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
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

        private void mb506FrmUsuarios_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try { cmbPerfil.DataSource = bllUsuario.mb506ObtenerPerfiles(); mb506Cargar(); }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); Close(); }
        }
    }
}
