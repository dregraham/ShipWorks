-- With FedEx Address Validation being removed any records referencing it
-- are being moved over to ShipWorks Address Validation
UPDATE FedExProfile
SET OriginResidentialDetermination = 4
WHERE OriginResidentialDetermination = 3
GO

UPDATE f
SET f.OriginResidentialDetermination = 4
FROM FedExShipment f
INNER JOIN Shipment s
	ON f.ShipmentID = s.ShipmentID
WHERE s.Processed = 0 AND f.OriginResidentialDetermination = 3
GO