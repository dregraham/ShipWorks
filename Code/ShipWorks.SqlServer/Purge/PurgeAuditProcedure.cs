using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.Purge;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using ShipWorks.SqlServer;


public partial class StoredProcedures
{
    private const string PurgeAuditAppLockName = "PurgeAudit";

    /// <summary>
    /// Purges old audit records from the database.
    /// </summary>
    /// <param name="olderThan">Indicates the date/time to use for determining
    /// which Audit records will be purged. Any records with an Audit.Date value earlier than
    /// this date will be purged.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeAudit(SqlDateTime olderThan, SqlDateTime runUntil)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeAuditCommandText, PurgeAuditAppLockName, olderThan, runUntil);
    }

    /// <summary>
    /// The T-SQL for the PurgeAudit stored procedure.
    /// </summary>
    private static string PurgeAuditCommandText
    {
        get
        {
            return @"
                SET NOCOUNT ON;

                -- create batch purge ID table
                CREATE TABLE #AuditPurgeBatch (
                    AuditID BIGINT PRIMARY KEY
                );

                DECLARE
                    @startTime DATETIME = GETUTCDATE(),
                    @batchSize INT = 1000,
                    @batchTotal BIGINT = 0;

                -- purge in batches while time allows
                WHILE @runUntil IS NULL OR GETUTCDATE() < @runUntil
                BEGIN
                    INSERT #AuditPurgeBatch
                    SELECT TOP (@batchSize) AuditID
                    FROM Audit
                    WHERE [Date] < @olderThan;

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

                    DELETE AuditChangeDetail
                    FROM AuditChangeDetail acd
                    INNER JOIN AuditChange ac ON
                        ac.AuditChangeID = acd.AuditChangeID
                    INNER JOIN #AuditPurgeBatch b ON
                        b.AuditID = ac.AuditID;

                    DELETE ObjectLabel
                    FROM AuditChange ac INNER JOIN #AuditPurgeBatch b ON b.AuditID = ac.AuditID
					INNER JOIN ObjectLabel ol ON ac.ObjectID = ol.ObjectID AND ol.IsDeleted = 1

                    DELETE AuditChange
                    FROM AuditChange ac
                    INNER JOIN #AuditPurgeBatch b ON
                        b.AuditID = ac.AuditID;

                    DELETE Audit
                    FROM Audit a
                    INNER JOIN #AuditPurgeBatch b ON
                        b.AuditID = a.AuditID;

                    COMMIT TRANSACTION;

                    TRUNCATE TABLE #AuditPurgeBatch;

                    -- update batch total and adjust batch size to an amount expected to complete in 10 seconds
                    SET @batchTotal = @batchTotal + @batchSize;
                    SET @batchSize = @batchTotal / @totalSeconds * 10;
                END;
            ";
        }
    }
}
