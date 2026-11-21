using System;
using System.Collections.Generic;
using mb506.BEBMTech.ServicioTecnico;

namespace mb506.BLLBMTech
{
    public class ReglasServicioTecnico
    {
        private void mb506Texto(string valor, string nombre, int maximo, bool obligatorio = true)
        {
            if ((obligatorio && string.IsNullOrWhiteSpace(valor)) || (valor ?? "").Length > maximo)
                throw new Exception("Revise " + nombre + ".");
        }
        private void mb506Importe(decimal importe)
        {
            if (importe < 0 || importe > 9999999999999999.99m || decimal.Round(importe, 2) != importe)
                throw new Exception("El importe debe ser positivo o cero y tener hasta dos decimales.");
        }
        public void mb506ValidarIngreso(OrdenServicio orden)
        {
            if (orden == null || orden.equipo == null) throw new Exception("Ingrese el equipo.");
            mb506Texto(orden.equipo.dniCliente, "cliente", 20);
            mb506Texto(orden.equipo.tipo, "tipo de equipo", 60);
            mb506Texto(orden.equipo.marca, "marca", 100);
            mb506Texto(orden.equipo.modelo, "modelo", 100);
            mb506Texto(orden.equipo.numeroSerie, "numero de serie", 100, false);
            mb506Texto(orden.problema, "problema informado", 1000);
            mb506Texto(orden.accesorios, "accesorios", 500, false);
            mb506Texto(orden.observaciones, "observaciones", 1000, false);
        }
        public void mb506ValidarEstado(string estado, string paso)
        {
            bool permitido = false;
            switch (paso)
            {
                case "ASIGNAR": permitido = estado == "Recibida" || estado == "Asignada"; break;
                case "DIAGNOSTICO": permitido = estado == "Asignada"; break;
                case "PRESUPUESTO": permitido = estado == "Diagnosticada" || estado == "Esperando respuesta"; break;
                case "COMUNICAR":
                case "RESPUESTA": permitido = estado == "Esperando respuesta"; break;
                case "REPARACION": permitido = estado == "Aprobada" || estado == "En reparacion"; break;
                case "PRUEBAS": permitido = estado == "En reparacion"; break;
                case "AVISO": permitido = estado == "Lista para retirar"; break;
                case "ENTREGA": permitido = estado == "Lista para retirar" || estado == "Rechazada"; break;
            }
            if (!permitido) throw new Exception("La orden no permite esta operacion en su estado actual.");
        }
        public void mb506ValidarPaso(OrdenServicio orden, string paso)
        {
            if (orden == null || orden.idOrden <= 0) throw new Exception("Seleccione una orden.");
            switch (paso)
            {
                case "ASIGNAR":
                    mb506Texto(orden.tecnico, "tecnico", 150);
                    if (orden.prioridad != "Normal" && orden.prioridad != "Alta") throw new Exception("Seleccione una prioridad.");
                    break;
                case "DIAGNOSTICO":
                    mb506Texto(orden.diagnostico.falla, "falla detectada", 1000);
                    mb506Texto(orden.diagnostico.trabajo, "trabajo propuesto", 1000);
                    mb506Texto(orden.diagnostico.repuestosPrevistos, "repuestos previstos", 1000, false);
                    if (orden.diagnostico.diasEstimados < 1 || orden.diagnostico.diasEstimados > 365)
                        throw new Exception("Indique entre 1 y 365 dias estimados.");
                    break;
                case "PRESUPUESTO":
                    mb506Importe(orden.presupuesto.manoObra);
                    if (orden.presupuesto.vigencia.Date < DateTime.Today) throw new Exception("La vigencia no puede estar vencida.");
                    HashSet<int> productos = new HashSet<int>();
                    if (orden.presupuesto.detalles == null) throw new Exception("Revise los repuestos del presupuesto.");
                    foreach (DetallePresupuesto detalle in orden.presupuesto.detalles)
                    {
                        if (detalle == null || detalle.idProducto <= 0 || detalle.cantidad <= 0 || !productos.Add(detalle.idProducto))
                            throw new Exception("Revise los repuestos y evite productos repetidos.");
                        mb506Importe(detalle.precioUnitario);
                        if (detalle.precioUnitario == 0) throw new Exception("El precio del repuesto debe ser mayor a cero.");
                    }
                    orden.presupuesto.total = orden.presupuesto.mb506CalcularTotal();
                    mb506Importe(orden.presupuesto.total);
                    if (orden.presupuesto.total == 0) throw new Exception("El presupuesto debe tener un importe mayor a cero.");
                    break;
                case "RESPUESTA":
                    if (orden.presupuesto.respuesta != "Aprobada" && orden.presupuesto.respuesta != "Rechazada")
                        throw new Exception("Seleccione aprobar o rechazar.");
                    mb506Texto(orden.presupuesto.observaciones, "observaciones de la respuesta", 1000, false);
                    break;
                case "REPARACION":
                    mb506Texto(orden.reparacion.tareas, "tareas realizadas", 2000);
                    if (orden.reparacion.repuestos == null) throw new Exception("Revise los repuestos utilizados.");
                    HashSet<int> usados = new HashSet<int>();
                    foreach (RepuestoServicio repuesto in orden.reparacion.repuestos)
                        if (repuesto == null || repuesto.idProducto <= 0 || repuesto.cantidad <= 0 || !usados.Add(repuesto.idProducto))
                            throw new Exception("Revise los repuestos y evite productos repetidos.");
                    break;
                case "PRUEBAS": mb506Texto(orden.prueba.descripcion, "pruebas y resultados", 2000); break;
                case "AVISO": mb506Texto(orden.aviso.medio, "medio del aviso", 100); break;
                case "ENTREGA":
                    mb506Texto(orden.entrega.personaRetira, "persona que retira", 150);
                    mb506Texto(orden.entrega.garantia, "garantia", 1000, false);
                    break;
                case "COMUNICAR": break;
                default: throw new Exception("Operacion no reconocida.");
            }
        }
        public void mb506ValidarRespuesta(Presupuesto presupuesto)
        {
            if (!presupuesto.comunicado) throw new Exception("Registre primero la comunicacion del presupuesto.");
            if (presupuesto.respuesta == "Aprobada" && presupuesto.vigencia.Date < DateTime.Today)
                throw new Exception("El presupuesto vencio. Genere y comunique uno vigente.");
        }
        public void mb506ValidarPago(PagoServicio pago, decimal total)
        {
            if (pago == null) throw new Exception("Ingrese el pago.");
            new BLLVenta().mb506ValidarPago(new mb506.BEBMTech.Venta.Pago {
                medioPago = pago.medioPago, importe = pago.importe,
                numeroOperacion = pago.numeroOperacion, verificado = pago.verificado
            }, total, true);
        }
    }
}
