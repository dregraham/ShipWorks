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
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeDownload(SqlDateTime olderThan, SqlDateTime runUntil)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeDownloadCommandText, PurgeDownloadAppLockName, olderThan, runUntil);              
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

                -- create batch purge ID table
                CREATE TABLE #DownloadPurgeBatch (
                    DownloadID BIGINT PRIMARY KEY
                );

                DECLARE
                    @startTime DATETIME = GETUTCDATE(),
                    @batchSize INT = 1000,
                    @batchTotal BIGINT = 0;

                -- purge in batches while time allows
                WHILE @runUntil IS NULL OR GETUTCDATE() < @runUntil
                BEGIN
                    INSERT #DownloadPurgeBatch
                    SELECT TOP (@batchSize) DownloadID
                    FROM Download
                    WHERE Started < @olderThan;

                    SET @batchSize = @@ROWCOUNT;
                    IF @batchSize = 0
                        BREAK;

                    DECLARE @totalSeconds INT = DATEDIFF(SECOND, @startTime, GETUTCDATE()) + 1;

                    -- stop if the batch isn't expected to complete in time
                    IF (
                        @runUntil IS NOT NULL AND
                        @batchTotal > 0 AND
                        DATEADD(SECOND, @totalSeconds * @batchSize / @batchTotal, GETUTCDATE()) > @runUntil
                    )
                        BREAK;

                    BEGIN TRANSACTION;

                    DELETE DownloadDetail
                    FROM DownloadDetail dd
                    INNER JOIN #DownloadPurgeBatch b ON
                        b.DownloadID = dd.DownloadID;

                    DELETE Download
                    FROM Download d
                    INNER JOIN #DownloadPurgeBatch b ON
                        b.DownloadID = d.DownloadID;

                    COMMIT TRANSACTION;

                    TRUNCATE TABLE #DownloadPurgeBatch;

                    -- update batch total and adjust batch size to an amount expected to complete in 10 seconds
                    SET @batchTotal = @batchTotal + @batchSize;
                    SET @batchSize = @batchTotal / @totalSeconds * 10;
                END;
            ";
        }
    }
}
