using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;

using ShipWorks.SqlServer;

public partial class StoredProcedures
{
    private const string AuditPurgeAppLockName = "PurgeAuditLock";

    /// <summary>
    /// Purges old audit records from the database.
    /// </summary>
    /// <param name="earliestRetentionDate">Indicates the date/time to use for determining
    /// which Audit records will be purged. Any records with an Audit.Date value earlier than
    /// this date will be purged.</param>
    [SqlProcedure]
    public static void PurgeAudit(SqlDateTime earliestRetentionDate)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            try
            {
                if (!SqlAppLockUtility.IsLocked(connection, AuditPurgeAppLockName))
                {
                    if (SqlAppLockUtility.AcquireLock(connection, AuditPurgeAppLockName))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = AuditPurgeCommandText;

                        command.Parameters.Add(new SqlParameter("@RetentionDate", earliestRetentionDate));

                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                else
                {
                    // Let the caller know that someone else is already running the purge. (It may
                    // be beneficial to create/throw a more specific exception.)
                    throw new Exception("Could not acquire applock for for purging audit logs.");
                }
            }
            finally
            {
                SqlAppLockUtility.ReleaseLock(connection, AuditPurgeAppLockName);
            }
        }
    }

    private static string AuditPurgeCommandText
    {
        get { return @"
            DECLARE 
                @RecordsToBeDeleted INT,
                @BatchSize INT

            -- if this is the first time we've run this, figure out which resources must go
            IF OBJECT_ID('tempdb..#AuditTemp') IS NULL
            BEGIN
                CREATE TABLE #AuditTemp (AuditID int)	
                CREATE INDEX IX_ResourceWorking on #AuditTemp (AuditID)
            END

            INSERT INTO #AuditTemp
            (
                AuditID
            )
            SELECT AuditID FROM Audit
            WHERE [DATE] < @RetentionDate
    

            SELECT @RecordsToBeDeleted = COUNT(0) FROM #AuditTemp
            IF @RecordsToBeDeleted > 0
            BEGIN

                DECLARE @CurrentBatch TABLE ( AuditID BIGINT )	
                DECLARE @DeletedAuditIds TABLE ( AuditChangeID BIGINT )
    
                INSERT INTO @CurrentBatch (	AuditID )
                    SELECT TOP 100 AuditID  FROM #AuditTemp 		
        
                WHILE @@ROWCOUNT > 0
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

                    --grab the next batch before ending loop
                    --Must be last statement in while loop or the @@ROWCOUNT will be an issue
                    INSERT INTO @CurrentBatch ( AuditID	)
                        SELECT TOP 100 AuditID  FROM #AuditTemp 		
                END

            END

            DROP TABLE #AuditTemp";
        }
    }
}
