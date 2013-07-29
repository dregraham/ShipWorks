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
    /// <param name="earliestRetentionDate">Indicates the date/time to use for determining
    /// which Audit records will be purged. Any records with an Audit.Date value earlier than
    /// this date will be purged.</param>
    /// <param name="latestExecutionTimeInUtc">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeAudit(SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeAuditCommandText, PurgeAuditAppLockName, earliestRetentionDateInUtc, latestExecutionTimeInUtc);
    }

    /// <summary>
    /// The T-SQL for the PurgeAudit stored procedure.
    /// </summary>
    private static string PurgeAuditCommandText
    {
        get { return @"
            SET NOCOUNT on
            DECLARE 
                @RecordsToBeDeleted INT,
                @BatchSize INT
            
            SET @BatchSize = 100

            CREATE TABLE #AuditTemp (AuditID BIGINT)	
            INSERT INTO #AuditTemp
            (
                AuditID
            )
            SELECT AuditID FROM Audit
            WHERE [DATE] < @earliestRetentionDateInUtc
            SELECT @RecordsToBeDeleted = @@ROWCOUNT     

            CREATE INDEX IX_ResourceWorking on #AuditTemp (AuditID)      

            IF @RecordsToBeDeleted > 0
            BEGIN

                DECLARE @CurrentBatch TABLE ( AuditID BIGINT )	
                DECLARE @DeletedAuditIds TABLE ( AuditChangeID BIGINT )
    
                INSERT INTO @CurrentBatch (	AuditID )
                    SELECT TOP (@BatchSize) AuditID  FROM #AuditTemp 		
        
                -- Honor the hard stop if one is provided
                WHILE (@@ROWCOUNT > 0) AND (@latestExecutionTimeInUtc > GETUTCDATE())
                BEGIN

                    BEGIN TRANSACTION
                        DELETE AuditChange
                        OUTPUT deleted.AuditChangeID into @DeletedAuditIds
                        FROM @CurrentBatch batch
                        INNER JOIN AuditChange 
                        ON AuditChange.AuditID = batch.AuditID
            
                        DELETE AuditChangeDetail
                        FROM @DeletedAuditIds deleted
                        INNER JOIN AuditChangeDetail
                        ON AuditChangeDetail.AuditChangeID = deleted.AuditChangeID
            
                        DELETE Audit
                        FROM @CurrentBatch batch
                        INNER JOIN Audit
                        ON Audit.AuditID = batch.AuditID
            
                        DELETE #AuditTemp
                        FROM #AuditTemp
                        INNER JOIN @CurrentBatch batch
                            ON #AuditTemp.AuditID = batch.AuditID
                    COMMIT TRANSACTION
        
        
                    DELETE @CurrentBatch
                    DELETE @DeletedAuditIds

                    -- Grab the next batch before ending loop
                    -- Must be last statement in while loop or the @@ROWCOUNT will be an issue
                    INSERT INTO @CurrentBatch ( AuditID	)
                        SELECT TOP (@BatchSize) AuditID  FROM #AuditTemp 
                END

            END
            DROP TABLE #AuditTemp
            ";
        }
    }
}
