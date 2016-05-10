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
    /// Represents an ODBC Schema
    /// </summary>
	public class OdbcSchema
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
	    public List<OdbcTable> Tables { get; private set; }

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

            Tables = new List<OdbcTable>();

            using (DbConnection connection = DataSource.CreateConnection())
            {
                try
                {
                    connection.Open();
                }
                catch (DbException ex)
                {
                    log.Error(ex.Message);
                    return;
                }

                DataTable tableData = connection.GetSchema("Tables");

                int position = tableData.Columns.Cast<DataColumn>().ToList()
                    .FindIndex(c => c.ColumnName.Equals("TABLE_NAME", StringComparison.OrdinalIgnoreCase));

                for (int i = 0; i < tableData.Rows.Count; i++)
                {
                    OdbcTable table = tableFactory.CreateTable(this, tableData.Rows[i].ItemArray[position].ToString());
                    Tables.Add(table);
                }
            }
        }
	}
}
