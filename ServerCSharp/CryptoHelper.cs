using System.Security.Cryptography;
using System.Text;

namespace ServerCSharp
{
    public static class CryptoHelper
    {
        public static string ToSHA256(string text)
        {
            var sha = new SHA256Managed();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            var builder = new StringBuilder();

            foreach(var b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }

        public static string Hash(string text, string key = null)
        {
            key = key ?? GetRandomKey();
            return ToSHA256(text + Globals.SaltKey + key);
        }

        public static string GetRandomKey()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Create().GetBytes(bytes);

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; ++i)
                builder.Append(bytes[i].ToString("X2"));

            return builder.ToString();
        }
    }
}