using System;
using System.Linq;
using Interapptive.Shared.Security.SecureTextVersions;
using log4net;
using Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace Interapptive.Shared.Tests.Security
{
    public class SecureTextVersion1Test
    {
        private readonly Mock<ILog> log;

        public SecureTextVersion1Test()
        {
            var mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            log = mock.Mock<ILog>();
        }

        [Fact]
        public void Encrypt_Creates_Random_Salt()
        {
            var plaintext = "This is a test";
            var password = "That Password";

            var testObject = new SecureTextVersion1(log.Object);

            var encrypted1 = Convert.FromBase64String(testObject.Encrypt(plaintext, password).Split(':')[0]);
            var firstSalt = encrypted1.Skip(encrypted1.Length - 16).Take(16);

            testObject.ClearCache();

            var encrypted2 = Convert.FromBase64String(testObject.Encrypt(plaintext, password).Split(':')[0]);
            var secondSalt = encrypted2.Skip(encrypted2.Length - 16).Take(16);

            Assert.NotEqual(firstSalt, secondSalt);
        }
    }
}
