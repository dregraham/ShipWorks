using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.Purge;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using ShipWorks.SqlServer;


public partial class StoredProcedures
{
    private const string PurgeEmailOutboundAppLockName = "PurgeEmailOutbound";

    /// <summary>
    /// Purges EmailOutbound resource data from the database and replaces it with a pointer to the dummy record.
    /// </summary>
    /// <param name="earliestRetentionDate">Indicates the date/time to use for determining
    /// which EmailOutbound records will be purged. Any records with an EmailOutbound.SentDate value earlier than
    /// this date will be purged.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeEmailOutbound(SqlDateTime olderThan, SqlDateTime runUntil)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeEmailOutboundCommandText, PurgeEmailOutboundAppLockName, olderThan, runUntil);             
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
                DECLARE @deletedEmailResourceID BIGINT = (
                    SELECT ResourceID FROM [Resource] WHERE [Filename] = '__EmailCleanup.swr'
                );

                IF @deletedEmailResourceID IS NULL
                BEGIN
                    INSERT INTO [Resource] (
                        [Data], [Checksum], [Compressed], [Filename]
                    )
                    VALUES (
                        0x1F8B0800000000000400ECBD07601C499625262F6DCA7B7F4AF54AD7E074A10880601324D8904010ECC188CDE692EC1D69472329AB2A81CA6556655D661640CCED9DBCF7DE7BEFBDF7DE7BEFBDF7BA3B9D4E27F7DFFF3F5C6664016CF6CE4ADAC99E2180AAC81F3F7E7C1F3F22DECC8B26CD175951A6F3AC492779BE4C57EBFA229FA593EBF4F5BC587DB7AADF3669B69CA5D47259A565B5BCC8EB34BBA477B249998FFF9F000000FFFF68E5632B43000000,
                        0x36C2A3B7068081B3C119CCE74F6CFA8879F078F412414B3A883E1F6B8F81135A,
                        1,
                        '__EmailCleanup.swr'
                    );

                    SET @deletedEmailResourceID = SCOPE_IDENTITY();
                END;

                -- find or create the 'deleted' HTML resource record
                DECLARE @deletedHTMLEmailResourceID BIGINT = (
                    SELECT ResourceID FROM [Resource] WHERE [Filename] = '__EmailCleanupHTML.swr'
                );

                IF @deletedHTMLEmailResourceID IS NULL
                BEGIN
                    INSERT INTO [Resource] (
                        [Data], [Checksum], [Compressed], [Filename]
                    )
                    VALUES (
                        0x1F8B08000000000004004D91514FC2301485DF49F80F377DE1C531301115BA258480920824B0C4E85BC7EE68E3D6CEED0E9DC6FF6ECB82D2873EDC9EF3DD93532E29CF422E512421274519863BA98A1C35C1DA904AD55E90329AFBED63B7C37324017BA3C96A0246F849BE834C602F4559210535A5DE1D03495478F85EAB63C066ADDC8B9A02998354D4585A072E4E6C92E60A48C419C237A4D6E1A522575933864848938B493BACD4178E61382868023F7F04EEB7C86EE71C518B1C03F6305FCFB7D368B365FF9957BBC768F56411FDC1A07F7F6DAFE1E8E676C442EE9F8AB00417064EC4802D36EBC8DB2D5FE7E7ADA7C162BA5A3EBD9CB3596F51E2A5A315436F66EA5261096BFCE8B13092AA02CC85CA408A0A62440D455D1E3081B80157FDB329DF2A103A01ABD40632A30FD62E8ED6E3BAE973DF6EB2515D4497D87DE02F7352D21AC7010000,
                        0xF7DECD3292BB70389B1D1C229331B568331883BCE2BCAE01F1DC4E448F2395DB,
                        1,
                        '__EmailCleanupHTML.swr'
                    );

                    SET @deletedHTMLEmailResourceID = SCOPE_IDENTITY();
                END;



                -- create batch purge ID table
                CREATE TABLE #EmailPurgeBatch (
                    EmailOutboundID BIGINT PRIMARY KEY
                );

                DECLARE
                    @startTime DATETIME = GETUTCDATE(),
                    @batchSize INT = 1000,
                    @batchTotal BIGINT = 0;

                -- purge in batches while time allows
                WHILE @runUntil IS NULL OR GETUTCDATE() < @runUntil
                BEGIN
                    INSERT #EmailPurgeBatch
                    SELECT TOP (@batchSize) e.EmailOutboundID
                    FROM EmailOutbound e
                    INNER JOIN ObjectReference o ON
                        o.ObjectReferenceID = e.PlainPartResourceID
                    WHERE
                        e.SentDate < @olderThan AND
                        e.SendStatus = 1 AND  -- only purge emails that have been sent
                        o.ObjectID <> @deletedEmailResourceID AND
                        o.ObjectID <> @deletedHTMLEmailResourceID;

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

                    -- update EmailOutbound.PlainPartResourceID to point to the @deletedEmailResourceID
                    UPDATE ObjectReference
                    SET
                        ObjectID = @deletedEmailResourceID,
                        ReferenceKey = '#' + convert(VARCHAR, @deletedEmailResourceID)
                    FROM ObjectReference o
                    INNER JOIN EmailOutbound e ON
                        e.EmailOutboundID = o.ConsumerID AND
                        e.PlainPartResourceID = o.ObjectReferenceID
                    INNER JOIN #EmailPurgeBatch p ON
                        p.EmailOutboundID = e.EmailOutboundID;

                    UPDATE ObjectReference
                    SET
                        ObjectID = @deletedHTMLEmailResourceID,
                        ReferenceKey = '#' + convert(VARCHAR, @deletedHTMLEmailResourceID)
                    FROM ObjectReference o
                    INNER JOIN EmailOutbound e ON
                        e.EmailOutboundID = o.ConsumerID AND
                        e.HtmlPartResourceID = o.ObjectReferenceID
                    INNER JOIN #EmailPurgeBatch p ON
                        p.EmailOutboundID = e.EmailOutboundID;
                   
                    -- delete ObjectReferences not explicitly pointed to by EmailOutbound (embedded email images)
                    DELETE ObjectReference
                    FROM ObjectReference o
                    INNER JOIN EmailOutbound e ON
                        e.EmailOutboundID = o.ConsumerID
                    INNER JOIN #EmailPurgeBatch p ON
                        p.EmailOutboundID = e.EmailOutboundID
                    WHERE o.ObjectID <> @deletedEmailResourceID
                        AND o.ObjectID <> @deletedHTMLEmailResourceID;

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
