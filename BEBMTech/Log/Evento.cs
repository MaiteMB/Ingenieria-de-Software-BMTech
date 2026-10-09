using System;

namespace BEBMTech.Log
{
    public class Evento
    {
        public int idLog { get; set; }
        public string email { get; set; }
        public DateTime fecha { get; set; }
        public string accion { get; set; }
        public string modulo { get; set; }
        public int criticidad { get; set; }
    }
}
