using mb506.BEBMTech.Producto;
using mb506.BLLBMTech;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace mb506.BMTech
{
    public partial class mb506FrmProducto : Form
    {
        private BLLProducto bllProducto;
        private int idProductoSeleccionado = 0;
        private bool productoActivoSeleccionado = true;

        public mb506FrmProducto()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
            bllProducto = new BLLProducto();
            lblMensaje.Text = "";
        }

        private void mb506btnRegistrarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                Producto producto = mb506ObtenerProductoDesdeFormulario();

                bool registrado = bllProducto.mb506RegistrarProducto(producto);

                if (registrado)
                {
                    mb506MostrarMensaje("Producto registrado correctamente.", true);
                    mb506LimpiarCampos();
                    mb506BuscarProductos();
                }
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnModificarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                Producto producto = mb506ObtenerProductoDesdeFormulario();

                bool modificado = bllProducto.mb506ModificarProducto(producto);

                if (modificado)
                {
                    mb506MostrarMensaje("Producto modificado correctamente.", true);
                    mb506LimpiarCampos();
                    mb506BuscarProductos();
                }
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnCambiarEstadoProducto_Click(object sender, EventArgs e)
        {
            try
            {
                bool nuevoEstado = !productoActivoSeleccionado;

                bool cambioEstado = bllProducto.mb506CambiarEstadoProducto(idProductoSeleccionado, nuevoEstado);

                if (cambioEstado)
                {
                    mb506MostrarMensaje("Estado del producto actualizado correctamente.", true);
                    mb506LimpiarCampos();
                    mb506BuscarProductos();
                }
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnBuscarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                mb506BuscarProductos();
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506btnLimpiar_Click(object sender, EventArgs e)
        {
            mb506LimpiarCampos();
        }

        private Producto mb506ObtenerProductoDesdeFormulario()
        {
            if (txtCodigo.Text.Trim() == "")
            {
                throw new Exception("Debe ingresar el código del producto.");
            }

            if (txtDescripcion.Text.Trim() == "")
            {
                throw new Exception("Debe ingresar la descripción del producto.");
            }

            if (txtMarca.Text.Trim() == "")
            {
                throw new Exception("Debe ingresar la marca del producto.");
            }

            if (txtModelo.Text.Trim() == "")
            {
                throw new Exception("Debe ingresar el modelo del producto.");
            }

            if (txtPrecio.Text.Trim() == "")
            {
                throw new Exception("Debe ingresar el precio del producto.");
            }

            if (txtStock.Text.Trim() == "")
            {
                throw new Exception("Debe ingresar el stock del producto.");
            }

            Producto producto = new Producto();

            producto.idProducto = idProductoSeleccionado;
            producto.codigo = txtCodigo.Text.Trim();
            producto.descripcion = txtDescripcion.Text.Trim();
            producto.marca = txtMarca.Text.Trim();
            producto.modelo = txtModelo.Text.Trim();
            producto.precio = Convert.ToDecimal(txtPrecio.Text.Trim());
            producto.stock = Convert.ToInt32(txtStock.Text.Trim());
            producto.activo = productoActivoSeleccionado;

            return producto;
        }

        private void mb506BuscarProductos()
        {
            List<Producto> productos = bllProducto.mb506BuscarProducto(txtBuscar.Text.Trim(), true);

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productos;

            if (productos.Count == 0)
            {
                mb506MostrarMensaje("No se encontraron productos.", false);
            }
            else
            {
                mb506MostrarMensaje("Productos encontrados.", true);
            }
        }

        private void mb506LimpiarCampos()
        {
            idProductoSeleccionado = 0;
            productoActivoSeleccionado = true;

            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtMarca.Clear();
            txtModelo.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtBuscar.Clear();

            txtCodigo.Enabled = true;
            lblMensaje.Text = "";
            txtCodigo.Focus();
        }

        private void mb506dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

            idProductoSeleccionado = Convert.ToInt32(fila.Cells["idProducto"].Value);
            txtCodigo.Text = fila.Cells["codigo"].Value.ToString();
            txtDescripcion.Text = fila.Cells["descripcion"].Value.ToString();
            txtMarca.Text = fila.Cells["marca"].Value.ToString();
            txtModelo.Text = fila.Cells["modelo"].Value.ToString();
            txtPrecio.Text = fila.Cells["precio"].Value.ToString();
            txtStock.Text = fila.Cells["stock"].Value.ToString();
            productoActivoSeleccionado = Convert.ToBoolean(fila.Cells["activo"].Value);

            txtCodigo.Enabled = false;
        }

        private void mb506MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Tag = mensaje;
            lblMensaje.Text = Mensajes.mb506Traducir(mensaje);
            mb506ColorMensaje(correcto);
        }

        private void mb506FrmProducto_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                mb506BuscarProductos();
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }
    }

}
