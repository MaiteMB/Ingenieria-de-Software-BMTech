using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace mb506.ServiciosBMTech.Seguridad
{
    public static class Cifrado
    {
        private static readonly object bloqueo = new object();
        private static readonly string archivoClave = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BMTech", "clave-aes.key");

        private static byte[] mb506ObtenerClave(bool crear)
        {
            lock (bloqueo)
            {
                if (!File.Exists(archivoClave))
                {
                    if (!crear) throw new Exception("Falta la clave de cifrado de BMTech.");
                    Directory.CreateDirectory(Path.GetDirectoryName(archivoClave));
                    byte[] nueva = new byte[32];
                    using (RandomNumberGenerator generador = RandomNumberGenerator.Create()) generador.GetBytes(nueva);
                    File.WriteAllBytes(archivoClave, nueva);
                }
                byte[] clave = File.ReadAllBytes(archivoClave);
                if (clave.Length != 32) throw new Exception("La clave de cifrado no es valida.");
                return clave;
            }
        }

        public static string mb506Cifrar(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            using (Aes aes = Aes.Create())
            {
                aes.Key = mb506ObtenerClave(true);
                aes.GenerateIV();
                byte[] datos = Encoding.UTF8.GetBytes(texto);
                using (ICryptoTransform cifrador = aes.CreateEncryptor())
                {
                    byte[] cifrado = cifrador.TransformFinalBlock(datos, 0, datos.Length);
                    byte[] resultado = new byte[aes.IV.Length + cifrado.Length];
                    Array.Copy(aes.IV, resultado, aes.IV.Length);
                    Array.Copy(cifrado, 0, resultado, aes.IV.Length, cifrado.Length);
                    return "AES:" + Convert.ToBase64String(resultado);
                }
            }
        }

        public static string mb506Descifrar(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            if (!texto.StartsWith("AES:", StringComparison.Ordinal)) throw new Exception("El dato no esta cifrado.");
            byte[] datos = Convert.FromBase64String(texto.Substring(4));
            if (datos.Length < 32) throw new Exception("El dato cifrado esta incompleto.");
            using (Aes aes = Aes.Create())
            {
                aes.Key = mb506ObtenerClave(false);
                byte[] iv = new byte[16];
                Array.Copy(datos, iv, iv.Length);
                aes.IV = iv;
                using (ICryptoTransform descifrador = aes.CreateDecryptor())
                    return Encoding.UTF8.GetString(descifrador.TransformFinalBlock(datos, 16, datos.Length - 16));
            }
        }

        public static void mb506ExportarClave(string destino)
        {
            File.WriteAllBytes(destino, mb506ObtenerClave(true));
        }

        public static void mb506ImportarClave(string origen)
        {
            byte[] clave = File.ReadAllBytes(origen);
            if (clave.Length != 32) throw new Exception("El archivo de clave no es valido.");
            Directory.CreateDirectory(Path.GetDirectoryName(archivoClave));
            File.WriteAllBytes(archivoClave, clave);
        }
    }
}
