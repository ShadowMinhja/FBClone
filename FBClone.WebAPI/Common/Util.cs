using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FBClone.WebAPI.Common
{
    public static class Util
    {
        public static string ImageToBase64URL(string imageUrl)
        {
            string base64String;
            using (Image image = Image.FromFile(imageUrl))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public static string ToCamelCase(this string value)
        {
            string result = value;
            if (value.Length > 0)
            {
                string firstLetter = value.Substring(0, 1).ToLower();
                result = String.Format("{0}{1}", firstLetter, value.Substring(1));
            }
            return result;
        }

        public static string GetMD5Hash(System.IO.Stream inputStream)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(inputStream);
            return hash.ToHex(false);
        }

        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}