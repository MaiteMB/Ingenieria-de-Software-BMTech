using mb506.BEBMTech.Usuario;
using mb506.BLLBMTech;
using mb506.ServiciosBMTech.Seguridad;
using System;
using System.Windows.Forms;

namespace mb506.BMTech
{
    public partial class mb506FrmLogin : Form
    {
        private readonly BLLUsuario bllUsuario;
        private readonly BLLLog bllLog;

        public mb506FrmLogin()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
            bllUsuario = new BLLUsuario();
            bllLog = new BLLLog();
            lblMensaje.Text = "";
        }

        private void mb506btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = bllUsuario.mb506Login(txtEmail.Text.Trim(), txtPassword.Text);
                SessionManager.mb506Login(usuario);
                if (!bllLog.mb506RegistrarEvento(usuario.email, "Login correcto", "Acceso", 1))
                    throw new Exception("No se permite ingresar sin registrar el inicio de sesion.");

                mb506FrmPrincipal mb506FrmPrincipal = new mb506FrmPrincipal();
                this.Hide();
                mb506FrmPrincipal.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                if (SessionManager.IsSessionActive) SessionManager.mb506Logout();
                try { bllLog.mb506RegistrarEvento(txtEmail.Text.Trim(), "Login fallido: " + ex.Message, "Acceso", 2); }
                catch { }
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mb506chkMostrarPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkMostrarPassword.Checked ? '\0' : '*';
        }

        private void mb506MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Tag = mensaje;
            lblMensaje.Text = Mensajes.mb506Traducir(mensaje);
            mb506ColorMensaje(correcto);
        }

        private void mb506FrmLogin_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                var idiomas = new BLLIdioma().mb506ObtenerIdiomas();
                string codigoActual = mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.idiomaActual;
                cmbIdioma.DataSource = idiomas;
                cmbIdioma.SelectedItem = idiomas.Find(i => i.codigo == codigoActual);
                var diferencias = new BLLIntegridad().mb506VerificarSistema();
                if (diferencias.Count > 0) Mensajes.mb506Mostrar("Revise integridad antes de operar. Diferencias en: " + string.Join(", ", diferencias));
            }
            catch (Exception ex) { mb506MostrarMensaje("Revise la conexion con la base de datos. " + ex.Message, false); }
        }

        private void mb506IdiomaCambiado(object sender, EventArgs e)
        {
            var idioma = cmbIdioma.SelectedItem as mb506.BEBMTech.Idiomas.Idioma;
            if (idioma != null) mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.mb506CambiarIdioma(idioma.codigo);
        }

        private void mb506PrepararIntegridad(object sender, EventArgs e)
        {
            try
            {
                if (Mensajes.mb506Mostrar("Esto acepta los datos actuales como correctos. Revise la base antes de continuar.",
                    "Preparar integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                bllUsuario.mb506InicializarIntegridad(txtEmail.Text, txtPassword.Text);
                Mensajes.mb506Mostrar("Integridad preparada. Ahora puede ingresar.");
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
