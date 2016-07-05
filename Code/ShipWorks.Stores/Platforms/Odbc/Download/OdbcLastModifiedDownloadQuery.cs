using System;
using System.Data.Common;
using System.Linq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Download Query where the results are limited to rows greater than the onlinelastmodified date
    /// </summary>
    public class OdbcLastModifiedDownloadQuery : IOdbcDownloadQuery
    {
        private readonly IOdbcDownloadQuery downloadQuery;
        private readonly DateTime onlineLastModifiedStartingPoint;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcDataSource dataSource;
        private readonly string columnName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="downloadQuery">Query to limit</param>
        /// <param name="onlineLastModifiedStartingPoint">date to limit the query from </param>
        public OdbcLastModifiedDownloadQuery(
            IOdbcDownloadQuery downloadQuery,
            DateTime onlineLastModifiedStartingPoint,
            IOdbcFieldMap fieldMap,
            IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcDataSource dataSource)
        {
            this.downloadQuery = downloadQuery;
            this.onlineLastModifiedStartingPoint = onlineLastModifiedStartingPoint;
            this.dbProviderFactory = dbProviderFactory;
            this.dataSource = dataSource;
            columnName = fieldMap.FindEntriesBy(OrderFields.OnlineLastModified).FirstOrDefault()?.ExternalField.Column.Name;
        }

        /// <summary>
        /// Makes the downloadQuery a sub query of limits the results to
        /// OnlineLastModified greater than the onlineLastModifiedStartingPoint
        /// </summary>
        /// <returns></returns>
        public string GenerateSql()
        {
            // If the onlinelastmodified column is not mapped we cannot generate the query
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ShipWorksOdbcException("The OnlineLastModified column must be mapped to download by OnlineLastModified.");
            }

            using (DbConnection connection = dataSource.CreateConnection())
            using (IShipWorksOdbcDataAdapter adapter = dbProviderFactory.CreateShipWorksOdbcDataAdapter(string.Empty, connection))
            using (IShipWorksOdbcCommandBuilder cmdBuilder = dbProviderFactory.CreateShipWorksOdbcCommandBuilder(adapter))
            {
                // Connect to the database to get the quoted identifier
                connection.Open();
                string columnNameInQuotes = cmdBuilder.QuoteIdentifier(columnName);

                // Generate the query
                return $@"SELECT sub.* FROM({downloadQuery.GenerateSql()}) sub WHERE {columnNameInQuotes} > '{onlineLastModifiedStartingPoint.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fff")}' ORDER BY {columnNameInQuotes} ASC";
            }
        }
    }
}
