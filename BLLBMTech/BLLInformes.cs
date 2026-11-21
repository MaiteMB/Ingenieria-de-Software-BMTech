using System;
using System.Data;
using System.Text;
using mb506.BEBMTech.Informes;
using mb506.DALBMTech;

namespace mb506.BLLBMTech
{
    public class BLLInformes
    {
        public void mb506ValidarFiltros(string tipo, DateTime desde, DateTime hasta)
        {
            if (tipo != "Ventas" && tipo != "Servicio tecnico") throw new Exception("Seleccione un tipo de informe.");
            if (desde.Date > hasta.Date) throw new Exception("La fecha inicial no puede superar la final.");
            if (desde.Date < new DateTime(1753,1,1) || hasta.Date >= DateTime.MaxValue.Date)
                throw new Exception("El rango de fechas no es valido para la base de datos.");
        }
        public InformeOperaciones mb506Generar(string tipo, DateTime desde, DateTime hasta)
        {
            mb506ValidarFiltros(tipo, desde, hasta);
            new BLLPermiso().mb506Validar(tipo == "Ventas" ? "VENTAS" : "SERVICIOS");
            string[] tablas = tipo == "Ventas" ? new[] { "Venta", "Cliente" } : new[] { "OrdenServicio", "ComprobanteServicio", "PagoServicio" };
            if (!new DALIntegridad().mb506VerificarTablas(tablas)) throw new Exception("Revise la integridad antes de generar el informe.");
            InformeOperaciones informe = new InformeOperaciones { tipo = tipo, desde = desde.Date, hasta = hasta.Date,
                datos = new DALInformes().mb506ObtenerOperaciones(tipo, desde, hasta) };
            foreach (DataRow fila in informe.datos.Rows)
            {
                informe.totalCobrado += Convert.ToDecimal(fila["cobrado"]);
                informe.totalPendiente += Convert.ToDecimal(fila["pendiente"]);
            }
            return informe;
        }
        public string mb506PrepararTexto(InformeOperaciones informe)
        {
            if (informe == null) throw new Exception("Genere el informe primero.");
            StringBuilder texto = new StringBuilder();
            texto.AppendLine("BMTech - Informe de " + informe.tipo);
            texto.AppendLine("Desde: " + informe.desde.ToString("dd/MM/yyyy") + " - Hasta: " + informe.hasta.ToString("dd/MM/yyyy"));
            texto.AppendLine("Operaciones: " + informe.cantidad);
            texto.AppendLine("Total cobrado: " + informe.totalCobrado.ToString("N2"));
            texto.AppendLine("Total pendiente: " + informe.totalPendiente.ToString("N2"));
            texto.AppendLine("Las ventas canceladas no se suman al cobrado ni al pendiente.");
            texto.AppendLine("Servicio tecnico incluye solamente los comprobantes de cobro emitidos.");
            foreach (DataRow fila in informe.datos.Rows)
            {
                texto.AppendLine(new string('-',65));
                texto.AppendLine("Numero: " + fila["numero"] + " - Fecha: " + Convert.ToDateTime(fila["fecha"]).ToString("dd/MM/yyyy HH:mm"));
                texto.AppendLine("Cliente: " + fila["cliente"] + " - Estado: " + fila["estado"]);
                texto.AppendLine("Importe: " + Convert.ToDecimal(fila["total"]).ToString("N2"));
            }
            return texto.ToString();
        }
    }
}
