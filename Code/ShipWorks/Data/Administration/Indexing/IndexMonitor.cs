using System.Collections.Generic;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.Indexing
{
    /// <summary>
    /// Responsible for getting the indexes that need to be rebuilt and has the ability to rebuild the indexes.
    /// </summary>
    public class IndexMonitor : IIndexMonitor
    {
        /// <summary>
        /// Intended to obtain a collection of indexes that need to be
        /// rebuilt based on performance history.
        /// </summary>
        public IEnumerable<TableIndex> GetIndexesToRebuild()
        {
            List<TableIndex> indexesToRebuild = new List<TableIndex>();

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                string sql = @"
                --Table to hold results 
                DECLARE @tablevar TABLE(lngid INT IDENTITY(1,1), objectid INT, index_id INT) 
 
                INSERT INTO @tablevar (objectid, index_id) 
                     SELECT [object_id],index_id 
                     FROM sys.dm_db_index_physical_stats (DB_ID(db_name()) 
                          ,NULL -- NULL to view all tables 
                          ,NULL -- NULL to view all indexes; otherwise, input index number 
                          ,NULL -- NULL to view all partitions of an index 
                          ,'DETAILED') --We want all information 
                     WHERE ((avg_fragmentation_in_percent > 15) -- Logical fragmentation 
                     OR (avg_page_space_used_in_percent < 60)) --Page density 
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

                SqlDataReader resultsToRebuild = SqlCommandProvider.ExecuteReader(con, sql);

                while (resultsToRebuild.Read())
                {
                    indexesToRebuild.Add(new TableIndex() {
                        TableName = resultsToRebuild.GetString(0),
                        IndexName = resultsToRebuild.GetString(1)
                    });
                }
            }

            return indexesToRebuild;
        }

        /// <summary>
        /// Rebuilds the given table index.
        /// </summary>
        public void RebuildIndex(TableIndex index)
        {
            using (DataAccessAdapter adapter = new DataAccessAdapter())
            {
                adapter.CallActionStoredProcedure("RebuildTableIndexProcedure", new SqlParameter[]
                {
                    new SqlParameter("@tableName", index.TableName),
                    new SqlParameter("@indexName", index.IndexName)
                });
            }
        }
    }
}