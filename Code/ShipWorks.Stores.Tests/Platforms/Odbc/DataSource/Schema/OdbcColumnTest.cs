using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataSource.Schema
{
    public class OdbcColumnTest
    {
        [Fact]
        public void Ctor_SetsName()
        {
            OdbcColumn testObject = new OdbcColumn("SomeName", "unknown");
            Assert.Equal("SomeName", testObject.Name);
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenNameIsTheSame()
        {
            const string name = "Odbc Column Name";
            OdbcColumn odbcColumn = new OdbcColumn(name, "unknown");
            OdbcColumn odbcColumnToCompare = new OdbcColumn(name, "unknown");

            Assert.Equal(odbcColumn, odbcColumnToCompare);
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenNameIsDifferent()
        {
            const string name = "Odbc Column Name";
            const string otherName = "Odbc Other Name";

            OdbcColumn odbcColumn = new OdbcColumn(name, "unknown");
            OdbcColumn odbcColumnToCompare = new OdbcColumn(otherName, "unknown");

            Assert.NotEqual(odbcColumn, odbcColumnToCompare);
        }

        [Fact]
        public void GetHashCode_ReturnsHashCodeOfName()
        {
            const string name = "Odbc Column Name";
            OdbcColumn odbcColumn = new OdbcColumn(name, "unknown");

            Assert.Equal(name.GetHashCode(), odbcColumn.GetHashCode());
        }
    }
}