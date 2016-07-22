using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataAccess
{
    public class OdbcUploadCommandFactoryTest
    {
        [Fact]
        public void CreateUploadCommand_DatasourceRestoreCalledWithEncryptedStoreConnectionString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connectionString = "encrypted connection string";

                var dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcUploadCommandFactory>();

                testObject.CreateUploadCommand(
                    new OdbcStoreEntity
                    {
                        UploadConnectionString = connectionString,
                        UploadStrategy = (int)OdbcShipmentUploadStrategy.UseShipmentDataSource
                    }, odbcFieldMap.Object);

                dataSource.Verify(p => p.Restore(It.Is<string>(s => s == connectionString)), Times.Once());
            }
        }

        [Fact]
        public void CreateUploadCommand_ReturnsOdbcUploadCommand()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcUploadCommandFactory>();

                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                IOdbcUploadCommand downloadCommand =
                    testObject.CreateUploadCommand(
                        new OdbcStoreEntity { UploadStrategy = (int)OdbcShipmentUploadStrategy.UseImportDataSource },
                        odbcFieldMap.Object);

                Assert.IsType<OdbcUploadCommand>(downloadCommand);
            }
        }

        [Fact]
        public void CreateUploadCommand_ThrowsShipWorksOdbcException_WhenStoreUploadStrategyIsDoNotUpload()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcUploadCommandFactory>();

                Mock<IOdbcFieldMap> odbcFieldMap = mock.Mock<IOdbcFieldMap>();

                Assert.Throws<ShipWorksOdbcException>(
                    () => testObject.CreateUploadCommand(new OdbcStoreEntity(), odbcFieldMap.Object));

            }
        }
    }
}