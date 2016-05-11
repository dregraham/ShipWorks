using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents a table from an ODBC schema
    /// </summary>
	public class OdbcTable
	{
	    private readonly IOdbcSchema schema;
        private IEnumerable<OdbcColumn> columns;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
		public OdbcTable(IOdbcSchema schema, string tableName, ILog log)
        {
            MethodConditions.EnsureArgumentIsNotNull(schema);
            MethodConditions.EnsureArgumentIsNotNull(log);

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
        public IEnumerable<OdbcColumn> Columns
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
            using (DbConnection connection = schema.DataSource.CreateConnection())
            {
                try
                {
                    connection.Open();
                }
                catch (DbException ex)
                {
                    log.Error(ex.Message);
                    throw new ShipWorksOdbcException($"An error occurred while attempting to open a connection to {schema.DataSource.Name}.", ex);
                }

                string[] restriction =
                {
                    null, // table_catalog
                    null, // table_schema
                    Name, // table_name
                    null // table_type
                };

                try
                {
                    DataTable columnData = connection.GetSchema("Columns", restriction);
                    List<OdbcColumn> localColumns = new List<OdbcColumn>();

                    for (int j = 0; j < columnData.Rows.Count; j++)
                    {
                        string columnName = columnData.Rows[j].ItemArray[3].ToString();
                        localColumns.Add(new OdbcColumn(columnName));
                    }

                    Columns = localColumns;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new ShipWorksOdbcException($"An error occurred while attempting to retrieve columns from information from the {Name} table.", ex);
                }
            }
        }
    }
}
