using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing.Decoding
{
    /// <summary>
    /// The raw license data before it has been interpreted as a ShipWorks license
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what ShipWorks currently uses")]
    class RawLicense
    {
        string code;
        string data1;
        string data2;
        string plainText;

        /// <summary>
        /// Construction
        /// </summary>
        public RawLicense(string code, string data1, string data2, string plainText)
        {
            this.code = code;
            this.data1 = data1;
            this.data2 = data2;
            this.plainText = plainText;
        }

        /// <summary>
        /// The full license code.
        /// </summary>
        public string Code { get { return code; } }

        /// <summary>
        /// Arbitrary data contained in the license.
        /// </summary>
        public string Data1 { get { return data1; } }

        /// <summary>
        /// Arbitrary data contained in the license.
        /// </summary>
        public string Data2 { get { return data2; } }

        /// <summary>
        /// The trailing plain-text part of the license.
        /// </summary>
        public string PlainText { get { return plainText; } }

        /// <summary>
        /// Computes a hash of the input string, and returns the result as a string
        /// </summary>
        public static string GetHashData(string hashInput)
        {
            // The hash algorithms expect Byte data
            byte[] data = new byte[hashInput.Length];
            for (int i = 0; i < hashInput.Length; i++)
            {
                data[i] = Convert.ToByte(hashInput[i]);
            }

            // Generate the hash
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(data);

            StringBuilder hashData = new StringBuilder(5, 5);

            // Now convert the hash to ASCII
            for (int i = 0; i < 5; i++)
            {
                hashData.Append(asciiDataSource[hashBytes[i] % asciiDataSource.Length]);
            }

            return hashData.ToString();
        }

        // This will be used to randomly select pointless data
        private static string asciiDataSource = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";
    }
}
