using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Security;
using Xunit;

namespace Interapptive.Shared.Tests.Security
{
    public class AesStreamEncryptionProviderTest : IDisposable
    {
        private readonly string sourceFileLocation = System.IO.Path.GetTempFileName();
        private readonly string outputFileLocation = System.IO.Path.GetTempFileName();
        private readonly string tmpOutputFileLocation = System.IO.Path.GetTempFileName();
        private Stream sourceStream;
        private Stream outputStream;

        private readonly AesStreamEncryptionProvider testObject;

        public AesStreamEncryptionProviderTest()
        {
            testObject = new AesStreamEncryptionProvider(new FileCipherKey());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1001)]
        [InlineData(2000)]
        [InlineData(2001)]
        public void Encrypt_ReturnsEncryptedStream_WhenGivenDecryptedStream(long sizeInKb)
        {
            // Create a file of the given size and store it's hash.
            byte[] expectedHashValue = CreateTestFile(sizeInKb);

            // Encrypt the source file and save it to the output file.
            using (sourceStream = new FileStream(sourceFileLocation, FileMode.OpenOrCreate))
            {
                using (outputStream = new FileStream(outputFileLocation, FileMode.OpenOrCreate))
                {
                    testObject.Encrypt(sourceStream, outputStream);
                }
            }

            // Now decrypt the output file to a temp file
            using (sourceStream = new FileStream(outputFileLocation, FileMode.OpenOrCreate))
            {
                using (FileStream tmpOutputStream = new FileStream(tmpOutputFileLocation, FileMode.OpenOrCreate))
                {
                    testObject.Decrypt(sourceStream, tmpOutputStream);
                }
            }

            // Load the decrypted file and comput it's hash to see if it is the same as the input file.
            byte[] hashValue;
            using (FileStream decryptedFileStream = new FileStream(tmpOutputFileLocation, FileMode.OpenOrCreate))
            {
                using (RIPEMD160 hasher = RIPEMD160.Create())
                {
                    hashValue = hasher.ComputeHash(decryptedFileStream);
                }
            }

            // Verify the hashes are the same.
            Assert.Equal(expectedHashValue, hashValue);
        }

        [Fact]
        public void Encrypt_ThrowsArgumentNullException_WhenGivenNullFileStream()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Encrypt(null, null));
        }

        [Fact]
        public void Decrypt_ThrowsArgumentNullException_WhenGivenNullFileStream()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Decrypt(null, null));
        }

        /// <summary>
        /// Create a test file of requested size in KB, returning it's computed hash.
        /// </summary>
        private byte[] CreateTestFile(long sizeInKb)
        {
            byte[] buffer = Enumerable.Repeat((byte)0x20, 1024).ToArray();
            byte[] hashValue;

            using (sourceStream = new FileStream(sourceFileLocation, FileMode.OpenOrCreate))
            {
                while (0 <= sizeInKb--)
                {
                    sourceStream.Write(buffer, 0, buffer.Length);
                }
            }

            // Have to close the stream so it gets saved to disk to be able to get a reproducable hash.
            using (sourceStream = new FileStream(sourceFileLocation, FileMode.OpenOrCreate))
            {
                using (RIPEMD160 hasher = RIPEMD160.Create())
                {
                    hashValue = hasher.ComputeHash(sourceStream);
                }
            }

            return hashValue;
        }

        /// <summary>
        /// Dispose.  Cleans up temp files.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(sourceFileLocation))
            {
                File.Delete(sourceFileLocation);
            }

            if (File.Exists(outputFileLocation))
            {
                File.Delete(outputFileLocation);
            }

            if (File.Exists(tmpOutputFileLocation))
            {
                File.Delete(tmpOutputFileLocation);
            }
        }
    }
}
