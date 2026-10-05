using BEBMTech.Producto;
using BLLBMTech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace BMTech
{
    public partial class FrmProducto : Form
    {
      
        private BLLProducto bllProducto;
        private int idProductoSeleccionado = 0;
        private bool productoActivoSeleccionado = true;

        public FrmProducto()
        {
            InitializeComponent();
            bllProducto = new BLLProducto();
            lblMensaje.Text = "";

            ConfigurarGrilla();
        }

        private void ConfigurarGrilla()
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

            dgvProductos.Columns.Add("marca", "Marca");
            dgvProductos.Columns["marca"].DataPropertyName = "marca";

            dgvProductos.Columns.Add("modelo", "Modelo");
            dgvProductos.Columns["modelo"].DataPropertyName = "modelo";

            dgvProductos.Columns.Add("precio", "Precio");
            dgvProductos.Columns["precio"].DataPropertyName = "precio";

            dgvProductos.Columns.Add("stock", "Stock");
            dgvProductos.Columns["stock"].DataPropertyName = "stock";

            dgvProductos.Columns.Add("activo", "Activo");
            dgvProductos.Columns["activo"].DataPropertyName = "activo";
        }

        private void btnRegistrarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                Producto producto = ObtenerProductoDesdeFormulario();

                bool registrado = bllProducto.RegistrarProducto(producto);

                if (registrado)
                {
                    MostrarMensaje("Producto registrado correctamente.", true);
                    LimpiarCampos();
                    BuscarProductos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnModificarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                Producto producto = ObtenerProductoDesdeFormulario();

                bool modificado = bllProducto.ModificarProducto(producto);

                if (modificado)
                {
                    MostrarMensaje("Producto modificado correctamente.", true);
                    LimpiarCampos();
                    BuscarProductos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnCambiarEstadoProducto_Click(object sender, EventArgs e)
        {
            try
            {
                bool nuevoEstado = !productoActivoSeleccionado;

                bool cambioEstado = bllProducto.CambiarEstadoProducto(idProductoSeleccionado, nuevoEstado);

                if (cambioEstado)
                {
                    MostrarMensaje("Estado del producto actualizado correctamente.", true);
                    LimpiarCampos();
                    BuscarProductos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarProductos();
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private Producto ObtenerProductoDesdeFormulario()
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

        private void BuscarProductos()
        {
            List<Producto> productos = bllProducto.BuscarProducto(txtBuscar.Text.Trim(), true);

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productos;

            if (productos.Count == 0)
            {
                MostrarMensaje("No se encontraron productos.", false);
            }
            else
            {
                MostrarMensaje("Productos encontrados.", true);
            }
        }

        private void LimpiarCampos()
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

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            try
            {
                BuscarProductos();
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }
    }

}
