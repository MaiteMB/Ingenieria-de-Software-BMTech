using BEBMTech.Cliente;
using BLLBMTech;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BMTech
{
    public partial class FrmCliente : Form
    {
        private BLLCliente bllCliente;

        public FrmCliente()
        {
            InitializeComponent();
            bllCliente = new BLLCliente();
            lblMensaje.Text = "";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            MostrarMensaje("", true);
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = bllCliente.BuscarCliente(txtDni.Text.Trim());

                if (cliente == null)
                {
                    MostrarMensaje("No se encontró un cliente con ese DNI.", false);
                    return;
                }

                txtNombre.Text = cliente.nombre;
                txtApellido.Text = cliente.apellido;
                txtTelefono.Text = cliente.telefono;
                txtCorreoElectronico.Text = cliente.correoElectronico;

                MostrarMensaje("Cliente encontrado.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnRegistrarCliente_Click(object sender, EventArgs e)
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

                bool registrado = bllCliente.RegistrarCliente(cliente);

                if (registrado)
                {
                    MostrarMensaje("Cliente registrado correctamente.", true);
                    LimpiarCampos();
                }
                else
                {
                    MostrarMensaje("No se pudo registrar el cliente.", false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void LimpiarCampos()
        {
            txtDni.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreoElectronico.Clear();
            txtDni.Focus();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {

        }
        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }
    }
}
