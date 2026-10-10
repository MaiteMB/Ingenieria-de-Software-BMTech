using System;
using System.Collections.Generic;

namespace mb506.ServiciosBMTech.Idiomas
{
    public class ObservableIdioma
    {
        private List<IObserver> observadores = new List<IObserver>();
        public string idiomaActual { get; private set; } = "es";

        public void mb506AgregarObservador(IObserver observador)
        {
            if (!observadores.Contains(observador)) observadores.Add(observador);
        }

        public void mb506EliminarObservador(IObserver observador)
        {
            observadores.Remove(observador);
        }

        public void mb506CambiarIdioma(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 2) throw new Exception("Idioma no disponible.");
            idiomaActual = codigo;
            foreach (IObserver observador in new List<IObserver>(observadores))
                observador.mb506ActualizarIdioma();
        }
    }

    public static class IdiomasStatic
    {
        public static ObservableIdioma Observer { get; } = new ObservableIdioma();
    }
}
