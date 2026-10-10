using mb506.BEBMTech.Producto;
using mb506.DALBMTech;
using mb506.ServiciosBMTech.Seguridad;
using System;
using System.Collections.Generic;

namespace mb506.BLLBMTech
{
    public class BLLProducto
    {
        private readonly DALProducto dalProducto;
        private readonly DigitoVerificador digitoVerificador;

        public BLLProducto()
        {
            dalProducto = new DALProducto();
            digitoVerificador = new DigitoVerificador();
        }

        public List<Producto> mb506BuscarProducto(string criterio, bool incluirInactivos = false)
        {
            if (criterio == null)
            {
                criterio = "";
            }

            return dalProducto.mb506BuscarProducto(criterio.Trim(), incluirInactivos);
        }

        public bool mb506RegistrarProducto(Producto producto)
        {
            new BLLPermiso().mb506Validar("PRODUCTOS");
            mb506ValidarProducto(producto);

            Producto productoExistente = dalProducto.mb506ObtenerProductoPorCodigo(producto.codigo);

            if (productoExistente != null)
            {
                throw new Exception("Ya existe un producto registrado con ese código.");
            }

            producto.activo = true;
            producto.digitoVerificador = digitoVerificador.mb506CalcularProducto(producto);

            bool registrado = dalProducto.mb506InsertarProducto(producto);

            if (registrado)
            {
                new BLLLog().mb506RegistrarEvento("Producto registrado: " + producto.codigo, "Productos", 1);
            }

            return registrado;
        }

        public bool mb506ModificarProducto(Producto producto)
        {
            new BLLPermiso().mb506Validar("PRODUCTOS");
            mb506ValidarProducto(producto);

            if (producto.idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto para modificar.");
            }

            producto.digitoVerificador = digitoVerificador.mb506CalcularProducto(producto);

            bool modificado = dalProducto.mb506ModificarProducto(producto);

            if (modificado)
            {
                new BLLLog().mb506RegistrarEvento("Producto modificado: " + producto.codigo, "Productos", 1);
            }

            return modificado;
        }

        public bool mb506CambiarEstadoProducto(int idProducto, bool activo)
        {
            new BLLPermiso().mb506Validar("PRODUCTOS");
            if (idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto.");
            }

            Producto producto = dalProducto.mb506ObtenerProductoPorId(idProducto);

            if (producto == null)
            {
                throw new Exception("No se encontró el producto seleccionado.");
            }

            producto.activo = activo;
            producto.digitoVerificador = digitoVerificador.mb506CalcularProducto(producto);

            bool cambiado = dalProducto.mb506CambiarEstadoProducto(producto);

            if (cambiado)
            {
                new BLLLog().mb506RegistrarEvento((activo ? "Producto activado: " : "Producto desactivado: ") + producto.codigo, "Productos", 1);
            }

            return cambiado;
        }

        private void mb506ValidarProducto(Producto producto)
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
            if (producto.codigo.Length > 50 || producto.descripcion.Length > 150 || producto.marca.Length > 100 || producto.modelo.Length > 100)
                throw new Exception("Los datos del producto superan el largo permitido.");
            if (producto.precio > 9999999999999999.99m || decimal.Round(producto.precio, 2) != producto.precio)
                throw new Exception("El precio debe tener como maximo dos decimales.");
        }
    }
}
