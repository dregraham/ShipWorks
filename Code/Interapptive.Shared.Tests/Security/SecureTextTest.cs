using Interapptive.Shared.Security;
using Xunit;

namespace Interapptive.Shared.Tests.Security
{
    public class SecureTextTest
    {
        [Fact]
        public void Encryption_Roundtrip_Succeeds()
        {
            var plaintext = "Encrypt me please!";

            var encrypted = SecureText.Encrypt(plaintext, "salt");
            var decrypted = SecureText.Decrypt(encrypted, "salt");

            Assert.Equal(plaintext, decrypted);
        }

        [Fact]
        public void Decrypt_UsesRC2_WhenAesGcm_Fails()
        {
            var encrypted = "tvM4uXTdWwhnI6zTNbZi+/QA/MSqvpic";

            var decrypted = SecureText.Decrypt(encrypted, "salt");

            Assert.Equal("Encrypt me please!", decrypted);
        }
    }
}
