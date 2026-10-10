using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using mb506.ServiciosBMTech.Seguridad;

namespace mb506.DALBMTech
{
    public class DALIntegridad
    {
        public static readonly string[] tablas = {
            "Cliente", "Producto", "Usuario", "Perfil", "Familia", "Patente",
            "FamiliaPatente", "PerfilFamilia", "LogEventos", "Venta", "DetalleVenta",
            "HistorialCambiosProducto", "Idioma", "Etiqueta", "Traduccion", "Pago", "Comprobante", "Backup"
        };

        private string mb506ObtenerColumnas(string tabla)
        {
            switch (tabla)
            {
                case "Cliente": return "dni,nombre,apellido,telefono,correoElectronico";
                case "Producto": return "idProducto,codigo,descripcion,marca,modelo,precio,stock,activo";
                case "Usuario": return "email,nombre,apellido,password,activo,intentos,idperfil";
                case "Perfil": return "idperfil";
                case "Familia": return "idFamilia,nombre";
                case "Patente": return "idPatente,nombre";
                case "FamiliaPatente": return "idFamilia,idPatente";
                case "PerfilFamilia": return "idperfil,idFamilia";
                case "LogEventos": return "idLog,email,fecha,accion,modulo,criticidad";
                case "Venta": return "idVenta,dniCliente,emailUsuario,fecha,total,estado,fechaEntrega";
                case "DetalleVenta": return "idDetalleVenta,idVenta,idProducto,cantidad,precioUnitario,subtotal,codigoProducto,descripcionProducto";
                case "Pago": return "idPago,idVenta,medioPago,importe,numeroOperacion,fecha,verificado";
                case "Comprobante": return "idComprobante,idVenta,fecha,cliente,dniCliente,vendedor,total";
                case "Backup": return "idBackup,fecha,usuario,ruta";
                case "HistorialCambiosProducto": return "idHistorial,idProducto,codigo,descripcion,marca,modelo,precio,stock,activo,usuario,fecha,accion";
                case "Idioma": return "codigo,nombre";
                case "Etiqueta": return "etiqueta";
                case "Traduccion": return "codigoIdioma,etiqueta,textoTraducido";
                default: throw new Exception("Tabla no habilitada para integridad.");
            }
        }

        private string[] mb506ObtenerClaves(string tabla)
        {
            if (tabla == "FamiliaPatente") return new[] { "idFamilia", "idPatente" };
            if (tabla == "PerfilFamilia") return new[] { "idperfil", "idFamilia" };
            if (tabla == "Traduccion") return new[] { "codigoIdioma", "etiqueta" };
            return new[] { mb506ObtenerColumnas(tabla).Split(',')[0] };
        }

        private DataTable mb506LeerTabla(string tabla, SqlConnection conexion, SqlTransaction transaccion)
        {
            string query = "SELECT " + mb506ObtenerColumnas(tabla) + ",digitoVerificador FROM [" + tabla + "]";
            if (transaccion != null) query += " WITH (UPDLOCK,HOLDLOCK)";
            DataTable datos = new DataTable();
            using (SqlCommand command = new SqlCommand(query, conexion, transaccion))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                adapter.Fill(datos);
            return datos;
        }

        private int mb506CalcularFila(DataRow fila)
        {
            List<object> valores = new List<object>();
            foreach (DataColumn columna in fila.Table.Columns)
                if (columna.ColumnName != "digitoVerificador") valores.Add(fila[columna]);
            return new DigitoVerificador().mb506CalcularDatos(valores);
        }

        private int? mb506LeerVertical(string tabla, SqlConnection conexion, SqlTransaction transaccion)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT digitoVertical FROM DigitoVerificador WHERE tabla=@tabla", conexion, transaccion))
            {
                command.Parameters.Add("@tabla", SqlDbType.VarChar, 50).Value = tabla;
                object resultado = command.ExecuteScalar();
                return resultado == null ? (int?)null : Convert.ToInt32(resultado);
            }
        }

        private void mb506GuardarVertical(string tabla, int digito, SqlConnection conexion, SqlTransaction transaccion)
        {
            using (SqlCommand command = new SqlCommand(@"
                UPDATE DigitoVerificador SET digitoVertical=@digito WHERE tabla=@tabla;
                IF @@ROWCOUNT=0 INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES(@tabla,@digito);",
                conexion, transaccion))
            {
                command.Parameters.Add("@tabla", SqlDbType.VarChar, 50).Value = tabla;
                command.Parameters.Add("@digito", SqlDbType.Int).Value = digito;
                command.ExecuteNonQuery();
            }
        }

        public bool mb506VerificarTabla(string tabla, SqlConnection conexion, SqlTransaction transaccion)
        {
            long suma = 0;
            foreach (DataRow fila in mb506LeerTabla(tabla, conexion, transaccion).Rows)
            {
                int calculado = mb506CalcularFila(fila);
                if (calculado != Convert.ToInt32(fila["digitoVerificador"])) return false;
                suma = (suma + calculado) % int.MaxValue;
            }
            int? guardado = mb506LeerVertical(tabla, conexion, transaccion);
            return guardado.HasValue && suma == guardado.Value;
        }

