using System;
using System.Data;
using System.Data.SqlClient;

namespace DALBMTech
{
    public class DALIntegridad
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALIntegridad()
        {
            dbConnection = DAL_AccesoSQL.GetInstance();
        }

        public int CalcularDigitoVerticalProducto()
        {
            string query = "SELECT ISNULL(SUM(digitoVerificador), 0) FROM Producto";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                conexion.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int ObtenerDigitoVerticalGuardado(string tabla)
        {
            string query = "SELECT digitoVertical FROM DigitoVerificador WHERE tabla = @tabla";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@tabla", SqlDbType.VarChar).Value = tabla;
                conexion.Open();

                object resultado = command.ExecuteScalar();
                return resultado == null ? 0 : Convert.ToInt32(resultado);
            }
        }

        public bool GuardarDigitoVertical(string tabla, int digitoVertical)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla = @tabla)
                BEGIN
                    UPDATE DigitoVerificador
                    SET digitoVertical = @digitoVertical
                    WHERE tabla = @tabla
                END
                ELSE
                BEGIN
                    INSERT INTO DigitoVerificador (tabla, digitoVertical)
                    VALUES (@tabla, @digitoVertical)
                END";

            using (SqlConnection conexion = dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@tabla", SqlDbType.VarChar).Value = tabla;
                command.Parameters.Add("@digitoVertical", SqlDbType.Int).Value = digitoVertical;

                conexion.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
