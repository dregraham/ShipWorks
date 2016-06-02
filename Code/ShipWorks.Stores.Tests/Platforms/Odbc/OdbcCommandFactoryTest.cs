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
        public void CreateDownloadCommand_LoadIsCalledWithStoreMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcStoreEntity odbcStore = new OdbcStoreEntity()
                {
                    Map = "I Am A Map"
                };

                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();
                OdbcCommandFactory testObject = mock.Create<OdbcCommandFactory>();
                testObject.CreateDownloadCommand(odbcStore);

                odbcFieldMap.Verify(p => p.Load(It.Is<string>(s => s == odbcStore.Map)), Times.Once());
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
