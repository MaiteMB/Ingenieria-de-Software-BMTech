using System;
using System.Data;
using mb506.BEBMTech.Producto;
using mb506.BEBMTech.Informes;
using mb506.BLLBMTech;

class PruebasEntrega3
{
    static int cantidad;
    static void Comprobar(bool resultado, string nombre)
    {
        if (!resultado) throw new Exception(nombre);
        cantidad++;
        Console.WriteLine("OK: " + nombre);
    }
    static void Rechazar(Action accion, string nombre)
    {
        bool rechazo = false;
        try { accion(); } catch (Exception) { rechazo = true; }
        Comprobar(rechazo, nombre);
    }
    public static int Ejecutar()
    {
        try
        {
            Producto producto = new Producto(1,"ABC","Teclado","Marca","Modelo",100m,5);
            var version = producto.mb506CrearVersion();
            producto.descripcion = "Modificado";
            producto.precio = 200m;
            producto.stock = 0;
            producto.activo = false;
            Comprobar(version.descripcion == "Teclado" && version.precio == 100m && version.stock == 5 && version.activo, "La version conserva el estado anterior");
            producto.mb506RestaurarVersion(version);
            Comprobar(producto.descripcion == "Teclado" && producto.precio == 100m && producto.stock == 5 && producto.activo, "Restaura los datos del producto");
            Rechazar(() => new Producto { idProducto = 2 }.mb506RestaurarVersion(version), "No restaura una version ajena");
            Rechazar(() => producto.mb506RestaurarVersion(null), "Rechaza una version nula");
            Comprobar(!version.GetType().GetProperty("precio").CanWrite, "El precio guardado no admite cambios externos");
            var basePerfil = new mb506.BEBMTech.Roles.Perfil { id_perfil="Base" };
            basePerfil.Roles.Add(new mb506.BEBMTech.Roles.Patente("SERVICIOS"));
            var perfil = new mb506.BEBMTech.Roles.Perfil { id_perfil="Completo" };
            perfil.Roles.Add(basePerfil);
            Comprobar(perfil.mb506ObtenerTodos().Exists(r => r.idRol == "SERVICIOS"), "Composite recorre perfiles incluidos");
            basePerfil.Roles.Add(perfil);
            Rechazar(() => perfil.mb506ObtenerTodos(), "Composite rechaza ciclos entre perfiles");
            var bll = new BLLInformes();
            bll.mb506ValidarFiltros("Ventas",DateTime.Today,DateTime.Today);
            bll.mb506ValidarFiltros("Servicio tecnico",DateTime.Today.AddDays(-1),DateTime.Today);
            Comprobar(true, "Admite ambos informes y rangos validos");
            Rechazar(() => bll.mb506ValidarFiltros("Otro",DateTime.Today,DateTime.Today), "Rechaza tipos desconocidos");
            Rechazar(() => bll.mb506ValidarFiltros("Ventas",DateTime.Today,DateTime.Today.AddDays(-1)), "Rechaza fechas invertidas");
            Rechazar(() => bll.mb506ValidarFiltros("Ventas",DateTime.MinValue,DateTime.Today), "Rechaza fechas anteriores a SQL");
            Rechazar(() => bll.mb506ValidarFiltros("Ventas",DateTime.Today,DateTime.MaxValue), "Evita desbordar el ultimo dia");
            Rechazar(() => bll.mb506PrepararTexto(null), "Exige generar el informe");
            DataTable tabla = new DataTable();
            foreach (string columna in new[] { "numero","fecha","cliente","estado","total" }) tabla.Columns.Add(columna,typeof(object));
            tabla.Rows.Add(25,DateTime.Today,"Cliente de prueba","PAGADA",100m);
            var informe = new InformeOperaciones { tipo="Ventas",desde=DateTime.Today,hasta=DateTime.Today,datos=tabla,totalCobrado=100m };
            string texto = bll.mb506PrepararTexto(informe);
            Comprobar(informe.cantidad == 1 && texto.Contains("Cliente de prueba") && texto.Contains("25") && texto.Contains("Total cobrado"), "El informe contiene resumen y operaciones");
            Console.WriteLine(cantidad + " pruebas de Entrega 3 correctas. No se ejecuto SQL.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }
}
