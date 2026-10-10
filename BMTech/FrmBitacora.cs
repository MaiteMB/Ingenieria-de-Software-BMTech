using System;
using System.Drawing;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmBitacora : Form
    {
        private DataGridView dgvEventos = new DataGridView();
        private DateTimePicker dtpDesde = new DateTimePicker();
        private DateTimePicker dtpHasta = new DateTimePicker();
        private TextBox txtUsuario = new TextBox();
        private TextBox txtAccion = new TextBox();

        public mb506FrmBitacora()
        {
            IdiomasFormulario.mb506Vincular(this);
            Text = "Bitacora";
            ClientSize = new Size(1100, 560);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);
            BackColor = Color.White;
            string[] nombres = { "Desde", "Hasta", "Usuario", "Actividad" };
            Control[] filtros = { dtpDesde, dtpHasta, txtUsuario, txtAccion };
            int[] anchos = { 150, 150, 245, 245 };
            int izquierda = 20;
            for (int i = 0; i < filtros.Length; i++)
            {
                Controls.Add(new Label { Text = nombres[i], Left = izquierda, Top = 15, Width = anchos[i], Height = 25 });
                filtros[i].SetBounds(izquierda, 45, anchos[i], 28);
                Controls.Add(filtros[i]);
                izquierda += anchos[i] + 20;
            }
            dtpDesde.Format = dtpHasta.Format = DateTimePickerFormat.Short;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            txtUsuario.MaxLength = 150; txtAccion.MaxLength = 255;
            Button buscar = new Button { Text = "Buscar", BackColor = Color.Teal, ForeColor = Color.White };
            buscar.SetBounds(950, 40, 130, 36);
            buscar.Click += mb506Cargar;
            dgvEventos.SetBounds(20, 95, 1060, 440);
            dgvEventos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEventos.ReadOnly = true;
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.AddRange(new Control[] { dgvEventos, buscar });
            Load += mb506Cargar;
        }

        private void mb506Cargar(object sender, EventArgs e)
        {
            try
            {
                dgvEventos.DataSource = new BLLLog().mb506ObtenerEventos(dtpDesde.Value, dtpHasta.Value, txtUsuario.Text, txtAccion.Text);
                if (dgvEventos.Columns.Contains("fecha")) dgvEventos.Columns["fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
