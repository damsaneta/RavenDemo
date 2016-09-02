using System.Security.Cryptography;
using System.Text;

namespace Demo.Common.Utils
{
    public static class CryptoHelper
    {
        private const string Salt = "RavenSalt";

        public static byte[] Hash(string value)
        {

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            value += Salt;
            using (var hasher = new SHA256CryptoServiceProvider())
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                var result = hasher.ComputeHash(bytes);
                return result;
            }
        }

        public static bool ComparePasswords(string password, byte[] passwordDb)
        {
            var hash = Hash(password);
            return Encoding.UTF8.GetString(hash) == Encoding.UTF8.GetString(passwordDb);
        }

    }
}
