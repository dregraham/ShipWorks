using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Executes the DownloadCommand to retrieve a list of orders
    /// </summary>
    public class OdbcDownloadCommand : IOdbcCommand
    {
        private readonly OdbcFieldMap fieldMap;
        private readonly OdbcDataSource dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommand"/> class.
        /// </summary>
        public OdbcDownloadCommand(OdbcFieldMap fieldMap, OdbcDataSource dataSource)
        {
            this.fieldMap = fieldMap;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Downloads all the orders.
        /// </summary>
        public IEnumerable<OdbcRecord> Execute()
        {
            List<OdbcRecord> records = new List<OdbcRecord>();

            using (OdbcConnection connection = (OdbcConnection) dataSource.CreateConnection())
            {
                string query = GetQuery(connection);

                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    OdbcDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        OdbcRecord odbcRecord = new OdbcRecord();
                        records.Add(odbcRecord);
                        
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            odbcRecord.AddField(reader.GetName(i), reader[i]);
                        }
                    }
                }
            }

            return records;
        }

        /// <summary>
        /// Gets the query text
        /// </summary>
        private string GetQuery(OdbcConnection connection)
        {
            using (OdbcDataAdapter adapter = new OdbcDataAdapter("", connection))
            using (OdbcCommandBuilder cmdBuilder = new OdbcCommandBuilder(adapter))
            {
                connection.Open();

                string tableNameInQuotes = cmdBuilder.QuoteIdentifier(fieldMap.ExternalTableName);

                IEnumerable<string> columnNamesInQuotes = fieldMap.Entries.Select(e => cmdBuilder.QuoteIdentifier(e.ExternalField.Column.Name));
                string columnsToProject = string.Join(",", columnNamesInQuotes);

                return $"SELECT {columnsToProject} FROM {tableNameInQuotes}";
            }
        }
    }
}