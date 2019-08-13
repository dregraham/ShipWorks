using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// Simple class for compressing and decomressing with GZip
    /// </summary>
    public static class GZipUtility
    {
        /// <summary>
        /// Compress the given input using gzip
        /// </summary>
        public static string Compress(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(Compress(bytes));
        }

        /// <summary>
        /// Compress the given input using gzip
        /// </summary>
        public static byte[] Compress(byte[] input)
        {
            using (MemoryStream targetStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(targetStream, CompressionMode.Compress, true))
                {
                    zipStream.Write(input, 0, input.Length);
                }

                return targetStream.ToArray();
            }
        }

        /// <summary>
        /// Compress the given input using gzip
        /// </summary>
        public static string Decompress(string input)
        {
            var bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(Decompress(bytes));
        }

        /// <summary>
        /// Decompress the given input that was zipped using gzip
        /// </summary>
        public static byte[] Decompress(byte[] input)
        {
            using (GZipStream zipStream = new GZipStream(new MemoryStream(input), CompressionMode.Decompress, false))
            {
                using (MemoryStream targetStream = new MemoryStream())
                {
                    byte[] buffer = new byte[1024];

                    while (true)
                    {
                        int read = zipStream.Read(buffer, 0, buffer.Length);
                        if (read > 0)
                        {
                            targetStream.Write(buffer, 0, read);
                        }
                        else
                        {
                            break;
                        }
                    }

                    return targetStream.ToArray();
                }
            }
        }
    }
}
