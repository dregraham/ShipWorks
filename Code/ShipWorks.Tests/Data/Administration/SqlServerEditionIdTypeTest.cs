using ShipWorks.Data.Administration;
using Xunit;

namespace ShipWorks.Tests.Data.Administration
{
    public class SqlServerEditionIdTypeTest
    {
        [Theory]
        [InlineData(SqlServerEditionIdType.Enterprise, true)]
        [InlineData(SqlServerEditionIdType.EnterpriseEditionCoreBasedLicensing, true)]
        [InlineData(SqlServerEditionIdType.EnterpriseEvaluation, true)]
        [InlineData(SqlServerEditionIdType.BusinessIntelligence, true)]
        [InlineData(SqlServerEditionIdType.Developer, true)]
        [InlineData(SqlServerEditionIdType.SqlDatabaseOrSqlDataWarehouse, true)]
        [InlineData(SqlServerEditionIdType.Standard, true)]
        [InlineData(SqlServerEditionIdType.ExpressWithAdvancedServices, false)]
        [InlineData(SqlServerEditionIdType.Express, false)]
        [InlineData(SqlServerEditionIdType.Web, false)]
        public void SupportsCompression_ReturnsCorrectValue(SqlServerEditionIdType editionIdType, bool expectedResult)
        {
            Assert.Equal(expectedResult, editionIdType.SupportsCompression());
        }

        [Theory]
        [InlineData("-1592396055", SqlServerEditionIdType.Express)]
        [InlineData("3", SqlServerEditionIdType.Express)]
        public void Parse_ReturnsCorrectValue(string editionIdType, SqlServerEditionIdType expectedResult)
        {
            SqlServerEditionIdType parsedValue = SqlServerEditionIdType.Express;
            parsedValue = parsedValue.Parse(editionIdType, SqlServerEditionIdType.Express);
            Assert.Equal(expectedResult, parsedValue);
        }
    }
}
