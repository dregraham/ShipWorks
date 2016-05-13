using Interapptive.Shared.Security;
using Xunit;

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

            Assert.Equal(decryptedKey, "36d5caf5fa4e17b35e915dbf61add688");
            Assert.Equal(decryptedPwd, "7e7a6b34705570389dd9f6fe976ff7f2");
        }
    }
}
