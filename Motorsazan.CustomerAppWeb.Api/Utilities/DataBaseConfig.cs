using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Motorsazan.CustomerAppWeb.Api.Utilities
{
    public static class DataBaseConfig
    {
        static DataBaseConfig()
        {
            var status = InitDataBase();
            if(status == false)
            {
            }
        }

        public static string ConnectionString { set; get; }

        public static int CommandTimeout { set; get; }

        private static bool InitDataBase()
        {
            const string domain = "Motorsazan";

            var projectNameParts = Assembly.GetCallingAssembly().GetName().Name.Split('.');
            var projectName = $"{projectNameParts[0]}.{projectNameParts[1]}";

            var dataBaseConfigFilePath = $@"C:\{domain}\{projectName}\Configure\DataBaseProperties.config";

            var cryptographyKey = $"{domain}@{projectName}";

            var encryptedText = File.ReadAllText(dataBaseConfigFilePath);
            var decryptedText = Decrypt(encryptedText, cryptographyKey);

            var configure = XDocument.Parse(decryptedText);
            ConnectionString = configure.Root.XPathSelectElement("/Configure/ConnectionString")?.Value;
            CommandTimeout = Convert.ToInt32(configure.Root.XPathSelectElement("/Configure/CommandTimeout")?.Value);
            return true;
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Encrypt(string plainText, string cryptographyKey)
        {
            string encryptedString;
            var vectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using(var password = new PasswordDeriveBytes(cryptographyKey, null))
            {
                var keyBytes = password.GetBytes(32);
                using(var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using(var encryptor = symmetricKey.CreateEncryptor(keyBytes, vectorBytes))
                    {
                        using(var memoryStream = new MemoryStream())
                        {
                            using(var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = memoryStream.ToArray();
                                encryptedString = Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }

            return encryptedString;
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Decrypt(string cipherText, string cryptographyKey)
        {
            string decryptedString;
            var vectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
            var cipherTextBytes = Convert.FromBase64String(cipherText);

            using(var password = new PasswordDeriveBytes(cryptographyKey, null))
            {
                var keyBytes = password.GetBytes(32);
                using(var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using(var decryptor = symmetricKey.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using(var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using(var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                decryptedString = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }

            return decryptedString;
        }
    }
}