using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    /// <summary>
    /// Odbc downloading query for downloading a specific order number
    /// </summary>
    public class OdbcOrderNumberDownloadQuery : IOdbcQuery
    {
        private readonly IOdbcQuery downloadQuery;
        private readonly string orderNumber;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcDataSource dataSource;
        private readonly string orderNumberColumnName;

        // Since OrderNumberComplete is an nvarchar(50) this is the size of the sql parameter
        private const int SizeOfOrderNumberParameter = 50;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcOrderNumberDownloadQuery(IOdbcQuery downloadQuery,
            string orderNumber,
            IOdbcFieldMap fieldMap,
            IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcDataSource dataSource)
        {
            this.downloadQuery = downloadQuery;
            this.orderNumber = orderNumber;
            this.dbProviderFactory = dbProviderFactory;
            this.dataSource = dataSource;
            orderNumberColumnName = fieldMap.FindEntriesBy(OrderFields.OrderNumberComplete).FirstOrDefault()?.ExternalField.Column.Name;
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

            string columnNameInQuotes = WrapColumnInQuoteIdentifier(orderNumberColumnName);

            // Generate the query

            return $"SELECT sub.* FROM ({downloadQuery.GenerateSql()}) sub WHERE {columnNameInQuotes} = ?";
        }

        /// <summary>
        /// Adds Command Text to the given sql command
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        public void ConfigureCommand(IShipWorksOdbcCommand command)
        {
            command.ChangeCommandText(GenerateSql());
            string columnNameInQuotes = WrapColumnInQuoteIdentifier(orderNumberColumnName);

            OdbcParameter param = new OdbcParameter(columnNameInQuotes, OdbcType.NVarChar, SizeOfOrderNumberParameter, ParameterDirection.Input, false, 1, 3, columnNameInQuotes, DataRowVersion.Current, orderNumber);
            command.AddParameter(param);
        }

        /// <summary>
        /// Wraps the given column string in the data sources quoted identifier
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        private string WrapColumnInQuoteIdentifier(string column)
        {
            using (DbConnection connection = dataSource.CreateConnection())
            using (IShipWorksOdbcDataAdapter adapter = dbProviderFactory.CreateShipWorksOdbcDataAdapter(string.Empty, connection))
            using (IShipWorksOdbcCommandBuilder cmdBuilder = dbProviderFactory.CreateShipWorksOdbcCommandBuilder(adapter))
            {
                connection.Open();
                return cmdBuilder.QuoteIdentifier(column);
            }
        }
    }
}