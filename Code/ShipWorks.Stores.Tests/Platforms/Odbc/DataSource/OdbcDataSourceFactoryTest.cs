using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
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
        public void CreateImportDataSource_CallsRestoreOnDataSourceWithStoreConnectionString()
        {
            var store = new OdbcStoreEntity() {ConnectionString = "foobarbaz"};

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            OdbcDataSourceFactory testObject = mock.Create<OdbcDataSourceFactory>();

            testObject.CreateImportDataSource(store);

            dataSource.Verify(d => d.Restore(store.ConnectionString));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}