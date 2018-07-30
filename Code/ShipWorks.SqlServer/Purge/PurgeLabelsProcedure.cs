using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer;
using ShipWorks.SqlServer.Purge;

public partial class StoredProcedures
{
    private const string LabelsPurgeAppLockName = "PurgeLabels";

    /// <summary>
    /// Purges old label records from the database.
    /// This script will remove all non-thermal shipping labels older than a certain date
    /// This is done by adding png and gif resources to ShipWorks that say 'This resource has been deleted'
    /// and then pointing all of the expired labels to this resource while deleting the old ones.
    /// </summary>
    /// <param name="olderThan">Indicates the date/time to use for determining
    /// which Label records will be purged. Any records with an Shipment.ProcessedDate value earlier than
    /// this date will be purged.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    /// <param name="softDelete">If true, resources/object references will be pointed to dummy entities.  Otherwise the full entity will be deleted.</param>
    [SqlProcedure]
    public static void PurgeLabels(SqlDateTime olderThan, SqlDateTime runUntil, SqlBoolean softDelete)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeLabelCommandText, LabelsPurgeAppLockName, olderThan, runUntil, softDelete);
    }

    private static string PurgeLabelCommandText
    {
        get
        {
            return @"
                SET NOCOUNT ON

                DECLARE 
                    @count int,
                    @pngResourceID bigint,
                    @gifResourceID bigint,
                    @jpgResourceID bigint,
                    @eplResourceID bigint,
                    @zplResourceID bigint,
                    @startTime DATETIME = GETUTCDATE(),
                    @batchSize INT = 1000,
                    @batchTotal BIGINT = 0;

                -- PNG --------------------------------
                SELECT @count = COUNT(*) FROM [Resource] WHERE [Filename] = '__purged_label.png'
                    IF (@count = 0)
                    BEGIN
                        INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES(0x1E, 0x1E, 0, '__purged_label.png');
                        SET @pngResourceID = @@IDENTITY
                    END
                    ELSE
                    BEGIN
                        SELECT @pngResourceID = ResourceID FROM [Resource] WHERE [Filename] = '__purged_label.png'
                    END

                -- GIF --------------------------------
                SELECT @count = COUNT(*) FROM [Resource] WHERE [Filename] = '__purged_label.gif'
                    IF (@count = 0)
                    BEGIN
                        INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES(0x1F, 0x1F, 0, '__purged_label.gif');
                        SET @gifResourceID = @@IDENTITY
                    END
                    ELSE
                    BEGIN
                        SELECT @gifResourceID = ResourceID FROM [Resource] WHERE [Filename] = '__purged_label.gif'
                    END

                -- JPG --------------------------------
                SELECT @count = COUNT(*) FROM [Resource] WHERE [Filename] = '__purged_label.jpg'
                IF (@count = 0)
                BEGIN
                    INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES(0x2A, 0x2A, 0, '__purged_label.jpg');
                    SET @jpgResourceID = @@IDENTITY
                END
                ELSE
                BEGIN
                    SELECT @jpgResourceID = ResourceID FROM [Resource] WHERE [Filename] = '__purged_label.jpg'
                END

                -- EPL  --------------------------------
                SELECT @count = COUNT(*) FROM [Resource] WHERE [Filename] = '__purged_label_epl.swr'
                IF (@count = 0)
                BEGIN
                    INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES(0x2B, 0x2B, 0, '__purged_label_epl.swr');
                    SET @eplResourceID = @@IDENTITY
                END
                ELSE
                BEGIN
                    SELECT @eplResourceID = ResourceID FROM [Resource] WHERE [Filename] = '__purged_label_epl.swr'
                END

                -- ZPL  --------------------------------
                SELECT @count = COUNT(*) FROM [Resource] WHERE [Filename] = '__purged_label_zpl.swr'
                IF (@count = 0)
                BEGIN
                    INSERT INTO [Resource] ([Data], [Checksum], [Compressed], [Filename]) VALUES(0x2C, 0x2C, 0, '__purged_label_zpl.swr');
                    SET @zplResourceID = @@IDENTITY
                END
                ELSE
                BEGIN
                    SELECT @zplResourceID = ResourceID FROM [Resource] WHERE [Filename] = '__purged_label_zpl.swr'
                END

                IF OBJECT_ID('tempdb.dbo.#ResourceIDsToIgnore', 'U') IS NOT NULL
                    DROP TABLE #ResourceIDsToIgnore; 

                -- Start
                CREATE TABLE #ResourceIDsToIgnore
                (
                    ResourceID bigint
                )

                INSERT INTO #ResourceIDsToIgnore ( ResourceID ) VALUES  (@gifResourceID )
                INSERT INTO #ResourceIDsToIgnore ( ResourceID ) VALUES  (@jpgResourceID )
                INSERT INTO #ResourceIDsToIgnore ( ResourceID ) VALUES  (@pngResourceID )
                INSERT INTO #ResourceIDsToIgnore ( ResourceID ) VALUES  (@eplResourceID )
                INSERT INTO #ResourceIDsToIgnore ( ResourceID ) VALUES  (@zplResourceID )

                IF OBJECT_ID('tempdb.dbo.#LabelsToCleanUp', 'U') IS NOT NULL
                    DROP TABLE #LabelsToCleanUp; 

                -- create temp tables
                CREATE TABLE #LabelsToCleanUp 
                ( 
                    ObjectReferenceID bigint,
                    ResourceID bigint,
                    ImageFormat nvarchar(5)
                )

                ;WITH ShipmentsToPurge AS
                (
	                SELECT s.ShipmentID, s.ActualLabelFormat
	                FROM Shipment s
	                WHERE s.ProcessedDate < @olderThan	
                      AND s.Processed = 1
                ),
                ObjectReferenceIDs as
                (
	                SELECT c.UpsPackageID as [ConsumerID], s.ActualLabelFormat FROM UpsPackage c INNER JOIN ShipmentsToPurge s ON c.ShipmentID = s.ShipmentID
	                UNION
	                SELECT c.FedExPackageID as [ConsumerID], s.ActualLabelFormat FROM FedExPackage c INNER JOIN ShipmentsToPurge s ON c.ShipmentID = s.ShipmentID
	                UNION
	                SELECT c.DhlExpressPackageID as [ConsumerID], s.ActualLabelFormat FROM DhlExpressPackage c INNER JOIN ShipmentsToPurge s ON c.ShipmentID = s.ShipmentID
	                UNION
	                SELECT c.iParcelPackageID as [ConsumerID], s.ActualLabelFormat FROM iParcelPackage c INNER JOIN ShipmentsToPurge s ON c.ShipmentID = s.ShipmentID
	                UNION
	                SELECT ShipmentID as [ConsumerID], ActualLabelFormat from ShipmentsToPurge
                )

                INSERT INTO #LabelsToCleanUp	
	                SELECT [ObjectReference].ObjectReferenceID AS ObjectReferenceID,
					       [ObjectReference].ObjectID AS ResourceID, 		
						   CASE 
						       WHEN ISNULL(s.ActualLabelFormat, -1) = 0 THEN 'EPL'
						       WHEN ISNULL(s.ActualLabelFormat, -1) = 1 THEN 'ZPL'
						       ELSE 'PNG'
						   END as ImageFormat
	                FROM ObjectReferenceIDs s 
		                INNER JOIN [ObjectReference] ON s.ConsumerID = [ObjectReference].ConsumerID		
	                ORDER BY ObjectReferenceID

                -- see if there's any actual work to do
                SELECT @count = COUNT(*) FROM #LabelsToCleanUp;
                IF @count > 0
                BEGIN

                    DECLARE @currentBatch TABLE 
                    ( 
                        ObjectReferenceID bigint,
                        ResourceID bigint,
                        ImageFormat nvarchar(5)
                    )

                    WHILE @runUntil IS NULL OR @runUntil > GetUtcDate()
                    BEGIN
                        INSERT INTO @currentBatch
                            SELECT TOP (@batchSize) * 
                            FROM #LabelsToCleanUp	

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
        
                        BEGIN TRANSACTION	
        
                        -- adjust the old reference to point to the new 'deleted' image resource, by image format			
                        UPDATE ObjectReference 
                        SET ObjectReference.ObjectID = 
                        (
                            CASE batch.ImageFormat
                                WHEN 'PNG' THEN @pngResourceID
                                WHEN 'GIF' THEN @gifResourceID
                                WHEN 'JPG' THEN @jpgResourceID
                                WHEN 'EPL' THEN @eplResourceID
                                ELSE @zplResourceID
                            END
                        )					
                        FROM @currentBatch AS batch
                        INNER JOIN ObjectReference
                            ON batch.ObjectReferenceID = ObjectReference.ObjectReferenceID

                        -- delete from our temp table
                        DELETE labelsToCleanup
                        FROM #LabelsToCleanUp AS labelsToCleanup 
                        INNER JOIN @currentBatch AS batch
                            ON labelsToCleanup.ResourceID = batch.ResourceID AND labelsToCleanup.ObjectReferenceID = batch.ObjectReferenceID
            
                        COMMIT TRANSACTION
                    
                        --grab the next batch before ending loop
                        DELETE @currentBatch	
        
                        -- update batch total and adjust batch size to an amount expected to complete in 10 seconds
                        SET @batchTotal = @batchTotal + @batchSize;
                        SET @batchSize = @batchTotal / @totalSeconds * 10;
                    END
                END

                DROP TABLE #LabelsToCleanUp
                DROP TABLE #ResourceIDsToIgnore
                ";
        }
    }
}
