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
        public void KeyCache_IsUnique_BetweenVersions()
        {
            var latestVersionEncrypted = SecureText.Encrypt("Some text", "salt");

            var latestVersionDecrypted = SecureText.Decrypt(latestVersionEncrypted, "salt");

            var version0Decrypted = SecureText.Decrypt("c6eW7VwJSF/sDIL93MhX6F0AR9VvouNC7mp9sdUvMYe7aimUcExSQvSlh0Rcm81OqVC+HRr67G945a7ugtsHZqfYXynzGpQwNSF86p5RjfuKDA67Doz3hRDjnv4MUWqw6vlyBqJztgqW2m4jHY5izMUSGY7w/NSJzw==", "salt");

            Assert.Equal("Some text", latestVersionDecrypted);
            Assert.Equal("String to Encrypt", version0Decrypted);
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
        [InlineData("c6eW7VwJSF/sDIL93MhX6F0AR9VvouNC7mp9sdUvMYe7aimUcExSQvSlh0Rcm81OqVC+HRr67G945a7ugtsHZqfYXynzGpQwNSF86p5RjfuKDA67Doz3hRDjnv4MUWqw6vlyBqJztgqW2m4jHY5izMUSGY7w/NSJzw==", "String to Encrypt")]
        public void Decrypt_DecryptsOldVersions(string encrypted, string expected)
        {
            var decrypted = SecureText.Decrypt(encrypted, "salt");

            Assert.Equal(expected, decrypted);
        }

        [Fact]
        public void Encrypt_AppendsCorrectVersion()
        {
            var encrypted = SecureText.Encrypt("Text to encrypt", "A password");

            var version = encrypted.Split(':');

            Assert.Equal("1", version[1]);
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
