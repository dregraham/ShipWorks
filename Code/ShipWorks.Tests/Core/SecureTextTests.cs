using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interapptive.Shared;
using System.Security.Cryptography;
using Interapptive.Shared.Utility;

namespace ShipWorks.Tests.Core
{
    [TestClass]
    public class SecureTextTests
    {
        [TestMethod]
        public void EncryptDecrypt()
        {
            string original = "Abcd Efgh 12345 $%^*(";
            string salt = "This is the salt!";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.AreNotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, salt);

            Assert.AreEqual(original, decrypted);
        }

        [TestMethod]
        public void EncryptDecryptDifferentSalt()
        {
            string original = "Abcd Efgh 12345 $%^*(";
            string salt = "This is the salt!";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.AreNotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, "whatever");

            Assert.AreEqual(string.Empty, decrypted);
        }

        [TestMethod]
        public void EncryptDecryptEmptySalt()
        {
            string original = "asdf $%^*(";
            string salt = "";

            string encrypted = SecureText.Encrypt(original, salt);

            Assert.AreNotEqual(original, encrypted);

            string decrypted = SecureText.Decrypt(encrypted, salt);

            Assert.AreEqual(original, decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullSalt()
        {
            SecureText.Encrypt("", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullValue()
        {
            SecureText.Encrypt(null, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNullSalt()
        {
            SecureText.Decrypt("", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNullValue()
        {
            SecureText.Decrypt(null, "");
        }
    }
}
