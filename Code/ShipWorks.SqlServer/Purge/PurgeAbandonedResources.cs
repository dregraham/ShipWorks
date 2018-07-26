using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.Purge;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using ShipWorks.SqlServer;

public partial class StoredProcedures
{
    private const string PurgeAppLockName = "PurgeAbandonedResources";

    /// <summary>
    /// Purges abandoned Resource records from the database.
    /// </summary>
    /// <param name="olderThan">Not used, but included to keep with the purge pattern.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    /// <param name="softDelete">If true, resources/object references will be pointed to dummy entities.  Otherwise the full entity will be deleted.</param>
    [SqlProcedure]
    public static void PurgeAbandonedResources(SqlDateTime olderThan, SqlDateTime runUntil, SqlBoolean softDelete)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeAbandonedResourcesCommandText, PurgeAppLockName, olderThan, runUntil, softDelete);
    }

    /// <summary>
    /// The T-SQL for the PurgeAbandonedResources stored procedure.
    /// </summary>
    private static string PurgeAbandonedResourcesCommandText
    {
        get
        {
            return @"
	            SET NOCOUNT ON;
	
                -- create batch purge ID table
                CREATE TABLE #ResourcePurgeBatch (
                    ResourceID BIGINT PRIMARY KEY
                );

	            DECLARE
		            @startTime DATETIME = GETUTCDATE(),
		            @batchSize INT = 4500,
		            @batchTotal BIGINT = 0,
		            @totalSeconds INT = 0
		
	            INSERT #ResourcePurgeBatch
		            SELECT ResourceID FROM dbo.Resource with (nolock)
		            WHERE ResourceID NOT IN (SELECT objectid FROM dbo.ObjectReference with (nolock))

	            -- purge in batches while time allows
	            WHILE @runUntil IS NULL OR GETUTCDATE() < @runUntil
	            BEGIN
		            -- stop if the batch isn't expected to complete in time
		            IF (
			            @runUntil IS NOT NULL AND
			            @batchTotal > 0 AND
			            DATEADD(SECOND, @totalSeconds * @batchSize / @batchTotal, GETUTCDATE()) > @runUntil
		            )
			            BREAK;

		            DELETE FROM [dbo].[Resource] WHERE ResourceID IN (SELECT TOP (@batchSize) ResourceID FROM #ResourcePurgeBatch)

		            DELETE FROM #ResourcePurgeBatch WHERE ResourceID IN (SELECT TOP (@batchSize) ResourceID FROM #ResourcePurgeBatch)
		
		            IF NOT EXISTS (SELECT TOP 1 ResourceID FROM #ResourcePurgeBatch)
			            BREAK;

		            SET @totalSeconds = DATEDIFF(SECOND, @startTime, GETUTCDATE()) + 1;
		            PRINT 'BatchTotal: ' + CONVERT(NVARCHAR(50), @batchTotal) + ', BatchSize: ' + CONVERT(NVARCHAR(50), @batchSize) + ', TotalSeconds: ' + CONVERT(NVARCHAR(50), @totalSeconds)

		            -- update batch total and adjust batch size to an amount expected to complete in 10 seconds
		            SET @batchTotal = @batchTotal + @batchSize;
		            SET @batchSize = @batchTotal / @totalSeconds * 10;
	            END;

	            DROP TABLE #ResourcePurgeBatch
            ";
        }
    }
}
