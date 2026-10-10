using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmHistorialVentas : Form
    {
        private DateTimePicker dtpDesde = new DateTimePicker();
        private DateTimePicker dtpHasta = new DateTimePicker();
        private DataGridView dgvVentas = new DataGridView();
        private DataGridView dgvDetalles = new DataGridView();
        private DataGridView dgvPagos = new DataGridView();
        private BLLVenta bllVenta = new BLLVenta();

        public mb506FrmHistorialVentas()
        {
            IdiomasFormulario.mb506Vincular(this);
            Text = "Historial de ventas";
            ClientSize = new Size(1000, 640);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);
            Label desde = new Label { Text = "Desde", AutoSize = true };
            desde.SetBounds(20, 24, 70, 25);
            Label hasta = new Label { Text = "Hasta", AutoSize = true };
            hasta.SetBounds(300, 24, 70, 25);
            dtpDesde.SetBounds(90, 20, 180, 28);
            dtpHasta.SetBounds(370, 20, 180, 28);
            dtpDesde.Format = dtpHasta.Format = DateTimePickerFormat.Short;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            Button buscar = new Button { Text = "Buscar", BackColor = Color.Teal, ForeColor = Color.White };
            buscar.SetBounds(580, 18, 130, 34);
            buscar.Click += mb506Buscar;
            dgvVentas.SetBounds(20, 75, 960, 270);
            dgvDetalles.SetBounds(20, 390, 960, 225);
            foreach (DataGridView grilla in new[] { dgvVentas, dgvDetalles })
            {
                grilla.ReadOnly = true;
                grilla.AllowUserToAddRows = false;
                grilla.MultiSelect = false;
                grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grilla.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }
            dgvDetalles.Anchor |= AnchorStyles.Bottom;
            dgvVentas.CellClick += mb506Seleccionar;
            dgvVentas.CellFormatting += delegate(object origen, DataGridViewCellFormattingEventArgs e)
            {
                if (dgvVentas.Columns[e.ColumnIndex].DataPropertyName == "estado" && e.Value != null)
                    e.Value = Mensajes.mb506Traducir(Convert.ToString(e.Value));
            };
            Label detalle = new Label { Text = "Detalle de venta", ForeColor = Color.Teal, AutoSize = true };
            detalle.SetBounds(20, 355, 300, 25);
            Controls.AddRange(new Control[] { desde, hasta, dtpDesde, dtpHasta, buscar, dgvVentas, detalle, dgvDetalles });
            detalle.Visible = false;
            TabControl tabs = new TabControl();
            tabs.SetBounds(20, 360, 960, 255);
            tabs.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            TabPage tabDetalle = new TabPage("Detalle de venta");
            TabPage tabPagos = new TabPage("Pagos");
            tabs.TabPages.AddRange(new[] { tabDetalle, tabPagos });
            tabDetalle.Controls.Add(dgvDetalles);
            dgvDetalles.Dock = DockStyle.Fill;
            dgvPagos.Dock = DockStyle.Fill;
            dgvPagos.ReadOnly = true;
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPagos.CellFormatting += delegate(object origen, DataGridViewCellFormattingEventArgs e)
            {
                if (dgvPagos.Columns[e.ColumnIndex].DataPropertyName == "medioPago" && e.Value != null)
                    e.Value = Mensajes.mb506Traducir(Convert.ToString(e.Value));
            };
            tabPagos.Controls.Add(dgvPagos);
            Controls.Add(tabs);
            string[] acciones = { "Verificar pago", "Comprobante", "Registrar entrega", "Cancelar venta" };
            for (int i = 0; i < acciones.Length; i++)
            {
                Button boton = new Button { Text = acciones[i], BackColor = Color.Teal, ForeColor = Color.White };
                boton.SetBounds(20 + i * 190, 635, 175, 36);
                int accion = i;
                boton.Click += delegate { mb506Accion(accion); };
                Controls.Add(boton);
            }
            ClientSize = new Size(1000, 690);
            Load += mb506Buscar;
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
                if (dgvVentas.Columns.Contains("total")) dgvVentas.Columns["total"].DefaultCellStyle.Format = "N2";
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
                dgvDetalles.Columns["precioUnitario"].DefaultCellStyle.Format = "N2";
                dgvDetalles.Columns["idProducto"].Visible = false;
                dgvDetalles.Columns["subtotal"].DefaultCellStyle.Format = "N2";
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
