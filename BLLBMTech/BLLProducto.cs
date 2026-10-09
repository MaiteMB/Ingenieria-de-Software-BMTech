using BEBMTech.Producto;
using DALBMTech;
using ServiciosBMTech.Seguridad;
using System;
using System.Collections.Generic;

namespace BLLBMTech
{
    public class BLLProducto
    {
        private readonly DALProducto dalProducto;
        private readonly DigitoVerificador digitoVerificador;
        private readonly BLLIntegridad bllIntegridad;

        public BLLProducto()
        {
            dalProducto = new DALProducto();
            digitoVerificador = new DigitoVerificador();
            bllIntegridad = new BLLIntegridad();
        }

        public List<Producto> BuscarProducto(string criterio, bool incluirInactivos = false)
        {
            if (criterio == null)
            {
                criterio = "";
            }

            return dalProducto.BuscarProducto(criterio.Trim(), incluirInactivos);
        }

        public bool RegistrarProducto(Producto producto)
        {
            ValidarProducto(producto);

            Producto productoExistente = dalProducto.ObtenerProductoPorCodigo(producto.codigo);

            if (productoExistente != null)
            {
                throw new Exception("Ya existe un producto registrado con ese código.");
            }

            producto.activo = true;
            producto.digitoVerificador = digitoVerificador.CalcularProducto(producto);

            bool registrado = dalProducto.InsertarProducto(producto);

            if (registrado)
            {
                bllIntegridad.ActualizarDigitoVerticalProducto();
            }

            return registrado;
        }

        public bool ModificarProducto(Producto producto)
        {
            ValidarProducto(producto);

            if (producto.idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto para modificar.");
            }

            producto.digitoVerificador = digitoVerificador.CalcularProducto(producto);

            bool modificado = dalProducto.ModificarProducto(producto);

            if (modificado)
            {
                bllIntegridad.ActualizarDigitoVerticalProducto();
            }

            return modificado;
        }

        public bool CambiarEstadoProducto(int idProducto, bool activo)
        {
            if (idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto.");
            }

            Producto producto = dalProducto.ObtenerProductoPorId(idProducto);

            if (producto == null)
            {
                throw new Exception("No se encontró el producto seleccionado.");
            }

            producto.activo = activo;
            producto.digitoVerificador = digitoVerificador.CalcularProducto(producto);

            bool cambiado = dalProducto.CambiarEstadoProducto(producto);

            if (cambiado)
            {
                bllIntegridad.ActualizarDigitoVerticalProducto();
            }

            return cambiado;
        }

        private void ValidarProducto(Producto producto)
        {
            if (producto == null)
            {
                throw new Exception("Debe ingresar los datos del producto.");
            }

            if (string.IsNullOrWhiteSpace(producto.codigo))
            {
                throw new Exception("Debe ingresar el código del producto.");
            }

            if (string.IsNullOrWhiteSpace(producto.descripcion))
            {
                throw new Exception("Debe ingresar la descripción del producto.");
            }

            if (string.IsNullOrWhiteSpace(producto.marca))
            {
                throw new Exception("Debe ingresar la marca del producto.");
            }

            if (string.IsNullOrWhiteSpace(producto.modelo))
            {
                throw new Exception("Debe ingresar el modelo del producto.");
            }

            if (producto.precio <= 0)
            {
                throw new Exception("El precio debe ser mayor a cero.");
            }

            if (producto.stock < 0)
            {
                throw new Exception("El stock no puede ser negativo.");
            }
        }
    }
}
