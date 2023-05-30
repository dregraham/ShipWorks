-- ShipEngine does not support EPL, so convert FedEx profiles and shipments to be zpl
UPDATE sp
SET sp.RequestedLabelFormat = 1
FROM ShippingProfile sp
INNER JOIN FedExProfile fp
	ON sp.ShippingProfileID = fp.ShippingProfileID
WHERE sp.RequestedLabelFormat = 0
GO

UPDATE f
SET f.RequestedLabelFormat = 1
FROM FedExShipment f
INNER JOIN Shipment s
	ON f.ShipmentID = s.ShipmentID
WHERE s.Processed = 0 and s.RequestedLabelFormat = 0
GO

UPDATE s
SET s.RequestedLabelFormat = 1
FROM FedExShipment f
INNER JOIN Shipment s
	ON f.ShipmentID = s.ShipmentID
WHERE s.Processed = 0 and s.RequestedLabelFormat = 0
GO