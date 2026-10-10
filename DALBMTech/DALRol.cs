using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALRol
    {
        private readonly DAL_AccesoSQL dbConnection;

        public DALRol()
        {
            dbConnection = DAL_AccesoSQL.mb506GetInstance();
        }

        public DataTable mb506ObtenerElementos(string tipo)
        {
            string query;
            if (tipo == "Perfil") query = "SELECT idperfil AS id, idperfil AS nombre FROM Perfil ORDER BY idperfil";
            else if (tipo == "Familia") query = "SELECT idFamilia AS id, nombre FROM Familia ORDER BY nombre";
            else if (tipo == "Patente") query = "SELECT idPatente AS id, nombre FROM Patente ORDER BY nombre";
            else throw new Exception("Tipo de permiso incorrecto.");
            DataTable tabla = new DataTable();
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, conexion))
                adapter.Fill(tabla);
            return tabla;
        }

        public void mb506GuardarElemento(string tipo, string id, string nombre, bool nuevo)
        {
            string query;
            if (tipo == "Perfil")
                query = "INSERT INTO Perfil(idperfil) VALUES(@id)";
            else if (tipo == "Familia")
                query = nuevo ? "INSERT INTO Familia(idFamilia,nombre) VALUES(@id,@nombre)" :
                    "UPDATE Familia SET nombre=@nombre WHERE idFamilia=@id";
            else if (tipo == "Patente")
                query = nuevo ? "INSERT INTO Patente(idPatente,nombre) VALUES(@id,@nombre)" :
                    "UPDATE Patente SET nombre=@nombre WHERE idPatente=@id";
            else throw new Exception("Tipo de permiso incorrecto.");
            if (tipo == "Perfil" && !nuevo) throw new Exception("El identificador del perfil no se modifica.");
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = nombre;
                conexion.Open();
                new DALIntegridad().mb506EjecutarComando(command, tipo);
            }
        }

        public List<string> mb506ObtenerAsignaciones(string tipo, string id)
        {
            string query;
            if (tipo == "Perfil") query = "SELECT idFamilia FROM PerfilFamilia WHERE idperfil=@id";
            else if (tipo == "Familia") query = "SELECT idPatente FROM FamiliaPatente WHERE idFamilia=@id";
            else throw new Exception("Las patentes no tienen asignaciones.");
            List<string> ids = new List<string>();
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                conexion.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read()) ids.Add(reader.GetString(0));
            }
            return ids;
        }

        public void mb506GuardarAsignaciones(string tipo, string id, List<string> asignaciones, string perfilActual)
        {
            string borrar;
            string insertar;
            if (tipo == "Perfil")
            {
                borrar = "DELETE FROM PerfilFamilia WHERE idperfil=@id";
                insertar = "INSERT INTO PerfilFamilia(idperfil,idFamilia) VALUES(@id,@hijo)";
            }
            else if (tipo == "Familia")
            {
                borrar = "DELETE FROM FamiliaPatente WHERE idFamilia=@id";
                insertar = "INSERT INTO FamiliaPatente(idFamilia,idPatente) VALUES(@id,@hijo)";
            }
            else throw new Exception("Tipo incorrecto.");
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    string tabla = tipo == "Perfil" ? "PerfilFamilia" : "FamiliaPatente";
                    DALIntegridad integridad = new DALIntegridad();
                    integridad.mb506PrepararOperacion(conexion, transaccion, tabla);
                    using (SqlCommand command = new SqlCommand(borrar, conexion, transaccion))
                    {
                        command.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                        command.ExecuteNonQuery();
                    }
                    foreach (string hijo in asignaciones)
                    {
                        using (SqlCommand command = new SqlCommand(insertar, conexion, transaccion))
                        {
                            command.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                            command.Parameters.Add("@hijo", SqlDbType.VarChar, 50).Value = hijo;
                            command.ExecuteNonQuery();
                        }
                    }
                    // Evita que el administrador se quite su acceso a Seguridad.
                    using (SqlCommand command = new SqlCommand(@"SELECT COUNT(*)
                        FROM PerfilFamilia pf JOIN FamiliaPatente fp ON pf.idFamilia=fp.idFamilia
                        WHERE pf.idperfil=@perfil AND fp.idPatente='SEGURIDAD'", conexion, transaccion))
                    {
                        command.Parameters.Add("@perfil", SqlDbType.VarChar, 50).Value = perfilActual;
                        if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                            throw new Exception("No puede quitar su propio acceso a Seguridad.");
                    }
                    integridad.mb506ActualizarTablas(conexion, transaccion, tabla);
                    transaccion.Commit();
                }
            }
        }

        public List<string> mb506ObtenerPatentesPorPerfil(string idPerfil)
        {
            List<string> patentes = new List<string>();

            string query = @"
                SELECT DISTINCT p.idPatente
                FROM PerfilFamilia pf
                INNER JOIN FamiliaPatente fp ON pf.idFamilia = fp.idFamilia
                INNER JOIN Patente p ON fp.idPatente = p.idPatente
                WHERE pf.idperfil = @idperfil";

            using (SqlConnection conexion = dbConnection.mb506GetConnection())
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

        public mb506.BEBMTech.Roles.Perfil mb506ObtenerPerfil(string idPerfil)
        {
            mb506.BEBMTech.Roles.Perfil perfil = new mb506.BEBMTech.Roles.Perfil { id_perfil = idPerfil };
            Dictionary<string, mb506.BEBMTech.Roles.Familia> familias = new Dictionary<string, mb506.BEBMTech.Roles.Familia>();
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                SELECT f.idFamilia,f.nombre,p.idPatente,p.nombre
                FROM PerfilFamilia pf JOIN Familia f ON pf.idFamilia=f.idFamilia
                LEFT JOIN FamiliaPatente fp ON f.idFamilia=fp.idFamilia
                LEFT JOIN Patente p ON fp.idPatente=p.idPatente
                WHERE pf.idperfil=@perfil", conexion))
            {
                command.Parameters.Add("@perfil", SqlDbType.VarChar, 50).Value = idPerfil;
                conexion.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idFamilia = reader.GetString(0);
                        if (!familias.ContainsKey(idFamilia))
                        {
                            mb506.BEBMTech.Roles.Familia familia = new mb506.BEBMTech.Roles.Familia(idFamilia);
                            familia.Vista = reader.GetString(1);
                            familias.Add(idFamilia, familia);
                            perfil.Roles.Add(familia);
                        }
                        if (!reader.IsDBNull(2))
                            familias[idFamilia].mb506Agregar(new mb506.BEBMTech.Roles.Patente(reader.GetString(2)) { Vista = reader.GetString(3) });
                    }
                }
            }
            return perfil;
        }

        public void mb506EliminarElemento(string tipo, string id)
        {
            string query;
            if (tipo == "Perfil") query = @"DELETE FROM Perfil WHERE idperfil=@id
                AND NOT EXISTS(SELECT 1 FROM Usuario WHERE idperfil=@id)
                AND NOT EXISTS(SELECT 1 FROM PerfilFamilia WHERE idperfil=@id)";
            else if (tipo == "Familia") query = @"DELETE FROM Familia WHERE idFamilia=@id
                AND NOT EXISTS(SELECT 1 FROM FamiliaPatente WHERE idFamilia=@id)
                AND NOT EXISTS(SELECT 1 FROM PerfilFamilia WHERE idFamilia=@id)";
            else throw new Exception("Las patentes del sistema no se eliminan.");
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                conexion.Open();
                if (new DALIntegridad().mb506EjecutarComando(command, tipo) == 0)
                    throw new Exception("El elemento esta asignado o ya no existe. Quite sus relaciones antes de eliminarlo.");
            }
        }
    }
}
