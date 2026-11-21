using System;
using System.Data.SqlClient;
using mb506.BEBMTech.ServicioTecnico;
using mb506.BEBMTech.Cliente;
using mb506.BEBMTech.Producto;
using mb506.BLLBMTech;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;

class PruebasServicioTecnico
{
    private static int cantidad;
    private static void Comprobar(bool condicion, string nombre)
    {
        if (!condicion) throw new Exception("FALLO: " + nombre);
        cantidad++;
        Console.WriteLine("OK: " + nombre);
    }
    private static bool Falla(Action accion)
    {
        try { accion(); return false; }
        catch (Exception ex) { Console.WriteLine("Rechazo esperado: " + ex.Message); return true; }
    }
    private static OrdenServicio Ingreso()
    {
        return new OrdenServicio { equipo = new Equipo { dniCliente = "98765432", tipo = "Notebook", marca = "Prueba", modelo = "Modelo 1", numeroSerie = "SERIE-1" }, problema = "No enciende" };
    }
    private static OrdenServicio Solicitud(int id) { return new OrdenServicio { idOrden = id }; }

    private static void Unitarias()
    {
        ReglasServicioTecnico reglas = new ReglasServicioTecnico();
        reglas.mb506ValidarIngreso(Ingreso()); Comprobar(true, "Ingreso valido");
        Comprobar(Falla(() => reglas.mb506ValidarIngreso(null)), "Ingreso nulo rechazado");
        OrdenServicio orden = Ingreso(); orden.problema = "";
        Comprobar(Falla(() => reglas.mb506ValidarIngreso(orden)), "Problema obligatorio");
        orden = Ingreso(); orden.equipo.marca = new string('X',101);
        Comprobar(Falla(() => reglas.mb506ValidarIngreso(orden)), "Longitud de marca validada");
        reglas.mb506ValidarEstado("Recibida", "ASIGNAR"); Comprobar(true, "Permite asignar ingreso");
        reglas.mb506ValidarEstado("Asignada", "DIAGNOSTICO"); Comprobar(true, "Permite diagnostico asignado");
        reglas.mb506ValidarEstado("Diagnosticada", "PRESUPUESTO"); Comprobar(true, "Permite presupuestar diagnostico");
        Comprobar(Falla(() => reglas.mb506ValidarEstado("Recibida", "REPARACION")), "No salta al trabajo sin aprobacion");
        Comprobar(Falla(() => reglas.mb506ValidarEstado("Rechazada", "REPARACION")), "Rechazo impide reparacion");
        Comprobar(Falla(() => reglas.mb506ValidarEstado("Aprobada", "ENTREGA")), "No entrega antes de las pruebas");
        Comprobar(Falla(() => reglas.mb506ValidarEstado("Entregada", "ENTREGA")), "No entrega dos veces");
        Comprobar(Falla(() => reglas.mb506ValidarEstado("Entregada", "ASIGNAR")), "Orden cerrada no se reasigna");
        Comprobar(Falla(() => reglas.mb506ValidarEstado("Recibida", "OTRO")), "Operacion desconocida rechazada");
        orden = Solicitud(1); orden.presupuesto.manoObra = 100;
        orden.presupuesto.detalles.Add(new DetallePresupuesto { idProducto = 1, cantidad = 2, precioUnitario = 15 });
        reglas.mb506ValidarPaso(orden, "PRESUPUESTO"); Comprobar(orden.presupuesto.total == 130, "Total de mano de obra y repuestos");
        orden.presupuesto.detalles.Add(new DetallePresupuesto { idProducto = 1, cantidad = 1, precioUnitario = 15 });
        Comprobar(Falla(() => reglas.mb506ValidarPaso(orden, "PRESUPUESTO")), "Repuesto duplicado rechazado");
        orden = Solicitud(1); orden.presupuesto.manoObra = -1;
        Comprobar(Falla(() => reglas.mb506ValidarPaso(orden, "PRESUPUESTO")), "Importe negativo rechazado");
        orden.presupuesto.manoObra = 1.001m;
        Comprobar(Falla(() => reglas.mb506ValidarPaso(orden, "PRESUPUESTO")), "Mas de dos decimales rechazado");
        orden.presupuesto.manoObra = 100; orden.presupuesto.vigencia = DateTime.Today.AddDays(-1);
        Comprobar(Falla(() => reglas.mb506ValidarPaso(orden, "PRESUPUESTO")), "Presupuesto vencido rechazado");
        orden.presupuesto.respuesta = "Aprobada";
        Comprobar(Falla(() => reglas.mb506ValidarRespuesta(orden.presupuesto)), "No aprueba presupuesto no comunicado");
        orden.presupuesto.comunicado = true;
        Comprobar(Falla(() => reglas.mb506ValidarRespuesta(orden.presupuesto)), "No aprueba presupuesto vencido");
        orden.presupuesto.respuesta = "Rechazada";
        reglas.mb506ValidarRespuesta(orden.presupuesto); Comprobar(true, "Puede registrar rechazo de presupuesto vencido");
        orden = Solicitud(1);
        Comprobar(Falla(() => reglas.mb506ValidarPaso(orden, "DIAGNOSTICO")), "Diagnostico vacio rechazado");
        Comprobar(Falla(() => reglas.mb506ValidarPaso(orden, "PRUEBAS")), "Pruebas vacias rechazadas");
        Comprobar(Falla(() => reglas.mb506ValidarPaso(orden, "ENTREGA")), "Persona que retira obligatoria");
        reglas.mb506ValidarPago(new PagoServicio { medioPago = "Efectivo", importe = 100, verificado = true },100);
        Comprobar(true, "Pago exacto verificado aceptado");
        Comprobar(Falla(() => reglas.mb506ValidarPago(new PagoServicio { medioPago = "Efectivo", importe = 90, verificado = true },100)), "Pago insuficiente rechazado");
        Comprobar(Falla(() => reglas.mb506ValidarPago(new PagoServicio { medioPago = "Efectivo", importe = 100, verificado = false },100)), "Pago no verificado rechazado");
        Comprobar(Falla(() => reglas.mb506ValidarPago(new PagoServicio { medioPago = "Transferencia", importe = 100, verificado = true },100)), "Transferencia requiere numero de operacion");
    }

