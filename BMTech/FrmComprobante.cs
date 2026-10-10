using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmComprobante : Form
    {
        private TextBox txtComprobante = new TextBox();
        private int idVenta;
        private string texto;

        public mb506FrmComprobante() : this(0) { }

        public mb506FrmComprobante(int venta)
        {
            IdiomasFormulario.mb506Vincular(this);
            idVenta = venta;
            Text = "Comprobante de venta";
            ClientSize = new Size(740, 640);
            StartPosition = FormStartPosition.CenterParent;
            txtComprobante.Multiline = true;
            txtComprobante.ReadOnly = true;
            txtComprobante.ScrollBars = ScrollBars.Both;
            txtComprobante.WordWrap = false;
            txtComprobante.Font = new Font("Consolas", 10);
            txtComprobante.SetBounds(20, 20, 700, 540);
            txtComprobante.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Button imprimir = new Button { Text = "Imprimir", BackColor = Color.Teal, ForeColor = Color.White };
            imprimir.SetBounds(20, 580, 130, 36);
            imprimir.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            imprimir.Click += mb506Imprimir;
            Controls.AddRange(new Control[] { txtComprobante, imprimir });
            string[] nombres = { "Guardar PDF", "Exportar XML" };
            for (int i = 0; i < nombres.Length; i++)
            {
                Button boton = new Button { Text = nombres[i], BackColor = Color.Teal, ForeColor = Color.White };
                boton.SetBounds(170 + i * 170, 580, 150, 36);
                boton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                bool pdf = i == 0;
                boton.Click += delegate { mb506Exportar(pdf); };
                Controls.Add(boton);
            }
            Load += mb506Cargar;
        }

        private void mb506Exportar(bool pdf)
        {
            try
            {
                new BLLVenta().mb506ObtenerComprobante(idVenta);
                using (SaveFileDialog dialogo = new SaveFileDialog { Filter = pdf ? "PDF (*.pdf)|*.pdf" : "XML (*.xml)|*.xml",
                    FileName = "BMTech_Venta_" + idVenta + (pdf ? ".pdf" : ".xml"), DefaultExt = pdf ? "pdf" : "xml" })
                {
                    if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                    if (pdf) ExportacionComprobante.mb506GuardarPdf(texto, dialogo.FileName);
                    else ExportacionComprobante.mb506GuardarXml(new BLLVenta().mb506ObtenerComprobante(idVenta), dialogo.FileName);
                    new BLLLog().mb506RegistrarEvento("Comprobante exportado de venta: " + idVenta, "Ventas", 1);
                    Mensajes.mb506Mostrar("Archivo guardado.");
                }
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Cargar(object sender, EventArgs e)
        {
            try
            {
                BLLVenta bll = new BLLVenta();
                var comprobante = bll.mb506ObtenerComprobante(idVenta);
                StringBuilder resultado = new StringBuilder();
                resultado.AppendLine(Mensajes.mb506Traducir("BMTech - COMPROBANTE INTERNO (NO FISCAL)"));
                resultado.AppendLine(Mensajes.mb506Traducir("Comprobante: ") + comprobante.idComprobante + Mensajes.mb506Traducir("   Venta: ") + idVenta);
                resultado.AppendLine(Mensajes.mb506Traducir("Fecha: ") + comprobante.fecha.ToString("dd/MM/yyyy HH:mm"));
                resultado.AppendLine(Mensajes.mb506Traducir("Cliente: ") + comprobante.cliente + "   DNI: " + comprobante.dniCliente);
                resultado.AppendLine(Mensajes.mb506Traducir("Vendedor: ") + comprobante.vendedor);
                resultado.AppendLine(new string('-', 65));
                foreach (DataRow fila in bll.mb506ObtenerDetalle(idVenta).Rows)
                    resultado.AppendLine(fila["codigo"] + " - " + fila["descripcion"] + " | " + fila["cantidad"] +
                        " x " + Convert.ToDecimal(fila["precioUnitario"]).ToString("N2") + " = " + Convert.ToDecimal(fila["subtotal"]).ToString("N2"));
                resultado.AppendLine(new string('-', 65));
                resultado.AppendLine(Mensajes.mb506Traducir("TOTAL: ") + comprobante.total.ToString("N2"));
                texto = resultado.ToString();
                txtComprobante.Text = texto;
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); Close(); }
        }

        private void mb506Imprimir(object sender, EventArgs e)
        {
            try
            {
                using (PrintDocument documento = new PrintDocument())
                using (PrintDialog dialogo = new PrintDialog())
                using (Font fuente = new Font("Consolas", 10))
                {
                    string pendiente = texto;
                    documento.DocumentName = "BMTech - Venta " + idVenta;
                    documento.PrintPage += delegate(object origen, PrintPageEventArgs pagina)
                    {
                        int caracteres, lineas;
                        pagina.Graphics.MeasureString(pendiente, fuente, pagina.MarginBounds.Size, StringFormat.GenericTypographic,
                            out caracteres, out lineas);
                        pagina.Graphics.DrawString(pendiente.Substring(0, caracteres), fuente, Brushes.Black,
                            pagina.MarginBounds, StringFormat.GenericTypographic);
                        pendiente = pendiente.Substring(caracteres);
                        pagina.HasMorePages = pendiente.Length > 0;
                    };
                    dialogo.Document = documento;
                    if (dialogo.ShowDialog(this) == DialogResult.OK) documento.Print();
                }
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
