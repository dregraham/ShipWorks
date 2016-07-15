using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using System;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataSource
{
    public class OdbcDataSourceFactoryTest : IDisposable
    {
        private readonly AutoMock mock = AutoMock.GetLoose();

        [Fact]
        public void CreateImportDataSource_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            OdbcDataSourceFactory testObject = mock.Create<OdbcDataSourceFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.CreateImportDataSource(null));
        }

        [Fact]
        public void CreateUploadDataSource_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            OdbcDataSourceFactory testObject = mock.Create<OdbcDataSourceFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.CreateUploadDataSource(null));
        }

        [Fact]
        public void CreateImportDataSource_CallsRestoreOnDataSourceWithStoreImportConnectionString()
        {
            OdbcStoreEntity store = new OdbcStoreEntity() {ImportConnectionString = "foobarbaz"};

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            OdbcDataSourceFactory testObject = mock.Create<OdbcDataSourceFactory>();

            testObject.CreateImportDataSource(store);

            dataSource.Verify(d => d.Restore(store.ImportConnectionString));
        }

        [Fact]
        public void CreateUploadDataSource_CallsRestoreOnDataSourceWithStoreUploadConnectionString_WhenStoreUploadStrategyIsUseUploadConnectionString()
        {
            OdbcStoreEntity store = new OdbcStoreEntity() { UploadConnectionString = "foobarbaz", UploadStrategy = (int) OdbcShipmentUploadStrategy.UseShipmentDataSource};

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            OdbcDataSourceFactory testObject = mock.Create<OdbcDataSourceFactory>();

            testObject.CreateUploadDataSource(store);

            dataSource.Verify(d => d.Restore(store.UploadConnectionString));
        }

        [Fact]
        public void CreateUploadDataSource_ReturnsNull_WhenStoreUploadStrategyIsDoNotUpload()
        {
            OdbcStoreEntity store = new OdbcStoreEntity() { UploadConnectionString = "foobarbaz", UploadStrategy = (int) OdbcShipmentUploadStrategy.DoNotUpload};

            OdbcDataSourceFactory testObject = mock.Create<OdbcDataSourceFactory>();
            IOdbcDataSource source = testObject.CreateUploadDataSource(store);

            Assert.Null(source);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}