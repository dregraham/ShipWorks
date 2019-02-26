﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using log4net;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
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
        public OdbcSampleDataCommand(IShipWorksDbProviderFactory dbProviderFactory, Func<Type, ILog> logFactory)
        {
            this.dbProviderFactory = dbProviderFactory;
            log = logFactory(typeof(OdbcSampleDataCommand));
        }

        /// <summary>
        /// Execute the query and return sample data
        /// </summary>
        /// <exception cref="ShipWorksOdbcException"></exception>
        public DataTable Execute(IOdbcDataSource dataSource, string query, int numberOfResults, OdbcImportStrategy odbcColumnSource)
        {
            try
            {
                using (DbConnection connection = dataSource.CreateConnection())
                {
                    connection.Open();

                    using (IShipWorksOdbcCommand cmd = dbProviderFactory.CreateOdbcCommand(query, connection))
                    {
                        cmd.AddParameter(GetParameter(odbcColumnSource));

                        using (DbDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
                        {
                            // Get an empty table that matches the schema of the query
                            DataTable resultTable = GetEmptyResultTable(reader);

                            int resultCount = 0;
                            while (reader.Read() && resultCount < numberOfResults)
                            {
                                // Create a row and populate it with values from the reader
                                DataRow row = resultTable.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[i] = reader.GetValue(i);
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
            catch (ShipWorksOdbcException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (OdbcException ex)
            {
                log.Error(ex);
                throw new ShipWorksOdbcException(
                    $"An error occurred executing the query... {Environment.NewLine}{Environment.NewLine}{ex.Message}",
                    ex);
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

        private OdbcParameter GetParameter(OdbcImportStrategy odbcImportStrategy)
        {
            switch (odbcImportStrategy)
            {
                case OdbcImportStrategy.ByModifiedTime:
                    DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow.Add(TimeSpan.FromDays(-30));
                    return new OdbcParameter("onlineLastModifiedStartingPoint", onlineLastModifiedStartingPoint);
                case OdbcImportStrategy.OnDemand:
                    int orderNumber = 0;
                    return new OdbcParameter("orderNumber", orderNumber);
                    break;
                    case OdbcImportStrategy.All:
                    default:
                        return null;
            }
        }

        /// <summary>
        /// Creates an empty result table from the reader
        /// </summary>
        private static DataTable GetEmptyResultTable(DbDataReader reader)
        {
            DataTable schemaTable = reader.GetSchemaTable();
            DataTable result = new DataTable();

            // Originally, I was using a period. This was causing blank values to show up in the result cells.
            // This fixes the issue. More info:
            // http://stackoverflow.com/questions/2940618/what-is-it-about-datatable-column-names-with-dots-that-makes-them-unsuitable-for
            string dot = "\u2024";

            foreach (DataRow row in schemaTable.Rows.OfType<DataRow>())
            {
                // For some reason when you do a union the table name comes back wrapped in []
                // strip out the [] before returning
                string tableName = row["BaseTableName"].ToString().Trim(new char[] { '[', ']' });
                string columnName = row["ColumnName"].ToString().Trim(new char[] { '[', ']' });

                string fullyQualifiedRowName = $"{tableName}{dot}{columnName}";
                if (result.Columns.Contains(fullyQualifiedRowName))
                {
                    throw new ShipWorksOdbcException($"Cannot specify a column more than once.{Environment.NewLine}{Environment.NewLine}{fullyQualifiedRowName} is in the query twice.");
                }

                result.Columns.Add(fullyQualifiedRowName);
            }

            return result;
        }
    }
}