using System.Data;
using System.Data.Odbc;
using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcShipWorksDbProviderFactoryTest
    {
        [Fact]
        public void CreateOdbcConnection_ReturnsOdbcConnection()
        {
            OdbcShipWorksDbProviderFactory testObject = new OdbcShipWorksDbProviderFactory();
            IDbConnection connection = testObject.CreateOdbcConnection();

            Assert.IsAssignableFrom<OdbcConnection>(connection);
        }
    }
}
