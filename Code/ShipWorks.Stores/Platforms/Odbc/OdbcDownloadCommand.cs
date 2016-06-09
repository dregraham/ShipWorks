using log4net;
using ShipWorks.Stores.Communication;
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
        private readonly IOdbcFieldMap fieldMap;
        private readonly IOdbcDataSource dataSource;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommand"/> class.
        /// </summary>
        public OdbcDownloadCommand(IOdbcFieldMap fieldMap, IOdbcDataSource dataSource, ILog log)
        {
            this.fieldMap = fieldMap;
            this.dataSource = dataSource;
            this.log = log;
        }

        /// <summary>
        /// Downloads all the orders.
        /// </summary>
        public IEnumerable<OdbcRecord> Execute()
        {
            try
            {
                List<OdbcRecord> records = new List<OdbcRecord>();

                using (OdbcConnection connection = (OdbcConnection) dataSource.CreateConnection())
                {
                    string query = GetQuery(connection);

                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            OdbcRecord odbcRecord = new OdbcRecord();
                            records.Add(odbcRecord);

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (!reader.IsDBNull(i))
                                {
                                    string columnName = reader.GetName(i);
                                    object value = reader[i];

                                    odbcRecord.AddField(columnName, value);
                                    if (columnName == fieldMap.RecordIdentifierSource)
                                    {
                                        odbcRecord.RecordIdentifier = value.ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                return records;
            }
            catch (OdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
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

                string query = $"SELECT {columnsToProject} FROM {tableNameInQuotes}";
                log.Info($"Query creted by OdbcDownloadCommand is \"{query}\"");

                return query;
            }
        }
    }
}