using mb506.BEBMTech.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALLog
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALLog()
        {
            dbConnection = DAL_AccesoSQL.mb506GetInstance();
        }

        public bool mb506RegistrarEvento(string email, string accion, string modulo, int criticidad)
        {
            string query = @"
                INSERT INTO LogEventos
                (
                    email,
                    accion,
                    modulo,
                    criticidad
                )
                VALUES
                (
                    @email,
                    @accion,
                    @modulo,
                    @criticidad
                )";

            try
            {
                using (SqlConnection conexion = dbConnection.mb506GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = email.Length > 150 ? email.Substring(0, 150) : email;
                    command.Parameters.Add("@accion", SqlDbType.VarChar, 255).Value = accion.Length > 255 ? accion.Substring(0, 255) : accion;
                    command.Parameters.Add("@modulo", SqlDbType.VarChar, 100).Value = modulo;
                    command.Parameters.Add("@criticidad", SqlDbType.Int).Value = criticidad;

                    conexion.Open();

                    return new DALIntegridad().mb506EjecutarComando(command, "LogEventos") > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("No se pudo guardar el evento en la bitacora.", ex);
            }
        }

        public List<Evento> mb506ObtenerEventos()
        {
            return mb506ObtenerEventos(new DateTime(1753, 1, 1), DateTime.Today, "", "");
        }

        public List<Evento> mb506ObtenerEventos(DateTime desde, DateTime hasta, string usuario, string accion)
        {
            List<Evento> eventos = new List<Evento>();

            string query = @"
                SELECT idLog, email, fecha, accion, modulo, criticidad
                FROM LogEventos
                WHERE fecha>=@desde AND fecha<@hasta
                  AND (@usuario='' OR email LIKE '%'+@usuario+'%')
                  AND (@accion='' OR accion LIKE '%'+@accion+'%')
                ORDER BY fecha DESC";

            try
            {
                using (SqlConnection conexion = dbConnection.mb506GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde.Date;
                    command.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta.Date.AddDays(1);
                    command.Parameters.Add("@usuario", SqlDbType.VarChar, 150).Value = usuario;
                    command.Parameters.Add("@accion", SqlDbType.VarChar, 255).Value = accion;
                    conexion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eventos.Add(new Evento
                            {
                                idLog = reader.GetInt32(0),
                                email = reader.GetString(1),
                                fecha = reader.GetDateTime(2),
                                accion = reader.GetString(3),
                                modulo = reader.GetString(4),
                                criticidad = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("No se pudo consultar la bitacora.", ex);
            }

            return eventos;
        }
    }
}
