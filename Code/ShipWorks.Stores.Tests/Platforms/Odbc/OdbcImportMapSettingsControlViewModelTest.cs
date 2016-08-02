using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcImportMapSettingsControlViewModelTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcImportMapSettingsControlViewModelTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void LoadMapSettings_SetsDownloadStrategyIsLastModified()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                ImportStrategy = (int)OdbcImportStrategy.ByModifiedTime,
                ImportColumnSourceType = (int)OdbcColumnSourceType.Table
            };

            OdbcImportMapSettingsControlViewModel testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.LoadMapSettings(store);

            Assert.True(testObject.DownloadStrategyIsLastModified);
        }

        [Fact]
        public void LoadMapSettings_SetsColumnSourceIsTable()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                ImportStrategy = (int)OdbcImportStrategy.ByModifiedTime,
                ImportColumnSourceType = (int)OdbcColumnSourceType.Table
            };

            OdbcImportMapSettingsControlViewModel testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.LoadMapSettings(store);

            Assert.True(testObject.ColumnSourceIsTable);
        }

        [Fact]
        public void SaveMapSettings_SetsStoreImportColumnSource()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                ImportStrategy = (int)OdbcImportStrategy.ByModifiedTime,
                ImportColumnSourceType = (int)OdbcColumnSourceType.Table
            };

            OdbcImportMapSettingsControlViewModel testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.ColumnSourceIsTable = false;
            testObject.SaveMapSettings(store);

            Assert.Equal((int) OdbcColumnSourceType.CustomQuery, store.ImportColumnSourceType);
        }

        [Fact]
        public void SaveMapSettings_SetsStoreImportStrategy()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                ImportStrategy = (int)OdbcImportStrategy.ByModifiedTime,
                ImportColumnSourceType = (int)OdbcColumnSourceType.Table
            };

            OdbcImportMapSettingsControlViewModel testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.DownloadStrategyIsLastModified = false;
            testObject.SaveMapSettings(store);

            Assert.Equal((int)OdbcImportStrategy.All, store.ImportStrategy);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}