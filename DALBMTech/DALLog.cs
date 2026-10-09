using BEBMTech.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DALBMTech
{
    public class DALLog
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALLog()
        {
            dbConnection = DAL_AccesoSQL.GetInstance();
        }

        public bool RegistrarEvento(string email, string accion, string modulo, int criticidad)
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
                using (SqlConnection conexion = dbConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    command.Parameters.Add("@accion", SqlDbType.VarChar).Value = accion;
                    command.Parameters.Add("@modulo", SqlDbType.VarChar).Value = modulo;
                    command.Parameters.Add("@criticidad", SqlDbType.Int).Value = criticidad;

                    conexion.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public List<Evento> ObtenerEventos()
        {
            List<Evento> eventos = new List<Evento>();

            string query = @"
                SELECT idLog, email, fecha, accion, modulo, criticidad
                FROM LogEventos
                ORDER BY fecha DESC";

            try
            {
                using (SqlConnection conexion = dbConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
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
            catch (SqlException)
            {
            }

            return eventos;
        }
    }
}
