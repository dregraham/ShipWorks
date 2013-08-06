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
        PurgeScriptRunner.RunPurgeScript(PurgePrintResultCommandText, PurgePrintResultAppLockName, earliestRetentionDateInUtc, latestExecutionTimeInUtc);
    }

    /// <summary>
    /// The T-SQL for the PurgePrintResult stored procedure.
    /// </summary>
    private static string PurgePrintResultCommandText
    {
        get
        {
            return @"
DECLARE @PrintImageResourceID BIGINT, 
        @PrintReferenceKey    VARCHAR(250), 
        @ImageData            VARBINARY(max), 
        @ImageChecksum        BINARY(32), 
        @EplResourceID        BIGINT, 
        @EplReferenceKey      VARCHAR(250), 
        @EplData              VARBINARY(max), 
        @EplChecksum          BINARY(32), 
        @startTime            DATETIME = Getutcdate(), 
        @batchSize            INT = 1000, 
        @batchTotal           BIGINT = 0; 

SET @BatchSize = 10000 

-- Setup data for the replacement images 
SELECT @ImageData = CONVERT(VARBINARY(max), 
0x1F8B0800000000000400ECBD07601C499625262F6DCA7B7F4AF54AD7E074A10880601324D8904010ECC188CDE692EC1D69472329AB2A81CA6556655D661640CCED9DBCF7DE7BEFBDF7DE7BEFBDF7BA3B9D4E27F7DFFF3F5C6664016CF6CE4ADAC99E2180AAC81F3F7E7C1F3F22DECC8B265DD5C5B24D7FBA9AA4F3AC492779BE4C57EBFA229FA593EBF4F5BC587DB7AADF3669B69CA5D47A59A565B5BCC8EB34BBCC8A329B94F9F8FF090000FFFF56DD225D47000000
), 
@ImageChecksum = CONVERT(BINARY, 0x6F7E431AD96DBB63B335CDEF1DBB696661C32506A1B967D13727395C73462CCC),
-- The content of thermal print jobs won't necessarily matter since you can't view thermal print jobs in ShipWorks
@EplData = CONVERT(VARBINARY(max), 'The contents of this print job have been purged by ShipWorks and is no longer available.'),
@EplChecksum = CONVERT(BINARY, 'Purged') 

-- Create a dummy resource reccord for non-thermal print jobs 
IF NOT EXISTS (SELECT 1 
               FROM   [resource] 
               WHERE  Filename = '__PrintCleanup.swr') 
  BEGIN 
      INSERT INTO [resource] 
                  ([data], 
                   [checksum], 
                   [compressed], 
                   [filename]) 
      VALUES      ( @ImageData, 
                    @ImageChecksum, 
                    1, 
                    '__PrintCleanup.swr' ); 

      SET @PrintImageResourceID = @@IDENTITY 
  END 
ELSE 
  BEGIN 
      -- We've run it once, so just locate the resource ID we are redirecting to 
      SELECT @PrintImageResourceID = resourceid 
      FROM   [resource] 
      WHERE  Filename = '__PrintCleanup.swr'
  END 

SELECT @PrintReferenceKey = '#' 
                            + CONVERT(VARCHAR(250), @PrintImageResourceID) 

-- Create dummy resource record for Thermal 
IF NOT EXISTS (SELECT 1 
               FROM   [resource] 
               WHERE  Filename = '__PrintCleanup_Thermal.swr') 
  BEGIN 
      INSERT INTO [resource] 
                  ([data], 
                   [checksum], 
                   [compressed], 
                   [filename]) 
      VALUES      ( @EplData, 
                    @EplChecksum, 
                    0, 
                    '__PrintCleanup_Thermal.swr' ); 

      SET @EplResourceID = @@IDENTITY 
  END 
ELSE 
  BEGIN 
      -- We've run it once, so just locate the resource id we are redirecting to 
      SELECT @EplResourceID = resourceid 
      FROM   [resource] 
      WHERE  Filename = '__PrintCleanup_Thermal.swr' 
  END 

SELECT @EplReferenceKey = '#' + CONVERT(VARCHAR(250), @EplResourceID) 

-- Create temp tables 
CREATE TABLE #printjobworking 
  ( 
     printresultid     BIGINT, 
     contentresourceid BIGINT, 
     isthermal         BIT 
  ) 

-- Find all of the old print jobs to wipe out 
INSERT INTO #printjobworking 
            (printresultid, 
             contentresourceid, 
             isthermal) 
SELECT printresult.printresultid, 
       printresult.contentresourceid, 
       CASE printresult.templatetype 
         WHEN 3 THEN 1 
         ELSE 0 
       END 
FROM   printresult 
       INNER JOIN objectreference 
               ON objectreference.objectreferenceid = printresult.contentresourceid 
WHERE  printresult.printdate < @earliestRetentionDateInUtc 
       AND objectreference.objectid NOT IN ( @PrintImageResourceID, @EplResourceID ) 

CREATE INDEX ix_printjobworking 
  ON #printjobworking (printresultid) 

-- See if there's any actual work to do 
IF EXISTS (SELECT 1 
           FROM   #printjobworking) 
  BEGIN 
      DECLARE @CurrentBatch TABLE 
        ( 
           printresultid     BIGINT, 
           contentresourceid BIGINT, 
           isthermal         BIT 
        ) 

      WHILE @latestExecutionTimeInUtc IS NULL 
             OR @latestExecutionTimeInUtc > Getutcdate() 
        BEGIN 
            INSERT INTO @CurrentBatch 
            SELECT TOP (@BatchSize) printresultid, 
                                    contentresourceid, 
                                    isthermal 
            FROM   #printjobworking 

            SET @batchSize = @@ROWCOUNT; 

            IF @batchSize = 0 
              BREAK; 

            DECLARE @totalSeconds INT = Datediff(second, @startTime, Getutcdate()) 
              + 1; 

            -- stop if the batch isn't expected to complete in time 
            IF ( @latestExecutionTimeInUtc IS NOT NULL 
                 AND @batchTotal > 0 
                 AND Dateadd(second, @totalSeconds * @batchSize / @batchTotal, Getutcdate()) > @latestExecutionTimeInUtc )
              BREAK; 

            -- Wrap edits in transaction 
            BEGIN TRANSACTION 

            -- Delete the previous resources (print job/image data) that the print results 
            -- in this batch were pointing at 
            DELETE resource 
            FROM   resource 
                   INNER JOIN objectreference 
                           ON objectreference.objectid = resource.resourceid 
                   INNER JOIN @CurrentBatch batch 
                           ON batch.printresultid = objectreference.consumerid 
                              AND batch.contentresourceid = objectreference.objectreferenceid

            -- Upate ObjectReference refrerenced by PrintResult.ContentResourceID so 
            -- they point to the generic, purged resource ID 
            UPDATE objRef 
            SET    objRef.objectid = CASE batch.isthermal 
                                       WHEN 1 THEN @EplResourceID 
                                       ELSE @PrintImageResourceID 
                                     END, 
                   objRef.referencekey = CASE batch.isthermal 
                                           WHEN 1 THEN @EplReferenceKey 
                                           ELSE @PrintReferenceKey 
                                         END 
            FROM   @CurrentBatch batch 
                   INNER JOIN objectreference objRef 
                           ON objRef.objectreferenceid = batch.contentresourceid 

            -- Delete ObjectReference not explicitly pointed at by PrintResult (and therefore updated to point to @PrintImageResourceID
            DELETE objectreference 
            FROM   objectreference 
                   INNER JOIN @CurrentBatch batch 
                           ON batch.printresultid = objectreference.consumerid 
                              AND batch.contentresourceid != objectreference.objectreferenceid

            --Delete items in the batch from the #PrintJobWorking table 
            DELETE printjobworking 
            FROM   #printjobworking printJobWorking 
                   INNER JOIN @CurrentBatch AS batch 
                           ON batch.printresultid = printjobworking.printresultid 

            COMMIT TRANSACTION 

            --grab the next batch before ending loop 
            DELETE @CurrentBatch 

            -- update batch total and adjust batch size to an amount expected to complete in 10 seconds 
            SET @batchTotal = @batchTotal + @batchSize; 
            SET @batchSize = @batchTotal / @totalSeconds * 10; 
        END 
  END ";
        }
    }
}
