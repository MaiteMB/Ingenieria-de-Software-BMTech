using System;
using System.Data;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmHistorialVentas : Form
    {
        private BLLVenta bllVenta = new BLLVenta();

        public mb506FrmHistorialVentas()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }

        private void mb506Accion(int accion)
        {
            try
            {
                if (dgvVentas.SelectedRows.Count == 0) throw new Exception("Seleccione una venta.");
                DataRowView fila = dgvVentas.SelectedRows[0].DataBoundItem as DataRowView;
                if (fila == null) return;
                int idVenta = Convert.ToInt32(fila["idVenta"]);
                string estado = Convert.ToString(fila["estado"]);
                if (accion == 0)
                {
                    if (estado != "PENDIENTE") throw new Exception("La venta no esta pendiente.");
                    using (mb506FrmPago pago = new mb506FrmPago(bllVenta.mb506ObtenerTotal(idVenta), true))
                    {
                        if (pago.ShowDialog(this) != DialogResult.OK) return;
                        bllVenta.mb506ConfirmarPago(idVenta, pago.pago);
                    }
                }
                else if (accion == 1)
                {
                    bllVenta.mb506GenerarComprobante(idVenta);
                    using (mb506FrmComprobante comprobante = new mb506FrmComprobante(idVenta)) comprobante.ShowDialog(this);
                }
                else if (Mensajes.mb506Mostrar(accion == 2 ? "Confirma la entrega de productos y comprobante?" : "Confirma cancelar esta venta pendiente?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (accion == 2) bllVenta.mb506EntregarVenta(idVenta);
                    else bllVenta.mb506CancelarVenta(idVenta);
                }
                mb506Buscar(this, EventArgs.Empty);
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Buscar(object sender, EventArgs e)
        {
            try
            {
                dgvDetalles.DataSource = null;
                dgvPagos.DataSource = null;
                dgvVentas.DataSource = bllVenta.mb506ObtenerVentas(dtpDesde.Value, dtpHasta.Value);
                dgvVentas.ClearSelection();

            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Seleccionar(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                DataRowView venta = dgvVentas.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (venta == null) return;
                dgvDetalles.DataSource = bllVenta.mb506ObtenerDetalle(Convert.ToInt32(venta["idVenta"]));
                dgvPagos.DataSource = bllVenta.mb506ObtenerPagos(Convert.ToInt32(venta["idVenta"]));



            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
        private void mb506VerificarPago(object sender, EventArgs e)
        {
            mb506Accion(0);
        }
        private void mb506Comprobante(object sender, EventArgs e)
        {
            mb506Accion(1);
        }
        private void mb506Entregar(object sender, EventArgs e)
        {
            mb506Accion(2);
        }
        private void mb506Cancelar(object sender, EventArgs e)
        {
            mb506Accion(3);
        }
        private void mb506FrmHistorialVentas_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            mb506Buscar(sender, e);
        }
        private void mb506FormatearVenta(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvVentas.Columns[e.ColumnIndex].DataPropertyName == "estado" && e.Value != null)
                e.Value = Mensajes.mb506Traducir(Convert.ToString(e.Value));
        }
        private void mb506FormatearPago(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvPagos.Columns[e.ColumnIndex].DataPropertyName == "medioPago" && e.Value != null)
                e.Value = Mensajes.mb506Traducir(Convert.ToString(e.Value));
        }

    }
}
