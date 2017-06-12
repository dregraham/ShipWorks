using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Import
{
    public class OdbcImportMapSettingsControlViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private const string DefaultFileName = "default file name";

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
        public void ValidateREquiredMapSettings_ReturnsFalse_WhenSampleDataCommandThrowsShipWorksOdbcException()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");
            Mock<IOdbcSampleDataCommand> sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
            sampleDataCommand.Setup(s => s.Execute(It.IsAny<IOdbcDataSource>(), It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new ShipWorksOdbcException("Something went wrong"));

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                ImportStrategy = (int)OdbcImportStrategy.ByModifiedTime,
                ImportColumnSourceType = (int)OdbcColumnSourceType.CustomQuery
            };

            OdbcImportMapSettingsControlViewModel testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.CustomQuery = "select *";
            testObject.ColumnSourceIsTable = false;
            testObject.SaveMapSettings(store);

            Assert.False(testObject.ValidateRequiredMapSettings());
        }

        [Fact]
        public void ValidateREquiredMapSettings_ReturnsTrue_WhenQueryIsValid()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
            columnSource.Setup(c => c.Name).Returns("Orders");
            Mock<IOdbcSampleDataCommand> sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
            sampleDataCommand.Setup(s => s.Execute(It.IsAny<IOdbcDataSource>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new DataTable());

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                ImportStrategy = (int)OdbcImportStrategy.ByModifiedTime,
                ImportColumnSourceType = (int)OdbcColumnSourceType.CustomQuery
            };

            OdbcImportMapSettingsControlViewModel testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.Load(dataSource.Object, schema.Object, "ColumnSource", store);
            testObject.CustomQuery = "select *";
            testObject.ColumnSourceIsTable = false;
            testObject.SaveMapSettings(store);

            Assert.True(testObject.ValidateRequiredMapSettings());
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

        [Fact]
        public void OpenMapSettingsFileCommand_SetsDefaultExtension()
        {
            var importSettingsFileMock = mock.Mock<IOdbcImportSettingsFile>();
            importSettingsFileMock.Setup(s => s.Extension).Returns(".blah");

            var dialogMock = MockOpenFileDialog(DialogResult.Abort, null);
            MockFieldMap();

            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.OpenMapSettingsFileCommand.Execute(null);

            dialogMock.VerifySet(d => d.DefaultExt = ".blah");
        }

        [Fact]
        public void OpenMapSettingsFileCommand_SetsDefaultFilter()
        {
            var importSettingsFileMock = mock.Mock<IOdbcImportSettingsFile>();
            importSettingsFileMock.Setup(s => s.Filter).Returns("filter");

            var dialogMock = MockOpenFileDialog(DialogResult.Abort, null);
            MockFieldMap();

            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.OpenMapSettingsFileCommand.Execute(null);

            dialogMock.VerifySet(d => d.Filter = "filter");
        }

        [Fact]
        public void OpenMapSettingsFileCommand_SetsDefaultFilename_FromOdbcMapName()
        {
            var dialogMock = MockOpenFileDialog(DialogResult.Abort, null);
            MockFieldMap();

            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

            testObject.OpenMapSettingsFileCommand.Execute(null);

            dialogMock.VerifySet(d => d.DefaultFileName = DefaultFileName);
        }

        [Fact]
        public void OpenMapSettingsFileCommand_DoesNotAttemptToReadStream_WhenUserCancels()
        {
            var dialogMock = MockOpenFileDialog(DialogResult.Abort, null);
            MockFieldMap();

            var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();

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

                var settingsMock = mock.Mock<IOdbcImportSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                OdbcStoreEntity store = new OdbcStoreEntity()
                {
                    ImportStrategy = (int)OdbcImportStrategy.ByModifiedTime,
                    ImportColumnSourceType = (int)OdbcColumnSourceType.Table
                };

                var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);
                
                testObject.OpenMapSettingsFileCommand.Execute(null);

                dialogMock.Verify(d => d.CreateFileStream(), Times.Once);
            }
        }

        [Fact]
        public void OpenMapSettingsFileCommand_ReadsStreamFromDialog_LoadsImportSettingsFileWithReaderFromDialog()
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcImportSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);

                bool correctStreamUsed = false;
                int streamHashCode = stream.GetHashCode();
                settingsMock.Setup(s => s.Open(It.Is<StreamReader>(r => r.BaseStream.GetHashCode() == streamHashCode)))
                    .Callback(() => correctStreamUsed = true);

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");

                OdbcStoreEntity store = new OdbcStoreEntity();

                var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.True(correctStreamUsed);
            }
        }

        [Theory]
        [InlineData(OdbcImportStrategy.All, false)]
        [InlineData(OdbcImportStrategy.ByModifiedTime, true)]
        public void OpenMapSettingsFileCommand_SetsDownloadStrategyIsLastModified(OdbcImportStrategy strategy, bool isLastModified)
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcImportSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.OdbcImportStrategy).Returns(strategy);
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");
                OdbcStoreEntity store = new OdbcStoreEntity();

                var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.Equal(testObject.DownloadStrategyIsLastModified, isLastModified);
            }
        }

        [Theory]
        [InlineData(OdbcColumnSourceType.CustomQuery, false)]
        [InlineData(OdbcColumnSourceType.Table, true)]
        public void OpenMapSettingsFileCommand_SetsColumnSourceIsTable(OdbcColumnSourceType sourceType, bool isTable)
        {
            using (var stream = new MemoryStream())
            {
                MockOpenFileDialog(DialogResult.OK, stream);
                var fieldMapMock = MockFieldMap();

                var settingsMock = mock.Mock<IOdbcImportSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(sourceType);
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.Mock<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");
                OdbcStoreEntity store = new OdbcStoreEntity();

                var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();
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

                var settingsMock = mock.Mock<IOdbcImportSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(OdbcColumnSourceType.Table);
                settingsMock.Setup(s => s.ColumnSource).Returns("a table");
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");
                OdbcStoreEntity store = new OdbcStoreEntity();

                var aTableColumnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
                aTableColumnSourceMock.Setup(x => x.Name).Returns("a table");
                var columnSourceFuncMock = mock.MockRepository.Create<Func<string, IOdbcColumnSource>>();
                columnSourceFuncMock.Setup(f => f("a table")).Returns(aTableColumnSourceMock.Object);
                mock.Provide(columnSourceFuncMock.Object);

                var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();
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

                var settingsMock = mock.Mock<IOdbcImportSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(OdbcColumnSourceType.CustomQuery);
                settingsMock.Setup(s => s.ColumnSource).Returns("my query");
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");
                OdbcStoreEntity store = new OdbcStoreEntity();

                var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();
                testObject.Load(dataSource.Object, schema.Object, "source", store);

                testObject.OpenMapSettingsFileCommand.Execute(null);

                Assert.Equal("my query", testObject.CustomQuery);
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

                var settingsMock = mock.Mock<IOdbcImportSettingsFile>();
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapMock.Object);
                settingsMock.Setup(s => s.ColumnSourceType).Returns(OdbcColumnSourceType.CustomQuery);
                settingsMock.Setup(s => s.ColumnSource).Returns("my query");
                settingsMock.Setup(s => s.OdbcFieldMap).Returns(fieldMapFromDisk.Object);
                settingsMock.Setup(s => s.Open(It.IsAny<TextReader>())).Returns(GenericResult.FromSuccess(new JObject()));

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcColumnSource> columnSource = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSource.Setup(c => c.Name).Returns("Orders");
                OdbcStoreEntity store = new OdbcStoreEntity();

                var testObject = mock.Create<OdbcImportMapSettingsControlViewModel>();
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
            var fileDialogMock = mock.CreateMock<IOpenFileDialog>(sd =>
            {
                sd.Setup(d => d.ShowDialog()).Returns(result);
                sd.Setup(d => d.CreateFileStream()).Returns(stream);
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