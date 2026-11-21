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
            if (tipo == "Perfil") query = @"SELECT 'F:'+idFamilia FROM PerfilFamilia WHERE idperfil=@id
                UNION ALL SELECT 'P:'+idPerfilHijo FROM PerfilPerfil WHERE idPerfilPadre=@id
                UNION ALL SELECT 'T:'+idPatente FROM PerfilPatente WHERE idperfil=@id";
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
                    string[] tablas = { "PerfilFamilia", "FamiliaPatente", "PerfilPerfil", "PerfilPatente" };
                    DALIntegridad integridad = new DALIntegridad();
                    integridad.mb506PrepararOperacion(conexion, transaccion, tablas);
                    if (tipo == "Perfil") borrar += "; DELETE FROM PerfilPerfil WHERE idPerfilPadre=@id; DELETE FROM PerfilPatente WHERE idperfil=@id";
                    using (SqlCommand command = new SqlCommand(borrar, conexion, transaccion))
                    {
                        command.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                        command.ExecuteNonQuery();
                    }
                    foreach (string hijo in asignaciones)
                    {
                        string consulta = insertar;
                        string valor = hijo;
                        if (tipo == "Perfil" && hijo.Length > 2 && hijo[1] == ':')
                        {
                            valor = hijo.Substring(2);
                            if (hijo.StartsWith("P:", StringComparison.Ordinal)) consulta = "INSERT INTO PerfilPerfil(idPerfilPadre,idPerfilHijo) VALUES(@id,@hijo)";
                            else if (hijo.StartsWith("T:", StringComparison.Ordinal)) consulta = "INSERT INTO PerfilPatente(idperfil,idPatente) VALUES(@id,@hijo)";
                            else if (!hijo.StartsWith("F:", StringComparison.Ordinal)) throw new Exception("Asignacion incorrecta.");
                        }
                        using (SqlCommand command = new SqlCommand(consulta, conexion, transaccion))
                        {
                            command.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                            command.Parameters.Add("@hijo", SqlDbType.VarChar, 50).Value = valor;
                            command.ExecuteNonQuery();
                        }
                    }
                    if (tipo == "Perfil") mb506LeerPerfil(id, conexion, transaccion, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
                    var actual = mb506LeerPerfil(perfilActual, conexion, transaccion, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
                    if (!actual.mb506ObtenerTodos().Exists(r => r is mb506.BEBMTech.Roles.Patente && r.idRol == "SEGURIDAD"))
                        throw new Exception("No puede quitar su propio acceso a Seguridad.");
                    integridad.mb506ActualizarTablas(conexion, transaccion, tablas);
                    transaccion.Commit();
                }
            }
        }

        public List<string> mb506ObtenerPatentesPorPerfil(string idPerfil)
        {
            List<string> patentes = new List<string>();
            foreach (var rol in mb506ObtenerPerfil(idPerfil).mb506ObtenerTodos())
                if (rol is mb506.BEBMTech.Roles.Patente && !patentes.Contains(rol.idRol)) patentes.Add(rol.idRol);
            return patentes;
        }

        public mb506.BEBMTech.Roles.Perfil mb506ObtenerPerfil(string idPerfil)
        {
            using (SqlConnection conexion = dbConnection.mb506GetConnection())
            {
                conexion.Open();
                return mb506LeerPerfil(idPerfil, conexion, null, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
            }
        }

        internal mb506.BEBMTech.Roles.Perfil mb506LeerPerfil(string idPerfil, SqlConnection conexion, SqlTransaction transaccion, HashSet<string> camino)
        {
            if (!camino.Add(idPerfil)) throw new Exception("Los perfiles no pueden formar ciclos.");
            mb506.BEBMTech.Roles.Perfil perfil = new mb506.BEBMTech.Roles.Perfil { id_perfil = idPerfil };
            Dictionary<string, mb506.BEBMTech.Roles.Familia> familias = new Dictionary<string, mb506.BEBMTech.Roles.Familia>();
            using (SqlCommand command = new SqlCommand(@"
                SELECT f.idFamilia,f.nombre,p.idPatente,p.nombre
                FROM PerfilFamilia pf JOIN Familia f ON pf.idFamilia=f.idFamilia
                LEFT JOIN FamiliaPatente fp ON f.idFamilia=fp.idFamilia
                LEFT JOIN Patente p ON fp.idPatente=p.idPatente
                WHERE pf.idperfil=@perfil", conexion, transaccion))
            {
                command.Parameters.Add("@perfil", SqlDbType.VarChar, 50).Value = idPerfil;
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
            using (SqlCommand command = new SqlCommand(@"SELECT p.idPatente,p.nombre FROM PerfilPatente pp
                JOIN Patente p ON pp.idPatente=p.idPatente WHERE pp.idperfil=@perfil", conexion, transaccion))
            {
                command.Parameters.Add("@perfil", SqlDbType.VarChar, 50).Value = idPerfil;
                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read()) perfil.Roles.Add(new mb506.BEBMTech.Roles.Patente(reader.GetString(0)) { Vista = reader.GetString(1) });
            }
            List<string> hijos = new List<string>();
            using (SqlCommand command = new SqlCommand("SELECT idPerfilHijo FROM PerfilPerfil WHERE idPerfilPadre=@perfil", conexion, transaccion))
            {
                command.Parameters.Add("@perfil", SqlDbType.VarChar, 50).Value = idPerfil;
                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read()) hijos.Add(reader.GetString(0));
            }
            foreach (string hijo in hijos) perfil.Roles.Add(mb506LeerPerfil(hijo, conexion, transaccion, camino));
            camino.Remove(idPerfil);
            return perfil;
        }

        public void mb506EliminarElemento(string tipo, string id)
        {
            string query;
            if (tipo == "Perfil") query = @"DELETE FROM Perfil WHERE idperfil=@id
                AND NOT EXISTS(SELECT 1 FROM Usuario WHERE idperfil=@id)
                AND NOT EXISTS(SELECT 1 FROM PerfilFamilia WHERE idperfil=@id)
                AND NOT EXISTS(SELECT 1 FROM PerfilPerfil WHERE idPerfilPadre=@id OR idPerfilHijo=@id)
                AND NOT EXISTS(SELECT 1 FROM PerfilPatente WHERE idperfil=@id)";
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
