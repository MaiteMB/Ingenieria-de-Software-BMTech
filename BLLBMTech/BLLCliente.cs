using BEBMTech.Cliente;
using DALBMTech;
using System;
using System.Collections.Generic;

namespace BLLBMTech
{
    public class BLLCliente
    {
        private readonly DALCliente dalCliente;

        public BLLCliente()
        {
            dalCliente = new DALCliente();
        }

        public Cliente BuscarCliente(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                throw new Exception("Debe ingresar el DNI del cliente.");
            }

            return dalCliente.BuscarCliente(dni.Trim());
        }


        public List<Cliente> BuscarClientes(string criterio)
        {
            if (criterio == null)
            {
                criterio = "";
            }

            return dalCliente.BuscarClientes(criterio.Trim());
        }
        public bool RegistrarCliente(Cliente cliente)
        {
            ValidarCliente(cliente);

            Cliente clienteExistente = dalCliente.BuscarCliente(cliente.dni);

            if (clienteExistente != null)
            {
                throw new Exception("El cliente ya se encuentra registrado.");
            }

            cliente.digitoVerificador = new ServiciosBMTech.Seguridad.DigitoVerificador().CalcularCliente(cliente);
            return dalCliente.InsertarCliente(cliente);
        }

        private void ValidarCliente(Cliente cliente)
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
        }
    }
}

