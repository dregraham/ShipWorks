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

                Assert.IsAssignableFrom<JsonOdbcFieldMapReader>(testObject.CreateReader(new MemoryStream()));
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