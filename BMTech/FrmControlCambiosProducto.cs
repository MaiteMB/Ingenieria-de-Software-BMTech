using mb506.BEBMTech.Cambios;
using mb506.BLLBMTech;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mb506.BMTech
{
    public partial class mb506FrmControlCambiosProducto : Form
    {
        private readonly BLLCambios bllCambios;
        private int idHistorialSeleccionado = 0;

        public mb506FrmControlCambiosProducto()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
            bllCambios = new BLLCambios();
            lblMensaje.Text = "";
        }

        private void mb506FrmControlCambiosProducto_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try { mb506CargarVersiones(); }
            catch (Exception ex) { mb506MostrarMensaje(ex.Message, false); }
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
            mb506ColorMensaje(correcto);
        }
    }
}
