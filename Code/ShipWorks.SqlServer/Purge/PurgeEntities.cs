using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public partial class StoredProcedures
{
    private static readonly string purgeAppLockName = "PurgeEntitiesAppLock";

    /// <summary>
    /// Purges abandoned Resource records from the database.
    /// </summary>
    /// <param name="tableName">Name of the table from which to delete rows.</param>
    /// <param name="tableColumnName">Column name for matching against the EntityIDsToDelete table.
    /// <param name="tablePrimaryKeyName">Primary key column name of the table for which we are deleting.  
    /// Used for filtering down rows specific to tableName when matching against the EntityIDsToDelete table.
    /// 
    /// The EntityIDsToDelete table is populated prior to calling this stored procedure.  For example,
    /// to delete orders older than a specific date, one could do:
    /// 
    /// 	SELECT o.OrderID as 'EntityID'
    ///        INTO dbo.[EntityIDsToDelete]
    ///        FROM dbo.[Order] o
    ///         WHERE o.OrderDate &lt;= '1/1/2018'
    ///        ORDER BY o.EntityID
    /// 
    ///     EXEC PurgeEntities 'Order', 'OrderID', 'OrderID'
    /// 
    /// This is partly a security feature so that you can't just run this procedure without having populated
    /// the holding table.
    /// 
    /// It also makes this proc generic, so you could do a similar query to delete Shipments:
    /// 
    /// 	SELECT s.ShipmentID as 'EntityID'
    ///        INTO dbo.[EntityIDsToDelete]
    ///        FROM dbo.[Order] o, dbo.[Shipment] s
    ///         WHERE s.OrderID = o.OrderID
    ///           AND o.OrderDate &lt;= '1/1/2018'
    ///        ORDER BY o.EntityID
    /// 
    ///     EXEC PurgeEntities 'Shipment', 'ShipmentID', 'ShipmentID'
    /// 
    /// </param>
    [SqlProcedure]
    public static void PurgeEntities(SqlString tableName, SqlString tableColumnName, SqlString tablePrimaryKeyName)
    {
        RunPurgeScript(tableName, tableColumnName, tablePrimaryKeyName);
    }

    /// <summary>
    /// Runs a sql script. 
    /// </summary>
    private static void RunPurgeScript(SqlString tableName, SqlString tableColumnName, SqlString tablePrimaryKeyName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            SqlAppLockUtility.RunLockedCommand(connection, purgeAppLockName, com =>
            {
                SqlCommand command = (SqlCommand) com;

                command.CommandText = PurgeEntitiesCommandText
                    .Replace("@tableName@", tableName.ToString())
                    .Replace("@tableColumnName@", tableColumnName.ToString())
                    .Replace("@tablePrimaryKeyName@", tablePrimaryKeyName.ToString());

                // Use ExecuteAndSend instead of ExecuteNonQuery when debugging to see output printed
                // to the console of client (i.e. SQL Management Studio)
                if (SqlContext.Pipe != null)
                {
                    SqlContext.Pipe.ExecuteAndSend(command);
                }
                else
                {
                    command.ExecuteNonQuery();
                }
            });
        }
    }

    /// <summary>
    /// The T-SQL for the PurgeEntities stored procedure.
    /// </summary>
    private static string PurgeEntitiesCommandText
    {
        get
        {
            return @"
SET NOCOUNT ON;

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
BEGIN
	IF OBJECT_ID('tempdb..#DeletePurgeBatch') IS NOT NULL
	BEGIN
		DROP TABLE #DeletePurgeBatch
	END

	IF OBJECT_ID('tempdb..#DeleteObjectReferences') IS NOT NULL
	BEGIN
		DROP TABLE #DeleteObjectReferences
	END

	-- create batch purge ID table
	CREATE TABLE #DeletePurgeBatch (
		EntityID BIGINT PRIMARY KEY
	);

	DECLARE
		@startTime DATETIME = GETUTCDATE(),
		@totalSeconds INT = 1
		
	INSERT INTO #DeletePurgeBatch (EntityID)
		SELECT DISTINCT @tablePrimaryKeyName@ FROM [dbo].[@tableName@] WHERE @tableColumnName@ IN (SELECT EntityID FROM [EntityIDsToDelete] with (nolock))
			
	print 'Deleting from @tableName@'
	DELETE FROM [dbo].[@tableName@] WHERE @tablePrimaryKeyName@ IN (SELECT EntityID FROM #DeletePurgeBatch)
				
	DELETE FROM ObjectReference WHERE ConsumerID in (SELECT EntityID from #DeletePurgeBatch);

	SET @totalSeconds = DATEDIFF(SECOND, @startTime, GETUTCDATE()) + 1;
	PRINT 'TotalSeconds: ' + CONVERT(NVARCHAR(50), @totalSeconds)
		
    -- Allow SQL Server write it's transaction buffer to disk.  This helps minimize the final log size.
	CHECKPOINT;
END
            ";
        }
    }
}
