using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents a table from an ODBC schema
    /// </summary>
	public class OdbcTable
	{
	    private readonly OdbcSchema schema;
        private List<OdbcColumn> columns;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
		public OdbcTable(OdbcSchema schema, string tableName, ILog log)
	    {
	        this.schema = schema;
            Name = tableName;
            this.log = log;
        }

        /// <summary>
        /// The table name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The columns in the table
        /// </summary>
        public List<OdbcColumn> Columns
        {
            get
            {
                if (columns == null)
                {
                    Load();
                }

                return columns;
            }

            private set { columns = value; }
        }

        /// <summary>
        /// Loads the columns for this table
        /// </summary>
        private void Load()
		{
            IOdbcDataSource dataSource = schema.DataSource;

            Columns = new List<OdbcColumn>();

            using (DbConnection connection = dataSource.CreateConnection())
            {
                connection.Open();

                try
                {
                    string[] restriction =
                    {
                        null, // table_catalog
                        null, // table_schema
                        Name, // table_name
                        null // table_type
                    };

                    DataTable columnData = connection.GetSchema("Columns", restriction);

                    for (int j = 0; j < columnData.Rows.Count; j++)
                    {
                        string columnName = columnData.Rows[j].ItemArray[3].ToString();
                        string type = columnData.Rows[j].ItemArray[13].ToString();
                        int typeCode;
                        int.TryParse(type, out typeCode);

                        Columns.Add(new OdbcColumn(columnName));
                    }
                }
                catch (Exception ex)
                {
                    // TODO consider creating a new exception for odbc
                    log.Error(ex);
                    throw;
                }
            }
        }
    }
}
