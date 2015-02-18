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
    /// <param name="earliestRetentionDate">Indicates the date/time to use for determining
    /// which Label records will be purged. Any records with an Shipment.ProcessedDate value earlier than
    /// this date will be purged.</param>
    /// <param name="runUntil">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeLabels(SqlDateTime olderThan, SqlDateTime runUntil)
    {
        PurgeScriptRunner.RunPurgeScript(PurgeLabelCommandText, LabelsPurgeAppLockName, olderThan, runUntil);
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

                -- create temp tables
                CREATE TABLE #LabelsToCleanUp 
                ( 
	                ObjectReferenceID bigint,
	                ResourceID bigint,
	                ImageFormat nvarchar(5)
                )

                -- find all of the UPS labels we want to wipe out
                INSERT INTO #LabelsToCleanUp
	                SELECT 
		                [ObjectReference].ObjectReferenceID AS ObjectReferenceID, 
		                [ObjectReference].ObjectID AS ResourceID, 
		                CASE 
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 0 THEN 'EPL'
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 1 THEN 'ZPL'
			                ELSE 'GIF'
		                END as ImageFormat		
	                FROM [UpsPackage]
		                INNER JOIN [Shipment] ON [UpsPackage].ShipmentID = [Shipment].ShipmentID
		                INNER JOIN [ObjectReference] ON [UpsPackage].UpsPackageID = [ObjectReference].ConsumerID			
	                WHERE 
		                [Shipment].ProcessedDate < @olderThan
		                AND [Shipment].Processed = 1
		                AND [Shipment].ShipmentType = 0 --UPS
		                AND [ObjectReference].ObjectID NOT IN (SELECT resourceid FROM #ResourceIDsToIgnore)

                -- find all of the fedex labels we want to wipe out
                INSERT INTO #LabelsToCleanUp		
	                SELECT 
		                [ObjectReference].ObjectReferenceID AS ObjectReferenceID, 
		                [ObjectReference].ObjectID AS ResourceID, 		
		                CASE 
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 0 THEN 'EPL'
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 1 THEN 'ZPL'
			                ELSE 'PNG'
		                END as ImageFormat
	                FROM [FedexPackage]
		                INNER JOIN [Shipment] ON [FedexPackage].ShipmentID = [Shipment].ShipmentID
		                INNER JOIN [ObjectReference] ON [FedexPackage].FedexPackageID = [ObjectReference].ConsumerID
	                WHERE 
		                [Shipment].ProcessedDate < @olderThan	
		                AND [Shipment].Processed = 1
		                AND [Shipment].ShipmentType = 6	-- FedEx
		                AND [ObjectReference].ObjectID NOT IN (SELECT resourceid FROM #ResourceIDsToIgnore)
	
                -- find all of the onTrac labels we want to wipe out
                INSERT INTO #LabelsToCleanUp		
	                SELECT 
		                [ObjectReference].ObjectReferenceID AS ObjectReferenceID, 
		                [ObjectReference].ObjectID AS ResourceID, 
		                CASE 
			                WHEN ISNULL(Shipment.ActualLabelFormat, -1) = 0 THEN 'EPL'
			                WHEN ISNULL(Shipment.ActualLabelFormat, -1) = 1 THEN 'ZPL'
			                ELSE 'GIF'
		                END as ImageFormat
	                FROM OnTracShipment
		                INNER JOIN Shipment ON OnTracShipment.ShipmentID = Shipment.ShipmentID
		                INNER JOIN [ObjectReference] ON Shipment.ShipmentID = [ObjectReference].ConsumerID		
		                INNER JOIN [Resource] ON [Resource].ResourceID = [ObjectReference].ObjectID
	                where 
		                Shipment.Processed = 1
		                AND Shipment.ShipmentType = 11	-- OnTrac
		                AND Shipment.ProcessedDate < @olderThan	
		                AND [ObjectReference].ObjectID NOT IN (SELECT resourceid FROM #ResourceIDsToIgnore)
		
                -- Endicia, Express1, & USPS labels
                INSERT INTO #LabelsToCleanUp		
	                SELECT 
		                [ObjectReference].ObjectReferenceID AS ObjectReferenceID, 
		                [ObjectReference].ObjectID AS ResourceID, 		
		                CASE 
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 0 THEN 'EPL'
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 1 THEN 'ZPL'
			                ELSE 'PNG'
		                END as ImageFormat
	                FROM [PostalShipment]
		                INNER JOIN [Shipment] ON [PostalShipment].ShipmentID = [Shipment].ShipmentID		
		                INNER JOIN [ObjectReference] ON [PostalShipment].ShipmentID = [ObjectReference].ConsumerID		
	                WHERE 
		                [Shipment].ProcessedDate < @olderThan
		                AND [Shipment].Processed = 1
		                AND [Shipment].ShipmentType in (2,9,15,4) -- endicia, express1, USPS, w/o postage
		                AND [ObjectReference].ObjectID NOT IN (SELECT resourceid FROM #ResourceIDsToIgnore)

                -- find all of the iParcel labels we want to wipe out
                INSERT INTO #LabelsToCleanUp		
	                SELECT 
		                [ObjectReference].ObjectReferenceID AS ObjectReferenceID, 
		                [ObjectReference].ObjectID AS ResourceID, 		
		                CASE 
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 0 THEN 'EPL'
			                WHEN ISNULL([Shipment].ActualLabelFormat, -1) = 1 THEN 'ZPL'
			                ELSE 'JPG'
		                END as ImageFormat
	                FROM iParcelShipment i
		                INNER JOIN Shipment ON i.ShipmentID = Shipment.ShipmentID
		                INNER JOIN [ObjectReference] ON Shipment.ShipmentID = [ObjectReference].ConsumerID		
		                INNER JOIN [Resource] ON [Resource].ResourceID = [ObjectReference].ObjectID
	                where 
		                Shipment.Processed = 1
		                AND Shipment.ShipmentType = 12	-- iParcel
		                AND Shipment.ProcessedDate < @olderThan	
		                AND [ObjectReference].ObjectID NOT IN (SELECT resourceid FROM #ResourceIDsToIgnore)

                CREATE INDEX IX_ResourceWorking on #LabelsToCleanUp (ObjectReferenceID, ResourceID)		

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
