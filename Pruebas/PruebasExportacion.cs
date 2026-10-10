using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using mb506.BEBMTech.Venta;
using mb506.BMTech;
using PdfSharp.Pdf.IO;

class PruebasExportacion
{
    static int Main(string[] args)
    {
        try
        {
            string carpeta = args.Length == 0 ? Path.Combine(Path.GetTempPath(), "BMTechPruebas") : args[0];
            Directory.CreateDirectory(carpeta);
            Comprobante comprobante = new Comprobante { idComprobante = 1, idVenta = 1, fecha = new DateTime(2026,10,9),
                dniCliente = "12345678", cliente = "Ana Perez", vendedor = "Vendedor de prueba", total = 1500.50m };
            comprobante.detalles.Add(new DetalleVenta { idProducto = 5, codigoProducto = "P001", descripcionProducto = "Notebook",
                cantidad = 1, precioUnitario = 1500.50m, subtotal = 1500.50m });
            string xml = Path.Combine(carpeta, "comprobante-prueba.xml");
            ExportacionComprobante.mb506GuardarXml(comprobante, xml);
            using (FileStream archivo = File.OpenRead(xml))
            {
                Comprobante copia = (Comprobante)new XmlSerializer(typeof(Comprobante)).Deserialize(archivo);
                if (copia.total != 1500.50m || copia.detalles.Count != 1 || copia.detalles[0].codigoProducto != "P001")
                    throw new Exception("La serializacion no conserva los datos.");
            }
            Console.WriteLine("OK: XML conserva encabezado y detalle.");
            string pdf = Path.Combine(carpeta, "comprobante-prueba.pdf");
            ExportacionComprobante.mb506GuardarPdf("BMTech - COMPROBANTE INTERNO\nCliente: Ana Perez\nProducto: Notebook\nTotal: 1500,50", pdf);
            using (var documento = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                if (documento.PageCount != 1) throw new Exception("El PDF basico no tiene una pagina.");
            Console.WriteLine("OK: PDF basico se puede abrir.");
            StringBuilder texto = new StringBuilder("BMTech - COMPROBANTE EXTENSO\n");
            for (int i = 1; i <= 100; i++) texto.AppendLine("Producto " + i + ": " + new string('x', 160));
            string extenso = Path.Combine(carpeta, "comprobante-extenso.pdf");
            ExportacionComprobante.mb506GuardarPdf(texto.ToString(), extenso);
            using (var documento = PdfReader.Open(extenso, PdfDocumentOpenMode.Import))
                if (documento.PageCount < 2) throw new Exception("El PDF extenso debe dividirse en paginas.");
            Console.WriteLine("OK: PDF extenso divide lineas y paginas.");
            Console.WriteLine("3 pruebas de exportacion correctas. No se ejecuto SQL.");
            return 0;
        }
        catch (Exception ex) { Console.WriteLine(ex); return 1; }
    }
}
