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
    /// <param name="latestExecutionTimeInUtc">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeEmailOutbound(SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeEmailOutboundCommandText, PurgeEmailOutboundAppLockName, earliestRetentionDateInUtc, latestExecutionTimeInUtc);             
    }

    /// <summary>
    /// The T-SQL for the PurgeEmailOutbound stored procedure.
    /// </summary>
    private static string PurgeEmailOutboundCommandText
    {
        get
        {
            return @"
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

                -- create batch purge ID table
                CREATE TABLE #EmailPurgeBatch (
                    EmailOutboundID BIGINT PRIMARY KEY
                );

                -- purge in batches while time allows
                WHILE GETUTCDATE() < @latestExecutionTimeInUtc
                BEGIN
                    INSERT #EmailPurgeBatch
                    SELECT TOP 100 e.EmailOutboundID
                    FROM EmailOutbound e
                    INNER JOIN ObjectReference o ON
                        o.ObjectReferenceID = e.PlainPartResourceID
                    WHERE
                        e.SentDate < @earliestRetentionDateInUtc AND
                        o.ObjectID <> @deletedEmailResourceID;

                    IF @@ROWCOUNT = 0
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

                    -- update EmailOutbound.HtmlPartResourceID to point to null
                    UPDATE EmailOutbound
                    SET HtmlPartResourceID = NULL
                    FROM EmailOutbound e
                    INNER JOIN #EmailPurgeBatch p ON
                        p.EmailOutboundID = e.EmailOutboundID;

                    -- delete ObjectReferences not explicitly pointed to by EmailOutbound (embedded email images)
                    DELETE ObjectReference
                    FROM ObjectReference o
                    INNER JOIN EmailOutbound e ON
                        e.EmailOutboundID = o.ConsumerID
                    INNER JOIN #EmailPurgeBatch p ON
                        p.EmailOutboundID = e.EmailOutboundID
                    WHERE o.ObjectID <> @deletedEmailResourceID;

                    COMMIT TRANSACTION;

                    TRUNCATE TABLE #EmailPurgeBatch;
                END;
            ";
        }
    }
}
