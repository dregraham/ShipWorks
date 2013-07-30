using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;

using ShipWorks.SqlServer;
using ShipWorks.SqlServer.Purge;

public partial class StoredProcedures
{
    private const string PurgeDownloadAppLockName = "PurgeDownload";

    /// <summary>
    /// Purges old download records from the database.
    /// </summary>
    /// <param name="earliestRetentionDate">Indicates the date/time to use for determining
    /// which Download records will be purged. Any records with an Download.Started value earlier than
    /// this date will be purged.</param>
    /// <param name="latestExecutionTimeInUtc">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeDownload(SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeDownloadCommandText, PurgeDownloadAppLockName, earliestRetentionDateInUtc, latestExecutionTimeInUtc);              
    }

    /// <summary>
    /// The TSQL for the PurgeDownload stored procedure.
    /// </summary>
    private static string PurgeDownloadCommandText
    {
        get
        {
            return @"
            SET NOCOUNT ON;
            DECLARE 
	            @oldCount int,
	            @batchSize INT

            -- if this is the first time we've run this, figure out which resources must go
            IF OBJECT_ID('tempdb..#DownloadTemp') IS NULL
            BEGIN

	            SELECT DownloadID 
	            INTO #DownloadTemp
	            FROM Download
	            WHERE Started < @earliestRetentionDateInUtc
	
	            CREATE INDEX IX_DownloadWorking on #DownloadTemp (DownloadID)
            END

            SELECT @oldCount = COUNT(*) FROM #DownloadTemp
            IF @oldCount > 0
            BEGIN

	            DECLARE @currentBatch TABLE ( DownloadID bigint )
	
	            INSERT INTO @currentBatch
		            SELECT TOP 100 *  FROM #DownloadTemp 
		
	            WHILE @@ROWCOUNT > 0 AND (@latestExecutionTimeInUtc IS NULL OR @latestExecutionTimeInUtc > GetUtcDate())
	            BEGIN
		            BEGIN TRANSACTION
			            DELETE dd
			            FROM @currentBatch curBatch
			            INNER JOIN Downloaddetail dd
			            ON curBatch.DownloadID = dd.DownloadID
			
			            DELETE d
			            FROM @currentBatch curBatch
			            INNER JOIN Download d
			            ON d.DownloadID= curBatch.DownloadID
			
			            DELETE dt
			            FROM #DownloadTemp dt
			            INNER JOIN @currentBatch curBatch
				            ON dt.DownloadID = curBatch.DownloadID
				
		            COMMIT TRANSACTION
		
		            DELETE @currentBatch

		            --grab the next batch before ending loop
		            --Must be last statement in while loop or the @@rowcount will be an issue
		            INSERT INTO @currentBatch
			            SELECT TOP 100 *  FROM #DownloadTemp	
	            END

            END

            DROP TABLE #DownloadTemp
            ";
        }
    }
}
