using BEBMTech.Producto;
using System.Globalization;

namespace ServiciosBMTech.Seguridad
{
    public class DigitoVerificador
    {
        public int CalcularCliente(BEBMTech.Cliente.Cliente cliente)
        {
            string datos = cliente.dni + "|" + cliente.nombre + "|"
                + cliente.apellido + "|" + cliente.telefono + "|" + cliente.correoElectronico;
            int resultado = 0;
            foreach (char caracter in datos)
            {
                resultado += caracter;
            }
            return resultado;
        }

        public int CalcularProducto(Producto producto)
        {
            string datos = producto.codigo
                + producto.descripcion
                + producto.marca
                + producto.modelo
                + producto.precio.ToString("0.00", CultureInfo.InvariantCulture)
                + producto.stock.ToString()
                + producto.activo.ToString();

            int resultado = 0;

            foreach (char caracter in datos)
            {
                resultado += caracter;
            }

            return resultado;
        }
    }
}
