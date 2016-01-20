using System;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore.Licensing;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class LicenseEncryptionProviderTest
    {
        private readonly LicenseEncryptionProvider testObject;

        public LicenseEncryptionProviderTest()
        {

            Guid guid = new Guid("CF136821-5D3C-4237-ABFB-F5560C65A3D0");

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDatabaseIdentifier>().Setup(x => x.Get()).Returns(guid);

                testObject = new LicenseEncryptionProvider(mock.Mock<IDatabaseIdentifier>().Object);
            }
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenDecryptedString_Test()
        {
            string encryptedText = testObject.Encrypt("Some decrypted text");

            Assert.Equal("qRiG4SmDSj9lM0HX4hltSQr1joOCJ+eN", encryptedText);
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenEmptyString_Test()
        {
            string encryptedText = testObject.Encrypt("");

            Assert.Equal("jojgM63ozWI=", encryptedText);
        }

        [Fact]
        public void Decrypt_ReturnsDecryptedString_WhenGivenEncryptedString_Test()
        {
            string decryptedText = testObject.Decrypt("qRiG4SmDSj9lM0HX4hltSQr1joOCJ+eN");

            Assert.Equal("Some decrypted text", decryptedText);
        }

        [Fact]
        public void Decrypt_ThrowsArgumentException_WhenGivenEmptyString_Test()
        {
            Assert.Throws<ArgumentException>(() => testObject.Decrypt(""));
        }
    }
}
