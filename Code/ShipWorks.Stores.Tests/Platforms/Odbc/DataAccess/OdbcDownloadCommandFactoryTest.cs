using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataAccess
{
    public class OdbcDownloadCommandFactoryTest
    {
        [Fact]
        public void CreateDownloadCommand_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connectionString = "encrypted connection string";

                var dataSource = mock.Mock<IOdbcDataSource>();

                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcDownloadCommandFactory>();


                testObject.CreateDownloadCommand(new OdbcStoreEntity {ImportConnectionString = connectionString}, odbcFieldMap.Object);

                dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommand_ReturnsOdbcDownloadCommand()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();
                var testObject = mock.Create<OdbcDownloadCommandFactory>();

                IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity(), odbcFieldMap.Object);

                Assert.IsType<OdbcDownloadCommand>(downloadCommand);
            }
        }

        [Fact]
        public void CreateDownloadCommandWithDateTime_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connectionString = "encrypted connection string";
                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                var dataSource = mock.Mock<IOdbcDataSource>();

                var testObject = mock.Create<OdbcDownloadCommandFactory>();

                testObject.CreateDownloadCommand(new OdbcStoreEntity { ImportConnectionString = connectionString }, DateTime.UtcNow, odbcFieldMap.Object);

                dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommandWithDateTime_ReturnsOdbcDownloadCommand()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcDownloadCommandFactory>();
                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity(), DateTime.UtcNow, odbcFieldMap.Object);

                Assert.IsType<OdbcDownloadCommand>(downloadCommand);
            }
        }
    }
}
