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

        /// <summary>
        /// Constructor
        /// </summary>
        private OdbcSampleDataCommand(IShipWorksDbProviderFactory dbProviderFactory, OdbcColumnSource)
        {
            this.dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Execute the query and return sample data
        /// </summary>
        public DataTable Execute(IOdbcDataSource dataSource, string query)
        {
            using (DbConnection connection = dataSource.CreateConnection())
            {
                connection.Open();

                using (IShipWorksOdbcCommand cmd = dbProviderFactory.CreateOdbcCommand(query, connection))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable resultTable = GetEmptyResultTable(reader);

                        int resultCount = 0;
                        while (reader.Read() && resultCount < 4)
                        {
                            DataRow row = resultTable.NewRow();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }

                            resultTable.Rows.Add(row);

                            resultCount++;
                        }

                        return resultTable;
                    }
                }
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