using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using System;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using Xunit;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataSource
{
    public class OdbcDataSourceFactoryTest : IDisposable
    {
        private readonly AutoMock mock = AutoMock.GetLoose();

        [Fact]
        public void CreateImportDataSource_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();

            Assert.Throws<ArgumentNullException>(() => testObject.GetImportDataSource(null));
        }

        [Fact]
        public void CreateUploadDataSource_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();

            Assert.Throws<ArgumentNullException>(() => testObject.GetUploadDataSource(null, false));
        }

        [Fact]
        public void CreateImportDataSource_CallsRestoreOnDataSourceWithStoreImportConnectionString()
        {
            OdbcStoreEntity store = new OdbcStoreEntity() {ImportConnectionString = "foobarbaz"};

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();

            testObject.GetImportDataSource(store);

            dataSource.Verify(d => d.Restore(store.ImportConnectionString));
        }

        [Fact]
        public void CreateUploadDataSource_CallsRestoreOnDataSourceWithStoreUploadConnectionString_WhenStoreUploadStrategyIsUseUploadConnectionString()
        {
            OdbcStoreEntity storeEntity = new OdbcStoreEntity() { UploadConnectionString = "foobarbaz", UploadStrategy = (int) OdbcShipmentUploadStrategy.UseShipmentDataSource};

            OdbcStore store = new OdbcStore();
            store.UploadStrategy = storeEntity.UploadStrategy;
            var storeRepo = mock.Mock<IOdbcStoreRepository>();

            storeRepo.Setup(s => s.GetStore(storeEntity)).Returns(store);

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();

            testObject.GetUploadDataSource(storeEntity, false);

            dataSource.Verify(d => d.Restore(storeEntity.UploadConnectionString));
        }

        [Fact]
        public void CreateUploadDataSource_ReturnsNull_WhenStoreUploadStrategyIsDoNotUpload()
        {
            OdbcStoreEntity storeEntity = new OdbcStoreEntity() { UploadConnectionString = "foobarbaz", UploadStrategy = (int) OdbcShipmentUploadStrategy.DoNotUpload};

            OdbcStore store = new OdbcStore();
            store.UploadStrategy = storeEntity.UploadStrategy;
            var storeRepo = mock.Mock<IOdbcStoreRepository>();

            storeRepo.Setup(s => s.GetStore(storeEntity)).Returns(store);

            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();
            IOdbcDataSource source = testObject.GetUploadDataSource(storeEntity, false);

            Assert.Null(source);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}