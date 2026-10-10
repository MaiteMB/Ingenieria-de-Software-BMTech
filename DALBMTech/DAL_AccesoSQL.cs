using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DAL_AccesoSQL
    {
        private readonly string connectionString;

        private static DAL_AccesoSQL instance;

        private DAL_AccesoSQL()
        {
            string servidor = System.Environment.GetEnvironmentVariable("BMTECH_SQL_SERVER");
            SqlConnectionStringBuilder configuracion = new SqlConnectionStringBuilder();
            configuracion.DataSource = string.IsNullOrWhiteSpace(servidor) ? "Maite15\\MSSQLSERVER01" : servidor;
            configuracion.InitialCatalog = "BMTech";
            configuracion.IntegratedSecurity = true;
            configuracion.ConnectTimeout = 10;
            connectionString = configuracion.ConnectionString;
        }

        public SqlConnection mb506GetMasterConnection()
        {
            SqlConnectionStringBuilder configuracion = new SqlConnectionStringBuilder(connectionString);
            configuracion.InitialCatalog = "master";
            configuracion.Pooling = false;
            return new SqlConnection(configuracion.ConnectionString);
        }

        public static DAL_AccesoSQL mb506GetInstance()
        {
            if (instance == null)
            {
                instance = new DAL_AccesoSQL();
            }

            return instance;
        }

        public SqlConnection mb506GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public bool mb506ComprobarConexion()
        {
            try
            {
                using (var conexion = mb506GetConnection())
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
