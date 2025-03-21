using System.Security.Cryptography;
using System.Text;

namespace Barangay_Office.Utilities
{
    public class PasswordEncryptionManager
    {
        private readonly static string key = "ThisIsA16ByteKey";

        public static string Encrypt(string password)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key,aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using(CryptoStream cs = new CryptoStream((Stream)ms, encrypt, CryptoStreamMode.Write))
                    {
                        using(StreamWriter sw = new StreamWriter((Stream)cs))
                        {
                            sw.Write(password);
                        }
                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string password)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(password);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream((Stream)ms, decrypt, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
