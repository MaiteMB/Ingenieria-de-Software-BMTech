using BEBMTech;
using BEBMTech.Producto;
using DALBMTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLBMTech
{
    public class BLLProducto
    {
        private readonly DALProducto dalProducto;

        public BLLProducto()
        {
            dalProducto = new DALProducto();
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

            return dalProducto.InsertarProducto(producto);
        }

        public bool ModificarProducto(Producto producto)
        {
            ValidarProducto(producto);

            if (producto.idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto para modificar.");
            }

            return dalProducto.ModificarProducto(producto);
        }

        public bool CambiarEstadoProducto(int idProducto, bool activo)
        {
            if (idProducto <= 0)
            {
                throw new Exception("Debe seleccionar un producto.");
            }

            return dalProducto.CambiarEstadoProducto(idProducto, activo);
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
