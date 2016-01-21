﻿using System;
using System.Text;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore.Licensing;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class LicenseEncryptionProviderTest
    {
        private readonly LicenseEncryptionProvider testObject;
        private readonly string encryptedString;
        private readonly string encryptedEmptyString;
        private readonly string decryptedString;

        public LicenseEncryptionProviderTest()
        {
            Guid guid = new Guid("CF136821-5D3C-4237-ABFB-F5560C65A3D0");
            encryptedEmptyString = "Jy6fKoaDX5/Lk3/4NEBY8dePXyOrJvW5D3sBY3ibdCA=";
            encryptedString = "URhygxDF+rPETcibOlwEJZrJSm89GduOQ8IIt2qQchg=";
            decryptedString = "Some decrypted text";
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDatabaseIdentifier>().Setup(x => x.Get()).Returns(guid);

                testObject = new LicenseEncryptionProvider(mock.Mock<IDatabaseIdentifier>().Object);
            }
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenDecryptedString_Test()
        {
            string encryptedText = testObject.Encrypt(decryptedString);

            Assert.Equal(encryptedString, encryptedText);
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenEmptyString_Test()
        {
            string encryptedText = testObject.Encrypt(string.Empty);

            Assert.Equal(encryptedEmptyString, encryptedText);
        }

        [Fact]
        public void Decrypt_ReturnsDecryptedString_WhenGivenEncryptedString_Test()
        {
            string decryptedText = testObject.Decrypt(encryptedString);

            Assert.Equal(decryptedString, decryptedText);
        }

        [Fact]
        public void Decrypt_ReturnsEmptyString_WhenGivenEncryptedEmptyString_Test()
        {
            string decryptedText = testObject.Decrypt(encryptedEmptyString);

            Assert.Equal(string.Empty, decryptedText);
        }

        [Fact]
        public void Decrypt_ThrowsArgumentException_WhenGivenEmptyString_Test()
        {
            Assert.Throws<ShipWorksLicenseException>(() => testObject.Decrypt(""));
        }
    }
}
