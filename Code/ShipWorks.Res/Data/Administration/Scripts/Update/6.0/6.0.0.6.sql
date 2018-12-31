PRINT N'Updating Shipment.OnlineShipmentID'
GO
	UPDATE Shipment SET OnlineShipmentID = CONVERT(NVARCHAR(128), ShipmentID) WHERE Processed = 1 AND OnlineShipmentID = ''
GO
