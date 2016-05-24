using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents an ODBC Schema which contains ODBC Tables and Data Source
    /// </summary>
	public class OdbcSchema : IOdbcSchema
    {
	    private readonly OdbcTableFactory tableFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
	    public OdbcSchema(Func<Type, ILog> logFactory, OdbcTableFactory tableFactory)
        {
            log = logFactory(typeof(OdbcSchema));
	        this.tableFactory = tableFactory;
	    }

        /// <summary>
        /// List of ODBC tables in the schema
        /// </summary>
	    public IEnumerable<OdbcTable> Tables { get; private set; }

        /// <summary>
        /// The data source where this schema comes from
        /// </summary>
        public IOdbcDataSource DataSource { get; private set; }

        /// <summary>
        /// Loads the given Data Source's Schema
        /// </summary>
        /// <param name="dataSource"></param>
        public void Load(IOdbcDataSource dataSource)
        {
            DataSource = dataSource;

            using (OdbcConnection connection = DataSource.CreateConnection())
            {
                try
                {
                    connection.Open();
                }
                catch (DbException ex)
                {
                    log.Error(ex.Message);
                    throw new ShipWorksOdbcException($"An error occurred while attempting to open a connection to {dataSource.Name}.", ex);
                }

                try
                {
                    DataTable tableData = connection.GetSchema("Tables");

                    int position = tableData.Columns.Cast<DataColumn>().ToList()
                        .FindIndex(c => c.ColumnName.Equals("TABLE_NAME", StringComparison.OrdinalIgnoreCase));

                    List<OdbcTable> tables = new List<OdbcTable>();

                    for (int i = 0; i < tableData.Rows.Count; i++)
                    {
                        OdbcTable table = tableFactory.CreateTable(tableData.Rows[i].ItemArray[position].ToString());
                        tables.Add(table);
                    }

                    Tables = tables;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new ShipWorksOdbcException($"An error occurred while attempting to retrieve a list of tables from the {dataSource.Name} data source.", ex);
                }

            }
        }
	}
}
