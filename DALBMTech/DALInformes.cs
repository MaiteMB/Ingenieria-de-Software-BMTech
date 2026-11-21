using System;
using System.Data;
using System.Data.SqlClient;

namespace mb506.DALBMTech
{
    public class DALInformes
    {
        public DataTable mb506ObtenerOperaciones(string tipo, DateTime desde, DateTime hasta)
        {
            string consulta;
            if (tipo == "Ventas")
                consulta = @"SELECT v.idVenta AS numero,v.fecha,c.nombre+' '+c.apellido AS cliente,v.estado,v.total,
                    CASE WHEN v.estado IN('PAGADA','ENTREGADA') THEN v.total ELSE 0 END AS cobrado,
                    CASE WHEN v.estado='PENDIENTE' THEN v.total ELSE 0 END AS pendiente
                    FROM Venta v JOIN Cliente c ON c.dni=v.dniCliente
                    WHERE v.fecha>=@desde AND v.fecha<@hasta ORDER BY v.fecha,v.idVenta";
            else if (tipo == "Servicio tecnico")
                consulta = @"SELECT c.idOrden AS numero,c.fecha,c.cliente,o.estado,c.total,
                    c.total AS cobrado,CONVERT(DECIMAL(18,2),0) AS pendiente
                    FROM ComprobanteServicio c JOIN OrdenServicio o ON o.idOrden=c.idOrden
                    WHERE c.fecha>=@desde AND c.fecha<@hasta ORDER BY c.fecha,c.idOrden";
            else throw new Exception("Seleccione un tipo de informe.");
            DataTable datos = new DataTable();
            using (SqlConnection conexion = DAL_AccesoSQL.mb506GetInstance().mb506GetConnection())
            using (SqlCommand command = new SqlCommand(consulta, conexion))
            {
                command.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde.Date;
                command.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta.Date.AddDays(1);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command)) adapter.Fill(datos);
            }
            return datos;
        }
    }
}
