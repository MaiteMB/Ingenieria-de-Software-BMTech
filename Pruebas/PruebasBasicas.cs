using System;
using System.Globalization;
using System.Threading;
using mb506.BEBMTech.Cliente;
using mb506.BEBMTech.Roles;
using mb506.BEBMTech.Usuario;
using mb506.BEBMTech.Venta;
using mb506.BLLBMTech;
using mb506.ServiciosBMTech.Idiomas;
using mb506.ServiciosBMTech.Seguridad;

class PruebasBasicas
{
    private static int cantidad;
    private static void mb506Comprobar(bool condicion, string nombre)
    {
        if (!condicion) throw new Exception("FALLO: " + nombre);
        cantidad++;
        Console.WriteLine("OK: " + nombre);
    }
    private static bool mb506Falla(Action accion)
    {
        try { accion(); return false; }
        catch { return true; }
    }
    private class ObservadorPrueba : IObserver
    {
        public int llamadas;
        public void mb506ActualizarIdioma() { llamadas++; }
    }
    static int Main()
    {
        try
        {
            DigitoVerificador dv = new DigitoVerificador();
            mb506Comprobar(dv.mb506CalcularDatos(new object[] { "AB" }) != dv.mb506CalcularDatos(new object[] { "BA" }), "DVH detecta letras intercambiadas");
            mb506Comprobar(dv.mb506CalcularDatos(new object[] { "Ana", "Perez" }) != dv.mb506CalcularDatos(new object[] { "Perez", "Ana" }), "DVH detecta campos intercambiados");
            mb506Comprobar(dv.mb506CalcularDatos(new object[] { "ab", "c" }) != dv.mb506CalcularDatos(new object[] { "a", "bc" }), "DVH distingue limites de campos");
            mb506Comprobar(dv.mb506CalcularDatos(new object[] { null }) != dv.mb506CalcularDatos(new object[] { "" }), "DVH distingue null de vacio");
            CultureInfo original = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
            int argentino = dv.mb506CalcularDatos(new object[] { 1500.50m, new DateTime(2026, 10, 9, 12, 30, 0) });
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            int ingles = dv.mb506CalcularDatos(new object[] { 1500.50m, new DateTime(2026, 10, 9, 12, 30, 0) });
            Thread.CurrentThread.CurrentCulture = original;
            mb506Comprobar(argentino == ingles, "DVH no depende del idioma");

            Familia familia = new Familia("VENTA");
            Familia subfamilia = new Familia("CLIENTES");
            subfamilia.mb506Agregar(new Patente("REGISTRAR"));
            familia.mb506Agregar(subfamilia);
            mb506Comprobar(familia.mb506Contiene("REGISTRAR"), "Composite busca recursivamente");
            mb506Comprobar(familia.mb506ObtenerTodos().Count == 2, "Composite obtiene familias y hojas");
            mb506Comprobar(mb506Falla(() => subfamilia.mb506Agregar(familia)), "Composite rechaza ciclos");
            familia.mb506Quitar(subfamilia);
            mb506Comprobar(!familia.mb506Contiene("REGISTRAR"), "Composite quita una rama");

            SessionManager.mb506Logout();
            var sesion = SessionManager.getSession;
            mb506Comprobar(ReferenceEquals(sesion, SessionManager.getSession), "Singleton conserva una instancia");
            mb506Comprobar(mb506Falla(() => SessionManager.mb506Login(null)), "Sesion rechaza usuario nulo");
            SessionManager.mb506Login(new Usuario { email = "prueba@bmtech.com", activo = true });
            mb506Comprobar(SessionManager.IsSessionActive, "Inicio de sesion");
            mb506Comprobar(mb506Falla(() => SessionManager.mb506Login(new Usuario { email = "otra@bmtech.com" })), "No permite una segunda sesion");
            SessionManager.mb506Logout();
            mb506Comprobar(!SessionManager.IsSessionActive, "Cierre de sesion");

            ObservableIdioma observable = new ObservableIdioma();
            ObservadorPrueba observador = new ObservadorPrueba();
            observable.mb506AgregarObservador(observador);
            observable.mb506AgregarObservador(observador);
            observable.mb506CambiarIdioma("en");
            mb506Comprobar(observador.llamadas == 1, "Observer notifica sin duplicar suscripciones");
            observable.mb506EliminarObservador(observador);
            observable.mb506CambiarIdioma("es");
            mb506Comprobar(observador.llamadas == 1, "Observer deja de notificar al eliminarlo");

            HashPassword hash = new HashPassword();
            string clave = hash.mb506GenerarHash("Prueba1234");
            mb506Comprobar(hash.mb506VerificarPassword("Prueba1234", clave), "BCrypt acepta la clave correcta");
            mb506Comprobar(!hash.mb506VerificarPassword("Otra", clave), "BCrypt rechaza una clave incorrecta");
            mb506Comprobar(!hash.mb506VerificarPassword("Prueba1234", "hash-invalido"), "BCrypt rechaza un hash mal formado");
            mb506Comprobar(mb506Falla(() => hash.mb506GenerarHash(new string('a', 73))), "BCrypt no trunca claves largas");

            BLLCliente clientes = new BLLCliente();
            mb506Comprobar(mb506Falla(() => clientes.mb506ValidarCliente(new Cliente())), "Cliente rechaza campos vacios");
            Cliente cliente = new Cliente("12345678", "Ana", "Perez", "1123456789", "ana@ejemplo.com");
            clientes.mb506ValidarCliente(cliente);
            mb506Comprobar(true, "Cliente acepta datos completos");
            cliente.correoElectronico = "sin-arroba";
            mb506Comprobar(mb506Falla(() => clientes.mb506ValidarCliente(cliente)), "Cliente rechaza correo invalido");

            BLLVenta ventas = new BLLVenta();
            Pago pago = new Pago { medioPago = "Efectivo", importe = 100, verificado = true };
            ventas.mb506ValidarPago(pago, 100, true);
            mb506Comprobar(pago.verificado, "Pago correcto queda verificado");
            ventas.mb506ValidarPago(pago, 200, false);
            mb506Comprobar(!pago.verificado, "Importe diferente queda pendiente");
            mb506Comprobar(mb506Falla(() => ventas.mb506ValidarPago(pago, 200, true)), "No confirma un pago insuficiente");
            mb506Comprobar(mb506Falla(() => ventas.mb506ValidarPago(new Pago { medioPago = "Transferencia", importe = 100 }, 100, false)), "Transferencia requiere operacion");

            string secreto = Cifrado.mb506Cifrar("Operacion de prueba 123");
            mb506Comprobar(secreto != "Operacion de prueba 123", "AES no guarda el dato como texto plano");
            mb506Comprobar(Cifrado.mb506Descifrar(secreto) == "Operacion de prueba 123", "AES recupera el dato original");
            mb506Comprobar(Cifrado.mb506Cifrar("Operacion de prueba 123") != secreto, "AES genera un IV diferente");
            Console.WriteLine(cantidad + " pruebas correctas. No se ejecuto SQL.");
            return 0;
        }
        catch (Exception ex) { Console.WriteLine(ex); return 1; }
        finally { SessionManager.mb506Logout(); }
    }
}
