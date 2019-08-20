﻿#region

using System.Data;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Tests.Shared;
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
        public void Read_DoesNotCallCheckForChangesNeeded_BecauseWeDontUseConfurationDataAnymore_WhenCustomerKeyDoesNotThrow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                SetupEncryption(mock);

                var dataReader = mock.CreateMock<IDataReader>();
                dataReader.Setup(d => d.GetString(0)).Returns(EncryptedCustomerKey);
                dataReader.SetupSequence(d => d.Read()).Returns(true);

                Mock<ISqlAdapter> sqlAdapter = mock.Mock<ISqlAdapter>();
                sqlAdapter.Setup(x => x.FetchDataReader(It.IsAny<ResultsetFields>(), null, CommandBehavior.CloseConnection, 0, null, true))
                    .Returns(dataReader.Object);

                mock.Mock<ISqlAdapterFactory>()
                    .Setup(x => x.Create()).Returns(sqlAdapter.Object);

                var config = mock.Mock<IConfigurationData>();
                config.Setup(c => c.FetchReadOnly())
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

                var dataReader = mock.CreateMock<IDataReader>();
                dataReader.Setup(d => d.GetString(0)).Returns(EncryptedCustomerKey);
                dataReader.SetupSequence(d => d.Read()).Returns(true);

                Mock<ISqlAdapter> sqlAdapter = mock.Mock<ISqlAdapter>();
                sqlAdapter.Setup(x => x.FetchDataReader(It.IsAny<ResultsetFields>(), null, CommandBehavior.CloseConnection, 0, null, true))
                    .Returns(dataReader.Object);

                mock.Mock<ISqlAdapterFactory>()
                    .Setup(x => x.Create()).Returns(sqlAdapter.Object);

                var testObject = mock.Create<CustomerLicenseReader>();

                Assert.Equal(DecryptedCustomerKey, testObject.Read());
            }
        }
    }
}