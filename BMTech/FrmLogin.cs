using mb506.BEBMTech.Usuario;
using mb506.BLLBMTech;
using mb506.ServiciosBMTech.Seguridad;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace mb506.BMTech
{
    public partial class mb506FrmLogin : Form
    {
        private readonly BLLUsuario bllUsuario;
        private readonly BLLLog bllLog;

        public mb506FrmLogin()
        {
            IdiomasFormulario.mb506Vincular(this);
            InitializeComponent();
            ComboBox cmbIdioma = new ComboBox();
            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdioma.SetBounds(ClientSize.Width - 150, 25, 125, 28);
            cmbIdioma.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Load += delegate
            {
                try
                {
                    var idiomas = new BLLIdioma().mb506ObtenerIdiomas();
                    string codigoActual = mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.idiomaActual;
                    cmbIdioma.DataSource = idiomas;
                    cmbIdioma.SelectedItem = idiomas.Find(i => i.codigo == codigoActual);
                    var diferencias = new BLLIntegridad().mb506VerificarSistema();
                    if (diferencias.Count > 0) Mensajes.mb506Mostrar("Revise integridad antes de operar. Diferencias en: " + string.Join(", ", diferencias));
                }
                catch (Exception ex) { mb506MostrarMensaje("Revise la conexion y ejecute ActualizarEntrega1.sql. " + ex.Message, false); }
            };
            cmbIdioma.SelectedIndexChanged += delegate
            {
                var idioma = cmbIdioma.SelectedItem as mb506.BEBMTech.Idiomas.Idioma;
                if (idioma != null) mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.mb506CambiarIdioma(idioma.codigo);
            };
            Controls.Add(cmbIdioma);
            cmbIdioma.BringToFront();
            Button btnPreparar = new Button { Text = "Preparar integridad" };
            btnPreparar.SetBounds(390, 445, 185, 32);
            btnPreparar.Click += delegate
            {
                try
                {
                    if (Mensajes.mb506Mostrar("Esto acepta los datos actuales como correctos. Revise la base antes de continuar.",
                        "Preparar integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                    bllUsuario.mb506InicializarIntegridad(txtEmail.Text, txtPassword.Text);
                    Mensajes.mb506Mostrar("Integridad preparada. Ahora puede ingresar.");
                }
                catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
            };
            Controls.Add(btnPreparar);
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
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }
    }
}

