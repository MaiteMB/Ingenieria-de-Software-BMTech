using System;
using System.Collections.Generic;
using System.Globalization;

namespace mb506.ServiciosBMTech.Seguridad
{
    public class DigitoVerificador
    {
        public int mb506CalcularDatos(IEnumerable<object> valores)
        {
            long resultado = 0;
            foreach (object valor in valores)
            {
                string texto;
                if (valor == null || valor == DBNull.Value) texto = "-1:";
                else
                {
                    string dato;
                    if (valor is DateTime) dato = ((DateTime)valor).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    else if (valor is decimal) dato = ((decimal)valor).ToString("0.00", CultureInfo.InvariantCulture);
                    else dato = Convert.ToString(valor, CultureInfo.InvariantCulture);
                    texto = dato.Length + ":" + dato;
                }
                foreach (char caracter in texto)
                    resultado = (resultado * 31 + caracter) % int.MaxValue;
            }
            return (int)resultado;
        }

        public int mb506CalcularCliente(mb506.BEBMTech.Cliente.Cliente cliente)
        {
            return mb506CalcularDatos(new object[] { cliente.dni, cliente.nombre, cliente.apellido,
                cliente.telefono, cliente.correoElectronico });
        }

        public int mb506CalcularProducto(mb506.BEBMTech.Producto.Producto producto)
        {
            return mb506CalcularDatos(new object[] { producto.idProducto, producto.codigo,
                producto.descripcion, producto.marca, producto.modelo, producto.precio, producto.stock, producto.activo });
        }
    }
}
