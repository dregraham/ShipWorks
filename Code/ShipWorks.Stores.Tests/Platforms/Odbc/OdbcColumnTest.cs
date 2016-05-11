using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
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