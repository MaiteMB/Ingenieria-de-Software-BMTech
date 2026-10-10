using System;
using System.Collections.Generic;
using mb506.DALBMTech;

namespace mb506.BLLBMTech
{
    public class BLLIdioma
    {
        public Dictionary<string, string> mb506ObtenerTraducciones(string codigo)
        {
            if (!mb506ObtenerIdiomas().Exists(i => i.codigo == codigo)) throw new Exception("Idioma no disponible.");
            return new DALIdioma().mb506ObtenerTraducciones(codigo);
        }

        public List<mb506.BEBMTech.Idiomas.Idioma> mb506ObtenerIdiomas()
        {
            return new DALIdioma().mb506ObtenerIdiomas();
        }

        public System.Data.DataTable mb506ObtenerTextos(string codigo)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            return new DALIdioma().mb506ObtenerTextos(codigo);
        }

        public void mb506GuardarIdioma(string codigo, string nombre, System.Data.DataTable textos)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (codigo == null || codigo.Length != 2 || !System.Text.RegularExpressions.Regex.IsMatch(codigo, "^[a-z]{2}$"))
                throw new Exception("El codigo del idioma debe tener dos letras minusculas.");
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Length > 50) throw new Exception("Indique un nombre de idioma valido.");
            if (textos == null) throw new Exception("Faltan las traducciones.");
            foreach (System.Data.DataRow fila in textos.Rows)
                if (string.IsNullOrWhiteSpace(Convert.ToString(fila["textoTraducido"])) || Convert.ToString(fila["textoTraducido"]).Length > 250)
                    throw new Exception("Cada traduccion debe tener entre 1 y 250 caracteres.");
            new DALIdioma().mb506GuardarIdioma(codigo, nombre.Trim(), textos);
            new BLLLog().mb506RegistrarEvento("Idioma guardado: " + codigo, "Idiomas", 1);
            mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.mb506CambiarIdioma(mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.idiomaActual);
        }

        public void mb506EliminarIdioma(string codigo)
        {
            new BLLPermiso().mb506Validar("SEGURIDAD");
            if (codigo == "es" || codigo == "en" || codigo == mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.idiomaActual)
                throw new Exception("No puede eliminar un idioma base o el idioma en uso.");
            new DALIdioma().mb506EliminarIdioma(codigo);
            new BLLLog().mb506RegistrarEvento("Idioma eliminado: " + codigo, "Idiomas", 1);
        }
    }
}
