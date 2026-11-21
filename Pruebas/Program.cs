using System;

namespace mb506.Pruebas
{
    class Program
    {
        static int Main(string[] args)
        {
            bool conBaseDePruebas = args.Length > 0 && args[0] == "--sql";
            int errores = 0;

            Console.WriteLine("=== Pruebas basicas ===");
            errores += PruebasBasicas.Ejecutar();

            Console.WriteLine("=== Pruebas de exportacion ===");
            errores += PruebasExportacion.Ejecutar();

            Console.WriteLine("=== Pruebas de entrega 3 ===");
            errores += PruebasEntrega3.Ejecutar();

            Console.WriteLine("=== Pruebas de servicio tecnico ===");
            errores += PruebasServicioTecnico.Ejecutar(conBaseDePruebas);

            if (errores == 0)
                Console.WriteLine("Todas las pruebas terminaron bien.");
            else
                Console.WriteLine("Hubo pruebas con errores.");

            return errores;
        }
    }
}
