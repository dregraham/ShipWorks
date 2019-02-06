using System;
using System.IO;
using System.Security.Cryptography;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// A ShipWorks installation file
    /// </summary>
    public class InstallFile
    {
        private readonly string downloadedHash;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public InstallFile(string installationFilePath, string installationFileHash)
        {
            Path = installationFilePath;
            downloadedHash = installationFileHash;
        }

        /// <summary>
        /// Path to the file
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Whether or not the file is valid based on its SHA256 checksum
        /// </summary>
        public bool IsValid
        {
            get
            {
                string computedHash = ComputeSHA256CheckSum(Path);

                return computedHash == downloadedHash;
            }
        }
        
        /// <summary>
        /// Compute the SHA256 checksum of the file at the given path
        /// </summary>
        private string ComputeSHA256CheckSum(string filePath)
        {
            try
            {
                using (SHA256 sha = SHA256Managed.Create())
                {
                    using (FileStream fileStream = File.OpenRead(filePath))
                    {
                        byte[] hash = sha.ComputeHash(fileStream);
                        return BitConverter.ToString(hash).Replace("-", string.Empty);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}