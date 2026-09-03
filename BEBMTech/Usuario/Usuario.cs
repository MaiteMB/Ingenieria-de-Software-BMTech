using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEBMTech.Usuario
{
    public class Usuario
    {
        public string email { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        private string password;
        public bool activo { get; set; } = true;
        public int intentos { get; set; } = 3;
        public string perfil { get; set; }

        public Usuario() { }

        public Usuario(string email, string nombre, string apellido, string rol)
        {
            this.email = email;
            this.nombre = nombre;
            this.apellido = apellido;
            this.perfil = rol;
        }

        public Usuario(string email, bool activo, int intentos)
        {
            this.email = email;
            this.activo = activo;
            this.intentos = intentos;
        }

        public Usuario(string email, string nombre, string apellido, string password, bool activo, int intentos, string rol)
        {
            this.email = email;
            this.nombre = nombre;
            this.apellido = apellido;
            this.password = password;
            this.activo = activo;
            this.intentos = intentos;
            this.perfil = rol;
        }

        public string GetPassword() => password;

        public void SetPassword(string password) { this.password = password; }
    }
}