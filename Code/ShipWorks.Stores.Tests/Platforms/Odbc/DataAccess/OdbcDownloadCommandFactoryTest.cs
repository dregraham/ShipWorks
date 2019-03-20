using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataAccess
{
    public class OdbcDownloadCommandFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        
        public OdbcDownloadCommandFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateDownloadCommand_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            var connectionString = "encrypted connection string";

            var dataSource = mock.Mock<IOdbcDataSource>();

            Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

            var testObject = mock.Create<OdbcDownloadCommandFactory>();

            testObject.CreateDownloadCommand(new OdbcStoreEntity {ImportConnectionString = connectionString}, odbcFieldMap.Object);

            dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
        }

        [Fact]
        public void CreateDownloadCommand_ReturnsOdbcDownloadCommand()
        {
            Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();
            var testObject = mock.Create<OdbcDownloadCommandFactory>();

            IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity(), odbcFieldMap.Object);

            Assert.IsType<OdbcDownloadCommand>(downloadCommand);
        }

        [Fact]
        public void CreateDownloadCommandWithDateTime_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            var connectionString = "encrypted connection string";

            var dataSource = mock.Mock<IOdbcDataSource>();
            var odbcFieldMap = mock.Mock<IOdbcFieldMap>();

            var testObject = mock.Create<OdbcDownloadCommandFactory>();

            testObject.CreateDownloadCommand(new OdbcStoreEntity {ImportConnectionString = connectionString},
                                             DateTime.UtcNow, odbcFieldMap.Object);

            dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
        }

        [Fact]
        public void CreateDownloadCommandWithDateTime_ReturnsOdbcDownloadCommand()
        {
            var testObject = mock.Create<OdbcDownloadCommandFactory>();
            Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

            IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity(), DateTime.UtcNow, odbcFieldMap.Object);

            Assert.IsType<OdbcDownloadCommand>(downloadCommand);
        }

        [Fact]
        public void CreateDownloadCommandWithOrderNumber_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            var connectionString = "encrypted connection string";
            Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

            var dataSource = mock.Mock<IOdbcDataSource>();

            var testObject = mock.Create<OdbcDownloadCommandFactory>();

            testObject.CreateDownloadCommand(new OdbcStoreEntity {ImportConnectionString = connectionString}, "123", odbcFieldMap.Object);

            dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
        }

        [Fact]
        public void CreateDownloadCommandWithOrderNumber_ReturnsOdbcDownloadCommand()
        {
            var testObject = mock.Create<OdbcDownloadCommandFactory>();
            Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

            IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity(), "123", odbcFieldMap.Object);

            Assert.IsType<OdbcDownloadCommand>(downloadCommand);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
