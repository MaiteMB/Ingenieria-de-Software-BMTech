using System;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALBackup
    {
        public DataTable mb506ObtenerBackups()
        {
            DataTable datos = new DataTable();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT idBackup,fecha,usuario,ruta FROM [Backup] ORDER BY fecha DESC", conexion))
                adapter.Fill(datos);
            return datos;
        }

        public void mb506Crear(string ruta, string usuario)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlCommand registro = new SqlCommand("INSERT INTO [Backup](fecha,usuario,ruta) VALUES(GETDATE(),@usuario,@ruta)", conexion))
                {
                    registro.Parameters.Add("@usuario", SqlDbType.VarChar, 150).Value = usuario;
                    registro.Parameters.Add("@ruta", SqlDbType.NVarChar, 500).Value = ruta;
                    new DALIntegridad().mb506EjecutarComando(registro, "Backup");
                }
                try
                {
                    using (SqlCommand command = new SqlCommand("BACKUP DATABASE [BMTech] TO DISK=@ruta WITH CHECKSUM", conexion))
                    {
                        command.Parameters.Add("@ruta", SqlDbType.NVarChar, 500).Value = ruta;
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    using (SqlCommand command = new SqlCommand("DELETE FROM [Backup] WHERE ruta=@ruta", conexion))
                    {
                        command.Parameters.Add("@ruta", SqlDbType.NVarChar, 500).Value = ruta;
                        new DALIntegridad().mb506EjecutarComando(command, "Backup");
                    }
                    throw;
                }
            }
        }

        public void mb506Restaurar(string ruta)
        {
            SqlConnection.ClearAllPools();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetMasterConnection())
            {
                conexion.Open();
                DataTable encabezado = new DataTable();
                using (SqlCommand command = new SqlCommand("RESTORE HEADERONLY FROM DISK=@ruta", conexion))
                {
                    command.Parameters.Add("@ruta", SqlDbType.NVarChar, 500).Value = ruta;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(encabezado);
                }
                if (encabezado.Rows.Count != 1 || Convert.ToString(encabezado.Rows[0]["DatabaseName"]) != "BMTech"
                    || Convert.ToInt32(encabezado.Rows[0]["BackupType"]) != 1)
                    throw new Exception("Seleccione una copia completa de BMTech con un unico backup.");
                using (SqlCommand command = new SqlCommand("RESTORE VERIFYONLY FROM DISK=@ruta WITH CHECKSUM", conexion))
                {
                    command.Parameters.Add("@ruta", SqlDbType.NVarChar, 500).Value = ruta;
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                }
                DataTable archivos = new DataTable();
                using (SqlCommand command = new SqlCommand("RESTORE FILELISTONLY FROM DISK=@ruta", conexion))
                {
                    command.Parameters.Add("@ruta", SqlDbType.NVarChar, 500).Value = ruta;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(archivos);
                }
                DataTable destino = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT type,physical_name FROM sys.master_files WHERE database_id=DB_ID('BMTech')", conexion)) adapter.Fill(destino);
                if (archivos.Rows.Count != 2 || destino.Rows.Count != 2) throw new Exception("Esta restauracion admite una base con un archivo de datos y uno de log.");
                string datos = null, log = null, nombreDatos = null, nombreLog = null;
                foreach (DataRow fila in destino.Rows)
                    if (Convert.ToInt32(fila["type"]) == 0) datos = Convert.ToString(fila["physical_name"]);
                    else if (Convert.ToInt32(fila["type"]) == 1) log = Convert.ToString(fila["physical_name"]);
                foreach (DataRow fila in archivos.Rows)
                    if (Convert.ToString(fila["Type"]) == "D") nombreDatos = Convert.ToString(fila["LogicalName"]);
                    else if (Convert.ToString(fila["Type"]) == "L") nombreLog = Convert.ToString(fila["LogicalName"]);
                if (datos == null || log == null || nombreDatos == null || nombreLog == null) throw new Exception("La estructura de la copia no es compatible.");
                try
                {
                    using (SqlCommand modo = new SqlCommand("ALTER DATABASE [BMTech] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", conexion)) modo.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(@"RESTORE DATABASE [BMTech] FROM DISK=@ruta
                        WITH REPLACE,CHECKSUM,MOVE @nombreDatos TO @datos,MOVE @nombreLog TO @log", conexion))
                    {
                        command.Parameters.Add("@ruta", SqlDbType.NVarChar, 500).Value = ruta;
                        command.Parameters.Add("@nombreDatos", SqlDbType.NVarChar, 128).Value = nombreDatos;
                        command.Parameters.Add("@datos", SqlDbType.NVarChar, 500).Value = datos;
                        command.Parameters.Add("@nombreLog", SqlDbType.NVarChar, 128).Value = nombreLog;
                        command.Parameters.Add("@log", SqlDbType.NVarChar, 500).Value = log;
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    using (SqlCommand modo = new SqlCommand("ALTER DATABASE [BMTech] SET MULTI_USER", conexion)) modo.ExecuteNonQuery();
                    SqlConnection.ClearAllPools();
                }
            }
        }
    }
}
