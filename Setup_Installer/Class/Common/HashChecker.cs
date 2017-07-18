using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ProlexNetSetup.Class.Common
{
    public class HashCheck
    {
        public static bool Check(string downloadedFile, string originalHash)
        {
            string source = downloadedFile;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5FileHash(md5Hash, source);
                return VerifyMd5Hash(md5Hash, source, originalHash);
            }
        }

        static string GetMd5FileHash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(File.ReadAllBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5FileHash(md5Hash, input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
