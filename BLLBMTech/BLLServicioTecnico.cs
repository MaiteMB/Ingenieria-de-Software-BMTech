using System;
using System.Data;
using mb506.BEBMTech.ServicioTecnico;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;

namespace mb506.BLLBMTech
{
    public class BLLServicioTecnico
    {
        private readonly DALServicioTecnico dalServicio = new DALServicioTecnico();
        private readonly ReglasServicioTecnico reglas = new ReglasServicioTecnico();

        private void mb506ValidarAcceso()
        {
            new BLLPermiso().mb506Validar("SERVICIOS");
            if (!dalServicio.mb506VerificarIntegridad()) throw new Exception("Revise la integridad del servicio tecnico.");
        }
        public DataTable mb506ObtenerOrdenes(string criterio)
        {
            mb506ValidarAcceso();
            if ((criterio ?? "").Length > 150) throw new Exception("La busqueda es demasiado larga.");
            return dalServicio.mb506ObtenerOrdenes((criterio ?? "").Trim());
        }
        public DataTable mb506ObtenerTecnicos()
        {
            mb506ValidarAcceso();
            return dalServicio.mb506ObtenerTecnicos();
        }
        public OrdenServicio mb506ObtenerOrden(int idOrden)
        {
            mb506ValidarAcceso();
            return dalServicio.mb506ObtenerOrden(idOrden);
        }
        public DataTable mb506ObtenerRepuestos(int idOrden)
        {
            mb506ValidarAcceso();
            return dalServicio.mb506ObtenerRepuestos(idOrden);
        }
        public DataTable mb506ObtenerPresupuestos(int idOrden)
        {
            mb506ValidarAcceso();
            return dalServicio.mb506ObtenerPresupuestos(idOrden);
        }
        public int mb506RegistrarIngreso(OrdenServicio orden)
        {
            mb506ValidarAcceso();
            reglas.mb506ValidarIngreso(orden);
            if (new DALCliente().mb506BuscarCliente(orden.equipo.dniCliente) == null)
                throw new Exception("El cliente no existe. Registrelo antes de recibir el equipo.");
            int id = dalServicio.mb506RegistrarIngreso(orden);
            new BLLLog().mb506RegistrarEvento("Ingreso de equipo. Orden: " + id, "Servicio tecnico", 1);
            return id;
        }
        private void mb506Guardar(OrdenServicio orden, string paso)
        {
            mb506ValidarAcceso();
            reglas.mb506ValidarPaso(orden, paso);
            OrdenServicio actual = dalServicio.mb506ObtenerOrden(orden.idOrden);
            if (orden.presupuesto.idPresupuesto != 0 && orden.presupuesto.idPresupuesto != actual.presupuesto.idPresupuesto)
                throw new Exception("El presupuesto cambio. Actualice la orden antes de continuar.");
            reglas.mb506ValidarEstado(actual.estado, paso);
            if (paso == "DIAGNOSTICO" || paso == "REPARACION" || paso == "PRUEBAS")
            {
                string usuario = SessionManager.getSession.Usuario.email;
                if (!string.Equals(actual.tecnico, usuario, StringComparison.OrdinalIgnoreCase) &&
                    !new BLLRol().mb506ObtenerPatentesPorPerfil(SessionManager.getSession.Usuario.perfil).Contains("SEGURIDAD"))
                    throw new Exception("Esta orden esta asignada a otro tecnico.");
            }
            if (paso == "RESPUESTA")
            {
                actual.presupuesto.respuesta = orden.presupuesto.respuesta;
                reglas.mb506ValidarRespuesta(actual.presupuesto);
            }
            if (paso == "ENTREGA" && actual.estado != "Rechazada") reglas.mb506ValidarPago(orden.pago, actual.presupuesto.total);
            dalServicio.mb506GuardarPaso(orden, paso, actual);
            new BLLLog().mb506RegistrarEvento(paso + ". Orden: " + orden.idOrden, "Servicio tecnico", paso == "ENTREGA" ? 2 : 1);
        }
        public void mb506AsignarTecnico(OrdenServicio orden) { mb506Guardar(orden, "ASIGNAR"); }
        public void mb506RegistrarDiagnostico(OrdenServicio orden) { mb506Guardar(orden, "DIAGNOSTICO"); }
        public void mb506GenerarPresupuesto(OrdenServicio orden) { mb506Guardar(orden, "PRESUPUESTO"); }
        public void mb506ComunicarPresupuesto(int idOrden) { mb506Guardar(new OrdenServicio { idOrden = idOrden }, "COMUNICAR"); }
        public void mb506RegistrarRespuesta(OrdenServicio orden) { mb506Guardar(orden, "RESPUESTA"); }
        public void mb506RegistrarReparacion(OrdenServicio orden) { mb506Guardar(orden, "REPARACION"); }
        public void mb506RegistrarPruebas(OrdenServicio orden) { mb506Guardar(orden, "PRUEBAS"); }
        public void mb506RegistrarAviso(OrdenServicio orden) { mb506Guardar(orden, "AVISO"); }
        public void mb506EntregarEquipo(OrdenServicio orden) { mb506Guardar(orden, "ENTREGA"); }
        public ComprobanteServicio mb506ObtenerComprobante(int idOrden)
        {
            mb506ValidarAcceso();
            return dalServicio.mb506ObtenerComprobante(idOrden);
        }
    }
}
