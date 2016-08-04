using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Upload;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Upload
{
    public class OdbcUploadMapSettingsControlViewModelTest
    {
        private readonly AutoMock mock;
        private const string InitialQueryComment =
            "/**********************************************************************/\n" +
            "/*                                                                    */\n" +
            "/* A sample query highlighting a few of the tokens that can be        */\n" +
            "/* used for uploading shipment details to your database has           */\n" +
            "/* been provided below.                                               */\n" +
            "/*                                                                    */\n" +
            "/* For more samples and additional information on how to              */\n" +
            "/* leverage ShipWorks tokens when uploading shipment details          */\n" +
            "/* using a custom query, please visit                                 */\n" +
            "/* http://support.shipworks.com/support/solutions/articles/4000085355 */\n" +
            "/*                                                                    */\n" +
            "/**********************************************************************/\n\n" +
            "UPDATE ShipmentDetails\n" +
            "SET TrackingNumber = '{//TrackingNumber}'\n" +
            "WHERE OrderID = {//Order/Number}";

        public OdbcUploadMapSettingsControlViewModelTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Constructor_SetsCustomQueryToInitial()
        {
            OdbcUploadMapSettingsControlViewModel testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            Assert.Equal(InitialQueryComment, testObject.CustomQuery);
        }

        [Fact]
        public void LoadMapSettings_SetsColumnSourceIsTable()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");

            OdbcStoreEntity store = new OdbcStoreEntity
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table
            };

            OdbcUploadMapSettingsControlViewModel testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.LoadMapSettings(store);

            Assert.True(testObject.ColumnSourceIsTable);
        }

        [Fact]
        public void SaveMapSettings_SetsStoreUploadColumnSource()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table
            };

            OdbcUploadMapSettingsControlViewModel testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.ColumnSourceIsTable = false;
            testObject.SaveMapSettings(store);

            Assert.Equal((int)OdbcColumnSourceType.CustomQuery, store.UploadColumnSourceType);
        }

        [Fact]
        public void SaveMapSettings_SetsStoreUploadColumnSourceType()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table
            };

            OdbcUploadMapSettingsControlViewModel testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.ColumnSourceIsTable = false;
            testObject.SaveMapSettings(store);

            Assert.Equal((int)OdbcColumnSourceType.CustomQuery, store.UploadColumnSourceType);
        }

        [Fact]
        public void SetColumnSourceIsTable_ShowDialog_WhenFalse()
        {
            var mockDialog = mock.Mock<IDialog>();

            var func = mock.MockRepository.Create<Func<string, IDialog>>();
            func.Setup(x => x(It.IsAny<string>())).Returns(mockDialog.Object);
            mock.Provide(func.Object);

            var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();


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

            var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());
            testObject.ColumnSourceIsTable = true;

            func.Verify(f => f("OdbcCustomQueryWarningDlg"), Times.Never);
        }

        [Fact]
        public void SetColumnSourceIsTable_ColumnSourceIsSelectedTable_WhenTrue()
        {
            var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

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

            var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

            var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
            testObject.CustomQueryColumnSource = columnSourceMock.Object;

            testObject.ColumnSourceIsTable = false;

            Assert.Equal(testObject.CustomQueryColumnSource, testObject.ColumnSource);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}