-- set FedEx Profiles with an OriginResidentialDetermination 
-- of 3 ("FedEx address lookup") to 0 ("Commercial if company is entered")
UPDATE FedExProfile
SET OriginResidentialDetermination = 0
WHERE OriginResidentialDetermination = 3

GO

-- set unprocessed shipments with an OriginResidentialDetermination 
-- of 3 ("FedEx address lookup") to 0 ("Commercial if company is entered")
UPDATE f
SET f.OriginResidentialDetermination = 0
FROM FedExShipment f
INNER JOIN Shipment s
	ON f.ShipmentID = s.ShipmentID
WHERE s.Processed = 0 AND f.OriginResidentialDetermination = 3

GO