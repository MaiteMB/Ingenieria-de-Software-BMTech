namespace mb506.BEBMTech.Idiomas
{
    public class Idioma
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public override string ToString() { return nombre; }
    }
}
