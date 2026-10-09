using BEBMTech.Cambios;
using BLLBMTech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BMTech
{
    public partial class FrmControlCambiosProducto : Form
    {
        private readonly BLLCambios bllCambios;
        private int idHistorialSeleccionado = 0;

        public FrmControlCambiosProducto()
        {
            InitializeComponent();
            bllCambios = new BLLCambios();
            lblMensaje.Text = "";
            ConfigurarGrillaVersiones();
            ConfigurarGrillaDetalle();
        }

        private void FrmControlCambiosProducto_Load(object sender, EventArgs e)
        {
            CargarVersiones();
        }

        private void ConfigurarGrillaVersiones()
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

            dgvVersiones.Columns.Add("accion", "Acción");
            dgvVersiones.Columns["accion"].DataPropertyName = "accion";
        }

        private void ConfigurarGrillaDetalle()
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

        private void CargarVersiones()
        {
            List<VersionCambio> versiones = bllCambios.ObtenerTodasLasVersionesProducto();

            dgvVersiones.DataSource = null;
            dgvVersiones.DataSource = versiones;

            dgvDetalle.DataSource = null;
            idHistorialSeleccionado = 0;
        }

        private void dgvVersiones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvVersiones.Rows[e.RowIndex];
            idHistorialSeleccionado = Convert.ToInt32(fila.Cells["idHistorial"].Value);

            VersionCambio version = bllCambios.ObtenerVersion(idHistorialSeleccionado);
            List<VersionCambio> detalle = new List<VersionCambio>();
            detalle.Add(version);

            dgvDetalle.DataSource = null;
            dgvDetalle.DataSource = detalle;

            MostrarMensaje("Versión seleccionada.", true);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarVersiones();
            MostrarMensaje("Historial actualizado.", true);
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idHistorialSeleccionado == 0)
                {
                    throw new Exception("Debe seleccionar una versión.");
                }

                DialogResult respuesta = MessageBox.Show(
                    "¿Confirma restaurar el producto a la versión seleccionada?",
                    "Restaurar producto",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.No)
                {
                    return;
                }

                bool restaurado = bllCambios.RestaurarProducto(idHistorialSeleccionado);

                if (restaurado)
                {
                    MostrarMensaje("Producto restaurado correctamente.", true);
                    CargarVersiones();
                }
                else
                {
                    MostrarMensaje("No se pudo restaurar el producto.", false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = correcto ? Color.Teal : Color.Firebrick;
        }
    }
}
