using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosBMTech.Seguridad
{
    public class HashPassword
    {
        public string GenerarHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public bool VerificarPassword(string passwordIngresada, string hashAlmacenado)
        {
            return BCrypt.Net.BCrypt.Verify(passwordIngresada, hashAlmacenado);
        }
    }
}
