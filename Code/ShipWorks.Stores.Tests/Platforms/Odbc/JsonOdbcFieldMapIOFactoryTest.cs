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
                var map = mock.Create<OdbcFieldMap>();

                JsonOdbcFieldMapIOFactory testObject = new JsonOdbcFieldMapIOFactory();

                Assert.IsAssignableFrom<JsonOdbcFieldMapReader>(testObject.CreateReader(map));
            }
        }

        [Fact]
        public void CreateReader_ReturnsJsonOdbcFieldMapWriter()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Create<OdbcFieldMap>();

                JsonOdbcFieldMapIOFactory testObject = new JsonOdbcFieldMapIOFactory();

                Assert.IsAssignableFrom<JsonOdbcFieldMapWriter>(testObject.CreateWriter(map));
            }
        }
    }
}