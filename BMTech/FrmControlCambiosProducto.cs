using mb506.BEBMTech.Cambios;
using mb506.BLLBMTech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace mb506.BMTech
{
    public partial class mb506FrmControlCambiosProducto : Form
    {
        private readonly BLLCambios bllCambios;
        private int idHistorialSeleccionado = 0;

        public mb506FrmControlCambiosProducto()
        {
            IdiomasFormulario.mb506Vincular(this);
            InitializeComponent();
            bllCambios = new BLLCambios();
            lblMensaje.Text = "";
            mb506ConfigurarGrillaVersiones();
            mb506ConfigurarGrillaDetalle();
        }

        private void mb506FrmControlCambiosProducto_Load(object sender, EventArgs e)
        {
            try { mb506CargarVersiones(); }
            catch (Exception ex) { mb506MostrarMensaje(ex.Message, false); }
        }

        private void mb506ConfigurarGrillaVersiones()
        {
            dgvVersiones.AutoGenerateColumns = false;
            dgvVersiones.Columns.Clear();

            dgvVersiones.Columns.Add("idHistorial", "Historial");
            dgvVersiones.Columns["idHistorial"].DataPropertyName = "idHistorial";
            dgvVersiones.Columns["idHistorial"].Visible = false;

            dgvVersiones.Columns.Add("idProducto", "Producto");
            dgvVersiones.Columns["idProducto"].DataPropertyName = "idProducto";

            dgvVersiones.Columns.Add("codigo", "Código");
            dgvVersiones.Columns["codigo"].DataPropertyName = "codigo";

            dgvVersiones.Columns.Add("descripcion", "Descripción");
            dgvVersiones.Columns["descripcion"].DataPropertyName = "descripcion";

            dgvVersiones.Columns.Add("fecha", "Fecha");
            dgvVersiones.Columns["fecha"].DataPropertyName = "fecha";
            dgvVersiones.Columns["fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            dgvVersiones.Columns.Add("usuario", "Usuario");
            dgvVersiones.Columns["usuario"].DataPropertyName = "usuario";

            dgvVersiones.Columns.Add("accion", "Acción");
            dgvVersiones.Columns["accion"].DataPropertyName = "accion";
        }

        private void mb506ConfigurarGrillaDetalle()
        {
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.Columns.Clear();

            dgvDetalle.Columns.Add("codigo", "Código");
            dgvDetalle.Columns["codigo"].DataPropertyName = "codigo";

            dgvDetalle.Columns.Add("descripcion", "Descripción");
            dgvDetalle.Columns["descripcion"].DataPropertyName = "descripcion";

            dgvDetalle.Columns.Add("marca", "Marca");
            dgvDetalle.Columns["marca"].DataPropertyName = "marca";

            dgvDetalle.Columns.Add("modelo", "Modelo");
            dgvDetalle.Columns["modelo"].DataPropertyName = "modelo";

            dgvDetalle.Columns.Add("precio", "Precio");
            dgvDetalle.Columns["precio"].DataPropertyName = "precio";

            dgvDetalle.Columns.Add("stock", "Stock");
            dgvDetalle.Columns["stock"].DataPropertyName = "stock";

            dgvDetalle.Columns.Add("activo", "Activo");
            dgvDetalle.Columns["activo"].DataPropertyName = "activo";
        }

        private void mb506CargarVersiones()
        {
            List<VersionCambio> versiones = bllCambios.mb506ObtenerTodasLasVersionesProducto();

            dgvVersiones.DataSource = null;
            dgvVersiones.DataSource = versiones;

            dgvDetalle.DataSource = null;
            idHistorialSeleccionado = 0;
        }

        private void mb506dgvVersiones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvVersiones.Rows[e.RowIndex];
            idHistorialSeleccionado = Convert.ToInt32(fila.Cells["idHistorial"].Value);

            VersionCambio version = bllCambios.mb506ObtenerVersion(idHistorialSeleccionado);
            List<VersionCambio> detalle = new List<VersionCambio>();
            detalle.Add(version);

            dgvDetalle.DataSource = null;
            dgvDetalle.DataSource = detalle;

            mb506MostrarMensaje("Versión seleccionada.", true);
            }
            catch (Exception ex) { mb506MostrarMensaje(ex.Message, false); }
        }

        private void mb506btnActualizar_Click(object sender, EventArgs e)
        {
            try { mb506CargarVersiones(); mb506MostrarMensaje("Historial actualizado.", true); }
            catch (Exception ex) { mb506MostrarMensaje(ex.Message, false); }
        }

        private void mb506btnRestaurar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idHistorialSeleccionado == 0)
                {
                    throw new Exception("Debe seleccionar una versión.");
                }

                DialogResult respuesta = Mensajes.mb506Mostrar(
                    "¿Confirma restaurar el producto a la versión seleccionada?",
                    "Restaurar producto",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.No)
                {
                    return;
                }

                bool restaurado = bllCambios.mb506RestaurarProducto(idHistorialSeleccionado);

                if (restaurado)
                {
                    mb506MostrarMensaje("Producto restaurado correctamente.", true);
                    mb506CargarVersiones();
                }
                else
                {
                    mb506MostrarMensaje("No se pudo restaurar el producto.", false);
                }
            }
            catch (Exception ex)
            {
                mb506MostrarMensaje(ex.Message, false);
            }
        }

        private void mb506MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Tag = mensaje;
            lblMensaje.Text = Mensajes.mb506Traducir(mensaje);
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }
    }
}
