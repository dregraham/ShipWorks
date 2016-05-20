using System.IO;
using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class JsonOdbcFieldMapIOFactoryTest
    {
        [Fact]
        public void CreateReader_ReturnsJsonOdbcFieldMapReader()
        {
            using (var mock = AutoMock.GetLoose())
            {
                JsonOdbcFieldMapIOFactory testObject = mock.Create<JsonOdbcFieldMapIOFactory>();

                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);
                sw.Write("{\n\t\"Entries\": [{\n\t\t\"ShipWorksField\": {\n\t\t\t\"ElementName\": \"OrderEntity\",\n\t\t\t\"ElementFieldValue\": \"OrderNumber\",\n\t\t\t\"DisplayName\": \"Order Number\"\n\t\t},\n\t\t\"ExternalField\": {\n\t\t\t\"Table\": {\n\t\t\t\t\"Name\": \"ActionQueue\"\n\t\t\t},\n\t\t\t\"Column\": {\n\t\t\t\t\"Name\": \"ActionName\"\n\t\t\t},\n\t\t\t\"DisplayName\": \"ActionQueue ActionName\"\n\t\t}\n\t}],\n\t\"ExternalTableName\": \"ActionQueue\"\n}");
                sw.Flush();

                Assert.IsAssignableFrom<JsonOdbcFieldMapReader>(testObject.CreateReader(ms));
            }
        }

        [Fact]
        public void CreateReader_ReturnsJsonOdbcFieldMapWriter()
        {
            using (var mock = AutoMock.GetLoose())
            {
                JsonOdbcFieldMapIOFactory testObject = mock.Create<JsonOdbcFieldMapIOFactory>();

                Assert.IsAssignableFrom<JsonOdbcFieldMapWriter>(testObject.CreateWriter());
            }
        }
    }
}