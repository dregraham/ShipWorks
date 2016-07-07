using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    public class TableOdbcDownloadQuery : IOdbcDownloadQuery
    {
        private readonly OdbcStoreEntity store;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcDataSource dataSource;
        private readonly IOdbcFieldMap fieldMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableOdbcDownloadQuery"/> class.
        /// </summary>
        public TableOdbcDownloadQuery(OdbcStoreEntity store, IShipWorksDbProviderFactory dbProviderFactory, IOdbcFieldMap fieldMap, IOdbcDataSource dataSource)
        {
            this.store = store;
            this.dbProviderFactory = dbProviderFactory;
            this.dataSource = dataSource;
            this.fieldMap = fieldMap;

            if (store.OdbcColumnSourceType != (int) OdbcColumnSourceType.Table)
            {
                throw new ShipWorksOdbcException("An incompatible source was provided to perform an ODBC table based query.");
            }
        }

        /// <summary>
        /// Generates the Sql to download orders.
        /// </summary>
        public string GenerateSql()
        {
            using (DbConnection connection = dataSource.CreateConnection())
            using (IShipWorksOdbcDataAdapter adapter = dbProviderFactory.CreateShipWorksOdbcDataAdapter(string.Empty, connection))
            using (IShipWorksOdbcCommandBuilder cmdBuilder = dbProviderFactory.CreateShipWorksOdbcCommandBuilder(adapter))
            {
                connection.Open();

                string tableNameInQuotes = cmdBuilder.QuoteIdentifier(store.OdbcColumnSource);

                List<string> columnNamesInQuotes = fieldMap.Entries.Select(e => cmdBuilder.QuoteIdentifier(e.ExternalField.Column.Name)).ToList();
                columnNamesInQuotes.Add(cmdBuilder.QuoteIdentifier(fieldMap.RecordIdentifierSource));
                string columnsToProject = string.Join(", ", columnNamesInQuotes.Distinct());

                string query = $"SELECT {columnsToProject} FROM {tableNameInQuotes}";

                return query;
            }
        }
    }
}
