using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;
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
            byte[] iv = { 125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14 };
            
            // When decrypted, the value will be "ShipWorks legacy user"
            encryptedEmptyString = "Jy6fKoaDX5/Lk3/4NEBY8dePXyOrJvW5D3sBY3ibdCA="; 
            encryptedString = "URhygxDF+rPETcibOlwEJZrJSm89GduOQ8IIt2qQchg=";
            decryptedString = "Some decrypted text";
            using (var mock = AutoMock.GetLoose())
            {
                var booleanParameter = new TypedParameter(typeof(bool), false);
                var keyParameter = new NamedParameter("key", guid.ToByteArray());
                var ivParameter = new NamedParameter("initializationVector", iv);
                testObject = mock.Create<LicenseEncryptionProvider>(booleanParameter, keyParameter, ivParameter);
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
    }
}
