using System;
using System.Text;

namespace Modules.Encryption
{
    public static class Extensions
    {
        public static string Encrypt(this IEncryptor encryptor, string input)
        {
            byte[] bytes = encryptor.EncryptBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static byte[] EncryptBytes(this IEncryptor encryptor, string input)
        {
            byte[] convertedInput = Encoding.UTF8.GetBytes(input);
            byte[] encryptedInput = encryptor.Encrypt(convertedInput);
            return encryptedInput;
        }

        public static string Decrypt(this IEncryptor encryptor, string input)
        {
            byte[] convertedInput = Convert.FromBase64String(input);
            byte[] decryptedInput = encryptor.Decrypt(convertedInput);
            return Encoding.UTF8.GetString(decryptedInput);
        }
    }
}
