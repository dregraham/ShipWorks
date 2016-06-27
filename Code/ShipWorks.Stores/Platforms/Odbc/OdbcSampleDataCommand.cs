using log4net;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Command to retrieve sample data from the results of a query.
    /// </summary>
    public class OdbcSampleDataCommand : IOdbcSampleDataCommand
    {
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        private OdbcSampleDataCommand(IShipWorksDbProviderFactory dbProviderFactory, ILog log)
        {
            this.dbProviderFactory = dbProviderFactory;
            this.log = log;
        }

        /// <summary>
        /// Execute the query and return sample data
        /// </summary>
        public DataTable Execute(IOdbcDataSource dataSource, string query)
        {
            try {
                using (DbConnection connection = dataSource.CreateConnection())
                {
                    connection.Open();

                    using (IShipWorksOdbcCommand cmd = dbProviderFactory.CreateOdbcCommand(query, connection))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            // Get an empty table that matches the schema of the query
                            DataTable resultTable = GetEmptyResultTable(reader);

                            int resultCount = 0;
                            while (reader.Read() && resultCount < 4)
                            {
                                // Create a row and populate it with values from the reader
                                DataRow row = resultTable.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }

                                resultTable.Rows.Add(row);
                                resultCount++;
                            }

                            // Cancel the command once we have the number
                            // of results we want, this will hopefully improve
                            // performance for queries that return a lot of rows
                            cmd.Cancel();
                            return resultTable;
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                log.Error(ex);
                throw new ShipWorksOdbcException(
                    $"An error occurred while attempting to open a connection to {dataSource.Name}.", ex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ShipWorksOdbcException(
                    $"An error occurred while attempting to retrieve columns for the custom query.",
                    ex);
            }
        }

        /// <summary>
        /// Creates an empty result table from the reader
        /// </summary>
        private static DataTable GetEmptyResultTable(DbDataReader reader)
        {
            DataTable schemaTable = reader.GetSchemaTable();
            DataTable result = new DataTable();

            foreach (DataRow row in schemaTable.Rows.OfType<DataRow>())
            {
                result.Columns.Add(row["BaseColumnName"].ToString());
            }

            return result;
        }
    }
}