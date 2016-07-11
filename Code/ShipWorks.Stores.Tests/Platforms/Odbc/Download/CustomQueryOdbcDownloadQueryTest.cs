using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Download
{
    public class CustomQueryOdbcDownloadQueryTest
    {
        [Fact]
        public void GenerateSql_ReturnsCustomQueryFromMap()
        {
            var testObject = new CustomQueryOdbcDownloadQuery(new OdbcStoreEntity() {OdbcColumnSource = "someQuery"});

            Assert.Equal("someQuery", testObject.GenerateSql());
        }
    }
}