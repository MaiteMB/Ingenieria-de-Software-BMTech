using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mb506.BLLBMTech;
using mb506.ServiciosBMTech.Idiomas;

namespace mb506.BMTech
{
    public static class Mensajes
    {
        private static Dictionary<string, string> textos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static string idioma;

        public static void mb506Actualizar()
        {
            textos = new BLLIdioma().mb506ObtenerTraducciones(IdiomasStatic.Observer.idiomaActual);
            idioma = IdiomasStatic.Observer.idiomaActual;
        }

        public static string mb506Traducir(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            if (idioma != IdiomasStatic.Observer.idiomaActual)
            {
                try { mb506Actualizar(); }
                catch { return texto; }
            }
            string resultado;
            if (textos.TryGetValue(texto, out resultado)) return resultado;
            // Conserva los datos variables que siguen a una leyenda traducible.
            string prefijo = null;
            foreach (string etiqueta in textos.Keys)
                if ((etiqueta.EndsWith(" ", StringComparison.Ordinal) || etiqueta.EndsWith("$", StringComparison.Ordinal)) && texto.StartsWith(etiqueta, StringComparison.Ordinal)
                    && (prefijo == null || etiqueta.Length > prefijo.Length)) prefijo = etiqueta;
            return prefijo == null ? texto : textos[prefijo] + texto.Substring(prefijo.Length);
        }

        public static DialogResult mb506Mostrar(string texto, string titulo = "BMTech",
            MessageBoxButtons botones = MessageBoxButtons.OK, MessageBoxIcon icono = MessageBoxIcon.Information)
        {
            return MessageBox.Show(mb506Traducir(texto), mb506Traducir(titulo), botones, icono);
        }
    }
}
