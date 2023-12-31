﻿using System;
using System.IO;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Upload;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Upload
{
    public class OdbcUploadMapSettingsControlViewModelTest : IDisposable
    {
        private const string DefaultFileName = "default file name";

        private readonly AutoMock mock;
        private readonly OdbcStore odbcStore;
        private OdbcStoreEntity store;

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
            "/* https://shipworks.zendesk.com/hc/en-us/articles/360022649411       */\n" +
            "/*                                                                    */\n" +
            "/**********************************************************************/\n\n" +
            "UPDATE ShipmentDetails\n" +
            "SET TrackingNumber = '{//TrackingNumber}'\n" +
            "WHERE OrderID = {//Order/Number}";

        public OdbcUploadMapSettingsControlViewModelTest()
        {
            mock = AutoMock.GetLoose();
            
            odbcStore = new OdbcStore();
            store = new OdbcStoreEntity();

            mock.Mock<IOdbcStoreRepository>()
                .Setup(r => r.GetStore(store))
                .Returns(odbcStore);
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

            store = new OdbcStoreEntity
            {
                UploadColumnSourceType = (int) OdbcColumnSourceType.Table
            };

            odbcStore.UploadColumnSourceType = (int) OdbcColumnSourceType.Table;

            mock.Mock<IOdbcStoreRepository>()
                .Setup(r => r.GetStore(store))
                .Returns(odbcStore);

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

            store = new OdbcStoreEntity()
            {
                UploadColumnSourceType = (int) OdbcColumnSourceType.Table
            };

            odbcStore.UploadColumnSourceType = (int) OdbcColumnSourceType.Table;

            mock.Mock<IOdbcStoreRepository>()
                .Setup(r => r.GetStore(store))
                .Returns(odbcStore);

            OdbcUploadMapSettingsControlViewModel testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.ColumnSourceIsTable = false;
            testObject.SaveMapSettings(store);

            Assert.Equal((int) OdbcColumnSourceType.CustomQuery, store.UploadColumnSourceType);
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

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", store);
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

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", store);
            testObject.ColumnSourceIsTable = true;

            func.Verify(f => f("OdbcCustomQueryWarningDlg"), Times.Never);
        }

        [Fact]
        public void SetColumnSourceIsTable_ColumnSourceIsSelectedTable_WhenTrue()
        {
            var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
            var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", store);

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

            testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", store);

            var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
            testObject.CustomQueryColumnSource = columnSourceMock.Object;

            testObject.ColumnSourceIsTable = false;

            Assert.Equal(testObject.CustomQueryColumnSource, testObject.ColumnSource);
        }

        [Fact]
        public void OpenMapSettingsFileCommand_DoesNotAttemptToReadStream_WhenUserCancels()
        {
            var dialogMock = MockOpenFileDialog(DialogResult.Abort, null);
            MockFieldMap();

            var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();

            testObject.OpenMapSettingsFileCommand.Execute(null);

            dialogMock.Verify(d => d.CreateFileStream(), Times.Never);
        }

        [Fact]
        public void OpenMapSettingsFileCommand_ReadsStreamFromDialog_WhenUserSelectsFile()
        {
            using (var stream = new MemoryStream())
            {
                var dialogMock = MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                dialogMock.Verify(d => d.CreateFileStream(), Times.Once);
            }
        }

        [Fact]
        public void OpenMapSettingsFileCommand_ReadsStreamFromDialog_LoadsUploadSettingsFileWithReaderFromDialog()
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);

                bool correctStreamUsed = false;
                int streamHashCode = stream.GetHashCode();
                settingsMock.Setup(s => s.Open(It.Is<StreamReader>(r => r.BaseStream.GetHashCode() == streamHashCode)))
                    .Callback(() => correctStreamUsed = true);

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.True(correctStreamUsed);
            }
        }

        [Theory]
        [InlineData(OdbcColumnSourceType.CustomQuery, false)]
        [InlineData(OdbcColumnSourceType.CustomParameterizedQuery, false)]
        [InlineData(OdbcColumnSourceType.Table, true)]
        public void OpenMapSettingsFileCommand_SetsColumnSourceIsTable(OdbcColumnSourceType sourceType, bool isTable)
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(sourceType);
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.Equal(testObject.ColumnSourceIsTable, isTable);
            }
        }

        [Fact]
        public void OpenMapSettingsFileCommand_SetsColumnSource()
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(OdbcColumnSourceType.Table);
                settingsMock.Setup(s => s.ColumnSource).Returns("a table");
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                var aTableColumnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
                aTableColumnSourceMock.Setup(x => x.Name).Returns("a table");
                var columnSourceFuncMock = mock.MockRepository.Create<Func<string, IOdbcColumnSource>>();
                columnSourceFuncMock.Setup(f => f("a table")).Returns(aTableColumnSourceMock.Object);
                mock.Provide(columnSourceFuncMock.Object);

                var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.Equal("a table", testObject.SelectedTable.Name);
            }
        }

        [Fact]
        public void OpenMapSettingsFileCommand_SetsCustomQuery()
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(OdbcColumnSourceType.CustomQuery);
                settingsMock.Setup(s => s.ColumnSource).Returns("my query");
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.Equal("my query", testObject.CustomQuery);
            }
        }

        [Fact]
        public void OpenMapSettingsFile_CallsUpgradeToAlphanumericOrderNumbers()
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(OdbcColumnSourceType.CustomQuery);
                settingsMock.Setup(s => s.ColumnSource).Returns("my query");
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                fieldMapMock.Verify(f => f.UpgradeToAlphanumericOrderNumbers(), Times.Once);
            }
        }

        [Fact]
        public void OpenMapSettingsFileCommand_FieldMapNameIsSetFromDisk()
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                Mock<IOdbcFieldMap> fieldMapFromDisk = mock.MockRepository.Create<IOdbcFieldMap>();
                fieldMapFromDisk.SetupGet(f => f.Name).Returns("name from disk");

                var settingsMock = mock.Mock<IOdbcSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(OdbcColumnSourceType.CustomQuery);
                settingsMock.Setup(s => s.ColumnSource).Returns("my query");
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapFromDisk.Object);
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                var testObject = mock.Create<OdbcUploadMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.Equal("name from disk", testObject.MapName);
            }
        }

        private Mock<IOdbcFieldMap> MockFieldMap()
        {
            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.SetupGet(f => f.Name).Returns(DefaultFileName);
            return fieldMap;
        }


        private Mock<IOpenFileDialog> MockOpenFileDialog(DialogResult result, MemoryStream stream)
        {
            var fileDialogMock = mock.CreateMock<IOpenFileDialog>(
                dialog =>
                {
                    dialog.Setup(d => d.ShowDialog()).Returns(result);
                    dialog.Setup(d => d.CreateFileStream()).Returns(stream);
                });

            mock.MockFunc(fileDialogMock);

            return fileDialogMock;
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
