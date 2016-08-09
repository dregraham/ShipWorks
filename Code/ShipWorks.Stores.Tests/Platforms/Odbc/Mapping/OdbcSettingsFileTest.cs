using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcSettingsFileTest : IDisposable
    {
        private AutoMock mock;
        private string defaultFileName = "Field map name";
        
        public OdbcSettingsFileTest()
        {
            mock = AutoMock.GetLoose();    
        }

        [Fact]
        public void GetStreamToSave_DefaultExtensionIsSetInFileDialog()
        {
            var mockDialog = MockDialog(FileDialogType.Save, DialogResult.Abort, null);

            var testObject = mock.Create<FakeOdbcSettingsFile>();

            testObject.OdbcFieldMap = MockFieldMap().Object;

            testObject.GetStreamToSave();

            mockDialog.VerifySet(d => d.DefaultExt = testObject.Extension);
        }

        [Fact]
        public void GetStreamToSave_FilterIsSetInFileDialog()
        {
            var mockDialog = MockDialog(FileDialogType.Save, DialogResult.Abort, null);

            var testObject = mock.Create<FakeOdbcSettingsFile>();
            testObject.OdbcFieldMap = MockFieldMap().Object;

            testObject.GetStreamToSave();

            mockDialog.VerifySet(d => d.Filter = $"ShipWorks ODBC {testObject.Action} Map (*{testObject.Extension})|*{testObject.Extension}");
        }

        [Fact]
        public void GetStreamToSave_DefaultFileNameIsSetInFileDialog()
        {
            var mockDialog = MockDialog(FileDialogType.Save, DialogResult.Abort, null);

            var testObject = mock.Create<FakeOdbcSettingsFile>();
            testObject.OdbcFieldMap = MockFieldMap().Object;

            testObject.GetStreamToSave();

            mockDialog.VerifySet(d => d.DefaultFileName = defaultFileName);
        }

        [Fact]
        public void GetStreamToSave_StreamNotRequested_WhenUserCancelsDialog()
        {
            var mockDialog = MockDialog(FileDialogType.Save, DialogResult.Abort, null);

            var testObject = mock.Create<FakeOdbcSettingsFile>();
            testObject.OdbcFieldMap = MockFieldMap().Object;

            testObject.GetStreamToSave();

            mockDialog.Verify(d=>d.CreateFileStream(), Times.Never);
        }

        [Fact]
        public void GetStreamToSave_StreamNotReturned_WhenUserCancelsDialog()
        {
            using (var stream = new MemoryStream())
            {
                MockDialog(FileDialogType.Save, DialogResult.Abort, stream);

                var testObject = mock.Create<FakeOdbcSettingsFile>();
                testObject.OdbcFieldMap = MockFieldMap().Object;

                Stream streamToSave = testObject.GetStreamToSave();

                Assert.Null(streamToSave);
            }
        }

        [Fact]
        public void GetStreamToSave_StreamRequested_WhenUserSelectsAFile()
        {
            using (var stream = new MemoryStream())
            {
                var mockDialog = MockDialog(FileDialogType.Save, DialogResult.OK, stream);

                var testObject = mock.Create<FakeOdbcSettingsFile>();
                testObject.OdbcFieldMap = MockFieldMap().Object;

                testObject.GetStreamToSave();

                mockDialog.Verify(d => d.CreateFileStream(), Times.Once);
            }
        }

        [Fact]
        public void GetStreamToSave_StreamReturned_WhenUserSelectsAFile()
        {
            using (var stream = new MemoryStream())
            {
                MockDialog(FileDialogType.Save, DialogResult.OK, stream);

                var testObject = mock.Create<FakeOdbcSettingsFile>();
                testObject.OdbcFieldMap = MockFieldMap().Object;

                Stream streamToSave = testObject.GetStreamToSave();

                Assert.Equal(stream, streamToSave);
            }
        }

        [Fact]
        public void Save_ColumnSourceTypeSavedToStream()
        {
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            {
                var testObject = mock.Create<FakeOdbcSettingsFile>();
                testObject.OdbcFieldMap = MockFieldMap().Object;
                testObject.ColumnSourceType = OdbcColumnSourceType.Table;

                testObject.Save(streamWriter);

                string savedJson = Encoding.UTF8.GetString(stream.ToArray());

                JObject jsonResults = JObject.Parse(savedJson);
                Assert.Equal(EnumHelper.GetApiValue(testObject.ColumnSourceType), jsonResults.GetValue("ColumnSourceType"));
            }
        }

        [Fact]
        public void Save_ColumnSourceSavedToStream()
        {
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            {
                var testObject = mock.Create<FakeOdbcSettingsFile>();
                testObject.OdbcFieldMap = MockFieldMap().Object;
                testObject.ColumnSource = "my source";

                testObject.Save(streamWriter);

                string savedJson = Encoding.UTF8.GetString(stream.ToArray());

                JObject jsonResults = JObject.Parse(savedJson);
                Assert.Equal(testObject.ColumnSource, jsonResults.GetValue("ColumnSource"));
            }
        }

        [Fact]
        public void Save_SerializedMapSavedToStream()
        {
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            {
                var testObject = mock.Create<FakeOdbcSettingsFile>();
                var mockFieldMap = MockFieldMap();

                string sampleJson = "{ \"Entries\":[{\"Index\":0}]}";

                mockFieldMap.Setup(m => m.Serialize()).Returns(sampleJson);

                testObject.OdbcFieldMap = mockFieldMap.Object;
                testObject.ColumnSource = "my source";

                testObject.Save(streamWriter);

                string savedJson = Encoding.UTF8.GetString(stream.ToArray());

                JObject jsonResults = JObject.Parse(savedJson);
                Assert.Equal(sampleJson, jsonResults.GetValue("FieldMap"));
            }
        }


        private Mock<IOdbcFieldMap> MockFieldMap()
        {
            Mock<IOdbcFieldMap> fieldMap = mock.MockRepository.Create<IOdbcFieldMap>();
            fieldMap.Setup(f => f.Name).Returns(defaultFileName);
            return fieldMap;
        }

        private Mock<IFileDialog> MockDialog(FileDialogType dialogType, DialogResult result, MemoryStream stream)
        {
            var fileDialogMock = mock.MockRepository.Create<IFileDialog>();
            var dialogIndex = mock.MockRepository.Create<IIndex<FileDialogType, IFileDialog>>();

            fileDialogMock.Setup(d => d.ShowDialog()).Returns(result);
            fileDialogMock.Setup(d => d.CreateFileStream()).Returns(stream);

            dialogIndex.Setup(i => i[dialogType]).Returns(fileDialogMock.Object);

            mock.Provide(dialogIndex.Object);

            return fileDialogMock;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}