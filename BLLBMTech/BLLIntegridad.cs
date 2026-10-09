using BEBMTech.Producto;
using DALBMTech;
using ServiciosBMTech.Seguridad;
using System.Collections.Generic;

namespace BLLBMTech
{
    public class BLLIntegridad
    {
        public bool VerificarIntegridadCliente()
        {
            DALCliente dalCliente = new DALCliente();
            int suma = 0;
            foreach (BEBMTech.Cliente.Cliente cliente in dalCliente.BuscarClientes(""))
            {
                int digito = digitoVerificador.CalcularCliente(cliente);
                if (digito != cliente.digitoVerificador)
                {
                    return false;
                }
                suma += digito;
            }
            return suma == dalIntegridad.ObtenerDigitoVerticalGuardado("Cliente");
        }

        public void RegenerarIntegridadCliente()
        {
            DALCliente dalCliente = new DALCliente();
            int suma = 0;
            foreach (BEBMTech.Cliente.Cliente cliente in dalCliente.BuscarClientes(""))
            {
                int digito = digitoVerificador.CalcularCliente(cliente);
                dalCliente.ActualizarDigitoVerificador(cliente.dni, digito);
                suma += digito;
            }
            dalIntegridad.GuardarDigitoVertical("Cliente", suma);
        }

        private readonly DALIntegridad dalIntegridad;
        private readonly DALProducto dalProducto;
        private readonly DigitoVerificador digitoVerificador;

        public BLLIntegridad()
        {
            dalIntegridad = new DALIntegridad();
            dalProducto = new DALProducto();
            digitoVerificador = new DigitoVerificador();
        }

        public void ActualizarDigitoVerticalProducto()
        {
            int digitoVertical = dalIntegridad.CalcularDigitoVerticalProducto();
            dalIntegridad.GuardarDigitoVertical("Producto", digitoVertical);
        }


        public void RegenerarIntegridadProducto()
        {
            List<Producto> productos = dalProducto.BuscarProducto("", true);

            foreach (Producto producto in productos)
            {
                producto.digitoVerificador = digitoVerificador.CalcularProducto(producto);
                dalProducto.ActualizarDigitoVerificador(producto.idProducto, producto.digitoVerificador);
            }

            ActualizarDigitoVerticalProducto();
        }
        public bool VerificarIntegridadProducto()
        {
            List<Producto> productos = dalProducto.BuscarProducto("", true);
            int sumaCalculada = 0;

            foreach (Producto producto in productos)
            {
                int digitoCalculado = digitoVerificador.CalcularProducto(producto);

                if (digitoCalculado != producto.digitoVerificador)
                {
                    return false;
                }

                sumaCalculada += digitoCalculado;
            }

            int sumaGuardada = dalIntegridad.ObtenerDigitoVerticalGuardado("Producto");
            return sumaCalculada == sumaGuardada;
        }
    }
}

