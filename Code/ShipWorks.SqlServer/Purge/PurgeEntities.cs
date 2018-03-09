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
    /// The EntityIDsToDelete table is populated prior to calling this stored procedure.  For example,
    /// to delete orders older than a specific date, one could do:
    /// 
    /// 	SELECT o.OrderID as 'EntityID'
    ///        INTO dbo.[EntityIDsToDelete]
    ///        FROM dbo.[Order] o
    ///         WHERE o.OrderDate &lt;= '1/1/2018'
    ///        ORDER BY o.EntityID
    /// 
    ///     EXEC PurgeEntities 'Order', 'OrderID'
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
    ///     EXEC PurgeEntities 'Shipment', 'ShipmentID'
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

                //command.Parameters.Add(new SqlParameter("tableName", tableName));
                //command.Parameters.Add(new SqlParameter("tableColumnName", tableColumnName));

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

	-- create holding table object references to delete
	CREATE TABLE #DeleteObjectReferences (
		EntityID BIGINT PRIMARY KEY
	);

	DECLARE
		@startTime DATETIME = GETUTCDATE(),
		@batchSize INT = 4500,
		@batchTotal BIGINT = 1,
		@totalSeconds INT = 1
		
	INSERT INTO #DeletePurgeBatch (EntityID)
		SELECT EntityID FROM [EntityIDsToDelete] with (nolock)
			
	-- purge in batches while time allows
	WHILE EXISTS (SELECT TOP 1 EntityID FROM #DeletePurgeBatch)
		BEGIN
			-- Populate object reference holding table
			INSERT INTO #DeleteObjectReferences (EntityID)
				SELECT @tablePrimaryKeyName@ FROM [dbo].[@tableName@] WHERE @tableColumnName@ IN (SELECT TOP (@batchSize) EntityID FROM #DeletePurgeBatch)

			print 'Deleting from @tableName@'
			DELETE FROM [dbo].[@tableName@] WHERE @tableColumnName@ IN (SELECT TOP (@batchSize) EntityID FROM #DeletePurgeBatch)
				
			DELETE FROM #DeletePurgeBatch WHERE EntityID IN (SELECT TOP (@batchSize) EntityID FROM #DeletePurgeBatch)

			DELETE FROM ObjectReference WHERE ConsumerID in (SELECT EntityID from #DeleteObjectReferences);

			TRUNCATE TABLE #DeleteObjectReferences
		
			IF NOT EXISTS (SELECT TOP 1 EntityID FROM #DeletePurgeBatch)
			BEGIN
				BREAK;
			END

			SET @totalSeconds = DATEDIFF(SECOND, @startTime, GETUTCDATE()) + 1;
			PRINT 'BatchTotal: ' + CONVERT(NVARCHAR(50), @batchTotal) + ', BatchSize: ' + CONVERT(NVARCHAR(50), @batchSize) + ', TotalSeconds: ' + CONVERT(NVARCHAR(50), @totalSeconds)

			-- update batch total and adjust batch size to an amount expected to complete in 10 seconds
			SET @batchTotal = @batchTotal + @batchSize;
			SET @batchSize = @batchTotal / @totalSeconds * 10;
				
			--select @batchTotal as 'batchTotal'
			--select @batchSize as 'batchSize'
		END;
END
            ";
        }
    }
}