        public List<string> mb506VerificarSistema()
        {
            List<string> diferencias = new List<string>();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    string[] ordenadas = (string[])tablas.Clone();
                    Array.Sort(ordenadas, StringComparer.Ordinal);
                    foreach (string tabla in ordenadas)
                        if (!mb506VerificarTabla(tabla, conexion, transaccion)) diferencias.Add(tabla);
                    transaccion.Commit();
                }
            }
            return diferencias;
        }

        public bool mb506NecesitaInicializacion()
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                foreach (string tabla in tablas)
                {
                    int? valor = mb506LeerVertical(tabla, conexion, null);
                    if (!valor.HasValue || valor.Value < 0) return true;
                }
                return false;
            }
        }

        public bool mb506VerificarTablas(params string[] seleccionadas)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    Array.Sort(seleccionadas, StringComparer.Ordinal);
                    foreach (string tabla in seleccionadas)
                        if (!mb506VerificarTabla(tabla, conexion, transaccion)) return false;
                    transaccion.Commit();
                    return true;
                }
            }
        }

        public void mb506PrepararOperacion(SqlConnection conexion, SqlTransaction transaccion, params string[] afectadas)
        {
            Array.Sort(afectadas, StringComparer.Ordinal);
            foreach (string tabla in afectadas)
                if (!mb506VerificarTabla(tabla, conexion, transaccion))
                    throw new Exception("Integridad incorrecta en " + tabla + ". Revise los datos antes de continuar.");
            string usuario = SessionManager.IsSessionActive ? SessionManager.getSession.Usuario.email : "sistema";
            using (SqlCommand command = new SqlCommand(
                "EXEC sys.sp_set_session_context @key=N'usuario', @value=@usuario", conexion, transaccion))
            {
                command.Parameters.Add("@usuario", SqlDbType.NVarChar, 150).Value = usuario;
                command.ExecuteNonQuery();
            }
        }

        public void mb506ActualizarTablas(SqlConnection conexion, SqlTransaction transaccion, params string[] afectadas)
        {
            foreach (string tabla in afectadas)
            {
                DataTable datos = mb506LeerTabla(tabla, conexion, transaccion);
                long suma = 0;
                foreach (DataRow fila in datos.Rows)
                {
                    int digito = mb506CalcularFila(fila);
                    suma = (suma + digito) % int.MaxValue;
                    if (digito == Convert.ToInt32(fila["digitoVerificador"])) continue;
                    string condicion = "";
                    string[] claves = mb506ObtenerClaves(tabla);
                    for (int i = 0; i < claves.Length; i++)
                        condicion += (i == 0 ? "" : " AND ") + "[" + claves[i] + "]=@clave" + i;
                    using (SqlCommand command = new SqlCommand(
                        "UPDATE [" + tabla + "] SET digitoVerificador=@digito WHERE " + condicion, conexion, transaccion))
                    {
                        command.Parameters.Add("@digito", SqlDbType.Int).Value = digito;
                        for (int i = 0; i < claves.Length; i++)
                            command.Parameters.AddWithValue("@clave" + i, fila[claves[i]]);
                        command.ExecuteNonQuery();
                    }
                }
                mb506GuardarVertical(tabla, (int)suma, conexion, transaccion);
            }
        }

        public int mb506EjecutarComando(SqlCommand command, params string[] afectadas)
        {
            using (SqlTransaction transaccion = command.Connection.BeginTransaction())
            {
                command.Transaction = transaccion;
                mb506PrepararOperacion(command.Connection, transaccion, afectadas);
                int filas = command.ExecuteNonQuery();
                mb506ActualizarTablas(command.Connection, transaccion, afectadas);
                transaccion.Commit();
                return filas;
            }
        }

        public void mb506RegenerarSistema()
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    string[] ordenadas = (string[])tablas.Clone();
                    Array.Sort(ordenadas, StringComparer.Ordinal);
                    mb506ActualizarTablas(conexion, transaccion, ordenadas);
                    transaccion.Commit();
                }
            }
        }

        public int mb506CalcularDigitoVerticalProducto()
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                long suma = 0;
                foreach (DataRow fila in mb506LeerTabla("Producto", conexion, null).Rows)
                    suma = (suma + Convert.ToInt32(fila["digitoVerificador"])) % int.MaxValue;
                return (int)suma;
            }
        }

        public int mb506ObtenerDigitoVerticalGuardado(string tabla)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                return mb506LeerVertical(tabla, conexion, null) ?? -1;
            }
        }

        public bool mb506GuardarDigitoVertical(string tabla, int digito)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                mb506GuardarVertical(tabla, digito, conexion, null);
                return true;
            }
        }
    }
}
