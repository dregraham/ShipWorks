﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcSettingsFileTest : IDisposable
    {
        private readonly AutoMock mock;
        private const string DefaultFileName = "Field map name";

        public OdbcSettingsFileTest()
        {
            mock = AutoMock.GetLoose();
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

        [Fact]
        public void Open_SetsColumnSourceType_WhenDialogResultIsOK()
        {
            using (Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(
                "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapSavedToDisk.json"))
            using (var streamReader = new StreamReader(stream))
            {
                var ioFactory = mock.Create<JsonOdbcFieldMapIOFactory>();
                var map = mock.Create<OdbcFieldMap>(new TypedParameter(typeof(IOdbcFieldMapIOFactory), ioFactory));

                MockOpenFileDialog(DialogResult.OK, stream);

                var testObject = mock.Create<FakeOdbcSettingsFile>(new TypedParameter(typeof(IOdbcFieldMap), map));
                testObject.Open(streamReader);

                Assert.Equal(OdbcColumnSourceType.Table, testObject.ColumnSourceType);
            }
        }

        [Fact]
        public void Open_SetsColumnSource_WhenDialogResultIsOK()
        {
            using (Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(
                    "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapSavedToDisk.json"))
            using (var streamReader = new StreamReader(stream))
            {
                var ioFactory = mock.Create<JsonOdbcFieldMapIOFactory>();
                var map = mock.Create<OdbcFieldMap>(new TypedParameter(typeof(IOdbcFieldMapIOFactory), ioFactory));

                MockOpenFileDialog(DialogResult.OK, stream);

                var testObject = mock.Create<FakeOdbcSettingsFile>(new TypedParameter(typeof(IOdbcFieldMap), map));
                testObject.Open(streamReader);

                Assert.Equal("Table", testObject.ColumnSource);
            }
        }

        [Fact]
        public void Open_SetsOdbcFieldMap_WhenDialogResultIsOK()
        {
            using (Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(
                    "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapSavedToDisk.json"))
            using (var streamReader = new StreamReader(stream))
            {
                var ioFactory = mock.Create<JsonOdbcFieldMapIOFactory>();
                var map = mock.Create<OdbcFieldMap>(new TypedParameter(typeof(IOdbcFieldMapIOFactory), ioFactory));

                MockOpenFileDialog(DialogResult.OK, stream);

                var testObject = mock.Create<FakeOdbcSettingsFile>(new TypedParameter(typeof(IOdbcFieldMap), map));
                testObject.Open(streamReader);

                Assert.Equal(2, testObject.OdbcFieldMap.Entries.Count());
            }
        }

        [Fact]
        public void Open_DelegatesToMessageHelper_WhenIOExceptionIsThrown()
        {
            var streamReader = mock.Mock<TextReader>();
            streamReader.Setup(r => r.ReadToEnd()).Throws(new IOException());
            var messageHelper = mock.Mock<IMessageHelper>();
            var testObject = mock.Create<FakeOdbcSettingsFile>();

            testObject.Open(streamReader.Object);

            messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Open_DelegatesToMessageHelper_WhenUnauthorizedAccessExceptionIsThrown()
        {
            var streamReader = mock.Mock<TextReader>();
            streamReader.Setup(r => r.ReadToEnd()).Throws(new UnauthorizedAccessException());
            var messageHelper = mock.Mock<IMessageHelper>();
            var testObject = mock.Create<FakeOdbcSettingsFile>();

            testObject.Open(streamReader.Object);

            messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Open_DelegatesToMessageHelper_WhenShipWorksOdbcExceptionIsThrown()
        {
            var streamReader = mock.Mock<TextReader>();
            streamReader.Setup(r => r.ReadToEnd()).Throws(new ShipWorksOdbcException());
            var messageHelper = mock.Mock<IMessageHelper>();
            var testObject = mock.Create<FakeOdbcSettingsFile>();

            testObject.Open(streamReader.Object);

            messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        [SuppressMessage("SonarLint", "S3928: Parameter names used into ArgumentException constructors should match an existing one",
            Justification = "We don't care which parameter is null")]
        public void Open_ThrowsException_WhenUnhandledExceptionIsThrown()
        {
            var streamReader = mock.Mock<TextReader>();
            streamReader.Setup(r => r.ReadToEnd()).Throws(new ArgumentNullException());
            var testObject = mock.Create<FakeOdbcSettingsFile>();

            Assert.Throws<ArgumentNullException>(() => (object) testObject.Open(streamReader.Object));
        }

        private Mock<IOdbcFieldMap> MockFieldMap()
        {
            Mock<IOdbcFieldMap> fieldMap = mock.MockRepository.Create<IOdbcFieldMap>();
            fieldMap.Setup(f => f.Name).Returns(DefaultFileName);
            return fieldMap;
        }

        private void MockOpenFileDialog(DialogResult result, Stream stream)
        {
            var fileDialogMock = mock.CreateMock<IOpenFileDialog>(
                dialog =>
                {
                    dialog.Setup(d => d.ShowDialog()).Returns(result);
                    dialog.Setup(d => d.CreateFileStream()).Returns(stream);
                });

            mock.MockFunc(fileDialogMock);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}