using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataAccess
{
    public class OdbcUploadCommandFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private OdbcShipmentUploadStrategy uploadStrategyFromRepo;

        public OdbcUploadCommandFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            uploadStrategyFromRepo = OdbcShipmentUploadStrategy.UseImportDataSource;


            mock.Mock<IOdbcStoreRepository>()
                .Setup(r => r.GetStore(It.IsAny<OdbcStoreEntity>()))
                .Returns(() => new OdbcStore() { UploadStrategy = (int) uploadStrategyFromRepo });

            var dataSource = mock.Mock<IOdbcDataSource>();
            mock.Mock<IOdbcDataSourceService>()
                .Setup(s => s.GetDataSource(It.IsAny<string>()))
                .Returns(dataSource.Object);
        }

        [Fact]
        public void CreateUploadCommand_DelegatesToOdbcDataSourceService_WhenCreatingUploadCommand()
        {
            var connectionString = "encrypted connection string";

            var testObject = mock.Create<OdbcUploadCommandFactory>();

            OdbcStoreEntity store = new OdbcStoreEntity
            {
                UploadConnectionString = connectionString,
                UploadStrategy = (int) OdbcShipmentUploadStrategy.UseShipmentDataSource
            };
            testObject.CreateUploadCommand(store, new ShipmentEntity() { Order = new OrderEntity() });

            mock.Mock<IOdbcDataSourceService>().Verify(f => f.GetUploadDataSource(store, false));
        }

        [Fact]
        public void CreateUploadCommand_ReturnsOdbcUploadCommand()
        {
            var testObject = mock.Create<OdbcUploadCommandFactory>();

            IOdbcUploadCommand upload =
                testObject.CreateUploadCommand(
                    new OdbcStoreEntity { UploadStrategy = (int) OdbcShipmentUploadStrategy.UseImportDataSource },
                    new ShipmentEntity() { Order = new OrderEntity() });

            Assert.IsType<OdbcUploadCommand>(upload);
        }

        [Fact]
        public void CreateUploadCommand_ThrowsShipWorksOdbcException_WhenStoreUploadStrategyIsDoNotUpload()
        {
            uploadStrategyFromRepo = OdbcShipmentUploadStrategy.DoNotUpload;

            var testObject = mock.Create<OdbcUploadCommandFactory>();

            Assert.Throws<ShipWorksOdbcException>(
                () => testObject.CreateUploadCommand(new OdbcStoreEntity(), new ShipmentEntity() { Order = new OrderEntity() }));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}