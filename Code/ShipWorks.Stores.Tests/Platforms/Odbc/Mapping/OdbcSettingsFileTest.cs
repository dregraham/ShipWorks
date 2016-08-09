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

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}