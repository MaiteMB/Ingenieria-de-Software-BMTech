using mb506.BEBMTech.Cliente;
using mb506.BLLBMTech;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace mb506.BMTech
{
    public partial class mb506FrmCliente : Form
    {
        private BLLCliente bllCliente;

        public mb506FrmCliente()
        {
            IdiomasFormulario.mb506Vincular(this);
            InitializeComponent();
            bllCliente = new BLLCliente();
            lblMensaje.Text = "";
        }

        private void mb506btnLimpiar_Click(object sender, EventArgs e)
        {
            mb506LimpiarCampos();
            mb506MostrarMensaje("", true);
        }

        private void mb506btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = bllCliente.mb506BuscarCliente(txtDni.Text.Trim());

                if (cliente == null)
                {
                    mb506MostrarMensaje("No se encontró un cliente con ese DNI.", false);
                    return;
                }

                txtNombre.Text = cliente.nombre;
                txtApellido.Text = cliente.apellido;
                txtTelefono.Text = cliente.telefono;
                txtCorreoElectronico.Text = cliente.correoElectronico;

                mb506MostrarMensaje("Cliente encontrado.", true);
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnRegistrarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = new Cliente(
                    txtDni.Text.Trim(),
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    txtCorreoElectronico.Text.Trim()
                );

                bllCliente.mb506ValidarCliente(cliente);
                if (bllCliente.mb506BuscarCliente(cliente.dni) != null) throw new Exception("El cliente ya se encuentra registrado.");
                if (Mensajes.mb506Mostrar("Confirma registrar este cliente?", "Registrar cliente", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes) return;
                bool registrado = bllCliente.mb506RegistrarCliente(cliente);

                if (registrado)
                {
                    mb506MostrarMensaje("Cliente registrado correctamente.", true);
                    mb506LimpiarCampos();
                }
                else
                {
                    mb506MostrarMensaje("No se pudo registrar el cliente.", false);
                }
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506LimpiarCampos()
        {
            txtDni.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreoElectronico.Clear();
            txtDni.Focus();
        }

        private void mb506FrmCliente_Load(object sender, EventArgs e)
        {

        }
        private void mb506MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Tag = mensaje;
            lblMensaje.Text = Mensajes.mb506Traducir(mensaje);
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }
    }
}
