using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Class for generating an Amazon-compatible HMAC-* request signature
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5350: Do not use insecure cryptographic algorithm SHA1",
        Justification = "This is what ShipWorks currently uses")]
    public static class RequestSignature
    {
        /// <summary>
        /// Creates a signature for the provided string using the specified hashing algorithm
        /// </summary>
        public static string CreateRequestSignature(string valueToSign, string secretKey, SigningAlgorithm hashingAlgorithm)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("secretKey cannot be null or empty", "secretKey");
            }

            if (string.IsNullOrEmpty(valueToSign))
            {
                throw new ArgumentException("stringToSign cannot be null or empty", "valueToSign");
            }

            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            // create the algorithm to use to hash the signature
            HMAC algorithm;
            if (hashingAlgorithm == SigningAlgorithm.SHA1)
            {
                algorithm = new HMACSHA1();
            }
            if (hashingAlgorithm == SigningAlgorithm.SHA256)
            {
                algorithm = new HMACSHA256();
            }
            else
            {
                algorithm = new HMACSHA512();
            }

            // create the signature
            using (algorithm)
            {
                algorithm.Key = key;

                byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(valueToSign));
                foreach (byte b in hash)
                {
                    Debug.Write(b.ToString("X2"));
                }

                // base 64 encode it
                return Convert.ToBase64String(hash);
            }
        }
    }
}
