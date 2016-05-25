using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using Autofac;
using log4net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Represents a table from an ODBC schema
    /// </summary>
    public class OdbcTable : IOdbcTable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcTable(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The table name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The columns in the table
        /// </summary>
        public IEnumerable<OdbcColumn> Columns { get; private set; }

        /// <summary>
        /// Loads the columns for this table
        /// </summary>
        public void Load(IOdbcDataSource dataSource, ILog log)
        {
            using (DbConnection connection = dataSource.CreateConnection())
            {
                string[] restriction =
                {
                    null, // table_catalog
                    null, // table_schema
                    Name, // table_name
                    null // table_type
                };

                try
                {
                    connection.Open();

                    DataTable columnData = connection.GetSchema("Columns", restriction);
                    Columns = new List<OdbcColumn>();

                    for (int j = 0; j < columnData.Rows.Count; j++)
                    {
                        string columnName = columnData.Rows[j].ItemArray[3].ToString();
                        Columns = Columns.Concat(new[] {new OdbcColumn(columnName)});
                    }
                }
                catch (DbException ex)
                {
                    log.Error(ex.Message);
                    throw new ShipWorksOdbcException(
                        $"An error occurred while attempting to open a connection to {dataSource.Name}.", ex);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new ShipWorksOdbcException(
                        $"An error occurred while attempting to retrieve columns from information from the {Name} table.",
                        ex);
                }
            }
        }

        /// <summary>
        /// Resets the columns.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ResetColumns()
        {
            Columns = null;
        }
    }
}
