using log4net;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommand"/> class.
        /// </summary>
        public OdbcDownloadCommand(IOdbcFieldMap fieldMap, IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory, ILog log)
        {
            this.fieldMap = fieldMap;
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
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

                using (DbConnection connection = dataSource.CreateConnection())
                {
                    string query = GetQuery(connection);

                    using (IShipWorksOdbcCommand command = dbProviderFactory.CreateOdbcCommand(query, connection))
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OdbcRecord odbcRecord = CreateOdbcRecord(reader);

                            // Only add the record if it contains values. Was causing an issue with
                            // excel where it was trying to download emtpy rows.
                            if (odbcRecord.HasValues)
                            {
                                records.Add(odbcRecord);
                            }
                        }
                    }
                }

                return records;
            }
            catch (OdbcException ex)
            {
                // cant unit test OdbcException, rethrow as ShipWorksOdbcException
                throw new ShipWorksOdbcException(ex.Message, ex);
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates an ODBC record.
        /// </summary>
        private OdbcRecord CreateOdbcRecord(DbDataReader reader)
        {
            OdbcRecord odbcRecord = new OdbcRecord();

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

            return odbcRecord;
        }

        /// <summary>
        /// Gets the query text
        /// </summary>
        private string GetQuery(DbConnection connection)
        {
            using (IShipWorksOdbcDataAdapter adapter = dbProviderFactory.CreateShipWorksOdbcDataAdapter(string.Empty, connection))
            using (IShipWorksOdbcCommandBuilder cmdBuilder = dbProviderFactory.CreateShipWorksOdbcCommandBuilder(adapter))
            {
                connection.Open();

                string tableNameInQuotes = cmdBuilder.QuoteIdentifier(fieldMap.GetExternalTableName());

                List<string> columnNamesInQuotes = fieldMap.Entries.Select(e => cmdBuilder.QuoteIdentifier(e.ExternalField.Column.Name)).ToList();
                columnNamesInQuotes.Add(cmdBuilder.QuoteIdentifier(fieldMap.RecordIdentifierSource));
                string columnsToProject = string.Join(",", columnNamesInQuotes.Distinct());

                string query = $"SELECT {columnsToProject} FROM {tableNameInQuotes}";
                log.Info($"Query created by OdbcDownloadCommand is \"{query}\"");

                return query;
            }
        }
    }
}