    private static int Numero(string sql)
    {
        using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
        using (SqlCommand command = new SqlCommand(sql, conexion))
        {
            conexion.Open(); return Convert.ToInt32(command.ExecuteScalar());
        }
    }
    private static int Preparar(BLLServicioTecnico bll)
    {
        int id = bll.mb506RegistrarIngreso(Ingreso());
        OrdenServicio orden = Solicitud(id); orden.tecnico = "admin@bmtech.com"; orden.prioridad = "Normal";
        bll.mb506AsignarTecnico(orden);
        orden.diagnostico = new Diagnostico { falla = "Fuente averiada", trabajo = "Cambiar fuente", diasEstimados = 2 };
        bll.mb506RegistrarDiagnostico(orden);
        return id;
    }
    private static void Integracion()
    {
        string bd = Environment.GetEnvironmentVariable("BMTECH_SQL_DATABASE") ?? "";
        if (!bd.StartsWith("BMTech_PruebasEntrega2_", StringComparison.Ordinal)) throw new Exception("Solo se permite SQL en una base aislada de pruebas.");
        new DALIntegridad().mb506RegenerarSistema();
        SessionManager.mb506Login(new BLLUsuario().mb506Login("admin@bmtech.com", "1234"));
        BLLServicioTecnico bll = new BLLServicioTecnico();
        new BLLCliente().mb506RegistrarCliente(new Cliente { dni = "98765432", nombre = "Ana", apellido = "Prueba", telefono = "1123456789", correoElectronico = "ana@example.com" });
        new BLLProducto().mb506RegistrarProducto(new Producto { codigo = "REP-TEST", descripcion = "Fuente", marca = "Prueba", modelo = "Modelo", precio = 15, stock = 4 });
        int producto = Numero("SELECT idProducto FROM Producto WHERE codigo='REP-TEST'");
        int id = Preparar(bll);
        Comprobar(bll.mb506ObtenerOrden(id).estado == "Diagnosticada", "SQL ingreso, asignacion y diagnostico");
        OrdenServicio orden = Solicitud(id); orden.presupuesto.manoObra = 100;
        orden.presupuesto.detalles.Add(new DetallePresupuesto { idProducto = producto, cantidad = 2, precioUnitario = 15 });
        bll.mb506GenerarPresupuesto(orden);
        Comprobar(bll.mb506ObtenerOrden(id).presupuesto.total == 130, "SQL presupuesto persistido");
        orden.presupuesto.respuesta = "Aprobada";
        Comprobar(Falla(() => bll.mb506RegistrarRespuesta(orden)), "SQL exige comunicar presupuesto");
        bll.mb506ComunicarPresupuesto(id); bll.mb506RegistrarRespuesta(orden);
        orden.reparacion.tareas = "Se reemplaza un repuesto";
        orden.reparacion.repuestos.Add(new RepuestoServicio { idProducto = producto, cantidad = 1 });
        bll.mb506RegistrarReparacion(orden);
        Comprobar(Numero("SELECT stock FROM Producto WHERE codigo='REP-TEST'") == 3, "SQL descuenta stock al confirmar consumo");
        orden.reparacion.repuestos[0].cantidad = 2;
        Comprobar(Falla(() => bll.mb506RegistrarReparacion(orden)), "SQL no consume mas de lo autorizado");
        Comprobar(Numero("SELECT stock FROM Producto WHERE codigo='REP-TEST'") == 3, "SQL rollback conserva stock");
        orden.reparacion.repuestos[0].cantidad = 1;
        bll.mb506RegistrarReparacion(orden);
        Comprobar(Numero("SELECT stock FROM Producto WHERE codigo='REP-TEST'") == 2, "SQL consumo incremental sin duplicar guardados previos");
        orden.prueba = new PruebaEquipo { descripcion = "Primera prueba no satisfactoria", satisfactoria = false };
        bll.mb506RegistrarPruebas(orden);
        Comprobar(bll.mb506ObtenerOrden(id).estado == "En reparacion", "SQL prueba fallida no finaliza reparacion");
        orden.prueba = new PruebaEquipo { descripcion = "Encendido y funcionamiento correctos", satisfactoria = true };
        bll.mb506RegistrarPruebas(orden);
        orden.aviso.medio = "Telefono"; bll.mb506RegistrarAviso(orden);
        orden.entrega.personaRetira = "Ana Prueba"; orden.entrega.garantia = "30 dias";
        orden.pago = new PagoServicio { medioPago = "Transferencia", importe = 129, numeroOperacion = "OP-PRUEBA", verificado = true };
        Comprobar(Falla(() => bll.mb506EntregarEquipo(orden)), "SQL no entrega con importe incompleto");
        orden.pago.importe = 130; bll.mb506EntregarEquipo(orden);
        Comprobar(bll.mb506ObtenerOrden(id).estado == "Entregada", "SQL entrega finaliza orden");
        Comprobar(bll.mb506ObtenerComprobante(id).total == 130, "SQL emite comprobante por total aprobado");
        Comprobar(Numero("SELECT COUNT(*) FROM PagoServicio WHERE numeroOperacion='OP-PRUEBA'") == 0, "SQL cifra numero de operacion");
        Comprobar(Falla(() => bll.mb506EntregarEquipo(orden)), "SQL evita doble pago y entrega");

        int rechazo = Preparar(bll);
        Comprobar(Numero("SELECT COUNT(*) FROM Equipo WHERE numeroSerie='SERIE-1'") == 1, "SQL reutiliza el equipo identificado en otro ingreso");
        orden = Solicitud(rechazo); orden.presupuesto.manoObra = 80;
        bll.mb506GenerarPresupuesto(orden); bll.mb506ComunicarPresupuesto(rechazo);
        orden.presupuesto.respuesta = "Rechazada"; bll.mb506RegistrarRespuesta(orden);
        orden.reparacion.tareas = "No debe guardarse";
        Comprobar(Falla(() => bll.mb506RegistrarReparacion(orden)), "SQL rechazo bloquea reparacion");
        orden.entrega.personaRetira = "Ana Prueba"; bll.mb506EntregarEquipo(orden);
        Comprobar(bll.mb506ObtenerOrden(rechazo).estado == "Entregada", "SQL devuelve equipo sin reparar");
        Comprobar(Numero("SELECT COUNT(*) FROM PagoServicio WHERE idOrden=" + rechazo) == 0, "SQL devolucion rechazada no inventa pago");
        Comprobar(Numero("SELECT COUNT(*) FROM ComprobanteServicio WHERE idOrden=" + rechazo) == 0, "SQL devolucion rechazada no inventa comprobante de cobro");

        int sinStock = Preparar(bll);
        orden = Solicitud(sinStock); orden.presupuesto.manoObra = 100;
        orden.presupuesto.detalles.Add(new DetallePresupuesto { idProducto = producto, cantidad = 3, precioUnitario = 15 });
        bll.mb506GenerarPresupuesto(orden); bll.mb506ComunicarPresupuesto(sinStock);
        orden.presupuesto.respuesta = "Aprobada"; bll.mb506RegistrarRespuesta(orden);
        orden.reparacion.tareas = "No debe guardarse por falta de stock";
        orden.reparacion.repuestos.Add(new RepuestoServicio { idProducto = producto, cantidad = 3 });
        Comprobar(Falla(() => bll.mb506RegistrarReparacion(orden)), "SQL stock insuficiente rechaza consumo aprobado");
        Comprobar(Numero("SELECT COUNT(*) FROM Reparacion WHERE idOrden=" + sinStock) == 0, "SQL rollback no deja reparacion incompleta");
        Comprobar(Numero("SELECT stock FROM Producto WHERE codigo='REP-TEST'") == 2, "SQL falta de stock no descuenta parcialmente");
        Comprobar(bll.mb506ObtenerOrden(sinStock).estado == "Aprobada", "SQL error conserva estado previo");
        SessionManager.mb506Logout();
        SessionManager.mb506Login(new BLLUsuario().mb506Login("vendedor@bmtech.com", "1234"));
        orden.reparacion.repuestos.Clear(); orden.reparacion.tareas = "Otro tecnico";
        Comprobar(Falla(() => bll.mb506RegistrarReparacion(orden)), "SQL otro tecnico no registra trabajo ajeno");
        SessionManager.mb506Logout();
        SessionManager.mb506Login(new BLLUsuario().mb506Login("admin@bmtech.com", "1234"));

        int revision = Preparar(bll);
        orden = Solicitud(revision); orden.presupuesto.manoObra = 100;
        bll.mb506GenerarPresupuesto(orden);
        int presupuestoAnterior = bll.mb506ObtenerOrden(revision).presupuesto.idPresupuesto;
        orden.presupuesto.manoObra = 120; bll.mb506GenerarPresupuesto(orden);
        bll.mb506ComunicarPresupuesto(revision);
        orden.presupuesto.idPresupuesto = presupuestoAnterior; orden.presupuesto.respuesta = "Aprobada";
        Comprobar(Falla(() => bll.mb506RegistrarRespuesta(orden)), "SQL rechaza aprobacion de una version desactualizada");
        Comprobar(bll.mb506ObtenerPresupuestos(revision).Rows.Count == 2, "SQL conserva versiones del presupuesto");
        Comprobar(new DALIntegridad().mb506VerificarSistema().Count == 0, "SQL integridad correcta tras todo el circuito");
        var informes = new BLLInformes();
        var informe = informes.mb506Generar("Servicio tecnico", DateTime.Today, DateTime.Today);
        Comprobar(informe.cantidad == 1 && informe.totalCobrado == 130m && informe.totalPendiente == 0m, "SQL informe incluye el cobro y excluye la devolucion sin pago");
        Comprobar(informes.mb506Generar("Servicio tecnico", DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1)).cantidad == 0, "SQL informe respeta el rango de fechas");
        var dalProducto = new DALProducto();
        Producto antes = dalProducto.mb506ObtenerProductoPorId(producto);
        string descripcionAnterior = antes.descripcion;
        antes.descripcion = "Descripcion modificada para probar restauracion";
        Comprobar(dalProducto.mb506ModificarProducto(antes), "SQL modifica producto y conserva historial");
        var cambios = new BLLCambios();
        var versiones = cambios.mb506ObtenerVersionesProducto(producto);
        var anterior = versiones.Find(v => v.descripcion == descripcionAnterior && v.stock == antes.stock);
        Comprobar(anterior != null && cambios.mb506RestaurarProducto(anterior.idHistorial), "SQL restaura una version del historial");
        Comprobar(dalProducto.mb506ObtenerProductoPorId(producto).descripcion == descripcionAnterior, "SQL recupera los datos anteriores");
        Comprobar(!cambios.mb506RestaurarProducto(int.MaxValue), "SQL no restaura una version inexistente");
        Comprobar(new DALIntegridad().mb506VerificarSistema().Count == 0, "SQL integridad valida despues de restaurar");
        var roles = new BLLRol();
        roles.mb506GuardarElemento("Perfil", "TecnicoBase", "TecnicoBase", true);
        roles.mb506GuardarElemento("Perfil", "TecnicoCompleto", "TecnicoCompleto", true);
        roles.mb506GuardarAsignaciones("Perfil", "TecnicoBase", new System.Collections.Generic.List<string> { "T:SERVICIOS" });
        roles.mb506GuardarAsignaciones("Perfil", "TecnicoCompleto", new System.Collections.Generic.List<string> { "P:TecnicoBase", "F:VENTA" });
        Comprobar(roles.mb506ObtenerPatentesPorPerfil("TecnicoCompleto").Contains("SERVICIOS"), "SQL hereda permisos de un perfil incluido");
        Comprobar(roles.mb506ObtenerPatentesPorPerfil("TecnicoCompleto").Contains("VENTAS"), "SQL combina perfiles y familias");
        Comprobar(Falla(() => roles.mb506GuardarAsignaciones("Perfil", "TecnicoBase", new System.Collections.Generic.List<string> { "P:TecnicoCompleto" })), "SQL rechaza ciclos entre perfiles");
        Comprobar(roles.mb506ObtenerPatentesPorPerfil("TecnicoBase").Contains("SERVICIOS"), "SQL conserva asignaciones despues de rechazar ciclo");
        Comprobar(Falla(() => roles.mb506GuardarAsignaciones("Perfil", "TecnicoBase", new System.Collections.Generic.List<string> { "P:TecnicoBase" })), "SQL rechaza perfil dentro de si mismo");
        Comprobar(Falla(() => roles.mb506GuardarAsignaciones("Perfil", "Administrador", new System.Collections.Generic.List<string>())), "SQL impide quitar el propio acceso a Seguridad");
        new BLLUsuario().mb506RegistrarUsuario(new mb506.BEBMTech.Usuario.Usuario { email="tecnico@example.com",nombre="Carlos",apellido="Lopez",perfil="TecnicoCompleto" }, "1234");
        Comprobar(new DALServicioTecnico().mb506ObtenerTecnicos().Select("email='tecnico@example.com'").Length == 1, "SQL tecnico con permiso heredado aparece disponible");
        Comprobar(Numero("SELECT COUNT(*) FROM sys.tables WHERE name IN ('Perfil','Familia','Patente','PerfilFamilia','FamiliaPatente','PerfilPerfil','PerfilPatente')") == 7, "SQL siete tablas de permisos");
        Comprobar(new DALIntegridad().mb506VerificarSistema().Count == 0, "SQL integridad correcta de perfiles y relaciones");
        new BLLRol().mb506GuardarElemento("Perfil", "SinServicios", "Sin servicios", true);
        new BLLUsuario().mb506RegistrarUsuario(new mb506.BEBMTech.Usuario.Usuario { email = "sinservicios@example.com", nombre = "Sin", apellido = "Permiso", perfil = "SinServicios" }, "1234");
        SessionManager.mb506Logout();
        SessionManager.mb506Login(new BLLUsuario().mb506Login("sinservicios@example.com", "1234"));
        Comprobar(Falla(() => informes.mb506Generar("Servicio tecnico", DateTime.Today, DateTime.Today)), "SQL perfil sin permiso no genera informes");
        Comprobar(Falla(() => cambios.mb506ObtenerVersionesProducto(producto)), "SQL perfil sin Seguridad no consulta versiones");
        Comprobar(Falla(() => bll.mb506ObtenerOrdenes("")), "SQL perfil sin Servicios no consulta ordenes");
        SessionManager.mb506Logout();
        Comprobar(Falla(() => bll.mb506ObtenerOrdenes("")), "SQL sin sesion no consulta ordenes");
    }
    public static int Ejecutar(bool conBaseDePruebas)
    {
        try
        {
            Unitarias();
            if (conBaseDePruebas) Integracion();
            Console.WriteLine(cantidad + " pruebas de Servicio tecnico correctas.");
            return 0;
        }
        catch (Exception ex) { Console.WriteLine(ex); return 1; }
        finally { SessionManager.mb506Logout(); }
    }
}
