using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcImportSettingsFileTest : IDisposable
    {
        private readonly AutoMock mock;
        private const string DefaultFileName = "Field map name";

        public OdbcImportSettingsFileTest()
        {
            {
                mock = AutoMock.GetLoose();
            }
        }

        [Fact]
        public void Open_OdbcImportStrategySet()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\r\n  \"ColumnSourceType\": \"Table\",\r\n  \"ColumnSource\": null,\r\n  \"FieldMap\": null,\r\n  \"ImportStrategy\": \"ByModifiedTime\"\r\n}")))
            using (var streamReader = new StreamReader(stream))
            {
                var testObject = mock.Create<OdbcImportSettingsFile>();

                testObject.Open(streamReader);

                Assert.Equal(OdbcImportStrategy.ByModifiedTime, testObject.OdbcImportStrategy);
            }
        }

        [Fact]
        public void Save_ImportStrategySaved()
        {
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            {
                var testObject = mock.Create<OdbcImportSettingsFile>();
                testObject.OdbcFieldMap = MockFieldMap().Object;
                testObject.OdbcImportStrategy = OdbcImportStrategy.ByModifiedTime;

                testObject.Save(streamWriter);

                string savedJson = Encoding.UTF8.GetString(stream.ToArray());

                JObject jsonResults = JObject.Parse(savedJson);
                Assert.Equal(EnumHelper.GetApiValue(testObject.OdbcImportStrategy), jsonResults.GetValue("ImportStrategy"));
            }
        }

        [Fact]
        public void Action_IsImport()
        {
            var testObject = mock.Create<OdbcImportSettingsFile>();
            Assert.Equal("Import", testObject.Action);
        }

        [Fact]
        public void Extension_IsSwoim()
        {
            var testObject = mock.Create<OdbcImportSettingsFile>();
            Assert.Equal(".swoim", testObject.Extension);
        }

        private Mock<IOdbcFieldMap> MockFieldMap()
        {
            Mock<IOdbcFieldMap> fieldMap = mock.MockRepository.Create<IOdbcFieldMap>();
            fieldMap.Setup(f => f.Name).Returns(DefaultFileName);
            return fieldMap;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
