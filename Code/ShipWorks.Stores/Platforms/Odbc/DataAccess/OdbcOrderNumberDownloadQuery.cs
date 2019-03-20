using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Odbc download query for downloading a specific order number
    /// </summary>
    public class OdbcOrderNumberDownloadQuery : IOdbcQuery
    {
        private readonly OdbcColumnSourceType importColumnSourceType;
        private readonly IOdbcQuery downloadQuery;
        private readonly string orderNumber;
        private readonly string orderNumberColumnName;
        private readonly string quotedOrderNumberColumnName;

        // Since OrderNumberComplete is an nvarchar(50) this is the size of the sql parameter
        private const int SizeOfOrderNumberParameter = 50;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcOrderNumberDownloadQuery(OdbcColumnSourceType importColumnSourceType,
            IOdbcQuery downloadQuery,
            string orderNumber,
            string orderNumberColumnName,
            string quotedOrderNumberColumnName)
        {
            this.importColumnSourceType = importColumnSourceType;
            this.downloadQuery = downloadQuery;
            this.orderNumber = orderNumber;
            this.orderNumberColumnName = orderNumberColumnName;
            this.quotedOrderNumberColumnName = quotedOrderNumberColumnName;
        }

        /// <summary>
        /// Generates the Sql for the query
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Order Number column is not mapped</exception>
        public string GenerateSql()
        {
            // If the orderNumber column is not mapped we cannot generate the query
            if (string.IsNullOrWhiteSpace(orderNumberColumnName))
            {
                throw new ShipWorksOdbcException("The OrderNumber column must be mapped to download orders On Demand.");
            }

            // Generate the query
            if (importColumnSourceType == OdbcColumnSourceType.CustomParameterizedQuery)
            {
                return downloadQuery.GenerateSql();
            }

            return $"SELECT sub.* FROM ({downloadQuery.GenerateSql()}) sub WHERE {quotedOrderNumberColumnName} = ?";
        }

        /// <summary>
        /// Adds Command Text to the given sql command
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        public void ConfigureCommand(IShipWorksOdbcCommand command)
        {
            command.ChangeCommandText(GenerateSql());

            OdbcParameter param = new OdbcParameter(quotedOrderNumberColumnName, OdbcType.NVarChar, SizeOfOrderNumberParameter, ParameterDirection.Input, false, 1, 3, quotedOrderNumberColumnName, DataRowVersion.Current, orderNumber);
            command.AddParameter(param);
        }
    }
}