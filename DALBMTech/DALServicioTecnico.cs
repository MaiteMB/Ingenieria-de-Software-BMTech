using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using mb506.BEBMTech.ServicioTecnico;
using mb506.ServiciosBMTech.Seguridad;

namespace mb506.DALBMTech
{
    public class DALServicioTecnico
    {
        private readonly string[] tablas = { "Cliente", "Usuario", "Producto", "HistorialCambiosProducto",
            "Equipo", "OrdenServicio", "Diagnostico", "Presupuesto", "DetallePresupuesto", "Reparacion",
            "RepuestoServicio", "PruebaEquipo", "AvisoRetiro", "PagoServicio", "ComprobanteServicio", "EntregaServicio" };

        private SqlParameter mb506P(string nombre, object valor)
        {
            if (valor == null)
            {
                valor = DBNull.Value;
            }

            SqlParameter parametro = new SqlParameter(nombre, valor);

            if (valor is decimal)
            {
                parametro.SqlDbType = SqlDbType.Decimal;
                parametro.Precision = 18;
                parametro.Scale = 2;
            }

            return parametro;
        }

        private SqlCommand mb506Comando(string sql, SqlConnection conexion, SqlTransaction transaccion, params SqlParameter[] parametros)
        {
            SqlCommand command = new SqlCommand(sql, conexion, transaccion);
            command.Parameters.AddRange(parametros);
            return command;
        }

        private int mb506Ejecutar(string sql, SqlConnection conexion, SqlTransaction transaccion, params SqlParameter[] parametros)
        {
            using (SqlCommand command = mb506Comando(sql, conexion, transaccion, parametros))
            {
                return command.ExecuteNonQuery();
            }
        }

        private int mb506Numero(string sql, SqlConnection conexion, SqlTransaction transaccion, params SqlParameter[] parametros)
        {
            using (SqlCommand command = mb506Comando(sql, conexion, transaccion, parametros))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private DataTable mb506Datos(string sql, SqlConnection conexion, SqlTransaction transaccion, params SqlParameter[] parametros)
        {
            DataTable datos = new DataTable();

            using (SqlCommand command = mb506Comando(sql, conexion, transaccion, parametros))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(datos);
            }

            return datos;
        }

        public bool mb506VerificarIntegridad()
        {
            return new DALIntegridad().mb506VerificarTablas((string[])tablas.Clone());
        }

        public DataTable mb506ObtenerOrdenes(string criterio)
        {
            string query = @"
                SELECT o.idOrden, o.fechaIngreso, e.dniCliente, c.nombre + ' ' + c.apellido AS cliente,
                       e.tipo, e.marca, e.modelo, o.estado, o.tecnico, o.prioridad
                FROM OrdenServicio o
                JOIN Equipo e ON e.idEquipo = o.idEquipo
                JOIN Cliente c ON c.dni = e.dniCliente
                WHERE e.dniCliente LIKE @buscar
                   OR c.nombre LIKE @buscar
                   OR c.apellido LIKE @buscar
                   OR CONVERT(VARCHAR(12), o.idOrden) = @numero
                   OR o.estado LIKE @buscar
                ORDER BY o.idOrden DESC";

            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                return mb506Datos(query, conexion, null, mb506P("@buscar", "%" + criterio + "%"), mb506P("@numero", criterio));
            }
        }

        public DataTable mb506ObtenerTecnicos()
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                DataTable usuarios = mb506Datos("SELECT email, idperfil FROM Usuario WHERE activo = 1 ORDER BY email", conexion, null);

                DataTable tecnicos = new DataTable();
                tecnicos.Columns.Add("email", typeof(string));

                foreach (DataRow usuario in usuarios.Rows)
                {
                    if (mb506TieneServicios(Convert.ToString(usuario["idperfil"]), conexion, null))
                    {
                        tecnicos.Rows.Add(usuario["email"]);
                    }
                }

