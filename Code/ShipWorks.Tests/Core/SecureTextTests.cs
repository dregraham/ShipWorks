using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Interapptive.Shared;
using System.Security.Cryptography;
using Interapptive.Shared.Utility;

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
