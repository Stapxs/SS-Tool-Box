using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace SS_Tool_Box
{
    // ---------------------------------
    // 注意：这个文件没有被编译，请在需要使用时让其编译进入程序
    // ---------------------------------
    public class Safe
    {
        /// <summary>
        /// PBKDF2 加密算法
        /// </summary>
        public class PBKDF2
        {
            #region HASH | PBKDF2 HMAC SHA-256

            class PasswordHash
            {
                private const int SaltByteSize = 32;
                private const int HashByteSize = 64;
                private const int Iterations = 30000;

                private static string GetSalt()
                {
                    var cryptoProvider = new RNGCryptoServiceProvider();
                    byte[] b_salt = new byte[SaltByteSize];
                    cryptoProvider.GetBytes(b_salt);
                    return Convert.ToBase64String(b_salt);
                }

                public static string GetPasswordHash(string password, string salt)
                {

                    byte[] saltBytes = Convert.FromBase64String(salt);
                    byte[] derived;

                    using (var pbkdf2 = new Rfc2898DeriveBytes(
                        password,
                        saltBytes,
                        Iterations,
                        HashAlgorithmName.SHA512))
                    {
                        derived = pbkdf2.GetBytes(HashByteSize);
                    }

                    return ":pbkdf2:sha512:" + string.Format("{0}:{1}:{2}:{3}", Iterations, HashByteSize, Convert.ToBase64String(saltBytes), Convert.ToBase64String(derived));
                }

                public static bool VerifyPasswordHash(string password, string hash)
                {
                    try
                    {
                        string[] parts = hash.Split(new char[] { ':' });

                        byte[] saltBytes = Convert.FromBase64String(parts[2]);
                        byte[] derived;

                        int iterations = Convert.ToInt32(parts[0]);

                        using (var pbkdf2 = new Rfc2898DeriveBytes(
                            password,
                            saltBytes,
                            iterations,
                            HashAlgorithmName.SHA512))
                        {
                            derived = pbkdf2.GetBytes(HashByteSize);
                        }

                        string new_hash = string.Format("{0}:{1}:{2}", Iterations, Convert.ToBase64String(derived), Convert.ToBase64String(saltBytes));

                        return hash == new_hash;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            #endregion
        }
    }

}
