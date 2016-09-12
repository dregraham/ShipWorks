﻿using Autofac.Extras.Moq;
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
            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();

            Assert.Throws<ArgumentNullException>(() => testObject.GetImportDataSource(null));
        }

        [Fact]
        public void CreateUploadDataSource_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();

            Assert.Throws<ArgumentNullException>(() => testObject.GetUploadDataSource(null));
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
            OdbcStoreEntity store = new OdbcStoreEntity() { UploadConnectionString = "foobarbaz", UploadStrategy = (int) OdbcShipmentUploadStrategy.UseShipmentDataSource};

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();

            testObject.GetUploadDataSource(store);

            dataSource.Verify(d => d.Restore(store.UploadConnectionString));
        }

        [Fact]
        public void CreateUploadDataSource_ReturnsNull_WhenStoreUploadStrategyIsDoNotUpload()
        {
            OdbcStoreEntity store = new OdbcStoreEntity() { UploadConnectionString = "foobarbaz", UploadStrategy = (int) OdbcShipmentUploadStrategy.DoNotUpload};

            OdbcDataSourceService testObject = mock.Create<OdbcDataSourceService>();
            IOdbcDataSource source = testObject.GetUploadDataSource(store);

            Assert.Null(source);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}