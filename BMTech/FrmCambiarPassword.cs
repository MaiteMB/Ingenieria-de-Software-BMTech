using System;
using System.Drawing;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmCambiarPassword : Form
    {
        private TextBox txtActual = new TextBox();
        private TextBox txtNueva = new TextBox();
        private TextBox txtConfirmacion = new TextBox();

        public mb506FrmCambiarPassword()
        {
            IdiomasFormulario.mb506Vincular(this);
            Text = "Cambiar contraseña";
            ClientSize = new Size(440, 230);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            string[] textos = { "Clave actual", "Nueva clave", "Confirmar clave" };
            TextBox[] campos = { txtActual, txtNueva, txtConfirmacion };
            for (int i = 0; i < campos.Length; i++)
            {
                Label label = new Label { Text = textos[i] };
                label.SetBounds(20, 25 + i * 45, 130, 25);
                campos[i].SetBounds(160, 25 + i * 45, 250, 25);
                campos[i].UseSystemPasswordChar = true;
                Controls.Add(label); Controls.Add(campos[i]);
            }
            Button guardar = new Button { Text = "Guardar", BackColor = Color.Teal, ForeColor = Color.White };
            guardar.SetBounds(270, 170, 140, 35);
            guardar.Click += mb506Guardar;
            Controls.Add(guardar);
            AcceptButton = guardar;
        }

        private void mb506Guardar(object sender, EventArgs e)
        {
            try
            {
                if (txtNueva.Text != txtConfirmacion.Text) throw new Exception("Las claves no coinciden.");
                new BLLUsuario().mb506CambiarPassword(txtActual.Text, txtNueva.Text);
                Mensajes.mb506Mostrar("Clave modificada."); Close();
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
