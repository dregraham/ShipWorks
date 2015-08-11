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

            Assert.AreNotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, salt);

            Assert.AreEqual(original, decrypted);
        }

        [Fact]
        public void EncryptDecryptDifferentSalt()
        {
            string original = "Abcd Efgh 12345 $%^*(";
            string salt = "This is the salt!";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.AreNotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, "whatever");

            Assert.AreEqual(string.Empty, decrypted);
        }

        [Fact]
        public void EncryptDecryptEmptySalt()
        {
            string original = "asdf $%^*(";
            string salt = "";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.AreNotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, salt);

            Assert.AreEqual(original, decrypted);
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullSalt()
        {
            SecureText.Encrypt("", null);
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullValue()
        {
            SecureText.Encrypt(null, "");
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNullSalt()
        {
            SecureText.Decrypt("", null);
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNullValue()
        {
            SecureText.Decrypt(null, "");
        }
    }
}
