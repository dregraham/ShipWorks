using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using System;
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

        [Fact]
        public void SetColumnSourceIsTable_ShowDialog_WhenFalse()
        {
            var mockDialog = mock.Mock<IDialog>();

            var func = mock.MockRepository.Create<Func<string, IDialog>>();
            func.Setup(x => x(It.IsAny<string>())).Returns(mockDialog.Object);
            mock.Provide(func.Object);

            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();


            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());
            testObject.ColumnSourceIsTable = false;

            func.Verify(f => f("OdbcCustomQueryWarningDlg"), Times.Once);
            mockDialog.Verify(d => d.ShowDialog(), Times.Once);
        }

        [Fact]
        public void SetColumnSourceIsTable_DoNotShowDialog_WhenTrue()
        {
            var func = mock.MockRepository.Create<Func<string, IDialog>>();

            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());
            testObject.ColumnSourceIsTable = true;

            func.Verify(f => f("OdbcCustomQueryWarningDlg"), Times.Never);
        }

        [Fact]
        public void SetColumnSourceIsTable_ColumnSourceIsSelectedTable_WhenTrue()
        {
            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

            var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
            testObject.SelectedTable = columnSourceMock.Object;

            testObject.ColumnSourceIsTable = true;

            Assert.Equal(testObject.SelectedTable, testObject.ColumnSource);
        }

        [Fact]
        public void SetColumnSourceIsTable_ColumnSourceIsCustomQueryColumnSource_WhenFalse()
        {
            var mockDialog = mock.Mock<IDialog>();

            var func = mock.MockRepository.Create<Func<string, IDialog>>();
            func.Setup(x => x(It.IsAny<string>())).Returns(mockDialog.Object);
            mock.Provide(func.Object);

            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

            var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
            testObject.CustomQueryColumnSource = columnSourceMock.Object;

            testObject.ColumnSourceIsTable = false;

            Assert.Equal(testObject.CustomQueryColumnSource, testObject.ColumnSource);
        }

        [Fact]
        public void SetColumnSource_SetsMapName_WhenMapnameIsDatasourceName_AndNotNull()
        {
            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            string dataSourceName = "ds";
            dataSourceMock.Setup(m => m.Name).Returns(dataSourceName);

            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
            string columnSourceName = "column source name";
            columnSourceMock.SetupGet(s => s.Name).Returns(columnSourceName);

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

            testObject.MapName = "ds";

            testObject.ColumnSource = columnSourceMock.Object;

            Assert.Equal($"{dataSourceName} - {columnSourceName}", testObject.MapName);
        }

        [Fact]
        public void SetColumnSource_SetsMapName_WhenMapnameIsDatasourceName_AndNull()
        {
            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            string dataSourceName = "ds";
            dataSourceMock.Setup(m => m.Name).Returns(dataSourceName);

            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
            string columnSourceName = "column source name";
            columnSourceMock.SetupGet(s => s.Name).Returns(columnSourceName);

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

            testObject.MapName = "ds";

            testObject.ColumnSource = columnSourceMock.Object;

            Assert.Equal($"{dataSourceName} - {columnSourceName}", testObject.MapName);

            testObject.ColumnSource = null;

            Assert.Equal($"{dataSourceName}", testObject.MapName);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}