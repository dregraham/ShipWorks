using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Purge;

public partial class StoredProcedures
{
    private const string PurgePrintResultAppLockName = "PurgePrintResult";

    /// <summary>
    /// Purges PrintResult resource data from the database and replaces it with a pointer to the
    /// appropriate dummy record depending on whether an standard image was printed or a thermal image.
    /// </summary>
    /// <param name="olderThan">Indicates the date/time to use for determining
    /// which PrintResult records will be purged. Any records with an PrintResult.PrintDate value earlier than
    /// this date will be purged.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    /// <param name="softDelete">If true, resources/object references will be pointed to dummy entities.  Otherwise the full entity will be deleted.</param>
    [SqlProcedure]
    public static void PurgePrintResult(SqlDateTime olderThan, SqlDateTime runUntil, SqlBoolean softDelete)
    {
        PurgeScriptRunner.RunPurgeScript(PurgePrintResultCommandText, PurgePrintResultAppLockName, olderThan, runUntil, softDelete);
    }

    /// <summary>
    /// The T-SQL for the PurgePrintResult stored procedure.
    /// </summary>
    private static string PurgePrintResultCommandText
    {
        get
        {
            return @"
                SET NOCOUNT ON;

                DECLARE @htmlResourceID    BIGINT, 
                        @thermalResourceID BIGINT, 
                        @startTime         DATETIME = GETUTCDATE(), 
                        @batchSize         INT = 10000, 
                        @batchTotal        BIGINT = 0; 

                -- Create a dummy resource record for non-thermal print jobs 
                IF NOT EXISTS (SELECT 1 FROM [Resource] WHERE Filename = '__purged_print_html.swr') 
                    BEGIN 
                        INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES (0x1C, 0x1C, 0, '__purged_print_html.swr'); 

                        SET @htmlResourceID = @@IDENTITY 
                    END 
                ELSE 
                    BEGIN 
                        SELECT @htmlResourceID = ResourceID FROM [Resource] WHERE [Filename] = '__purged_print_html.swr'
                    END 

                -- Create dummy resource record for Thermal 
                IF NOT EXISTS (SELECT 1 FROM [Resource] WHERE [Filename] = '__purged_print_thermal.swr') 
                    BEGIN 
                        INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES (0x1D, 0x1D, 0, '__purged_print_thermal.swr'); 

                        SET @thermalResourceID = @@IDENTITY 
                    END 
                ELSE 
                    BEGIN 
                        SELECT @thermalResourceID = ResourceID FROM [Resource] WHERE [Filename] = '__purged_print_thermal.swr' 
                    END 

	            IF OBJECT_ID('tempdb.dbo.#PrintJobWorking', 'U') IS NOT NULL
		            DROP TABLE #PrintJobWorking; 

                -- Create temp tables 
                CREATE TABLE #PrintJobWorking 
                ( 
                    PrintResultID     BIGINT, 
                    ContentResourceID BIGINT, 
                    IsThermal         BIT 
                ) 

                -- Find all of the old print jobs to wipe out 
                INSERT INTO #PrintJobWorking (PrintResultID, ContentResourceID, IsThermal) 
	                SELECT 
		                PrintResult.PrintResultID, 
		                PrintResult.ContentResourceID, 
		                CASE PrintResult.TemplateType 
			                 WHEN 3 THEN 1 
			                 ELSE 0 
		                END 
	                FROM PrintResult INNER JOIN ObjectReference ON ObjectReference.ObjectReferenceID = PrintResult.ContentResourceID
	                WHERE PrintResult.PrintDate < @olderThan 
		              AND ObjectReference.ObjectID NOT IN ( @htmlResourceID, @thermalResourceID ) 
		              
                -- See if there's any actual work to do 
                IF EXISTS (SELECT 1 FROM #PrintJobWorking) 
                BEGIN 
                        DECLARE @CurrentBatch TABLE 
                        ( 
                        PrintResultID     BIGINT, 
                        ContentResourceID BIGINT, 
                        IsThermal         BIT 
                        ) 

                        WHILE (@runUntil IS NULL OR @runUntil > GETUTCDATE())
                        BEGIN 
                            INSERT INTO @CurrentBatch 
				                SELECT TOP (@batchSize) 
					                PrintResultID, 
					                ContentResourceID, 
					                IsThermal 
				                FROM #PrintJobWorking 

                            SET @batchSize = @@ROWCOUNT; 

                            IF @batchSize = 0 
                                BREAK; 

                            DECLARE @TotalSeconds INT = DATEDIFF(second, @startTime, GETUTCDATE()) + 1; 

                            -- stop if the batch isn't expected to complete in time 
                            IF ( @runUntil IS NOT NULL AND @batchTotal > 0 AND DATEADD(second, @TotalSeconds * @batchSize / @batchTotal, GETUTCDATE()) > @runUntil )
                                BREAK; 

                            -- Wrap edits in transaction 
                            BEGIN TRANSACTION 
							
								IF (@softDelete IS NULL OR @softDelete = 1)
								BEGIN  
									-- Update ObjectReference refrerenced by PrintResult.ContentResourceID so they point to the generic, purged resource ID 
									UPDATE objRef 
									SET    objRef.ObjectID = 
												CASE batch.IsThermal 
													WHEN 1 THEN @thermalResourceID 
													ELSE @htmlResourceID 
												END, 
											objRef.ReferenceKey = 
												CASE batch.IsThermal 
													WHEN 1 THEN '#' + convert(VARCHAR, @thermalResourceID)
													ELSE '#' + convert(VARCHAR, @htmlResourceID)
												END 
									FROM @CurrentBatch batch 
										INNER JOIN ObjectReference objRef ON objRef.objectreferenceid = batch.ContentResourceID 
								END
								ELSE
								BEGIN
									DELETE ObjectReference
									FROM ObjectReference
										INNER JOIN @CurrentBatch batch ON batch.ContentResourceID = ObjectReference.ObjectReferenceID

									DELETE PrintResult
									FROM PrintResult
										INNER JOIN @CurrentBatch AS batch ON batch.PrintResultID = PrintResult.PrintResultID 
								END

								-- Delete ObjectReference not explicitly pointed at by PrintResult (and therefore updated to point to @htmlResourceID
								DELETE ObjectReference 
								FROM ObjectReference 
									INNER JOIN @CurrentBatch batch ON batch.PrintResultID = ObjectReference.ConsumerID
										AND batch.ContentResourceID != ObjectReference.ObjectReferenceID

								--Delete items in the batch from the #PrintJobWorking table 
								DELETE printJobWorking 
								FROM #PrintJobWorking printJobWorking 
									INNER JOIN @CurrentBatch AS batch ON batch.PrintResultID = PrintJobWorking.PrintResultID 

                            COMMIT TRANSACTION 

                            --grab the next batch before ending loop 
                            DELETE @CurrentBatch 

                            -- update batch total and adjust batch size to an amount expected to complete in 10 seconds 
                            SET @batchTotal = @batchTotal + @batchSize; 
                            SET @batchSize = @batchTotal / @TotalSeconds * 10; 
                        END 
                END 

	            DROP TABLE #PrintJobWorking; 
            ";
        }
    }
}
