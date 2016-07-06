using log4net;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;

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
        private readonly IOdbcDownloadQuery downloadQuery;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDownloadCommand"/> class.
        /// </summary>
        public OdbcDownloadCommand(IOdbcFieldMap fieldMap,
            IOdbcDataSource dataSource,
            IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcDownloadQuery downloadQuery,
            ILog log)
        {
            this.fieldMap = fieldMap;
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.downloadQuery = downloadQuery;
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
                    connection.Open();
                    string query = downloadQuery.GenerateSql();

                    log.Info($"Query created by OdbcDownloadCommand is \"{query}\"");

                    using (IShipWorksOdbcCommand command = dbProviderFactory.CreateOdbcCommand(query, connection))
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OdbcRecord odbcRecord = CreateOdbcRecord(reader);

                            // Only add the record if it contains values. Was causing an issue with
                            // excel where it was trying to download empty rows.
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
        }

        /// <summary>
        /// Creates an ODBC record.
        /// </summary>
        private OdbcRecord CreateOdbcRecord(DbDataReader reader)
        {
            OdbcRecord odbcRecord = new OdbcRecord(fieldMap.RecordIdentifierSource);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                object value = reader[i];
                odbcRecord.AddField(columnName, value);
            }

            return odbcRecord;
        }
    }
}