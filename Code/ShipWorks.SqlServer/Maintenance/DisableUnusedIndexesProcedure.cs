using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public partial class StoredProcedures
{
    /// <summary>
    /// Disables ShipWorks default indexes if they haven't been used in a specified period of time.
    /// </summary>
    /// <param name="daysBack">Indicates the number of days to use for determining
    /// which indexes will be disabled.</param>
    [SqlProcedure]
    public static void DisableUnusedIndexes(SqlInt16 daysBack)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Execute the script
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = CommandText;
            cmd.Parameters.AddWithValue("@daysBack", daysBack);
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// The T-SQL for the DisableUnusedIndexes stored procedure.
    /// </summary>
    private static string CommandText
    {
        get
        {
            return @"
                DECLARE @maxDbDate DATETIME 
                DECLARE @daysBackDate DATETIME
                DECLARE @disableIndexSql NVARCHAR(MAX)
                SET @disableIndexSql = ''

                IF (@daysBack <= 0)
                BEGIN
	                SET @daysBackDate = GETDATE();
                END
                ELSE
                BEGIN
	                SET @daysBackDate = DATEADD(day, -@daysBack, GETDATE());
                END

                ;WITH DatabaseDates AS
                (
	                SELECT max(restore_date) AS DbDate FROM msdb.dbo.restorehistory WHERE destination_database_name = DB_NAME()
	                UNION
	                SELECT create_date FROM sys.databases WHERE [name] = DB_NAME()
	                UNION
	                SELECT sqlserver_start_time FROM sys.dm_os_sys_info
                )
                SELECT @maxDbDate = MAX(DbDate) FROM DatabaseDates

                SELECT @disableIndexSql =
	                (
		                SELECT indexUsage.DisableIndex + ';' + CHAR(10) 
		                FROM ShipWorksIndexUsage indexUsage
		                WHERE indexUsage.IsPrimaryKey = 0
		                  and indexUsage.HasConstraintColumn = 0
		                  and indexUsage.IsUnique = 0
		                  and indexUsage.IsEnabled = 1
		                  and indexUsage.UserQueries <= 50
		                  and indexUsage.IndexName like 'IX_SWDefault_%'
		                  and indexUsage.TableName not like 'Action%'
		                  and indexUsage.TableName not like 'Audit%'
		                  and indexUsage.TableName not like 'Download%'
		                  and indexUsage.TableName not like 'Email%'
		                  and indexUsage.TableName not like 'Filter%'
		                  and indexUsage.TableName not like 'ObjectLabel%'
		                  and indexUsage.TableName not like 'ObjectReference%'
		                  and indexUsage.TableName not like 'PrintResult%'
		                  and indexUsage.TableName not like 'Scheduling%'
		                  and (indexUsage.MaxLastQueryDate is null or indexUsage.MaxLastQueryDate <= @daysBackDate)
		                  and (@maxDbDate <= @daysBackDate)
		                FOR XML PATH('')
	                )

                EXEC sp_executesql @disableIndexSql;
            ";
        }
    }
}
