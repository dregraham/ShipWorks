-- This script will estimate the amount of space freed up by deleting standard labels

-- don't want to block anything
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
 
DECLARE 
	@deleteOlderThan datetime
	
SET @deleteOlderThan = '{CUTOFFDATE}'		--	yyyy-mm-dd

-- if this is the first time we've run this, figure out which resources must go
IF OBJECT_ID('tempdb..#resourceestimate') IS NULL
BEGIN
	-- create temp table
	CREATE TABLE #resourceestimate ( 
		ObjectReferenceID bigint,
		ResourceID bigint,
		ImageFormat nvarchar(5)
	)

	-- find all of the UPS labels we want to wipe out
	INSERT INTO #resourceestimate
		SELECT r.ObjectReferenceID AS ObjectReferenceID, ObjectID AS ResourceID, 'GIF' as ImageFormat
		FROM [UpsPackage] p, [Shipment] s, [ObjectReference] r
		WHERE 
		p.ShipmentID = s.ShipmentID
		AND p.UpsPackageID = r.ConsumerID	
		AND s.ProcessedDate < @deleteOlderThan
		AND s.Processed = 1
		AND s.ActualLabelFormat IS NULL -- don't wipe thermals
		AND s.ShipmentType = 0	-- UPS online tools

	-- find all of the fedex labels we want to wipe out
	INSERT INTO #resourceestimate		
		SELECT r.ObjectReferenceID AS ObjectReferenceID, ObjectID AS ResourceID, 'PNG' as ImageFormat
		FROM [FedexPackage] p, [Shipment] s, [ObjectReference] r
		WHERE 
		p.ShipmentID = s.ShipmentID
		AND p.FedexPackageID = r.ConsumerID		
		AND s.ProcessedDate < @deleteOlderThan	
		AND s.Processed = 1
		AND s.ActualLabelFormat IS NULL -- don't wipe thermals
		AND s.ShipmentType = 6	-- FedEx
		
	-- Endicia labels
	INSERT INTO #resourceestimate		
		SELECT r.ObjectReferenceID AS ObjectReferenceID, ObjectID AS ResourceID, 'PNG' as ImageFormat
		FROM [PostalShipment] p, [Shipment] s, [ObjectReference] r
		WHERE 
		p.ShipmentID = s.ShipmentID		
		AND p.ShipmentID = r.ConsumerID		
		AND s.ProcessedDate < @deleteOlderThan
		AND s.Processed = 1
		AND s.ActualLabelFormat IS NULL
		AND s.ShipmentType = 2 -- endicia

	-- Express1 labels
	INSERT INTO #resourceestimate		
		SELECT r.ObjectReferenceID AS ObjectReferenceID, ObjectID AS ResourceID, 'PNG' as ImageFormat
		FROM [PostalShipment] p, [Shipment] s, [ObjectReference] r
		WHERE 
		p.ShipmentID = s.ShipmentID		
		AND p.ShipmentID = r.ConsumerID		
		AND s.ProcessedDate < @deleteOlderThan
		AND s.Processed = 1
		AND s.ActualLabelFormat IS NULL
		AND s.ShipmentType = 9 -- express1
		
	-- Usps
	INSERT INTO #resourceestimate		
		SELECT r.ObjectReferenceID AS ObjectReferenceID, ObjectID AS ResourceID, 'PNG' as ImageFormat
		FROM [PostalShipment] p, [Shipment] s, [ObjectReference] r
		WHERE 
		p.ShipmentID = s.ShipmentID
		AND p.ShipmentID = r.ConsumerID		
		AND s.ProcessedDate < @deleteOlderThan
		AND s.Processed = 1
		AND s.ActualLabelFormat IS NULL
		AND s.ShipmentType = 15 -- usps	
END

SELECT SUM(DataLength(r.Data) + DataLength(r.Checksum)) from [Resource] r INNER JOIN #resourceestimate e 
ON e.resourceID = r.ResourceID

DROP TABLE #resourceestimate