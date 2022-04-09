using manilahub.core.Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace manilahub.core.Services
{
    public class CryptographyService : ICryptographyService
    {
        public CryptographyService()
        {

        }

        public string SHA512(string value)
        {
            SHA512Managed sha512Managed = new SHA512Managed();
            UTF8Encoding unicodeEncoding = new UTF8Encoding();

            byte[] hashedValue;
            byte[] messageBytes = unicodeEncoding.GetBytes(value);
            string hex = string.Empty;

            hashedValue = sha512Managed.ComputeHash(messageBytes);

            foreach (byte x in hashedValue)
            {
                hex += x.ToString("x2");
            }

            return hex.ToUpper();
        }
    }
}
