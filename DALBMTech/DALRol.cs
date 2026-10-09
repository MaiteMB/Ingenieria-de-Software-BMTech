using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DALBMTech
{
    public class DALRol
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALRol()
        {
            dbConnection = DAL_AccesoSQL.GetInstance();
        }

        public List<string> ObtenerPatentesPorPerfil(string idPerfil)
        {
            List<string> patentes = new List<string>();

            string query = @"
                SELECT DISTINCT p.idPatente
                FROM PerfilFamilia pf
                INNER JOIN FamiliaPatente fp ON pf.idFamilia = fp.idFamilia
                INNER JOIN Patente p ON fp.idPatente = p.idPatente
                WHERE pf.idperfil = @idperfil";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@idperfil", SqlDbType.VarChar).Value = idPerfil;
                conexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patentes.Add(reader.GetString(0));
                    }
                }
            }

            return patentes;
        }
    }
}
