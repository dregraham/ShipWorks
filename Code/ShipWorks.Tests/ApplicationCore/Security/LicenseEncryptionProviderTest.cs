using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Data.Administration;
using System;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Security
{
    public class LicenseEncryptionProviderTest
    {
        private readonly AesEncryptionProvider testObject;
        private readonly string encryptedString;
        private readonly string encryptedEmptyString;
        private readonly string decryptedString;

        public LicenseEncryptionProviderTest()
        {
            Guid guid = new Guid("CF136821-5D3C-4237-ABFB-F5560C65A3D0");

            // When decrypted, the value will be "ShipWorks legacy user"
            encryptedEmptyString = "Jy6fKoaDX5/Lk3/4NEBY8dePXyOrJvW5D3sBY3ibdCA="; 
            encryptedString = "URhygxDF+rPETcibOlwEJZrJSm89GduOQ8IIt2qQchg=";
            decryptedString = "Some decrypted text";
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ISqlSchemaVersion> sqlVersion = mock.Mock<ISqlSchemaVersion>();
                sqlVersion.Setup(s => s.GetInstalledSchemaVersion()).Returns(Version.Parse("4.9.0.0"));
                Mock<IAesParams> aesParams = mock.Mock<IAesParams>();
                aesParams.SetupGet(p => p.InitializationVector)
                    .Returns(new byte[] {125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14});
                aesParams.SetupGet(p => p.Key).Returns(guid.ToByteArray());
                aesParams.SetupGet(p => p.EmptyValue).Returns("ShipWorks legacy user");

                testObject = mock.Create<LicenseEncryptionProvider>();
            }
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenDecryptedString()
        {
            string encryptedText = testObject.Encrypt(decryptedString);

            Assert.Equal(encryptedString, encryptedText);
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenEmptyString()
        {
            string encryptedText = testObject.Encrypt(string.Empty);

            Assert.Equal(encryptedEmptyString, encryptedText);
        }

        [Fact]
        public void Decrypt_ReturnsDecryptedString_WhenGivenEncryptedString()
        {
            string decryptedText = testObject.Decrypt(encryptedString);

            Assert.Equal(decryptedString, decryptedText);
        }

        [Fact]
        public void Decrypt_ReturnsEmptyString_WhenGivenEncryptedEmptyString()
        {
            string decryptedText = testObject.Decrypt(encryptedEmptyString);

            Assert.Equal(string.Empty, decryptedText);
        }

        [Fact]
        public void Decrypt_ThrowsEncryptionException_WhenGivenEmptyString()
        {
            Assert.Throws<EncryptionException>(() => testObject.Decrypt(""));
        }

        [Fact]
        public void Encrypt_ThrowsEncryptionException_WhenDatabaseIdentifierNotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDatabaseIdentifier>().Setup(x => x.Get()).Throws<DatabaseIdentifierException>();

                Mock<ISqlSchemaVersion> sqlVersion = mock.Mock<ISqlSchemaVersion>();
                sqlVersion.Setup(s => s.GetInstalledSchemaVersion()).Returns(Version.Parse("4.9.0.0"));

                var testObject = mock.Create<LicenseEncryptionProvider>();

                Assert.Throws<EncryptionException>(() => testObject.Encrypt("test"));
            }
        }

        [Fact]
        public void Decrypt_ThrowsEncryptionException_WhenDatabaseIdentifierNotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDatabaseIdentifier>().Setup(x => x.Get()).Throws<DatabaseIdentifierException>();

                Mock<ISqlSchemaVersion> sqlVersion = mock.Mock<ISqlSchemaVersion>();
                sqlVersion.Setup(s => s.GetInstalledSchemaVersion()).Returns(Version.Parse("4.9.0.0"));

                var testObject = mock.Create<LicenseEncryptionProvider>();

                Assert.Throws<EncryptionException>(() => testObject.Decrypt("test"));
            }
        }
    }
}
