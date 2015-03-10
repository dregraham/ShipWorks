PRINT N'Updating data begin.';

GO

-- Update FedEx profiles to use address validation for residential determination
-- if it was using the default
UPDATE FedExProfile
SET ResidentialDetermination = 4
WHERE ResidentialDetermination = 0
	AND ShippingProfileID IN (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentTypePrimary = 1)

GO

-- Update UPS profiles to use address validation for residential determination
-- if it was using the default
UPDATE UpsProfile
SET ResidentialDetermination = 4
WHERE ResidentialDetermination = 0
	AND ShippingProfileID IN (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentTypePrimary = 1)

GO

-- Update OnTrac profiles to use address validation for residential determination
-- if it was using the default
UPDATE OnTracProfile
SET ResidentialDetermination = 4
WHERE ResidentialDetermination = 0
	AND ShippingProfileID IN (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentTypePrimary = 1)

GO

-- Mark the address validation status to pending for all orders in the past 30 days
-- that have either no shipments or at least one unprocessed shipment
UPDATE [Order]
SET ShipAddressValidationStatus = 1
FROM [Order]
	LEFT JOIN Shipment
		ON [Order].OrderId = Shipment.OrderID
WHERE DATEDIFF(d, [Order].OrderDate, GETDATE()) <= 30 
	AND (Shipment.ShipmentID IS NULL OR Shipment.Processed = 0)
GO

-- Mark the address validation status to pending for all unprocessed shipments that belong
-- to orders that have had their status updated
UPDATE Shipment
SET ShipAddressValidationStatus = 1
FROM [Order]
WHERE [Order].OrderId = Shipment.OrderId
	AND [Order].ShipAddressValidationStatus = 1
	AND Shipment.Processed = 0
GO

PRINT N'Update data complete.';

GO
