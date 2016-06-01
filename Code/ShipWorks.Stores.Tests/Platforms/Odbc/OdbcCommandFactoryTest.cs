using System.Security;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcCommandFactoryTest
    {
        [Fact]
        public void CreateDownloadCommand_CreatesOdbcEncryptionProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                var testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(new OdbcStoreEntity());

                encryptionProviderFactory.Verify(factory => factory.CreateOdbcEncryptionProvider(), Times.Once);
            }
        }

        [Fact]
        public void CreateDownloadCommand_DecryptCalledWithStoreMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapText = "12345";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                var testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(new OdbcStoreEntity() {Map = mapText});

                encryptionProvider.Verify(p => p.Decrypt(It.Is<string>(s => s == mapText)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommand_LoadIsCalledWithDecryptedMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var decryptedMap = "I'm the map!";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(p => p.Decrypt(It.IsAny<string>()))
                    .Returns(decryptedMap);

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                var odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(new OdbcStoreEntity());

                odbcFieldMap.Verify(p => p.Load(It.Is<string>(s => s == decryptedMap)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommand_DatasourceRestoreCalledWithStoreConnectionString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connectionString = "connect with this";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                var dataSource = mock.Mock<IOdbcDataSource>();

                var testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(new OdbcStoreEntity {ConnectionString = connectionString});

                dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommand_ReturnsOdbcDownloadCommand()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();

                var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(f => f.CreateOdbcEncryptionProvider())
                    .Returns(encryptionProvider.Object);


                var testObject = mock.Create<OdbcCommandFactory>();

                IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity());

                Assert.IsType<OdbcDownloadCommand>(downloadCommand);
            }
        }
    }
}