                return tecnicos;
            }
        }

        private bool mb506TieneServicios(string perfil, SqlConnection conexion, SqlTransaction transaccion)
        {
            HashSet<string> camino = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            mb506.BEBMTech.Roles.Perfil datosPerfil = new DALRol().mb506LeerPerfil(perfil, conexion, transaccion, camino);

            foreach (mb506.BEBMTech.Roles.Rol rol in datosPerfil.mb506ObtenerTodos())
            {
                if (rol is mb506.BEBMTech.Roles.Patente && rol.idRol == "SERVICIOS")
                {
                    return true;
                }
            }

            return false;
        }

        public DataTable mb506ObtenerRepuestos(int idOrden)
        {
            string query = @"
                SELECT r.idProducto, p.codigo, p.descripcion, SUM(r.cantidad) AS cantidad
                FROM RepuestoServicio r
                JOIN Reparacion a ON a.idReparacion = r.idReparacion
                JOIN Producto p ON p.idProducto = r.idProducto
                WHERE a.idOrden = @orden
                GROUP BY r.idProducto, p.codigo, p.descripcion";

            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                return mb506Datos(query, conexion, null, mb506P("@orden", idOrden));
            }
        }

        public DataTable mb506ObtenerPresupuestos(int idOrden)
        {
            string query = @"
                SELECT idPresupuesto, fecha, vigencia, manoObra, total, comunicado, respuesta
                FROM Presupuesto
                WHERE idOrden = @orden
                ORDER BY idPresupuesto DESC";

            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                return mb506Datos(query, conexion, null, mb506P("@orden", idOrden));
            }
        }

        public OrdenServicio mb506ObtenerOrden(int idOrden)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                return mb506LeerOrden(idOrden, conexion, null);
            }
        }

        private OrdenServicio mb506LeerOrden(int idOrden, SqlConnection conexion, SqlTransaction transaccion)
        {
            DataTable datos = mb506Datos(@"
                SELECT o.*, e.dniCliente, e.tipo, e.marca, e.modelo, e.numeroSerie
                FROM OrdenServicio o
                JOIN Equipo e ON e.idEquipo = o.idEquipo
                WHERE o.idOrden = @orden", conexion, transaccion, mb506P("@orden", idOrden));

            if (datos.Rows.Count == 0)
            {
                throw new Exception("La orden no existe.");
            }

            DataRow fila = datos.Rows[0];
            OrdenServicio orden = new OrdenServicio();
            orden.idOrden = idOrden;
            orden.fechaIngreso = Convert.ToDateTime(fila["fechaIngreso"]);
            orden.problema = Convert.ToString(fila["problema"]);
            orden.accesorios = Convert.ToString(fila["accesorios"]);
            orden.observaciones = Convert.ToString(fila["observaciones"]);
            orden.estado = Convert.ToString(fila["estado"]);
            orden.tecnico = Convert.ToString(fila["tecnico"]);
            orden.prioridad = Convert.ToString(fila["prioridad"]);

            if (fila.IsNull("fechaAsignacion"))
            {
                orden.fechaAsignacion = null;
            }
            else
            {
                orden.fechaAsignacion = Convert.ToDateTime(fila["fechaAsignacion"]);
            }

            orden.equipo.idEquipo = Convert.ToInt32(fila["idEquipo"]);
            orden.equipo.dniCliente = Convert.ToString(fila["dniCliente"]);
            orden.equipo.tipo = Convert.ToString(fila["tipo"]);
            orden.equipo.marca = Convert.ToString(fila["marca"]);
            orden.equipo.modelo = Convert.ToString(fila["modelo"]);
            orden.equipo.numeroSerie = Convert.ToString(fila["numeroSerie"]);

            datos = mb506Datos("SELECT * FROM Diagnostico WHERE idOrden = @orden", conexion, transaccion, mb506P("@orden", idOrden));
            if (datos.Rows.Count > 0)
            {
                fila = datos.Rows[0];
                orden.diagnostico.falla = Convert.ToString(fila["falla"]);
                orden.diagnostico.trabajo = Convert.ToString(fila["trabajo"]);
                orden.diagnostico.repuestosPrevistos = Convert.ToString(fila["repuestosPrevistos"]);
                orden.diagnostico.diasEstimados = Convert.ToInt32(fila["diasEstimados"]);
            }

            datos = mb506Datos("SELECT TOP 1 * FROM Presupuesto WHERE idOrden = @orden ORDER BY idPresupuesto DESC", conexion, transaccion, mb506P("@orden", idOrden));
            if (datos.Rows.Count > 0)
            {
                fila = datos.Rows[0];
                orden.presupuesto.idPresupuesto = Convert.ToInt32(fila["idPresupuesto"]);
                orden.presupuesto.vigencia = Convert.ToDateTime(fila["vigencia"]);
                orden.presupuesto.manoObra = Convert.ToDecimal(fila["manoObra"]);
                orden.presupuesto.total = Convert.ToDecimal(fila["total"]);
                orden.presupuesto.comunicado = Convert.ToBoolean(fila["comunicado"]);
                orden.presupuesto.respuesta = Convert.ToString(fila["respuesta"]);
                orden.presupuesto.observaciones = Convert.ToString(fila["observaciones"]);

                DataTable detalles = mb506Datos("SELECT * FROM DetallePresupuesto WHERE idPresupuesto = @id", conexion, transaccion,
                    mb506P("@id", orden.presupuesto.idPresupuesto));

                foreach (DataRow detalle in detalles.Rows)
                {
                    DetallePresupuesto nuevo = new DetallePresupuesto();
                    nuevo.idProducto = Convert.ToInt32(detalle["idProducto"]);
                    nuevo.cantidad = Convert.ToInt32(detalle["cantidad"]);
                    nuevo.precioUnitario = Convert.ToDecimal(detalle["precioUnitario"]);
                    orden.presupuesto.detalles.Add(nuevo);
                }
            }

            datos = mb506Datos("SELECT * FROM Reparacion WHERE idOrden = @orden", conexion, transaccion, mb506P("@orden", idOrden));
            if (datos.Rows.Count > 0)
            {
                orden.reparacion.tareas = Convert.ToString(datos.Rows[0]["tareas"]);
            }

            datos = mb506Datos(@"
                SELECT TOP 1 p.*
                FROM PruebaEquipo p
                JOIN Reparacion r ON r.idReparacion = p.idReparacion
                WHERE r.idOrden = @orden
                ORDER BY p.idPrueba DESC", conexion, transaccion, mb506P("@orden", idOrden));
            if (datos.Rows.Count > 0)
            {
                orden.prueba.descripcion = Convert.ToString(datos.Rows[0]["descripcion"]);
                orden.prueba.satisfactoria = Convert.ToBoolean(datos.Rows[0]["satisfactoria"]);
            }

            datos = mb506Datos("SELECT TOP 1 medio FROM AvisoRetiro WHERE idOrden = @orden ORDER BY idAviso DESC", conexion, transaccion, mb506P("@orden", idOrden));
            if (datos.Rows.Count > 0)
            {
                orden.aviso.medio = Convert.ToString(datos.Rows[0]["medio"]);
            }

            datos = mb506Datos("SELECT * FROM EntregaServicio WHERE idOrden = @orden", conexion, transaccion, mb506P("@orden", idOrden));
            if (datos.Rows.Count > 0)
            {
                orden.entrega.personaRetira = Convert.ToString(datos.Rows[0]["personaRetira"]);
                orden.entrega.garantia = Convert.ToString(datos.Rows[0]["garantia"]);
            }

            return orden;
        }

        public int mb506RegistrarIngreso(OrdenServicio orden)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    DALIntegridad integridad = new DALIntegridad();
                    integridad.mb506PrepararOperacion(conexion, transaccion, (string[])tablas.Clone());

                    string serie = orden.equipo.numeroSerie ?? "";

                    // Si se conoce la serie y coincide la identificacion, reutiliza el equipo en otro ingreso.
                    int idEquipo = mb506Numero(@"
                        SELECT TOP 1 idEquipo FROM Equipo
                        WHERE dniCliente = @dni AND tipo = @tipo AND marca = @marca AND modelo = @modelo
                          AND numeroSerie = @serie AND @serie <> ''
                        ORDER BY idEquipo", conexion, transaccion,
                        mb506P("@dni", orden.equipo.dniCliente), mb506P("@tipo", orden.equipo.tipo),
                        mb506P("@marca", orden.equipo.marca), mb506P("@modelo", orden.equipo.modelo), mb506P("@serie", serie));

                    if (idEquipo == 0)
                    {
                        idEquipo = mb506Numero(@"
                            INSERT INTO Equipo(dniCliente, tipo, marca, modelo, numeroSerie)
                            VALUES(@dni, @tipo, @marca, @modelo, @serie);
                            SELECT SCOPE_IDENTITY();", conexion, transaccion,
                            mb506P("@dni", orden.equipo.dniCliente), mb506P("@tipo", orden.equipo.tipo), mb506P("@marca", orden.equipo.marca),
                            mb506P("@modelo", orden.equipo.modelo), mb506P("@serie", serie));
                    }

                    int idOrden = mb506Numero(@"
                        INSERT INTO OrdenServicio(idEquipo, fechaIngreso, problema, accesorios, observaciones, estado, prioridad)
                        VALUES(@equipo, GETDATE(), @problema, @accesorios, @observaciones, 'Recibida', 'Normal');
                        SELECT SCOPE_IDENTITY();", conexion, transaccion,
                        mb506P("@equipo", idEquipo), mb506P("@problema", orden.problema),
                        mb506P("@accesorios", orden.accesorios ?? ""), mb506P("@observaciones", orden.observaciones ?? ""));

                    integridad.mb506ActualizarTablas(conexion, transaccion, (string[])tablas.Clone());
                    transaccion.Commit();
                    return idOrden;
                }
            }
        }

        public void mb506GuardarPaso(OrdenServicio orden, string paso, OrdenServicio anterior)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    DALIntegridad integridad = new DALIntegridad();
                    integridad.mb506PrepararOperacion(conexion, transaccion, (string[])tablas.Clone());

                    OrdenServicio actual = mb506LeerOrden(orden.idOrden, conexion, transaccion);
                    if (actual.estado != anterior.estado || actual.tecnico != anterior.tecnico || actual.presupuesto.idPresupuesto != anterior.presupuesto.idPresupuesto)
                    {
                        throw new Exception("La orden cambio. Actualice y vuelva a intentarlo.");
                    }

                    string estado = actual.estado;

                    switch (paso)
                    {
                        case "ASIGNAR":
                            DataTable tecnicos = mb506Datos("SELECT idperfil FROM Usuario WHERE email = @tecnico AND activo = 1",
                                conexion, transaccion, mb506P("@tecnico", orden.tecnico));
                            if (tecnicos.Rows.Count != 1 || !mb506TieneServicios(Convert.ToString(tecnicos.Rows[0]["idperfil"]), conexion, transaccion))
                            {
                                throw new Exception("Seleccione un tecnico activo con permiso de Servicios.");
                            }
                            mb506Ejecutar("UPDATE OrdenServicio SET tecnico = @tecnico, fechaAsignacion = GETDATE(), prioridad = @prioridad WHERE idOrden = @orden",
                                conexion, transaccion, mb506P("@tecnico", orden.tecnico), mb506P("@prioridad", orden.prioridad), mb506P("@orden", orden.idOrden));
                            estado = "Asignada";
                            break;

                        case "DIAGNOSTICO":
                            mb506Ejecutar(@"
                                INSERT INTO Diagnostico(idOrden, fecha, falla, trabajo, repuestosPrevistos, diasEstimados)
                                VALUES(@orden, GETDATE(), @falla, @trabajo, @repuestos, @dias)", conexion, transaccion,
                                mb506P("@orden", orden.idOrden), mb506P("@falla", orden.diagnostico.falla), mb506P("@trabajo", orden.diagnostico.trabajo),
                                mb506P("@repuestos", orden.diagnostico.repuestosPrevistos ?? ""), mb506P("@dias", orden.diagnostico.diasEstimados));
                            estado = "Diagnosticada";
                            break;

                        case "PRESUPUESTO":
                            int presupuesto = mb506Numero(@"
                                INSERT INTO Presupuesto(idOrden, fecha, vigencia, manoObra, total)
                                VALUES(@orden, GETDATE(), @vigencia, @mano, @total);
                                SELECT SCOPE_IDENTITY();", conexion, transaccion,
                                mb506P("@orden", orden.idOrden), mb506P("@vigencia", orden.presupuesto.vigencia.Date),
                                mb506P("@mano", orden.presupuesto.manoObra), mb506P("@total", orden.presupuesto.total));

                            foreach (DetallePresupuesto detalle in orden.presupuesto.detalles)
                            {
                                int existe = mb506Numero("SELECT COUNT(*) FROM Producto WHERE idProducto = @producto AND activo = 1",
                                    conexion, transaccion, mb506P("@producto", detalle.idProducto));
                                if (existe != 1)
                                {
                                    throw new Exception("El repuesto del presupuesto no existe o esta inactivo.");
                                }
                                mb506Ejecutar(@"
                                    INSERT INTO DetallePresupuesto(idPresupuesto, idProducto, cantidad, precioUnitario)
                                    VALUES(@id, @producto, @cantidad, @precio)", conexion, transaccion, mb506P("@id", presupuesto),
                                    mb506P("@producto", detalle.idProducto), mb506P("@cantidad", detalle.cantidad), mb506P("@precio", detalle.precioUnitario));
                            }
                            estado = "Esperando respuesta";
                            break;

                        case "COMUNICAR":
                            if (actual.presupuesto.idPresupuesto == 0)
                            {
                                throw new Exception("Genere el presupuesto primero.");
                            }
                            mb506Ejecutar("UPDATE Presupuesto SET comunicado = 1, fechaComunicacion = GETDATE() WHERE idPresupuesto = @id",
                                conexion, transaccion, mb506P("@id", actual.presupuesto.idPresupuesto));
                            break;

                        case "RESPUESTA":
                            if (!actual.presupuesto.comunicado || (orden.presupuesto.respuesta == "Aprobada" && actual.presupuesto.vigencia.Date < DateTime.Today))
                            {
                                throw new Exception("El presupuesto no fue comunicado o ya vencio.");
                            }
                            mb506Ejecutar("UPDATE Presupuesto SET respuesta = @respuesta, fechaRespuesta = GETDATE(), observaciones = @observaciones WHERE idPresupuesto = @id",
                                conexion, transaccion, mb506P("@respuesta", orden.presupuesto.respuesta), mb506P("@observaciones", orden.presupuesto.observaciones ?? ""),
                                mb506P("@id", actual.presupuesto.idPresupuesto));
                            estado = orden.presupuesto.respuesta;
                            break;

                        case "REPARACION":
                            mb506GuardarReparacion(orden, actual, conexion, transaccion);
                            estado = "En reparacion";
                            break;

                        case "PRUEBAS":
                            mb506Ejecutar(@"
                                INSERT INTO PruebaEquipo(idReparacion, fecha, descripcion, satisfactoria)
                                SELECT idReparacion, GETDATE(), @descripcion, @resultado FROM Reparacion WHERE idOrden = @orden",
                                conexion, transaccion, mb506P("@orden", orden.idOrden), mb506P("@descripcion", orden.prueba.descripcion), mb506P("@resultado", orden.prueba.satisfactoria));
                            if (orden.prueba.satisfactoria)
                            {
                                estado = "Lista para retirar";
                            }
                            else
                            {
                                estado = "En reparacion";
                            }
                            break;

                        case "AVISO":
                            mb506Ejecutar("INSERT INTO AvisoRetiro(idOrden, fecha, medio) VALUES(@orden, GETDATE(), @medio)", conexion, transaccion,
                                mb506P("@orden", orden.idOrden), mb506P("@medio", orden.aviso.medio));
                            break;

                        case "ENTREGA":
                            string garantia = orden.entrega.garantia ?? "";
                            if (actual.estado == "Rechazada")
                            {
                                garantia = "Sin reparacion";
                            }
                            else
                            {
                                mb506GuardarCobro(orden, actual, conexion, transaccion);
                            }
                            mb506Ejecutar("INSERT INTO EntregaServicio(idOrden, fecha, personaRetira, garantia) VALUES(@orden, GETDATE(), @persona, @garantia)",
                                conexion, transaccion, mb506P("@orden", orden.idOrden), mb506P("@persona", orden.entrega.personaRetira), mb506P("@garantia", garantia));
                            estado = "Entregada";
                            break;

                        default:
                            throw new Exception("Operacion no reconocida.");
                    }

                    mb506Ejecutar("UPDATE OrdenServicio SET estado = @estado WHERE idOrden = @orden", conexion, transaccion,
                        mb506P("@estado", estado), mb506P("@orden", orden.idOrden));
                    integridad.mb506ActualizarTablas(conexion, transaccion, (string[])tablas.Clone());
                    transaccion.Commit();
                }
            }
        }

        private void mb506GuardarReparacion(OrdenServicio orden, OrdenServicio actual, SqlConnection conexion, SqlTransaction transaccion)
        {
            string tareas;
            if (string.IsNullOrEmpty(actual.reparacion.tareas))
            {
                tareas = orden.reparacion.tareas;
            }
            else
            {
                tareas = actual.reparacion.tareas + Environment.NewLine + orden.reparacion.tareas;
            }

            if (tareas.Length > 2000)
            {
                throw new Exception("El historial de tareas supera los 2000 caracteres.");
            }

            int reparacion = mb506Numero("SELECT idReparacion FROM Reparacion WHERE idOrden = @orden", conexion, transaccion, mb506P("@orden", orden.idOrden));
            if (reparacion == 0)
            {
                reparacion = mb506Numero("INSERT INTO Reparacion(idOrden, fecha, tareas) VALUES(@orden, GETDATE(), @tareas); SELECT SCOPE_IDENTITY();",
                    conexion, transaccion, mb506P("@orden", orden.idOrden), mb506P("@tareas", tareas));
            }
            else
            {
                mb506Ejecutar("UPDATE Reparacion SET tareas = @tareas WHERE idReparacion = @id", conexion, transaccion,
                    mb506P("@tareas", tareas), mb506P("@id", reparacion));
            }

            foreach (RepuestoServicio repuesto in orden.reparacion.repuestos)
            {
                int autorizado = mb506Numero("SELECT cantidad FROM DetallePresupuesto WHERE idPresupuesto = @id AND idProducto = @producto", conexion, transaccion,
                    mb506P("@id", actual.presupuesto.idPresupuesto), mb506P("@producto", repuesto.idProducto));
                int utilizado = mb506Numero("SELECT COALESCE(SUM(cantidad), 0) FROM RepuestoServicio WHERE idReparacion = @id AND idProducto = @producto",
                    conexion, transaccion, mb506P("@id", reparacion), mb506P("@producto", repuesto.idProducto));

                if ((long)utilizado + repuesto.cantidad > autorizado)
                {
                    throw new Exception("El consumo supera lo aprobado en el presupuesto.");
                }

                int actualizados = mb506Ejecutar("UPDATE Producto SET stock = stock - @cantidad WHERE idProducto = @producto AND activo = 1 AND stock >= @cantidad",
                    conexion, transaccion, mb506P("@cantidad", repuesto.cantidad), mb506P("@producto", repuesto.idProducto));
                if (actualizados != 1)
                {
                    throw new Exception("El repuesto no esta activo o no tiene stock suficiente.");
                }

                mb506Ejecutar("INSERT INTO RepuestoServicio(idReparacion, idProducto, cantidad) VALUES(@id, @producto, @cantidad)", conexion, transaccion,
                    mb506P("@id", reparacion), mb506P("@producto", repuesto.idProducto), mb506P("@cantidad", repuesto.cantidad));
            }
        }

        private void mb506GuardarCobro(OrdenServicio orden, OrdenServicio actual, SqlConnection conexion, SqlTransaction transaccion)
        {
            if (!actual.prueba.satisfactoria || !orden.pago.verificado || orden.pago.importe != actual.presupuesto.total)
            {
                throw new Exception("Revise las pruebas y el pago por el importe aprobado.");
            }

            int pago = mb506Numero(@"
                INSERT INTO PagoServicio(idOrden, fecha, medioPago, importe, numeroOperacion, verificado)
                VALUES(@orden, GETDATE(), @medio, @importe, @operacion, 1);
                SELECT SCOPE_IDENTITY();", conexion, transaccion,
                mb506P("@orden", orden.idOrden), mb506P("@medio", orden.pago.medioPago), mb506P("@importe", orden.pago.importe),
                mb506P("@operacion", Cifrado.mb506Cifrar(orden.pago.numeroOperacion ?? "")));

            mb506Ejecutar(@"
                INSERT INTO ComprobanteServicio(idOrden, idPagoServicio, fecha, cliente, equipo, trabajo, total, medioPago)
                SELECT @orden, @pago, GETDATE(), c.nombre + ' ' + c.apellido, e.tipo + ' ' + e.marca + ' ' + e.modelo, @trabajo, @total, @medio
                FROM Equipo e
                JOIN Cliente c ON c.dni = e.dniCliente
                WHERE e.idEquipo = @equipo", conexion, transaccion,
                mb506P("@orden", orden.idOrden), mb506P("@pago", pago), mb506P("@trabajo", actual.reparacion.tareas),
                mb506P("@total", actual.presupuesto.total), mb506P("@medio", orden.pago.medioPago), mb506P("@equipo", actual.equipo.idEquipo));
        }

        public ComprobanteServicio mb506ObtenerComprobante(int idOrden)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                DataTable datos = mb506Datos("SELECT * FROM ComprobanteServicio WHERE idOrden = @orden", conexion, null, mb506P("@orden", idOrden));
                if (datos.Rows.Count == 0)
                {
                    throw new Exception("La orden todavia no tiene un comprobante de cobro.");
                }

                DataRow fila = datos.Rows[0];
                ComprobanteServicio comprobante = new ComprobanteServicio();
                comprobante.idComprobante = Convert.ToInt32(fila["idComprobante"]);
                comprobante.idOrden = idOrden;
                comprobante.fecha = Convert.ToDateTime(fila["fecha"]);
                comprobante.cliente = Convert.ToString(fila["cliente"]);
                comprobante.equipo = Convert.ToString(fila["equipo"]);
                comprobante.trabajo = Convert.ToString(fila["trabajo"]);
                comprobante.total = Convert.ToDecimal(fila["total"]);
                comprobante.medioPago = Convert.ToString(fila["medioPago"]);
                return comprobante;
            }
        }
    }
}
