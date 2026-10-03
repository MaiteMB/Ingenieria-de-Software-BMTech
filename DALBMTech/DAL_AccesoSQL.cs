using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALBMTech
{
    public class DAL_AccesoSQL
    {
        private readonly string connectionString = "Data Source=Maite15\\MSSQLSERVER01;Initial Catalog = BMTech; Integrated Security = True";

        private static DAL_AccesoSQL instance;

        private DAL_AccesoSQL() { }

        public static DAL_AccesoSQL GetInstance()
        {
            if (instance == null)
            {
                instance = new DAL_AccesoSQL();
            }

            return instance;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public bool ComprobarConexion()
        {
            try
            {
                using (var conexion = GetConnection())
                {
                    conexion.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
