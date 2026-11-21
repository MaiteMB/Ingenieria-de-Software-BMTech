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
            if (string.IsNullOrWhiteSpace(servidor)) servidor = System.Configuration.ConfigurationManager.AppSettings["ServidorSql"];
            SqlConnectionStringBuilder configuracion = new SqlConnectionStringBuilder();
            configuracion.DataSource = string.IsNullOrWhiteSpace(servidor) ? "Maite15\\MSSQLSERVER01" : servidor;
            string baseDatos = System.Environment.GetEnvironmentVariable("BMTECH_SQL_DATABASE");
            configuracion.InitialCatalog = string.IsNullOrWhiteSpace(baseDatos) ? "BMTech" : baseDatos;
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
