using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class CustomQueryOdbcDownloadQueryTest
    {
        [Fact]
        public void GenerateSql_ReturnsCustomQueryFromMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();
                map.Setup(m => m.CustomQuery).Returns("Query");

                var testObject = mock.Create<CustomQueryOdbcDownloadQuery>();

                Assert.Equal("Query", testObject.GenerateSql());
            }
        }
    }
}