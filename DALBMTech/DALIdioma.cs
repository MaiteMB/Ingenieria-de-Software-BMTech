using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALIdioma
    {
        public List<mb506.BEBMTech.Idiomas.Idioma> mb506ObtenerIdiomas()
        {
            List<mb506.BEBMTech.Idiomas.Idioma> idiomas = new List<mb506.BEBMTech.Idiomas.Idioma>();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand("SELECT codigo,nombre FROM Idioma ORDER BY nombre", conexion))
            {
                conexion.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read()) idiomas.Add(new mb506.BEBMTech.Idiomas.Idioma { codigo = reader.GetString(0), nombre = reader.GetString(1) });
            }
            return idiomas;
        }

        public DataTable mb506ObtenerTextos(string codigo)
        {
            DataTable textos = new DataTable();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand(@"SELECT e.etiqueta,COALESCE(t.textoTraducido,e.etiqueta) AS textoTraducido
                FROM Etiqueta e LEFT JOIN Traduccion t ON e.etiqueta=t.etiqueta AND t.codigoIdioma=@codigo ORDER BY e.etiqueta", conexion))
            {
                command.Parameters.Add("@codigo", SqlDbType.VarChar, 2).Value = codigo;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(textos);
            }
            return textos;
        }

        public void mb506GuardarIdioma(string codigo, string nombre, DataTable textos)
        {
            DALIntegridad integridad = new DALIntegridad();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    integridad.mb506PrepararOperacion(conexion, transaccion, "Idioma", "Etiqueta", "Traduccion");
                    using (SqlCommand command = new SqlCommand(@"UPDATE Idioma SET nombre=@nombre WHERE codigo=@codigo;
                        IF @@ROWCOUNT=0 INSERT INTO Idioma(codigo,nombre) VALUES(@codigo,@nombre)", conexion, transaccion))
                    {
                        command.Parameters.Add("@codigo", SqlDbType.VarChar, 2).Value = codigo;
                        command.Parameters.Add("@nombre", SqlDbType.NVarChar, 50).Value = nombre;
                        command.ExecuteNonQuery();
                    }
                    foreach (DataRow fila in textos.Rows)
                    {
                        using (SqlCommand command = new SqlCommand(@"UPDATE Traduccion SET textoTraducido=@texto WHERE codigoIdioma=@codigo AND etiqueta=@etiqueta;
                            IF @@ROWCOUNT=0 INSERT INTO Traduccion(codigoIdioma,etiqueta,textoTraducido) VALUES(@codigo,@etiqueta,@texto)", conexion, transaccion))
                        {
                            command.Parameters.Add("@codigo", SqlDbType.VarChar, 2).Value = codigo;
                            command.Parameters.Add("@etiqueta", SqlDbType.NVarChar, 200).Value = fila["etiqueta"];
                            command.Parameters.Add("@texto", SqlDbType.NVarChar, 250).Value = fila["textoTraducido"];
                            command.ExecuteNonQuery();
                        }
                    }
                    integridad.mb506ActualizarTablas(conexion, transaccion, "Idioma", "Traduccion");
                    transaccion.Commit();
                }
            }
        }

        public void mb506EliminarIdioma(string codigo)
        {
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand("DELETE FROM Traduccion WHERE codigoIdioma=@codigo; DELETE FROM Idioma WHERE codigo=@codigo", conexion))
            {
                conexion.Open();
                command.Parameters.Add("@codigo", SqlDbType.VarChar, 2).Value = codigo;
                new DALIntegridad().mb506EjecutarComando(command, "Idioma", "Traduccion");
            }
        }

        public Dictionary<string, string> mb506ObtenerTraducciones(string codigo)
        {
            Dictionary<string, string> traducciones = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT etiqueta, textoTraducido FROM Traduccion WHERE codigoIdioma=@codigo", conexion))
            {
                command.Parameters.Add("@codigo", SqlDbType.VarChar, 2).Value = codigo;
                conexion.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read()) traducciones.Add(reader.GetString(0), reader.GetString(1));
            }
            return traducciones;
        }
    }
}
