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
    private const string DownloadPurgeAppLockName = "PurgeDownloadEntries";

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
    public static void PurgeDownloads(SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            try
            {
                // Need to have an open connection for the duration of the lock acquisition/release
                connection.Open();

                if (!SqlAppLockUtility.IsLocked(connection, DownloadPurgeAppLockName))
                {
                    if (SqlAppLockUtility.AcquireLock(connection, DownloadPurgeAppLockName))
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = DownloadPurgeCommandText;

                            command.Parameters.Add(new SqlParameter("@RetentionDateInUtc", earliestRetentionDateInUtc));
                            command.Parameters.Add(new SqlParameter("@LatestExecutionTimeInUtc", latestExecutionTimeInUtc));
                            command.ExecuteNonQuery();

                            // Use ExecuteAndSend instead of ExecuteNonQuery when debuggging to see output printed 
                            // to the console of client (i.e. SQL Management Studio)
                            //SqlContext.Pipe.ExecuteAndSend(command);
                        }
                    }
                }
                else
                {
                    // Let the caller know that someone else is already running the purge. (It may
                    // be beneficial to create/throw a more specific exception.)
                    throw new PurgeException("Could not acquire applock for purging downloads.");                    
                }
            }
            finally
            {
                SqlAppLockUtility.ReleaseLock(connection, DownloadPurgeAppLockName);
            }
        }
    }

    /// <summary>
    /// The TSQL for the PurgeDownload stored procedure.
    /// </summary>
    private static string DownloadPurgeCommandText
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
	            WHERE Started < @RetentionDateInUtc
	
	            CREATE INDEX IX_DownloadWorking on #DownloadTemp (DownloadID)
            END

            SELECT @oldCount = COUNT(*) FROM #DownloadTemp
            IF @oldCount > 0
            BEGIN

	            DECLARE @currentBatch TABLE ( DownloadID bigint )
	
	            INSERT INTO @currentBatch
		            SELECT TOP 100 *  FROM #DownloadTemp 
		
	            WHILE @@ROWCOUNT > 0 AND (@LatestExecutionTimeInUtc > GetUtcDate())
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
