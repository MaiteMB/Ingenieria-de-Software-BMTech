using BEBMTech.Cliente;
using BEBMTech.Producto;
using BEBMTech.Venta;
using BLLBMTech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BMTech
{
    public partial class FrmVenta : Form
    {
        private BLLCliente bllCliente;
        private BLLProducto bllProducto;
        private BLLVenta bllVenta;

        private Cliente clienteSeleccionado;
        private Producto productoSeleccionado;
        private List<DetalleVenta> detallesVenta;

        public FrmVenta()
        {
            InitializeComponent();

            bllCliente = new BLLCliente();
            bllProducto = new BLLProducto();
            bllVenta = new BLLVenta();

            detallesVenta = new List<DetalleVenta>();

            lblMensaje.Text = "";
            lblCliente.Text = "";
            lblTotal.Text = "Total: 0,00";

            ConfigurarGrillaClientes();
            ConfigurarGrillaProductos();
            ConfigurarGrillaDetalle();
        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            CargarClientes();
            CargarProductos();
        }

        private void ConfigurarGrillaClientes()
        {
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.Columns.Clear();

            dgvClientes.Columns.Add("dni", "DNI");
            dgvClientes.Columns["dni"].DataPropertyName = "dni";

            dgvClientes.Columns.Add("nombre", "Nombre");
            dgvClientes.Columns["nombre"].DataPropertyName = "nombre";

            dgvClientes.Columns.Add("apellido", "Apellido");
            dgvClientes.Columns["apellido"].DataPropertyName = "apellido";
        }

        private void ConfigurarGrillaProductos()
        {
            dgvProductos.AutoGenerateColumns = false;
            dgvProductos.Columns.Clear();

            dgvProductos.Columns.Add("idProducto", "ID");
            dgvProductos.Columns["idProducto"].DataPropertyName = "idProducto";
            dgvProductos.Columns["idProducto"].Visible = false;

            dgvProductos.Columns.Add("codigo", "Código");
            dgvProductos.Columns["codigo"].DataPropertyName = "codigo";

            dgvProductos.Columns.Add("descripcion", "Descripción");
            dgvProductos.Columns["descripcion"].DataPropertyName = "descripcion";

            dgvProductos.Columns.Add("precio", "Precio");
            dgvProductos.Columns["precio"].DataPropertyName = "precio";

            dgvProductos.Columns.Add("stock", "Stock");
            dgvProductos.Columns["stock"].DataPropertyName = "stock";
        }

        private void ConfigurarGrillaDetalle()
        {
            dgvDetalleVenta.AutoGenerateColumns = false;
            dgvDetalleVenta.Columns.Clear();

            dgvDetalleVenta.Columns.Add("idProducto", "ID");
            dgvDetalleVenta.Columns["idProducto"].DataPropertyName = "idProducto";
            dgvDetalleVenta.Columns["idProducto"].Visible = false;

            dgvDetalleVenta.Columns.Add("codigoProducto", "Código");
            dgvDetalleVenta.Columns["codigoProducto"].DataPropertyName = "codigoProducto";

            dgvDetalleVenta.Columns.Add("descripcionProducto", "Descripción");
            dgvDetalleVenta.Columns["descripcionProducto"].DataPropertyName = "descripcionProducto";

            dgvDetalleVenta.Columns.Add("cantidad", "Cantidad");
            dgvDetalleVenta.Columns["cantidad"].DataPropertyName = "cantidad";

            dgvDetalleVenta.Columns.Add("precioUnitario", "Precio");
            dgvDetalleVenta.Columns["precioUnitario"].DataPropertyName = "precioUnitario";

            dgvDetalleVenta.Columns.Add("subtotal", "Subtotal");
            dgvDetalleVenta.Columns["subtotal"].DataPropertyName = "subtotal";
        }

        private void CargarClientes()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = bllCliente.BuscarClientes(txtDniCliente.Text.Trim());
        }

        private void CargarProductos()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = bllProducto.BuscarProducto(txtBuscarProducto.Text.Trim(), false);
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                CargarClientes();
                MostrarMensaje("Clientes encontrados.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvClientes.Rows[e.RowIndex];

            clienteSeleccionado = new Cliente();

            clienteSeleccionado.dni = fila.Cells["dni"].Value.ToString();
            clienteSeleccionado.nombre = fila.Cells["nombre"].Value.ToString();
            clienteSeleccionado.apellido = fila.Cells["apellido"].Value.ToString();

            txtDniCliente.Text = clienteSeleccionado.dni;
            lblCliente.Text = clienteSeleccionado.nombre + " " + clienteSeleccionado.apellido;

            MostrarMensaje("Cliente seleccionado.", true);
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                CargarProductos();
                MostrarMensaje("Productos encontrados.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

            productoSeleccionado = new Producto();

            productoSeleccionado.idProducto = Convert.ToInt32(fila.Cells["idProducto"].Value);
            productoSeleccionado.codigo = fila.Cells["codigo"].Value.ToString();
            productoSeleccionado.descripcion = fila.Cells["descripcion"].Value.ToString();
            productoSeleccionado.precio = Convert.ToDecimal(fila.Cells["precio"].Value);
            productoSeleccionado.stock = Convert.ToInt32(fila.Cells["stock"].Value);

            MostrarMensaje("Producto seleccionado.", true);
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (productoSeleccionado == null)
                {
                    throw new Exception("Debe seleccionar un producto.");
                }

                int cantidad = Convert.ToInt32(txtCantidad.Text.Trim());

                if (cantidad <= 0)
                {
                    throw new Exception("La cantidad debe ser mayor a cero.");
                }

                if (cantidad > productoSeleccionado.stock)
                {
                    throw new Exception("No hay stock suficiente.");
                }

                DetalleVenta detalle = new DetalleVenta();

                detalle.idProducto = productoSeleccionado.idProducto;
                detalle.codigoProducto = productoSeleccionado.codigo;
                detalle.descripcionProducto = productoSeleccionado.descripcion;
                detalle.cantidad = cantidad;
                detalle.precioUnitario = productoSeleccionado.precio;
                detalle.subtotal = cantidad * productoSeleccionado.precio;

                detallesVenta.Add(detalle);

                ActualizarDetalle();

                txtCantidad.Clear();
                productoSeleccionado = null;

                MostrarMensaje("Producto agregado a la venta.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (clienteSeleccionado == null)
                {
                    throw new Exception("Debe seleccionar un cliente.");
                }

                decimal total = 0;

                foreach (DetalleVenta detalle in detallesVenta)
                {
                    total += detalle.subtotal;
                }

                DialogResult respuesta = MessageBox.Show(
                    "¿Confirma registrar la venta por un total de $" + total.ToString("0.00") + "?",
                    "Confirmar venta",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.No)
                {
                    return;
                }

                Venta venta = new Venta();

                venta.dniCliente = clienteSeleccionado.dni;
                venta.emailUsuario = null;
                venta.detalles = detallesVenta;

                int idVenta = bllVenta.RegistrarVenta(venta);

                MostrarMensaje("Venta registrada correctamente. Nro: " + idVenta, true);
                LimpiarFormulario();
                CargarClientes();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            CargarClientes();
            CargarProductos();
        }

        private void ActualizarDetalle()
        {
            dgvDetalleVenta.DataSource = null;
            dgvDetalleVenta.DataSource = detallesVenta;

            decimal total = 0;

            foreach (DetalleVenta detalle in detallesVenta)
            {
                total += detalle.subtotal;
            }

            lblTotal.Text = "Total: " + total.ToString("0.00");
        }

        private void LimpiarFormulario()
        {
            clienteSeleccionado = null;
            productoSeleccionado = null;
            detallesVenta = new List<DetalleVenta>();

            txtDniCliente.Clear();
            txtBuscarProducto.Clear();
            txtCantidad.Clear();

            lblCliente.Text = "";
            lblTotal.Text = "Total: 0,00";
            lblMensaje.Text = "";

            dgvDetalleVenta.DataSource = null;

            txtDniCliente.Focus();
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }
    }
}
