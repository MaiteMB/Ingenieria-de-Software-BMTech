using System;
using System.Collections.Generic;

namespace mb506.BEBMTech.ServicioTecnico
{
    public class OrdenServicio
    {
        public int idOrden { get; set; }
        public Equipo equipo { get; set; } = new Equipo();
        public DateTime fechaIngreso { get; set; }
        public string problema { get; set; }
        public string accesorios { get; set; }
        public string observaciones { get; set; }
        public string estado { get; set; }
        public string tecnico { get; set; }
        public DateTime? fechaAsignacion { get; set; }
        public string prioridad { get; set; }
        public Diagnostico diagnostico { get; set; } = new Diagnostico();
        public Presupuesto presupuesto { get; set; } = new Presupuesto();
        public Reparacion reparacion { get; set; } = new Reparacion();
        public PruebaEquipo prueba { get; set; } = new PruebaEquipo();
        public AvisoRetiro aviso { get; set; } = new AvisoRetiro();
        public PagoServicio pago { get; set; } = new PagoServicio();
        public EntregaServicio entrega { get; set; } = new EntregaServicio();
    }

    public class Equipo
    {
        public int idEquipo { get; set; }
        public string dniCliente { get; set; }
        public string tipo { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string numeroSerie { get; set; }
    }
    public class Diagnostico
    {
        public string falla { get; set; }
        public string trabajo { get; set; }
        public string repuestosPrevistos { get; set; }
        public int diasEstimados { get; set; }
    }
    public class Presupuesto
    {
        public int idPresupuesto { get; set; }
        public DateTime vigencia { get; set; } = DateTime.Today.AddDays(7);
        public decimal manoObra { get; set; }
        public decimal total { get; set; }
        public string respuesta { get; set; }
        public string observaciones { get; set; }
        public bool comunicado { get; set; }
        public List<DetallePresupuesto> detalles { get; set; } = new List<DetallePresupuesto>();
        public decimal mb506CalcularTotal()
        {
            decimal importe = manoObra;
            foreach (DetallePresupuesto detalle in detalles)
                importe += detalle.cantidad * detalle.precioUnitario;
            return importe;
        }
    }
    public class DetallePresupuesto
    {
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
    }
    public class Reparacion
    {
        public string tareas { get; set; }
        public List<RepuestoServicio> repuestos { get; set; } = new List<RepuestoServicio>();
    }
    public class RepuestoServicio
    {
        public int idProducto { get; set; }
        public int cantidad { get; set; }
    }
    public class PruebaEquipo
    {
        public string descripcion { get; set; }
        public bool satisfactoria { get; set; }
    }
    public class AvisoRetiro
    {
        public string medio { get; set; }
    }
    public class PagoServicio
    {
        public string medioPago { get; set; }
        public decimal importe { get; set; }
        public string numeroOperacion { get; set; }
        public bool verificado { get; set; }
    }
    public class EntregaServicio
    {
        public string personaRetira { get; set; }
        public string garantia { get; set; }
    }
    public class ComprobanteServicio
    {
        public int idComprobante { get; set; }
        public int idOrden { get; set; }
        public DateTime fecha { get; set; }
        public string cliente { get; set; }
        public string equipo { get; set; }
        public string trabajo { get; set; }
        public decimal total { get; set; }
        public string medioPago { get; set; }
    }
}
