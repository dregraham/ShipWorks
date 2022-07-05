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
        [InlineData("6eV2gjBGo3e16RbFaX7o9SLzfFumOnfisqGi3JabJxcUiFHpBhg5QXWaXGYSCXe/NZN5Y3urGj6GxxcLvCyBy8+8S/K/KYWRylvcx+Dg12DDXXv87iMrXqn7Gg+Ww5HE88BG7bq+/hiFJYFUizh/G4+VTNcW0g==:1", "This is a test")]
        [InlineData("usZPIRnc/ZgE1PatujgoOqU/ZnKx9TWrDHT9co6YAWiaL+4Q6+vqpj84yDdoTsfSSwNfJvXNv1Pm7qd3dBCbDHdy9IyTb+HX8241InWPvx5VHI6pVm7lsoOQ7nFlyq/pp215mS/5LeQAF1bHJ8rjj+k=:2", "Some text")]
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

            Assert.Equal("2", version[1]);
        }

        [Fact]
        public void Test()
        {
            var decrypted = SecureText.Decrypt("TghC31DQgjc6oEJyoR9wfRYq59DmF5NkHz1YTsYSsKBBobcnF13tc/1CE5/Lfeq+YP3XPEjLbVa/gS/lFUQQLQpRfP5rJzj5JndHJGKWM+aIbDOR8HL+irEL0eCHN+dmTj+ENzJdWE7AXqUDl64eZ2X9pMozh1vr:1", "UPS");
        }

        [Fact]
        public void DecryptWorks_AfterClearingCache()
        {
            // There used to be an issue where we would cache a key for a given password
            // but when encrypting, we would store a newly generated salt, instead of the
            // salt for the cached key. This would result in decrypt failing after the cache was cleared

            string password = "thePassword";

            string firstTest = "blah";
            string secondTest = "thing";

            string encrypted1 = SecureText.Encrypt(firstTest, password);
            string encrypted2 = SecureText.Encrypt(secondTest, password);
            string decrypted1 = SecureText.Decrypt(encrypted1, password);

            SecureText.ClearCache();

            string decrypted2 = SecureText.Decrypt(encrypted2, password);

            Assert.Equal(firstTest, decrypted1);
            Assert.Equal(secondTest, decrypted2);
        }

        [Fact]
        public void EncryptNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => SecureText.Encrypt(null, ""));
        }

        [Fact]
        public void DecryptNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => SecureText.Decrypt(null, ""));
        }
    }
}
