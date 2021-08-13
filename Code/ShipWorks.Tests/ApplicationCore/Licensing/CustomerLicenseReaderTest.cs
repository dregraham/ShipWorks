﻿#region

using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

#endregion

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class CustomerLicenseReaderTest
    {
        const string EncryptedCustomerKey = "encrypted customer key";
        const string DecryptedCustomerKey = "decrypted customer key";

        /// <summary>
        /// Setups the encrypter. Expects EncryptedCustomerKey field and returns DecryptedCustomerKey field
        /// </summary>
        private static void SetupEncryption(AutoMock mock)
        {
            var encryptionProvider = mock.MockRepository.Create<IEncryptionProvider>();
            encryptionProvider.Setup(p => p.Decrypt(It.Is<string>(s => s == EncryptedCustomerKey)))
                .Returns(DecryptedCustomerKey);

            mock.Mock<IEncryptionProviderFactory>()
                .Setup(f => f.CreateLicenseEncryptionProvider())
                .Returns(encryptionProvider.Object);
        }

        [Fact]
        public void Read_DoesNotCallCheckForChangesNeeded_WhenCustomerKeyDoesNotThrow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                SetupEncryption(mock);

                var config = mock.Mock<IConfigurationData>();
                config.Setup(c => c.FetchCustomerKey(CustomerLicenseKeyType.WebReg))
                    .Returns(EncryptedCustomerKey);

                var testObject = mock.Create<CustomerLicenseReader>();

                testObject.Read();

                config.Verify(c => c.CheckForChangesNeeded(), Times.Never);
            }
        }

        [Fact]
        public void Read_ReturnsDecryptedCustomerKey()
        {
            using (var mock = AutoMock.GetLoose())
            {
                SetupEncryption(mock);

                mock.Mock<IConfigurationData>()
                    .Setup(c => c.FetchCustomerKey(CustomerLicenseKeyType.WebReg))
                    .Returns(EncryptedCustomerKey);

                var testObject = mock.Create<CustomerLicenseReader>();

                Assert.Equal(DecryptedCustomerKey, testObject.Read());
            }
        }
    }
}