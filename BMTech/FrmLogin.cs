using BEBMTech.Usuario;
using BLLBMTech;
using ServiciosBMTech.Seguridad;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BMTech
{
    public partial class FrmLogin : Form
    {
        private readonly BLLUsuario bllUsuario;
        private readonly BLLLog bllLog;

        public FrmLogin()
        {
            InitializeComponent();
            bllUsuario = new BLLUsuario();
            bllLog = new BLLLog();
            lblMensaje.Text = "";
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = bllUsuario.Login(txtEmail.Text.Trim(), txtPassword.Text.Trim());
                SessionManager.Login(usuario);
                bllLog.RegistrarEvento(usuario.email, "Login correcto", "Acceso", 1);

                FrmPrincipal frmPrincipal = new FrmPrincipal();
                this.Hide();
                frmPrincipal.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                bllLog.RegistrarEvento(txtEmail.Text.Trim(), "Login fallido: " + ex.Message, "Acceso", 2);
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkMostrarPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkMostrarPassword.Checked ? '\0' : '*';
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }
    }
}

