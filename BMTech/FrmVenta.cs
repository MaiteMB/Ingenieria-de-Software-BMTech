using mb506.BEBMTech.Cliente;
using mb506.BEBMTech.Producto;
using mb506.BEBMTech.Venta;
using mb506.BLLBMTech;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mb506.BMTech
{
    public partial class mb506FrmVenta : Form
    {
        private BLLCliente bllCliente;
        private BLLProducto bllProducto;
        private BLLVenta bllVenta;

        private Cliente clienteSeleccionado;
        private Producto productoSeleccionado;
        private List<DetalleVenta> detallesVenta;

        public mb506FrmVenta()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);

            bllCliente = new BLLCliente();
            bllProducto = new BLLProducto();
            bllVenta = new BLLVenta();

            detallesVenta = new List<DetalleVenta>();

            lblMensaje.Text = "";
            lblCliente.Text = "";
            lblTotal.Text = "Total: 0,00";
        }

        private void mb506FrmVenta_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try { mb506CargarClientes(); mb506CargarProductos(); }
            catch (Exception ex) { mb506MostrarMensaje(ex.Message, false); }
        }

        private void mb506CargarClientes()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = bllCliente.mb506BuscarClientes(txtDniCliente.Text.Trim());
        }

        private void mb506CargarProductos()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = bllProducto.mb506BuscarProducto(txtBuscarProducto.Text.Trim(), false);
        }

        private void mb506btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                mb506CargarClientes();
                mb506MostrarMensaje("Clientes encontrados.", true);
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
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

            mb506MostrarMensaje("Cliente seleccionado.", true);
        }

        private void mb506btnBuscarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                mb506CargarProductos();
                mb506MostrarMensaje("Productos encontrados.", true);
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

            mb506MostrarMensaje("Producto seleccionado.", true);
        }

        private void mb506btnAgregarProducto_Click(object sender, EventArgs e)
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

                DetalleVenta existente = detallesVenta.Find(d => d.idProducto == productoSeleccionado.idProducto);
                int cantidadTotal = checked(cantidad + (existente == null ? 0 : existente.cantidad));
                if (cantidadTotal > productoSeleccionado.stock)
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

                if (existente == null) detallesVenta.Add(detalle);
                else
                {
                    existente.cantidad = cantidadTotal;
                    existente.precioUnitario = productoSeleccionado.precio;
                    existente.subtotal = cantidadTotal * productoSeleccionado.precio;
                }

                mb506ActualizarDetalle();

                txtCantidad.Clear();
                productoSeleccionado = null;

                mb506MostrarMensaje("Producto agregado a la venta.", true);
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnRegistrarVenta_Click(object sender, EventArgs e)
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

                DialogResult respuesta = Mensajes.mb506Mostrar(
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
                if (!mb506.ServiciosBMTech.Seguridad.SessionManager.IsSessionActive)
                    throw new Exception("Debe iniciar sesion para registrar una venta.");
                venta.emailUsuario = mb506.ServiciosBMTech.Seguridad.SessionManager.getSession.Usuario.email;
                venta.detalles = detallesVenta;
                if (detallesVenta.Count == 0) throw new Exception("Debe agregar al menos un producto a la venta.");
                using (mb506FrmPago formularioPago = new mb506FrmPago(total, false))
                {
                    if (formularioPago.ShowDialog(this) != DialogResult.OK) return;
                    venta.pago = formularioPago.pago;
                }

                int idVenta = bllVenta.mb506RegistrarVenta(venta);

                mb506LimpiarFormulario();
                mb506CargarClientes();
                mb506CargarProductos();
                mb506MostrarMensaje("Venta registrada correctamente. Nro: " + idVenta, true);
                if (!venta.pago.verificado)
                    Mensajes.mb506Mostrar("Venta pendiente guardada. Verifique el pago desde Historial de ventas.");
                else
                {
                    try
                    {
                        bllVenta.mb506GenerarComprobante(idVenta);
                        using (mb506FrmComprobante comprobante = new mb506FrmComprobante(idVenta)) comprobante.ShowDialog(this);
                        if (Mensajes.mb506Mostrar("Confirma que entrego los productos y el comprobante?", "Registrar entrega",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            bllVenta.mb506EntregarVenta(idVenta);
                    }
                    catch (Exception ex)
                    {
                        Mensajes.mb506Mostrar("La venta quedo pagada. Puede reintentar el comprobante o la entrega desde Historial de ventas. " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnLimpiar_Click(object sender, EventArgs e)
        {
            try { mb506LimpiarFormulario(); mb506CargarClientes(); mb506CargarProductos(); }
            catch (Exception ex) { mb506MostrarMensaje(ex.Message, false); }
        }

        private void mb506ActualizarDetalle()
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

        private void mb506LimpiarFormulario()
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

        private void mb506MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Tag = mensaje;
            lblMensaje.Text = Mensajes.mb506Traducir(mensaje);
            mb506ColorMensaje(correcto);
        }

        private void mb506QuitarProducto(object sender, EventArgs e)
        {
            DetalleVenta detalle = dgvDetalleVenta.CurrentRow == null ? null : dgvDetalleVenta.CurrentRow.DataBoundItem as DetalleVenta;
            if (detalle == null) return;
            detallesVenta.Remove(detalle);
            mb506ActualizarDetalle();
        }
    }
}
