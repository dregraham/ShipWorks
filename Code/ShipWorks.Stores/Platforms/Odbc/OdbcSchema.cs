using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents an ODBC Schema which contains ODBC Tables and Data Source
    /// </summary>
	public class OdbcSchema : IOdbcSchema
    {
        private readonly Func<Type, ILog> logFactory;
        private readonly IOdbcColumnSourceFactory columnSourceFactory;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcSchema(Func<Type, ILog> logFactory,
            IOdbcColumnSourceFactory columnSourceFactory,
            IShipWorksDbProviderFactory dbProviderFactory)
        {
            log = logFactory(typeof(OdbcSchema));
            this.logFactory = logFactory;
            this.columnSourceFactory = columnSourceFactory;
            this.dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// List of ODBC tables in the schema
        /// </summary>
	    public IEnumerable<IOdbcColumnSource> Tables { get; private set; }

        /// <summary>
        /// Populates Table property with the schema of the given datasource.
        /// </summary>
        public void Load(IOdbcDataSource dataSource)
        {
            using (DbConnection connection = dataSource.CreateConnection())
            {
                try
                {
                    connection.Open();

                    DataTable tableData = connection.GetSchema("Tables");

                    int position = tableData.Columns.Cast<DataColumn>().ToList()
                        .FindIndex(c => c.ColumnName.Equals("TABLE_NAME", StringComparison.OrdinalIgnoreCase));

                    List<IOdbcColumnSource> odbcColumnSources = new List<IOdbcColumnSource>();

                    for (int i = 0; i < tableData.Rows.Count; i++)
                    {
                        IOdbcColumnSource table = columnSourceFactory.CreateTable(tableData.Rows[i].ItemArray[position].ToString());
                        odbcColumnSources.Add(table);
                    }

                    Tables = odbcColumnSources;
                }
                catch (DbException ex)
                {
                    log.Error(ex.Message);
                    throw new ShipWorksOdbcException($"An error occurred while attempting to open a connection to {dataSource.Name}.", ex);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new ShipWorksOdbcException(
                        $"An error occurred while attempting to retrieve a list of tables from the {dataSource.Name} data source.", ex);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Sets tables to be a single table representing the schema of this query.
        /// </summary>
        public void Load(IOdbcDataSource dataSource, string query)
        {
            IOdbcColumnSource odbcColumnSource = columnSourceFactory.CreateTable("Custom Query");
            odbcColumnSource.Load(dataSource, logFactory(typeof(OdbcColumnSource)), query, dbProviderFactory);

            Tables = new[] {odbcColumnSource};
        }
    }
}
