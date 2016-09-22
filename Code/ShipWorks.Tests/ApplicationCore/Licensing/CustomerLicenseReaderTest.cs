#region

using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
        public void Read_CallsCheckForChangesNeeded_WhenReadingCustomerKeyThrowsOutOfSyncException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                SetupEncryption(mock);

                var outOfSyncEntity = mock.MockRepository.Create<ConfigurationEntity>();
                outOfSyncEntity.Setup(c => c.CustomerKey)
                    .Throws(new ORMEntityOutOfSyncException("out of sync", outOfSyncEntity.Object));

                var configurationData = mock.Mock<IConfigurationData>();
                configurationData.SetupSequence(c => c.Fetch())
                    .Returns(outOfSyncEntity.Object)
                    .Returns(new ConfigurationEntity { CustomerKey = EncryptedCustomerKey });


                var testObject = mock.Create<CustomerLicenseReader>();
                testObject.Read();

                configurationData.Verify(c => c.CheckForChangesNeeded(), Times.AtLeastOnce);
            }
        }

        [Fact]
        public void Read_DoesNotCAllCheckForChangesNeeded_WhenCustomerKeyDoesNotThrow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                SetupEncryption(mock);

                var config = mock.Mock<IConfigurationData>();
                config.Setup(c => c.Fetch())
                    .Returns(new ConfigurationEntity { CustomerKey = EncryptedCustomerKey });

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
                    .Setup(c => c.Fetch())
                    .Returns(new ConfigurationEntity { CustomerKey = EncryptedCustomerKey });

                var testObject = mock.Create<CustomerLicenseReader>();

                Assert.Equal(DecryptedCustomerKey, testObject.Read());
            }
        }
    }
}