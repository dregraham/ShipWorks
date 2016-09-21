using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Administration.Indexing
{
    /// <summary>
    /// Responsible for getting the indexes that need to be rebuilt and has the ability to rebuild the indexes.
    /// </summary>
    public class IndexMonitor : IIndexMonitor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(IndexMonitor));
        private const int timeoutHours = 3;
        private static readonly int timeoutSeconds = (int) TimeSpan.FromHours(timeoutHours).TotalSeconds;
        private static string sqlConnectionString;

        /// <summary>
        /// Intended to obtain a collection of indexes that need to be
        /// rebuilt based on performance history.
        /// </summary>
        public IEnumerable<TableIndex> GetIndexesToRebuild()
        {
            List<TableIndex> indexesToRebuild = new List<TableIndex>();

            log.Debug("Starting determinging indexes to rebuild.");

            using (new LoggedStopwatch(log, "Finished determinging indexes to rebuild."))
            {
                using (DbConnection con = DataAccessAdapter.CreateConnection(ConnectionString))
                {
                    con.Open();

                    string sql = @"
                    --Table to hold results
                    DECLARE @tablevar TABLE(lngid INT IDENTITY(1,1), objectid INT, index_id INT)

                    INSERT INTO @tablevar (objectid, index_id)
                         SELECT [object_id],index_id
                         FROM sys.dm_db_index_physical_stats (DB_ID(db_name())
                              ,NULL -- NULL to view all tables
                              ,NULL -- NULL to view all indexes; otherwise, input index number
                              ,NULL -- NULL to view all partitions of an index
                              ,'LIMITED') --We want all information
                         WHERE
                             avg_fragmentation_in_percent > 15 -- Logical fragmentation
                         AND page_count > 8 -- We do not want indexes less than 1 extent in size
                         AND index_id NOT IN (0) --Only clustered and nonclustered indexes

                    SELECT distinct '[' + OBJECT_NAME(objectid) + ']' as 'TableName', ind.[name] as 'IndexName'
                    FROM @tablevar tv
                    INNER JOIN sys.indexes ind
                    ON tv.objectid = ind.[object_id]
                    AND tv.index_id = ind.index_id
                    INNER JOIN sys.objects ob
                    ON tv.objectid = ob.[object_id]
                    INNER JOIN sys.schemas sc
                    ON sc.schema_id = ob.schema_id";

                    try
                    {
                        using (DbCommand sqlCommand = con.CreateCommand())
                        {
                            sqlCommand.CommandTimeout = timeoutSeconds;
                            sqlCommand.CommandText = sql;
                            DbDataReader resultsToRebuild = sqlCommand.ExecuteReader();

                            while (resultsToRebuild.Read())
                            {
                                indexesToRebuild.Add(new TableIndex()
                                {
                                    TableName = resultsToRebuild.GetString(0),
                                    IndexName = resultsToRebuild.GetString(1)
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        log.Error("An Error occured when attempting to get tables to rebuild.", ex);
                    }
                }
            }

            return indexesToRebuild;
        }

        /// <summary>
        /// Rebuilds the given table index.
        /// </summary>
        public void RebuildIndex(TableIndex index)
        {
            string rebuildIndexSql = string.Format("exec RebuildTableIndex {0}, {1}", index.TableName, index.IndexName);

            log.Debug(string.Format("Starting rebuilding index.  SQL Statement: {0}", rebuildIndexSql));

            using (new LoggedStopwatch(log, string.Format("Finished rebuilding index.  SQL Statement: {0}", rebuildIndexSql)))
            {
                using (DbConnection sqlConnection = DataAccessAdapter.CreateConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    using (DbCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandTimeout = timeoutSeconds;
                        sqlCommand.CommandText = rebuildIndexSql;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a connection string, based on SqlAdapter.Default.ConnectionString, and modifies it to have a new
        /// number of minutes for the timeout.
        /// </summary>
        private static string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(SqlAdapter.Default.ConnectionString);
                    sqlConnectionStringBuilder.ConnectTimeout = timeoutSeconds;
                    sqlConnectionString = sqlConnectionStringBuilder.ConnectionString;
                }

                return sqlConnectionString;
            }
        }
    }
}