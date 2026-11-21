using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mb506.BEBMTech.ServicioTecnico;
using mb506.BEBMTech.Producto;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmServicioTecnico : Form
    {
        private readonly BLLServicioTecnico bllServicio = new BLLServicioTecnico();
        private OrdenServicio seleccionada;
        private BindingList<DetallePresupuesto> detalles = new BindingList<DetallePresupuesto>();
        private BindingList<RepuestoServicio> consumos = new BindingList<RepuestoServicio>();

        public mb506FrmServicioTecnico()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }

        private void mb506FrmServicioTecnico_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                cmbTecnico.DataSource = bllServicio.mb506ObtenerTecnicos();
                cmbProductoPresupuesto.DataSource = new BLLProducto().mb506BuscarProducto("");
                cmbProductoReparacion.DataSource = new BLLProducto().mb506BuscarProducto("");
                dgvPresupuesto.DataSource = detalles;
                dgvConsumos.DataSource = consumos;
                mb506Buscar(sender, e);
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Buscar(object sender, EventArgs e)
        {
            try
            {
                dgvOrdenes.DataSource = bllServicio.mb506ObtenerOrdenes(txtBuscar.Text);
                dgvOrdenes.ClearSelection();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Seleccionar(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                int idOrden = Convert.ToInt32(dgvOrdenes.Rows[e.RowIndex].Cells[colOrdenId.Name].Value);
                mb506Cargar(idOrden);
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Cargar(int idOrden)
        {
            seleccionada = bllServicio.mb506ObtenerOrden(idOrden);
            txtOrden.Text = seleccionada.idOrden + " - " + seleccionada.estado;

            txtDni.Text = seleccionada.equipo.dniCliente;
            txtTipo.Text = seleccionada.equipo.tipo;
            txtMarca.Text = seleccionada.equipo.marca;
            txtModelo.Text = seleccionada.equipo.modelo;
            txtSerie.Text = seleccionada.equipo.numeroSerie;
            txtProblema.Text = seleccionada.problema;
            txtAccesorios.Text = seleccionada.accesorios;
            txtObservacionesIngreso.Text = seleccionada.observaciones;

            cmbTecnico.SelectedValue = seleccionada.tecnico;
            cmbPrioridad.SelectedItem = seleccionada.prioridad;

            txtFalla.Text = seleccionada.diagnostico.falla;
            txtTrabajo.Text = seleccionada.diagnostico.trabajo;
            txtRepuestosPrevistos.Text = seleccionada.diagnostico.repuestosPrevistos;
            if (seleccionada.diagnostico.diasEstimados == 0)
            {
                nudDias.Value = 1;
            }
            else
            {
                nudDias.Value = seleccionada.diagnostico.diasEstimados;
            }

            nudManoObra.Value = seleccionada.presupuesto.manoObra;
            dtpVigencia.Value = seleccionada.presupuesto.vigencia;
            detalles = new BindingList<DetallePresupuesto>(seleccionada.presupuesto.detalles);
            dgvPresupuesto.DataSource = detalles;
            txtTotal.Text = seleccionada.presupuesto.total.ToString("N2");
            dgvHistorialPresupuestos.DataSource = bllServicio.mb506ObtenerPresupuestos(idOrden);
            txtRespuestaObservaciones.Text = seleccionada.presupuesto.observaciones;

            txtTareasAnteriores.Text = seleccionada.reparacion.tareas;
            txtTareas.Text = "";
            consumos.Clear();
            dgvRepuestosUsados.DataSource = bllServicio.mb506ObtenerRepuestos(idOrden);

            txtPruebas.Text = seleccionada.prueba.descripcion;
            chkSatisfactoria.Checked = seleccionada.prueba.satisfactoria;
            txtMedioAviso.Text = seleccionada.aviso.medio;

            if (seleccionada.estado == "Rechazada")
            {
                nudPago.Value = 0;
            }
            else
            {
                nudPago.Value = seleccionada.presupuesto.total;
            }
            txtOperacion.Text = "";
            chkPagoVerificado.Checked = false;
            txtPersonaRetira.Text = seleccionada.entrega.personaRetira;
            txtGarantia.Text = seleccionada.entrega.garantia;
            txtComprobante.Text = "";
        }

        private OrdenServicio mb506Solicitud()
        {
            if (seleccionada == null)
            {
                throw new Exception("Seleccione una orden.");
            }

            OrdenServicio solicitud = new OrdenServicio();
            solicitud.idOrden = seleccionada.idOrden;
            solicitud.presupuesto.idPresupuesto = seleccionada.presupuesto.idPresupuesto;
            return solicitud;
        }

        private void mb506Confirmado()
        {
            int id = seleccionada.idOrden;
            mb506Buscar(this, EventArgs.Empty);
            mb506Cargar(id);
            Mensajes.mb506Mostrar("Operacion guardada.");
        }

        private void mb506Nuevo(object sender, EventArgs e)
        {
            seleccionada = null;
            txtOrden.Text = "";
            txtDni.Clear();
            txtTipo.Clear();
            txtMarca.Clear();
            txtModelo.Clear();
            txtSerie.Clear();
            txtProblema.Clear();
            txtAccesorios.Clear();
            txtObservacionesIngreso.Clear();
            txtFalla.Clear();
            txtTrabajo.Clear();
            txtRepuestosPrevistos.Clear();
            txtTareas.Clear();
            txtTareasAnteriores.Clear();
            txtPruebas.Clear();
            txtMedioAviso.Clear();
            txtPersonaRetira.Clear();
            txtGarantia.Clear();
            txtComprobante.Clear();
            txtRespuestaObservaciones.Clear();
            txtOperacion.Clear();
            detalles.Clear();
            consumos.Clear();
            dgvHistorialPresupuestos.DataSource = null;
            dgvRepuestosUsados.DataSource = null;
        }

        private void mb506Cliente(object sender, EventArgs e)
        {
            using (mb506FrmCliente formulario = new mb506FrmCliente())
            {
                formulario.ShowDialog(this);
            }
        }

        private void mb506Ingreso(object sender, EventArgs e)
        {
            try
            {
                if (seleccionada != null)
                {
                    throw new Exception("Use Nuevo ingreso para registrar otro equipo.");
                }

                OrdenServicio orden = new OrdenServicio();
                orden.equipo.dniCliente = txtDni.Text.Trim();
                orden.equipo.tipo = txtTipo.Text.Trim();
                orden.equipo.marca = txtMarca.Text.Trim();
                orden.equipo.modelo = txtModelo.Text.Trim();
                orden.equipo.numeroSerie = txtSerie.Text.Trim();
                orden.problema = txtProblema.Text.Trim();
                orden.accesorios = txtAccesorios.Text.Trim();
                orden.observaciones = txtObservacionesIngreso.Text.Trim();

                int id = bllServicio.mb506RegistrarIngreso(orden);
                mb506Buscar(sender, e);
                mb506Cargar(id);
                Mensajes.mb506Mostrar("Orden registrada: " + id);
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Asignar(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                orden.tecnico = Convert.ToString(cmbTecnico.SelectedValue);
                orden.prioridad = Convert.ToString(cmbPrioridad.SelectedItem);
                bllServicio.mb506AsignarTecnico(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Diagnostico(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                orden.diagnostico.falla = txtFalla.Text.Trim();
                orden.diagnostico.trabajo = txtTrabajo.Text.Trim();
                orden.diagnostico.repuestosPrevistos = txtRepuestosPrevistos.Text.Trim();
                orden.diagnostico.diasEstimados = (int)nudDias.Value;
                bllServicio.mb506RegistrarDiagnostico(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506PrecioProducto(object sender, EventArgs e)
        {
            Producto producto = cmbProductoPresupuesto.SelectedItem as Producto;
            if (producto != null)
            {
                nudPrecioRepuesto.Value = producto.precio;
            }
        }

        private void mb506AgregarPresupuesto(object sender, EventArgs e)
        {
            Producto producto = cmbProductoPresupuesto.SelectedItem as Producto;
            if (producto == null) return;

            foreach (DetallePresupuesto detalle in detalles)
            {
                if (detalle.idProducto == producto.idProducto)
                {
                    Mensajes.mb506Mostrar("El producto ya esta agregado.");
                    return;
                }
            }

            DetallePresupuesto nuevo = new DetallePresupuesto();
            nuevo.idProducto = producto.idProducto;
            nuevo.cantidad = (int)nudCantidadPresupuesto.Value;
            nuevo.precioUnitario = nudPrecioRepuesto.Value;
            detalles.Add(nuevo);
            mb506Total(sender, e);
        }

        private void mb506QuitarPresupuesto(object sender, EventArgs e)
        {
            if (dgvPresupuesto.CurrentRow != null)
            {
                DetallePresupuesto detalle = dgvPresupuesto.CurrentRow.DataBoundItem as DetallePresupuesto;
                if (detalle != null)
                {
                    detalles.Remove(detalle);
                }
            }
            mb506Total(sender, e);
        }

        private void mb506Total(object sender, EventArgs e)
        {
            Presupuesto presupuesto = new Presupuesto();
            presupuesto.manoObra = nudManoObra.Value;
            presupuesto.detalles = new List<DetallePresupuesto>(detalles);
            txtTotal.Text = presupuesto.mb506CalcularTotal().ToString("N2");
        }

        private void mb506Presupuesto(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                Presupuesto presupuesto = new Presupuesto();
                presupuesto.manoObra = nudManoObra.Value;
                presupuesto.vigencia = dtpVigencia.Value.Date;
                presupuesto.detalles = new List<DetallePresupuesto>(detalles);
                orden.presupuesto = presupuesto;
                bllServicio.mb506GenerarPresupuesto(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Comunicar(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                if (Mensajes.mb506Mostrar("Confirma que comunico al cliente el ultimo presupuesto guardado?", "Presupuesto", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                bllServicio.mb506ComunicarPresupuesto(orden.idOrden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Respuesta(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                orden.presupuesto.respuesta = Convert.ToString(cmbRespuesta.SelectedItem);
                orden.presupuesto.observaciones = txtRespuestaObservaciones.Text.Trim();
                bllServicio.mb506RegistrarRespuesta(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506AgregarConsumo(object sender, EventArgs e)
        {
            Producto producto = cmbProductoReparacion.SelectedItem as Producto;
            if (producto == null) return;

            foreach (RepuestoServicio repuesto in consumos)
            {
                if (repuesto.idProducto == producto.idProducto)
                {
                    Mensajes.mb506Mostrar("El producto ya esta agregado.");
                    return;
                }
            }

            RepuestoServicio nuevo = new RepuestoServicio();
            nuevo.idProducto = producto.idProducto;
            nuevo.cantidad = (int)nudCantidadConsumo.Value;
            consumos.Add(nuevo);
        }

        private void mb506QuitarConsumo(object sender, EventArgs e)
        {
            if (dgvConsumos.CurrentRow != null)
            {
                RepuestoServicio repuesto = dgvConsumos.CurrentRow.DataBoundItem as RepuestoServicio;
                if (repuesto != null)
                {
                    consumos.Remove(repuesto);
                }
            }
        }

        private void mb506Reparacion(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                orden.reparacion.tareas = txtTareas.Text.Trim();
                orden.reparacion.repuestos = new List<RepuestoServicio>(consumos);
                bllServicio.mb506RegistrarReparacion(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Pruebas(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                orden.prueba.descripcion = txtPruebas.Text.Trim();
                orden.prueba.satisfactoria = chkSatisfactoria.Checked;
                bllServicio.mb506RegistrarPruebas(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Aviso(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                orden.aviso.medio = txtMedioAviso.Text.Trim();
                bllServicio.mb506RegistrarAviso(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Entregar(object sender, EventArgs e)
        {
            try
            {
                OrdenServicio orden = mb506Solicitud();
                orden.entrega.personaRetira = txtPersonaRetira.Text.Trim();
                orden.entrega.garantia = txtGarantia.Text.Trim();
                orden.pago.medioPago = Convert.ToString(cmbMedioPago.SelectedItem);
                orden.pago.importe = nudPago.Value;
                orden.pago.numeroOperacion = txtOperacion.Text.Trim();
                orden.pago.verificado = chkPagoVerificado.Checked;

                if (Mensajes.mb506Mostrar("Confirma el cobro correspondiente y la entrega del equipo?", "Entrega", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                bllServicio.mb506EntregarEquipo(orden);
                mb506Confirmado();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Comprobante(object sender, EventArgs e)
        {
            txtComprobante.Clear();
            try
            {
                ComprobanteServicio comprobante = bllServicio.mb506ObtenerComprobante(mb506Solicitud().idOrden);
                txtComprobante.Text = "BMTech - Comprobante de servicio " + comprobante.idComprobante + Environment.NewLine +
                    "Orden: " + comprobante.idOrden + " - " + comprobante.fecha.ToString("dd/MM/yyyy HH:mm") + Environment.NewLine +
                    "Cliente: " + comprobante.cliente + Environment.NewLine +
                    "Equipo: " + comprobante.equipo + Environment.NewLine +
                    "Trabajo: " + comprobante.trabajo + Environment.NewLine +
                    "Medio de pago: " + comprobante.medioPago + Environment.NewLine +
                    "Total: " + comprobante.total.ToString("N2");
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Pdf(object sender, EventArgs e)
        {
            try
            {
                mb506Comprobante(sender, e);
                if (string.IsNullOrEmpty(txtComprobante.Text)) return;

                dlgPdf.FileName = "Servicio_" + mb506Solicitud().idOrden + ".pdf";
                if (dlgPdf.ShowDialog(this) == DialogResult.OK)
                {
                    ExportacionComprobante.mb506GuardarPdf(txtComprobante.Text, dlgPdf.FileName);
                }
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506Cerrar(object sender, EventArgs e)
        {
            Close();
        }

        private void mb506Xml(object sender, EventArgs e)
        {
            try
            {
                ComprobanteServicio comprobante = bllServicio.mb506ObtenerComprobante(mb506Solicitud().idOrden);
                dlgServicioXml.FileName = "Servicio_" + comprobante.idOrden + ".xml";
                if (dlgServicioXml.ShowDialog(this) == DialogResult.OK)
                {
                    ExportacionComprobante.mb506GuardarXmlServicio(comprobante, dlgServicioXml.FileName);
                }
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
            }
        }
    }
}
