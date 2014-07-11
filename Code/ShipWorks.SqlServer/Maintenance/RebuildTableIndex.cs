using System;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.Purge;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using ShipWorks.SqlServer;

public partial class StoredProcedures
{
    /// <summary>
    /// Rebuilds a table's specified index.
    /// </summary>
    /// <param name="tableName">Name of the table to which the specified index belongs.</param>
    /// <param name="indexName">The index name to rebuild for the specified table.</param>
    [SqlProcedure]
    public static void RebuildTableIndex(string tableName, string indexName)
    {
        string rebuildSql = string.Empty;

        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandTimeout = (int)TimeSpan.FromHours(3).TotalSeconds;
            cmd.CommandText = SqlToGenerateRebuildScript();

            cmd.Parameters.AddWithValue("@TableName", tableName);
            cmd.Parameters.AddWithValue("@IndexName", indexName);

            using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
            {
                sqlDataReader.Read();
                rebuildSql = sqlDataReader[0].ToString();
            }

            cmd.CommandText = rebuildSql;
            cmd.ExecuteNonQuery();
        }
    }

    private static string SqlToGenerateRebuildScript()
    {
        return @"
declare @EngineEdition sql_variant

SELECT @EngineEdition = SERVERPROPERTY ('EngineEdition') 

if @EngineEdition = 3
	BEGIN
		;with NonPkIndexes as 
		(
			select i.index_id, i.name as 'IndexName', c.column_id, c.name as 'ColumnName', t.name as 'TypeName', t.is_assembly_type, c.max_length
			from sys.indexes i, sys.index_columns ic, sys.columns c, sys.types t
			where i.object_id = ic.object_id and ic.index_id = i.index_id
			  and ic.object_id = c.object_id and ic.column_id = c.column_id
			  and t.user_type_id = c.user_type_id
			  and i.is_primary_key = 0
			  and c.object_id = OBJECT_ID(@tableName)
			  and i.name = @IndexName
		  ),
		PkIndexes as 
		(
			select i.index_id, i.name as 'IndexName', c.column_id, c.name as 'ColumnName', t.name as 'TypeName', t.is_assembly_type, c.max_length
			from sys.columns c, sys.types t, sys.indexes i
			where i.object_id = c.object_id
			  and t.user_type_id = c.user_type_id
			  and i.is_primary_key = 1
			  and c.object_id = OBJECT_ID(@tableName)
			  and i.name = @IndexName
		  ),
		AllIndexes as
		(
			select * from NonPkIndexes
			union
			select * from PkIndexes
		),
		RebuildIndexesOffline as
		(
			select distinct IndexName, '' as RebuildOption
			from AllIndexes
			where 
				is_assembly_type = 1
			  or TypeName in ('text', 'ntext', 'image', 'xml')
			  or (max_length = -1 AND TypeName in ('varchar', 'nvarchar', 'varbinary'))
		),
		RebuildIndexesOnline as
		(
			select distinct IndexName, ' WITH (ONLINE = ON)' as RebuildOption
			from AllIndexes
			where IndexName not in (select IndexName from RebuildIndexesOffline)
		),
		RebuildSql as
		(
			select 'ALTER INDEX ' + IndexName + ' on dbo.[' + @tableName + '] REBUILD ' + RebuildOption AS 'RebuildSql' from RebuildIndexesOnline
			Union 
			select 'ALTER INDEX ' + IndexName + ' on dbo.[' + @tableName + '] REBUILD ' + RebuildOption AS 'RebuildSql' from RebuildIndexesOffline
		)
		select * from RebuildSql
	END
ELSE
	BEGIN
		select 'ALTER INDEX ' + @IndexName + ' on dbo.[' + @tableName + '] REBUILD ' AS 'RebuildSql'
	END";
    }
}


