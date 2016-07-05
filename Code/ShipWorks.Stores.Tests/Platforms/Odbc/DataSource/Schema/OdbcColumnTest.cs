using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataSource.Schema
{
    public class OdbcColumnTest
    {
        [Fact]
        public void Ctor_SetsName()
        {
            OdbcColumn testObject = new OdbcColumn("SomeName");
            Assert.Equal("SomeName", testObject.Name);
        }
    }
}