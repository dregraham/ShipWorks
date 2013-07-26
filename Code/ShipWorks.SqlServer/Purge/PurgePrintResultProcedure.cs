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
    private const string PurgePrintResultAppLockName = "PurgePrintResult";

    /// <summary>
    /// Purges PrintResult resource data from the database and replaces it with a pointer to the
    /// appropriate dummy record depending on whether an standard image was printed or a thermal image.
    /// </summary>
    /// <param name="earliestRetentionDate">Indicates the date/time to use for determining
    /// which PrintResult records will be purged. Any records with an PrintResult.PrintDate value earlier than
    /// this date will be purged.</param>
    /// <param name="latestExecutionTimeInUtc">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgePrintResult(SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            try
            {
                // Need to have an open connection for the duration of the lock acquisition/release
                connection.Open();

                if (!SqlAppLockUtility.IsLocked(connection, PurgePrintResultAppLockName))
                {
                    if (SqlAppLockUtility.AcquireLock(connection, PurgePrintResultAppLockName))
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = PurgePrintResultCommandText;

                            command.Parameters.Add(new SqlParameter("@earliestRetentionDateInUtc", earliestRetentionDateInUtc));
                            command.Parameters.Add(new SqlParameter("@latestExecutionTimeInUtc", latestExecutionTimeInUtc));
                            command.ExecuteNonQuery();

                            // Use ExecuteAndSend instead of ExecuteNonQuery when debuggging to see output printed 
                            // to the console of client (i.e. SQL Management Studio)
                            // SqlContext.Pipe.ExecuteAndSend(command);
                        }
                    }
                }
                else
                {
                    // Let the caller know that someone else is already running the purge.
                    throw new PurgeException("Could not acquire applock for purging print jobs.");
                }
            }
            finally
            {
                SqlAppLockUtility.ReleaseLock(connection, PurgePrintResultAppLockName);
            }
        }
    }

    /// <summary>
    /// The T-SQL for the PurgePrintResult stored procedure.
    /// </summary>
    private static string PurgePrintResultCommandText
    {
        get
        {
            return @"
                DECLARE 
                        @BatchSize            INT,
                        @PrintImageResourceID BIGINT,
                        @PrintReferenceKey    VARCHAR(250),
                        @ImageData            VARBINARY(MAX),
                        @ImageChecksum        BINARY(32),
                        @EplResourceID        BIGINT,
                        @EplReferenceKey      VARCHAR(250),
                        @EplData              VARBINARY(MAX),
                        @EplChecksum          BINARY(32)
        
                SET @BatchSize = 100

                -- Setup data for the replacement images
                SELECT @ImageData = convert(VARBINARY(MAX), 0x1F8B0800000000000400ECBD07601C499625262F6DCA7B7F4AF54AD7E074A10880601324D8904010ECC188CDE692EC1D69472329AB2A81CA6556655D661640CCED9DBCF7DE7BEFBDF7DE7BEFBDF7BA3B9D4E27F7DFFF3F5C6664016CF6CE4ADAC99E2180AAC81F3F7E7C1F3F22DECC8B265DD5C5B24D7FBA9AA4F3AC492779BE4C57EBFA229FA593EBF4F5BC587DB7AADF3669B69CA5D47A59A565B5BCC8EB34BBCC8A329B94F9F8FF090000FFFF56DD225D47000000),
                       @ImageChecksum = convert(BINARY, 0x6F7E431AD96DBB63B335CDEF1DBB696661C32506A1B967D13727395C73462CCC),
                       
                       -- The content of thermal print jobs won't necessarily matter since you can't view thermal print jobs in ShipWorks
                       @EplData = convert(VARBINARY(MAX), 'The contents of this print job have been purged by ShipWorks and is no longer available.'),
                       @EplChecksum = convert(BINARY, 'Purged')
                      
                -- Create a dummy resource reccord for non-thermal print jobs
                IF NOT EXISTS (SELECT 1 FROM [Resource] WHERE Data = @ImageData)
                BEGIN
                    INSERT INTO [Resource] 
                    ( 
                        [Data],
                        [Checksum],
                        [Compressed],
                        [Filename]
                    )
                    VALUES
                    (
                        @ImageData,
                        @ImageChecksum,
                        1,
                        '__PrintCleanup.swr'
                    );

                    SET @PrintImageResourceID = @@IDENTITY
                END
                ELSE
                BEGIN
                    -- We've run it once, so just locate the resource ID we are redirecting to
                    SELECT @PrintImageResourceID = ResourceID
                    FROM
                        [Resource]
                    WHERE
                        Data = @ImageData
                END

                SELECT @PrintReferenceKey = '#' + convert(VARCHAR(250), @PrintImageResourceID)

                -- Create dummy resource record for Thermal
                IF NOT EXISTS (SELECT 1 FROM [Resource] WHERE Data = @EplData)
                BEGIN
                    INSERT INTO [Resource] 
                    (
                        [Data],
                        [Checksum],
                        [Compressed],
                        [Filename]
                    )
                    VALUES
                    (
                        @EplData,
                        @EplChecksum,
                        0,
                        '__PrintCleanup_Thermal.swr'
                    );

                    SET @EplResourceID = @@IDENTITY
                END
                ELSE
                BEGIN
                    -- We've run it once, so just locate the resource id we are redirecting to
                    SELECT @EplResourceID = ResourceID
                    FROM
                        [Resource]
                    WHERE
                        Data = @EplData
                END

                SELECT @EplReferenceKey = '#' + convert(VARCHAR(250), @EplResourceID)

                -- create temp tables
                CREATE TABLE #PrintJobWorking
                (
                    PrintResultID BIGINT,
                    ContentResourceID BIGINT,
                    IsThermal BIT
                )

                -- Find all of the old print jobs to wipe out
                INSERT INTO #PrintJobWorking (PrintResultID, ContentResourceID, IsThermal)
                    SELECT 
                        PrintResult.PrintResultID,
                        PrintResult.ContentResourceID,
                        CASE PrintResult.TemplateType
                            WHEN 3 THEN
                                    1
                            ELSE
                                    0
                            END
                        FROM
                            PrintResult
                        INNER JOIN 
                                ObjectReference 
                            ON 
                                ObjectReference.ObjectReferenceID = PrintResult.ContentResourceID
                        WHERE
                            PrintResult.PrintDate < @earliestRetentionDateInUtc
                            AND ObjectReference.ObjectID NOT IN (@PrintImageResourceID, @EplResourceID)

                CREATE INDEX IX_PrintJobWorking ON #PrintJobWorking (PrintResultID)

                -- See if there's any actual work to do
                IF EXISTS (SELECT 1 FROM #PrintJobWorking)
                BEGIN
                    DECLARE @CurrentBatch TABLE
                    (
                        PrintResultID BIGINT,
                        ContentResourceID BIGINT,
                        IsThermal BIT
                    )

                    INSERT INTO @CurrentBatch
                        SELECT TOP (@BatchSize) 
                            PrintResultID,
                            ContentResourceID,
                            IsThermal
                        FROM
                            #PrintJobWorking

                    -- Honor the hard stop if one is provided
                    WHILE (@@ROWCOUNT > 0) AND (@latestExecutionTimeInUtc > GETUTCDATE())
                    BEGIN
                        -- Wrap edits in transaction
                        BEGIN TRANSACTION
                        /* Upate ObjectReference refrerenced by EmailOutbound columnPlainPartResourceID
                            so they point to to @PrintImageResourceID */

                        UPDATE o
                            SET
                                o.ObjectID = CASE batch.IsThermal
                                    WHEN 1 THEN
                                        @EplResourceID
                                    ELSE
                                        @PrintImageResourceID
                                    END,
                                o.ReferenceKey = CASE batch.IsThermal
                                    WHEN 1 THEN
                                        @EplReferenceKey
                                    ELSE
                                        @PrintReferenceKey
                                    END
                            FROM
                                @CurrentBatch batch
                                INNER JOIN 
                                    ObjectReference o 
                                ON 
                                    o.ObjectReferenceID = batch.ContentResourceID

                        /* Delete ObjectReference not explicitly pointed at by PrintResult (and therefore updated to point to @PrintImageResourceID */
                        DELETE ObjectReference
                        FROM
                            ObjectReference 
                        INNER JOIN 
                            @CurrentBatch batch 
                        ON 
                            batch.PrintResultID = ObjectReference.ConsumerID 
                            AND batch.ContentResourceID != ObjectReference.ObjectReferenceID

                        --Delete items in the batch from the #PrintJobWorking table
                        DELETE printJobWorking
                        FROM
                            #PrintJobWorking printJobWorking
                        INNER JOIN 
                            @CurrentBatch AS batch 
                        ON 
                            batch.PrintResultID = printJobWorking.PrintResultID

                        COMMIT TRANSACTION

                        --grab the next batch before ending loop
                        DELETE @CurrentBatch
        
                        INSERT INTO @CurrentBatch
                            SELECT TOP (@BatchSize) * FROM #PrintJobWorking
                    END
                END";
        }
    }
}
