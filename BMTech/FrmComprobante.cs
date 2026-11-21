using System;
using System.Data;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmComprobante : Form
    {
        private int idVenta;
        private string texto;

        public mb506FrmComprobante()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }

        public mb506FrmComprobante(int venta) : this()
        {
            idVenta = venta;
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
            if (DesignMode) return;
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
                {
                    string pendiente = texto;
                    documento.DocumentName = "BMTech - Venta " + idVenta;
                    documento.PrintPage += delegate(object origen, PrintPageEventArgs pagina)
                    {
                        int caracteres;
                        mb506DibujarPagina(pagina, pendiente, out caracteres);
                        pendiente = pendiente.Substring(caracteres);
                        pagina.HasMorePages = pendiente.Length > 0;
                    };
                    dialogo.Document = documento;
                    if (dialogo.ShowDialog(this) == DialogResult.OK) documento.Print();
                }
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506GuardarPdf(object sender, EventArgs e)
        {
            mb506Exportar(true);
        }

        private void mb506GuardarXml(object sender, EventArgs e)
        {
            mb506Exportar(false);
        }

        private void mb506ImportarXml(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialogo = new OpenFileDialog { Filter = "XML (*.xml)|*.xml" })
                {
                    if (dialogo.ShowDialog(this) != DialogResult.OK) return;

                    // Se lee el archivo y se compara con el comprobante guardado
                    var leido = ExportacionComprobante.mb506LeerXml(dialogo.FileName);
                    var guardado = new BLLVenta().mb506ObtenerComprobante(leido.idVenta);

                    StringBuilder resultado = new StringBuilder();
                    resultado.AppendLine(Mensajes.mb506Traducir("Datos recuperados del archivo XML"));
                    resultado.AppendLine(Mensajes.mb506Traducir("Comprobante: ") + leido.idComprobante + Mensajes.mb506Traducir("   Venta: ") + leido.idVenta);
                    resultado.AppendLine(Mensajes.mb506Traducir("Fecha: ") + leido.fecha.ToString("dd/MM/yyyy HH:mm"));
                    resultado.AppendLine(Mensajes.mb506Traducir("Cliente: ") + leido.cliente + "   DNI: " + leido.dniCliente);
                    resultado.AppendLine(Mensajes.mb506Traducir("Vendedor: ") + leido.vendedor);
                    resultado.AppendLine(new string('-', 65));
                    foreach (var detalle in leido.detalles)
                        resultado.AppendLine(detalle.codigoProducto + " - " + detalle.descripcionProducto + " | " + detalle.cantidad +
                            " x " + detalle.precioUnitario.ToString("N2") + " = " + detalle.subtotal.ToString("N2"));
                    resultado.AppendLine(new string('-', 65));
                    resultado.AppendLine(Mensajes.mb506Traducir("TOTAL: ") + leido.total.ToString("N2"));
                    txtComprobante.Text = resultado.ToString();

                    bool coincide = leido.idComprobante == guardado.idComprobante && leido.total == guardado.total &&
                        leido.dniCliente == guardado.dniCliente && leido.detalles.Count == guardado.detalles.Count;
                    new BLLLog().mb506RegistrarEvento("Comprobante importado de venta: " + leido.idVenta, "Ventas", 1);
                    if (coincide) Mensajes.mb506Mostrar("Los datos del archivo coinciden con el comprobante registrado.");
                    else Mensajes.mb506Mostrar("Los datos del archivo no coinciden con el comprobante registrado.");
                }
            }
            catch (InvalidOperationException) { Mensajes.mb506Mostrar("El archivo no es un comprobante de venta valido."); }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
