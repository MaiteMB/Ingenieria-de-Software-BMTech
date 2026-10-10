using mb506.BEBMTech.Cliente;
using mb506.DALBMTech;
using System;
using System.Collections.Generic;

namespace mb506.BLLBMTech
{
    public class BLLCliente
    {
        private readonly DALCliente dalCliente;

        public BLLCliente()
        {
            dalCliente = new DALCliente();
        }

        public Cliente mb506BuscarCliente(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                throw new Exception("Debe ingresar el DNI del cliente.");
            }

            return dalCliente.mb506BuscarCliente(dni.Trim());
        }


        public List<Cliente> mb506BuscarClientes(string criterio)
        {
            if (criterio == null)
            {
                criterio = "";
            }

            return dalCliente.mb506BuscarClientes(criterio.Trim());
        }
        public bool mb506RegistrarCliente(Cliente cliente)
        {
            new BLLPermiso().mb506Validar("CLIENTES");
            mb506ValidarCliente(cliente);

            Cliente clienteExistente = dalCliente.mb506BuscarCliente(cliente.dni);

            if (clienteExistente != null)
            {
                throw new Exception("El cliente ya se encuentra registrado.");
            }

            cliente.digitoVerificador = new mb506.ServiciosBMTech.Seguridad.DigitoVerificador().mb506CalcularCliente(cliente);
            bool registrado = dalCliente.mb506InsertarCliente(cliente);
            if (registrado) new BLLLog().mb506RegistrarEvento("Cliente registrado: " + cliente.dni, "Clientes", 1);
            return registrado;
        }

        public void mb506ValidarCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new Exception("Debe ingresar los datos del cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.dni))
            {
                throw new Exception("Debe ingresar el DNI del cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.nombre))
            {
                throw new Exception("Debe ingresar el nombre del cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.apellido))
            {
                throw new Exception("Debe ingresar el apellido del cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.telefono))
            {
                throw new Exception("Debe ingresar el teléfono del cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.correoElectronico))
            {
                throw new Exception("Debe ingresar el correo electrónico del cliente.");
            }
            if (cliente.dni.Length > 20 || cliente.nombre.Length > 100 || cliente.apellido.Length > 100 ||
                cliente.telefono.Length > 50 || cliente.correoElectronico.Length > 150)
                throw new Exception("Los datos del cliente superan el largo permitido.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(cliente.dni, "^[0-9]+$"))
                throw new Exception("El DNI debe contener solo numeros.");
            try
            {
                var correo = new System.Net.Mail.MailAddress(cliente.correoElectronico);
                if (correo.Address != cliente.correoElectronico) throw new FormatException();
            }
            catch (FormatException) { throw new Exception("El correo electronico no es valido."); }
        }
    }
}

