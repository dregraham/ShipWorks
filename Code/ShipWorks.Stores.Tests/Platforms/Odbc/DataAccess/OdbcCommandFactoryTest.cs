﻿using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
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
                    ImportMap = "I Am A Map"
                };

                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                OdbcCommandFactory testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(odbcStore);

                odbcFieldMap.Verify(p => p.Load(It.Is<string>(s => s == odbcStore.ImportMap)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommand_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connectionString = "encrypted connection string";

                var dataSource = mock.Mock<IOdbcDataSource>();

                var testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(new OdbcStoreEntity {ImportConnectionString = connectionString});

                dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommand_ReturnsOdbcDownloadCommand()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcCommandFactory>();

                IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity());

                Assert.IsType<OdbcDownloadCommand>(downloadCommand);
            }
        }

        [Fact]
        public void CreateDownloadCommandWithDateTime_LoadIsCalledWithStoreMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcStoreEntity odbcStore = new OdbcStoreEntity()
                {
                    ImportMap = "I Am A Map"
                };

                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                OdbcCommandFactory testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(odbcStore, DateTime.UtcNow);

                odbcFieldMap.Verify(p => p.Load(It.Is<string>(s => s == odbcStore.ImportMap)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommandWithDateTime_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connectionString = "encrypted connection string";

                var dataSource = mock.Mock<IOdbcDataSource>();

                var testObject = mock.Create<OdbcCommandFactory>();

                testObject.CreateDownloadCommand(new OdbcStoreEntity { ImportConnectionString = connectionString }, DateTime.UtcNow);

                dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
            }
        }

        [Fact]
        public void CreateDownloadCommandWithDateTime_ReturnsOdbcDownloadCommand()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcCommandFactory>();

                IOdbcCommand downloadCommand = testObject.CreateDownloadCommand(new OdbcStoreEntity(), DateTime.UtcNow);

                Assert.IsType<OdbcDownloadCommand>(downloadCommand);
            }
        }
    }
}
