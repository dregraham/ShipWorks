#region

using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;
using System;
using System.IO;
using System.Text;
using Xunit;

#endregion

namespace ShipWorks.Tests.ApplicationCore.Security
{
    public class LicenseEncryptionProviderTest
    {
        private const string DecryptedString = "Some decrypted text";
        private const string EncryptedEmptyString = "Jy6fKoaDX5/Lk3/4NEBY8dePXyOrJvW5D3sBY3ibdCA=";
        private const string EncryptedString = "URhygxDF+rPETcibOlwEJZrJSm89GduOQ8IIt2qQchg=";
        private Stream DecryptedStream;
        private Stream EncryptedStream;
        private Stream EncryptedEmptyStream;

        private readonly byte[] iv = {125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14};
        private readonly Guid key = new Guid("CF136821-5D3C-4237-ABFB-F5560C65A3D0");

        public LicenseEncryptionProviderTest()
        {
            EncryptedStream = GetStream(EncryptedString);
            DecryptedStream = GetStream(DecryptedString);
            EncryptedEmptyStream = GetStream(EncryptedEmptyString);
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenDecryptedString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                string encryptedText = testObject.Encrypt(DecryptedString);

                Assert.Equal(EncryptedString, encryptedText);
            }
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenEmptyString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                string encryptedText = testObject.Encrypt(string.Empty);

                Assert.Equal(EncryptedEmptyString, encryptedText);
            }
        }

        [Fact]
        public void Encrypt_ThrowsEncryptionException_WhenCustomerLicenseNotSupported()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, false);

                Assert.Throws<EncryptionException>(() => testObject.Encrypt(EncryptedString));
            }
        }

        [Fact]
        public void Decrypt_ReturnsDecryptedString_WhenGivenEncryptedString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                string decryptedText = testObject.Decrypt(EncryptedString);

                Assert.Equal(DecryptedString, decryptedText);
            }
        }

        [Fact]
        public void Decrypt_ReturnsEmptyString_WhenGivenEncryptedEmptyString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                string decryptedText = testObject.Decrypt(EncryptedEmptyString);

                Assert.Equal(string.Empty, decryptedText);
            }
        }

        [Fact]
        public void Decrypt_ThrowsEncryptionException_WhenGivenEmptyString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                Assert.Throws<EncryptionException>(() => testObject.Decrypt(""));
            }
        }

        [Fact]
        public void Decrypt_ReturnsEmptyString_WhenCustomerLicenseNotSupported()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, false);
                string decryptedText = testObject.Decrypt(EncryptedString);

                Assert.Equal(string.Empty, decryptedText);
            }
        }
        
        [Fact]
        public void Encrypt_ReturnsEncryptedStream_WhenGivenDecryptedStream()
        {

            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                testObject.Encrypt(DecryptedStream, EncryptedStream);

                string encryptedText = GetText(EncryptedStream);

                Assert.Equal(EncryptedString, encryptedText);
            }
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedStream_WhenGivenEmptyStream()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                MemoryStream ms = new MemoryStream();
                testObject.Encrypt(ms, ms);

                string encryptedText = GetText(ms);

                Assert.Equal(EncryptedEmptyString, encryptedText);
            }
        }

        [Fact]
        public void Decrypt_ReturnsDecryptedStream_WhenGivenEncryptedStream()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                testObject.Decrypt(EncryptedStream, DecryptedStream);

                string decryptedText = GetText(DecryptedStream);

                Assert.Equal(DecryptedString, decryptedText);
            }
        }

        [Fact]
        public void Decrypt_ReturnsEmptyStream_WhenGivenEncryptedEmptyStream()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                testObject.Decrypt(EncryptedEmptyStream, DecryptedStream);

                string decryptedText = GetText(DecryptedStream);

                Assert.Equal(string.Empty, decryptedText);
            }
        }

        [Fact]
        public void Decrypt_ThrowsEncryptionException_WhenGivenEmptyStream()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, true);
                Assert.Throws<EncryptionException>(() => testObject.Decrypt(""));
            }
        }

        [Fact]
        public void Decrypt_ReturnsEmptyStream_WhenCustomerLicenseNotSupported()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = GetLicenseEncryptionProvider(mock, false);
                testObject.Decrypt(EncryptedStream, DecryptedStream);

                string decryptedText = GetText(DecryptedStream);

                Assert.Equal(string.Empty, decryptedText);
            }
        }
        
        private MemoryStream GetStream(string text)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(text);
            MemoryStream ms = new MemoryStream(byteArray.Length);

            ms.Write(byteArray, 0, byteArray.Length);
            ms.Position = 0;
            return ms;
        }

        private string GetText(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private LicenseEncryptionProvider GetLicenseEncryptionProvider(AutoMock mock,
            bool isCustomerLicenseSupported)
        {
            var booleanParameter = new TypedParameter(typeof(bool), isCustomerLicenseSupported);

            var cipher = mock.Mock<ICipherKey>();
            cipher.SetupGet(c => c.Key).Returns(key.ToByteArray());
            cipher.SetupGet(c => c.InitializationVector).Returns(iv);

            return mock.Create<LicenseEncryptionProvider>(booleanParameter);
        }
    }
}