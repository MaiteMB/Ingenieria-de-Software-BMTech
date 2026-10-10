using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mb506.ServiciosBMTech.Seguridad
{
    public class HashPassword
    {
        public string mb506GenerarHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || System.Text.Encoding.UTF8.GetByteCount(password) > 72)
                throw new Exception("La clave debe tener entre 1 y 72 bytes.");
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public bool mb506VerificarPassword(string passwordIngresada, string hashAlmacenado)
        {
            if (string.IsNullOrWhiteSpace(passwordIngresada) || string.IsNullOrWhiteSpace(hashAlmacenado) ||
                System.Text.Encoding.UTF8.GetByteCount(passwordIngresada) > 72) return false;
            try { return BCrypt.Net.BCrypt.Verify(passwordIngresada, hashAlmacenado); }
            catch (BCrypt.Net.SaltParseException) { return false; }
        }
    }
}
