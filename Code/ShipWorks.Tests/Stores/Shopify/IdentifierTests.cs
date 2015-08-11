using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Tests.Stores.Shopify
{
    public class ApiTests
    {

        [Fact]
        public void GenEncryptedApiKeyPwd()
        {

            string encryptedKey = SecureText.Encrypt("36d5caf5fa4e17b35e915dbf61add688", "interapptive");
            string encryptedPwd = SecureText.Encrypt("7e7a6b34705570389dd9f6fe976ff7f2", "interapptive");

            string decryptedKey = SecureText.Decrypt(encryptedKey, "interapptive");
            string decryptedPwd = SecureText.Decrypt(encryptedPwd, "interapptive");

            Assert.AreEqual(decryptedKey, "36d5caf5fa4e17b35e915dbf61add688");
            Assert.AreEqual(decryptedPwd, "7e7a6b34705570389dd9f6fe976ff7f2");

        }

    }
}
