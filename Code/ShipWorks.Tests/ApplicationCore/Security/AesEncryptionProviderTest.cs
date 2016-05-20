using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Security
{
    public class AesEncryptionProviderTest
    {
        private readonly AesEncryptionProvider testObject;
        private readonly string encryptedString;
        private readonly string decryptedString;

        public AesEncryptionProviderTest()
        {
            byte[] key = new Guid("{87EBA780-261A-462F-911D-ADC5EB58DF96}").ToByteArray();
            byte[] iv = new Guid("{D4F8FD57-C68A-4623-88F4-0CB18769241C}").ToByteArray();

            encryptedString = "vC5iCiYlVkUiwj7JjSDzPY0+qwxUOkYdnFhqr6zxcC0=";
            decryptedString = "Some decrypted text";
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICipherKey>().Setup(c => c.Key).Returns(key);
                mock.Mock<ICipherKey>().Setup(c => c.InitializationVector).Returns(iv);

                testObject = mock.Create<AesEncryptionProvider>();
            }
        }

        [Fact]
        public void Encrypt_ReturnsEncryptedString_WhenGivenDecryptedString()
        {
            string encryptedText = testObject.Encrypt(decryptedString);

            Assert.Equal(encryptedString, encryptedText);
        }

        [Fact]
        public void Decrypt_ReturnsDecryptedString_WhenGivenEncryptedString()
        {
            string decryptedText = testObject.Decrypt(encryptedString);

            Assert.Equal(decryptedString, decryptedText);
        }

        [Fact]
        public void Encrypt_ThrowsEncryptionException_WhenGivenEmptyString()
        {
            Assert.Throws<EncryptionException>(() => testObject.Encrypt(""));
        }

        [Fact]
        public void Decrypt_ThrowsEncryptionException_WhenGivenEmptyString()
        {
            Assert.Throws<EncryptionException>(() => testObject.Decrypt(""));
        }

    }
}