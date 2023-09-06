-- With FedEx Address Validation being removed any records referencing it
-- are being moved over to ShipWorks Address Validation
UPDATE FedExProfile
SET ResidentialDetermination = 4
WHERE ResidentialDetermination = 3
GO

UPDATE s
SET s.ResidentialDetermination = 4
FROM FedExShipment f
INNER JOIN Shipment s
	ON f.ShipmentID = s.ShipmentID
WHERE s.ResidentialDetermination = 3