using System;
using Interapptive.Shared.Security;
using Xunit;

namespace ShipWorks.Tests.Core
{
    public class SecureTextTests
    {
        [Fact]
        public void EncryptDecrypt()
        {
            string original = "Abcd Efgh 12345 $%^*(";
            string salt = "This is the salt!";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.NotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, salt);

            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void EncryptDecryptDifferentSalt()
        {
            string original = "Abcd Efgh 12345 $%^*(";
            string salt = "This is the salt!";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.NotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, "whatever");

            Assert.Equal(string.Empty, decrypted);
        }

        [Fact]
        public void EncryptDecryptEmptySalt()
        {
            string original = "asdf $%^*(";
            string salt = "";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.NotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, salt);

            Assert.Equal(original, decrypted);
        }

        [Theory]
        [InlineData("tvM4uXTdWwhnI6zTNbZi+/QA/MSqvpic", "Encrypt me please!")]
        [InlineData("tvM4uXTdWwhnI6zTNbZi+/s3vpdLzn2iKFdQ+hwc7V/lCdBIxbGMjhF4Sfg28J+wjtPzDmd1E3do4SKLPEA+xIXUvKFVOb+UmuX2Z/VpU1jwDfnsUsYYctDJIz38Cy8N", "Encrypt me please! I'm a really long string that will hopefully cause a different exception.")]
        public void Decrypt_UsesRC2_WhenAesGcm_Fails(string encrypted, string expected)
        {
            var decrypted = SecureText.Decrypt(encrypted, "salt");

            Assert.Equal(expected, decrypted);
        }

        [Fact]
        public void EncryptNullSalt()
        {
            Assert.Throws<ArgumentNullException>(() => SecureText.Encrypt("", null));
        }

        [Fact]
        public void EncryptNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => SecureText.Encrypt(null, ""));
        }

        [Fact]
        public void DecryptNullSalt()
        {
            Assert.Throws<ArgumentNullException>(() => SecureText.Decrypt("", null));
        }

        [Fact]
        public void DecryptNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => SecureText.Decrypt(null, ""));
        }
    }
}
