using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Purge;
using System.Data.SqlTypes;

public partial class StoredProcedures
{
    private const string PurgeEmailOutboundAppLockName = "PurgeEmailOutbound";

    /// <summary>
    /// Purges EmailOutbound resource data from the database.
    /// </summary>
    /// <param name="olderThan">Indicates the date/time to use for determining
    /// which EmailOutbound records will be purged. Any records with an EmailOutbound.SentDate value earlier than
    /// this date will be purged.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    /// <param name="softDelete">If true, resources/object references will be pointed to dummy entities.  Otherwise the full entity will be deleted.</param>
    [SqlProcedure]
    public static void PurgeEmailOutbound(SqlDateTime olderThan, SqlDateTime runUntil, SqlBoolean softDelete)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeEmailOutboundCommandText, PurgeEmailOutboundAppLockName, olderThan, runUntil, softDelete);             
    }

    /// <summary>
    /// The T-SQL for the PurgeEmailOutbound stored procedure.
    /// </summary>
    private static string PurgeEmailOutboundCommandText
    {
        get
        {
            return @"
                SET NOCOUNT ON;

                -- find or create the 'deleted' resource record
                DECLARE @deletedPlainResourceID BIGINT = (SELECT ResourceID FROM [Resource] WHERE [Filename] = '__purged_email_plain.swr');

                IF @deletedPlainResourceID IS NULL
                BEGIN
                    INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename])  VALUES (0x1A, 0x1A, 0, '__purged_email_plain.swr');

                    SET @deletedPlainResourceID = SCOPE_IDENTITY();
                END;

                -- find or create the 'deleted' HTML resource record
                DECLARE @deletedHtmlResourceID BIGINT = (SELECT ResourceID FROM [Resource] WHERE [Filename] = '__purged_email_html_swr');

                IF @deletedHtmlResourceID IS NULL
                BEGIN
                    INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES (0x1B, 0x1B, 0, '__purged_email_html_swr');

                    SET @deletedHtmlResourceID = SCOPE_IDENTITY();
                END;
                        
                IF OBJECT_ID('tempdb.dbo.#EmailPurgeBatch', 'U') IS NOT NULL
                    DROP TABLE #EmailPurgeBatch; 
                
                -- create batch purge ID table
                CREATE TABLE #EmailPurgeBatch 
                (
                    EmailOutboundID BIGINT,
                    PlainPartResourceID BIGINT,
                    HtmlPartResourceID BIGINT
                );

                DECLARE
                    @startTime DATETIME = GETUTCDATE(),
                    @batchSize INT = 1000,
                    @batchTotal BIGINT = 0;

                -- purge in batches while time allows
                WHILE @runUntil IS NULL OR GETUTCDATE() < @runUntil
                BEGIN
                    INSERT #EmailPurgeBatch
                    SELECT DISTINCT TOP (@batchSize) e.EmailOutboundID, e.PlainPartResourceID, e.HtmlPartResourceID
                    FROM EmailOutbound e
                    INNER JOIN ObjectReference o ON
                        o.ObjectReferenceID = e.PlainPartResourceID
                        OR o.ObjectReferenceID = e.HtmlPartResourceID
                    WHERE
                        e.SentDate < @olderThan AND
                        e.SendStatus = 1 AND  -- only purge emails that have been sent
                        o.ObjectID not in ( @deletedPlainResourceID, @deletedHtmlResourceID);

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

                        IF (@softDelete IS NULL OR @softDelete = 1)
                        BEGIN   
	                        -- update EmailOutbound.PlainPartResourceID to point to the @deletedPlainResourceID
	                        UPDATE ObjectReference
	                        SET
		                        ObjectID = @deletedPlainResourceID,
		                        ReferenceKey = '#' + convert(VARCHAR, @deletedPlainResourceID)
	                        FROM ObjectReference o
		                        INNER JOIN #EmailPurgeBatch p ON p.PlainPartResourceID = o.ObjectReferenceID

	                        UPDATE ObjectReference
	                        SET
		                        ObjectID = @deletedHtmlResourceID,
		                        ReferenceKey = '#' + convert(VARCHAR, @deletedHtmlResourceID)
	                        FROM ObjectReference o
		                        INNER JOIN #EmailPurgeBatch p ON p.HtmlPartResourceID = o.ObjectReferenceID
                                   
	                        -- delete ObjectReferences not explicitly pointed to by EmailOutbound (embedded email images)
	                        DELETE ObjectReference
	                        select o.ObjectReferenceID
	                        FROM ObjectReference o
		                        INNER JOIN #EmailPurgeBatch p ON o.ConsumerID = p.EmailOutboundID
	                        WHERE o.ObjectID not in ( @deletedPlainResourceID, @deletedHtmlResourceID)
                        END
                        ELSE
                        BEGIN
                            DELETE ObjectReference
                            FROM ObjectReference
                                INNER JOIN #EmailPurgeBatch batch on batch.PlainPartResourceID = ObjectReference.ObjectReferenceID

                            DELETE ObjectReference
                            FROM ObjectReference
                                INNER JOIN #EmailPurgeBatch batch on batch.HtmlPartResourceID = ObjectReference.ObjectReferenceID
                            
                            DELETE ObjectReference
                            FROM ObjectReference
                                INNER JOIN #EmailPurgeBatch batch on batch.EmailOutboundID = ObjectReference.ConsumerID
                        END

                    COMMIT TRANSACTION;
                    
                    TRUNCATE TABLE #EmailPurgeBatch;

                    -- update batch total and adjust batch size to an amount expected to complete in 10 seconds
                    SET @batchTotal = @batchTotal + @batchSize;
                    SET @batchSize = @batchTotal / @totalSeconds * 10;
                END;
            ";
        }
    }
}
