using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Query for uploading to a single table
    /// </summary>
    public class OdbcTableUploadQuery : IOdbcQuery
    {
        private readonly IOdbcFieldMap fieldMap;
        private readonly OdbcStoreEntity store;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcDataSource dataSource;

        public OdbcTableUploadQuery(IOdbcFieldMap fieldMap,
            OdbcStoreEntity store,
            IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcDataSource dataSource)
        {
            this.fieldMap = fieldMap;
            this.store = store;
            this.dbProviderFactory = dbProviderFactory;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Generates the sql to upload to a single table
        /// </summary>
        public string GenerateSql()
        {
            StringBuilder builder = new StringBuilder($"UPDATE {store.UploadColumnSource} SET ");

            using (DbConnection connection = dataSource.CreateConnection())
            using (IShipWorksOdbcDataAdapter adapter = dbProviderFactory.CreateShipWorksOdbcDataAdapter(string.Empty, connection))
            using (IShipWorksOdbcCommandBuilder cmdBuilder = dbProviderFactory.CreateShipWorksOdbcCommandBuilder(adapter))
            {
                connection.Open();

                IOdbcFieldMapEntry lastEntry = fieldMap.Entries.Last();
                foreach (IOdbcFieldMapEntry entry in fieldMap.Entries)
                {
                    string columnNameInQuotes = cmdBuilder.QuoteIdentifier(entry.ExternalField.Column.Name);
                    // update the external column to the shipworks field value
                    builder.Append($"{columnNameInQuotes} = @{entry.ExternalField.Column.Name}");

                    builder.Append(entry != lastEntry ? ", " : ";");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Configures the given command
        /// </summary>
        /// <param name="command"></param>
        public void ConfigureCommand(IShipWorksOdbcCommand command)
        {
            command.ChangeCommandText(GenerateSql());

            foreach (IOdbcFieldMapEntry entry in fieldMap.Entries)
            {
                command.AddParameter(new OdbcParameter(entry.ExternalField.Column.Name, entry.ShipWorksField.Value));
            }
        }
    }
}