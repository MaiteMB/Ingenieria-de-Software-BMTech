using System;
using System.IO;
using System.Xml.Serialization;
using mb506.BEBMTech.Venta;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace mb506.BMTech
{
    public static class ExportacionComprobante
    {
        public static void mb506GuardarXml(Comprobante comprobante, string ruta)
        {
            XmlSerializer serializador = new XmlSerializer(typeof(Comprobante));
            using (FileStream archivo = File.Create(ruta)) serializador.Serialize(archivo, comprobante);
        }

        public static void mb506GuardarPdf(string texto, string ruta)
        {
            using (PdfDocument documento = new PdfDocument())
            {
                documento.Info.Title = "BMTech - Comprobante interno";
                XFont fuente = new XFont("Consolas", 10, XFontStyleEx.Regular);
                PdfPage pagina = documento.AddPage();
                XGraphics dibujo = XGraphics.FromPdfPage(pagina);
                double y = 45;
                try
                {
                    foreach (string linea in texto.Replace("\r", "").Split('\n'))
                    {
                        string pendiente = linea;
                        do
                        {
                            if (y > pagina.Height.Point - 45)
                            {
                                dibujo.Dispose();
                                pagina = documento.AddPage();
                                dibujo = XGraphics.FromPdfPage(pagina);
                                y = 45;
                            }
                            int largo = pendiente.Length;
                            while (largo > 0 && dibujo.MeasureString(pendiente.Substring(0, largo), fuente).Width > pagina.Width.Point - 90) largo--;
                            if (largo == 0 && pendiente.Length > 0) largo = 1;
                            dibujo.DrawString(pendiente.Substring(0, largo), fuente, XBrushes.Black, new XPoint(45, y));
                            pendiente = pendiente.Substring(largo);
                            y += 15;
                        } while (pendiente.Length > 0);
                    }
                }
                finally { dibujo.Dispose(); }
                for (int i = 0; i < documento.PageCount; i++)
                    using (XGraphics pie = XGraphics.FromPdfPage(documento.Pages[i], XGraphicsPdfPageOptions.Append))
                        pie.DrawString("BMTech - " + (i + 1) + "/" + documento.PageCount, fuente, XBrushes.Gray,
                            new XPoint(45, documento.Pages[i].Height.Point - 25));
                documento.Save(ruta);
            }
        }
    }
}
