using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    public class Hashing
    {
        public static string RNG(string password)
        {
            string hashed = "";
            try
            {
                var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
                var salt = new byte[22];
                rngCryptoServiceProvider.GetBytes(salt);

                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt) { IterationCount = 50000 };

                var hash = rfc2898DeriveBytes.GetBytes(22);
                hashed = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
            catch (CryptographicException ce)
            {
                hashed = password;
            }
            return hashed;
        }
        public static string MD5(string password)
        {
            string hashed = "";
            try
            {
                #region Text
                string Text = "MD5";
                #endregion
                byte[] result = new byte[Text.Length];
                MD5 md5 = new MD5CryptoServiceProvider();
                result = md5.ComputeHash(Encoding.UTF8.GetBytes(Text));
                hashed = Convert.ToBase64String(result);
            }
            catch (CryptographicException ce)
            {
                hashed = password;
            }
            return hashed;

        }
        public static string SHA1(string password)
        {
            string hashed = "";
            try
            {
                #region Text
                string Text = "SHA1";
                #endregion
                byte[] result = new byte[Text.Length];
                SHA1 sha = new SHA1CryptoServiceProvider();
                result = sha.ComputeHash(Encoding.UTF8.GetBytes(Text));
                hashed = Convert.ToBase64String(result);
            }
            catch (CryptographicException ce)
            {
                hashed = password;
            }
            return hashed;

        }
        public static string SHA256(string password)
        {
            string hashed = "";
            try
            {
                byte[] result = new byte[password.Length];
                SHA256 sha = new SHA256CryptoServiceProvider();
                result = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                hashed = Convert.ToBase64String(result);
            }
            catch (CryptographicException ce)
            {
                hashed = password;
            }
            return hashed;
        }
    }
}
