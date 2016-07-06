using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using log4net;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    /// <summary>
    /// Represents a table from an ODBC schema
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class OdbcColumnSource : IOdbcColumnSource
    {
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly ILog log;

        public OdbcColumnSource(string name, IShipWorksDbProviderFactory dbProviderFactory, Func<Type, ILog> logFactory)
        {
            Name = name;
            this.dbProviderFactory = dbProviderFactory;
            log = logFactory(typeof(OdbcColumnSource));
        }

        /// <summary>
        /// The columns in the table
        /// </summary>
        public IEnumerable<OdbcColumn> Columns { get; private set; }

        /// <summary>
        /// The column source name
        /// </summary>
        [Obfuscation(Exclude = false)]
        public string Name { get; private set; }

        /// <summary>
        /// Loads the columns for the column source using the given source
        /// </summary>
        [Obfuscation(Exclude = false)]
        public void Load(IOdbcDataSource dataSource, string source, OdbcColumnSourceType sourceType)
        {
            if (sourceType == OdbcColumnSourceType.CustomQuery)
            {
                LoadCustomQueryColumns(dataSource, source);
            }

            if (sourceType == OdbcColumnSourceType.Table)
            {
                LoadTableColumns(dataSource, source);
            }
        }


        /// <summary>
        /// Loads the columns, based on the table in the data source
        /// </summary>
        [Obfuscation(Exclude = false)]
        private void LoadTableColumns(IOdbcDataSource dataSource, string tableName)
        {
            Name = tableName;

            using (DbConnection connection = dataSource.CreateConnection())
            {
                string[] restriction =
                {
                    null, // table_catalog
                    null, // table_schema
                    tableName, // table_name
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
                        Columns = Columns.Concat(new[] { new OdbcColumn(columnName) });
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
                        $"An error occurred while attempting to retrieve columns from the {tableName} table.",
                        ex);
                }
            }
        }

        /// <summary>
        /// Loads the columns for the column source using the given query
        /// </summary>
        [Obfuscation(Exclude = false)]
        private void LoadCustomQueryColumns(IOdbcDataSource dataSource, string query)
        {
            using (DbConnection connection = dataSource.CreateConnection())
            {
                try
                {
                    connection.Open();

                    IShipWorksOdbcCommand cmd = dbProviderFactory.CreateOdbcCommand(query, connection);
                    Columns = new List<OdbcColumn>();

                    using (DbDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        DataTable table = reader.GetSchemaTable();

                        foreach (DataRow row in table.Rows.OfType<DataRow>())
                        {
                            Columns = Columns.Concat(new[] { new OdbcColumn(row["ColumnName"].ToString()) });
                        }

                        cmd.Cancel();
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
        }
    }
}
