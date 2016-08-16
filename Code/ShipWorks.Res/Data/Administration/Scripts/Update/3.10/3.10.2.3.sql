-- Update the tracking number of any SmartPost shipments to include the ApplicationId
UPDATE Shipment
	SET TrackingNumber = SmartPostUspsApplicationId + TrackingNumber
	FROM Shipment
		INNER JOIN FedExShipment
			ON Shipment.ShipmentId = FedExShipment.ShipmentID
	WHERE [Service] = 15
		AND Processed = 1
		AND TrackingNumber NOT LIKE SmartPostUspsApplicationId + '%'
