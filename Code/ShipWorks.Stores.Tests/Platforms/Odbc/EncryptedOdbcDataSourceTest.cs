using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class EncryptedOdbcDataSourceTest
    {
        [Fact]
        public void Restore_DelegatesToDecrypt()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string json = "{\"Name\":\"Custom...\",\"Username\":null,\"Password\":null,\"IsCustom\":true,\"ConnectionString\":\"CustomConnectionString\"}";
                string encryptedJson = "encrypted";

                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(p => p.Decrypt(encryptedJson)).Returns(json);

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                EncryptedOdbcDataSource testObject = mock.Create<EncryptedOdbcDataSource>();

                testObject.Restore(encryptedJson);

                encryptionProvider.Verify(p=>p.Decrypt(encryptedJson));
            }
        }

        [Fact]
        public void Restore_NameIsSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string sourceName = "my data source name";
                string json = $"{{\"Name\":\"{sourceName}\",\"Username\":null,\"Password\":null,\"IsCustom\":true,\"ConnectionString\":\"CustomConnectionString\"}}";
                string encryptedJson = "encrypted";

                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(p => p.Decrypt(encryptedJson)).Returns(json);

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                EncryptedOdbcDataSource testObject = mock.Create<EncryptedOdbcDataSource>();

                testObject.Restore(encryptedJson);

                Assert.Equal(sourceName, testObject.Name);
            }
        }

        [Fact]
        public void Restore_ShipWorksOdbcExceptionThrown_WhenDecryptThrowsEncryptionException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string encryptedJson = "encrypted";

                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(p => p.Decrypt(encryptedJson)).Throws<EncryptionException>();

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                EncryptedOdbcDataSource testObject = mock.Create<EncryptedOdbcDataSource>();

                Assert.Throws<ShipWorksOdbcException>(() => testObject.Restore(encryptedJson));
            }
        }

        [Fact]
        public void Serialize_ReturnsEncryptedResults()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string encryptedJson = "encrypted";

                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(p => p.Encrypt(It.IsAny<string>())).Returns(encryptedJson);

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                var testObject = mock.Create<EncryptedOdbcDataSource>();

                string serializationResult = testObject.Serialize();

                Assert.Equal(encryptedJson, serializationResult);
            }
        }

    }
}
