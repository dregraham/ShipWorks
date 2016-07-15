using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Download;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Download
{
    public class CustomQueryOdbcDownloadQueryTest
    {
        [Fact]
        public void GenerateSql_ReturnsCustomQueryFromMap()
        {
            var testObject = new CustomQueryOdbcDownloadQuery(new OdbcStoreEntity() {ImportColumnSource = "someQuery"});

            Assert.Equal("someQuery", testObject.GenerateSql());
        }
    }
}