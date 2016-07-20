using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
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
            if (store.UploadColumnSourceType != (int) OdbcColumnSourceType.Table)
            {
                throw new ShipWorksOdbcException("Unable to Generate SQL for non table upload column source.");
            }

            IOdbcFieldMapEntry orderNumberField = fieldMap.FindEntriesBy(OrderFields.OrderNumber).FirstOrDefault();
            if (orderNumberField == null)
            {
                throw new ShipWorksOdbcException("The Order Number field must be mapped to upload shipment data.");
            }

            StringBuilder builder = new StringBuilder();

            using (DbConnection connection = dataSource.CreateConnection())
            using (IShipWorksOdbcDataAdapter adapter = dbProviderFactory.CreateShipWorksOdbcDataAdapter(string.Empty, connection))
            using (IShipWorksOdbcCommandBuilder cmdBuilder = dbProviderFactory.CreateShipWorksOdbcCommandBuilder(adapter))
            {
                connection.Open();

                string tableToUpdate = cmdBuilder.QuoteIdentifier(store.UploadColumnSource);
                builder.Append($"UPDATE {tableToUpdate} SET ");

                IOdbcFieldMapEntry lastEntry = fieldMap.Entries.Except(new[] { orderNumberField }).LastOrDefault();
                foreach (IOdbcFieldMapEntry entry in fieldMap.Entries.Except(new [] {orderNumberField}))
                {
                    string columnNameInQuotes = cmdBuilder.QuoteIdentifier(entry.ExternalField.Column.Name);
                    // update the external column to the shipworks field value
                    builder.Append($"{columnNameInQuotes} = ?");

                    builder.Append(entry != lastEntry ? ", " : " ");
                }

                string orderNumberColumnInQuotes = cmdBuilder.QuoteIdentifier(orderNumberField.ExternalField.Column.Name);
                builder.Append($"WHERE {orderNumberColumnInQuotes} = ?");
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

            IOdbcFieldMapEntry orderNumberField = fieldMap.FindEntriesBy(OrderFields.OrderNumber).FirstOrDefault();
            if (orderNumberField == null)
            {
                throw new ShipWorksOdbcException("The Order Number field must be mapped to upload shipment data.");
            }

            // we have to add all of the parameters in the order that they were generated
            // this means that we have to add all but the order number first
            foreach (IOdbcFieldMapEntry entry in fieldMap.Entries.Except(new[] { orderNumberField }))
            {
                command.AddParameter(new OdbcParameter(entry.ExternalField.Column.Name, entry.ShipWorksField.Value));
            }

            // now finally add the order number
            command.AddParameter(new OdbcParameter(orderNumberField.ExternalField.Column.Name, orderNumberField.ShipWorksField.Value));
        }
    }
}