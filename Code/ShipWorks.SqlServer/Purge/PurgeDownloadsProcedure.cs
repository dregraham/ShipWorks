using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Purge;

public partial class StoredProcedures
{
    private const string PurgeDownloadsAppLockName = "PurgeDownloads";

    /// <summary>
    /// Purges PrintResult resource data from the database.
    /// </summary>
    /// <param name="olderThan">Indicates the date/time to use for determining
    /// which Download/DownloadDetail records will be purged. Any records with an Download.Started value earlier than
    /// this date will be purged.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeDownloads(SqlDateTime olderThan, SqlDateTime runUntil)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeDownloadsCommandText, PurgeDownloadsAppLockName, olderThan, runUntil);
    }

    /// <summary>
    /// The T-SQL for the PurgeDownloads stored procedure.
    /// </summary>
    private static string PurgeDownloadsCommandText
    {
        get
        {
            return @"
    SET NOCOUNT ON;

    IF OBJECT_ID('tempdb.dbo.#DownloadPurgeBatch', 'U') IS NOT NULL
        DROP TABLE #DownloadPurgeBatch; 
	    
    -- create batch purge ID table
    CREATE TABLE #DownloadPurgeBatch (
        DownloadID BIGINT PRIMARY KEY
    );

    INSERT #DownloadPurgeBatch
	    SELECT DISTINCT DownloadID FROM dbo.Download with (nolock)
	    WHERE Download.[Started] < @olderThan 
	    
    DECLARE
	    @startTime DATETIME = GETUTCDATE(),
	    @batchSize INT = 4500,
	    @batchTotal BIGINT = 0,
	    @totalSeconds INT = 0
		    
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

	    BEGIN TRAN
		    DELETE FROM [dbo].[DownloadDetail] WHERE DownloadID IN (SELECT TOP (@batchSize) DownloadID FROM #DownloadPurgeBatch)

		    DELETE FROM [dbo].[Download] WHERE DownloadID IN (SELECT TOP (@batchSize) DownloadID FROM #DownloadPurgeBatch)

		    DELETE FROM #DownloadPurgeBatch WHERE DownloadID IN (SELECT TOP (@batchSize) DownloadID FROM #DownloadPurgeBatch)
	    COMMIT TRAN

	    IF NOT EXISTS (SELECT DownloadID FROM #DownloadPurgeBatch)
		    BREAK;

	    SET @totalSeconds = DATEDIFF(SECOND, @startTime, GETUTCDATE()) + 1;
	    PRINT 'BatchTotal: ' + CONVERT(NVARCHAR(50), @batchTotal) + ', BatchSize: ' + CONVERT(NVARCHAR(50), @batchSize) + ', TotalSeconds: ' + CONVERT(NVARCHAR(50), @totalSeconds)

	    -- update batch total and adjust batch size to an amount expected to complete in 10 seconds
	    SET @batchTotal = @batchTotal + @batchSize;
	    SET @batchSize = @batchTotal / @totalSeconds * 10;
    END;

    DROP TABLE #DownloadPurgeBatch;
            ";
        }
    }
}
