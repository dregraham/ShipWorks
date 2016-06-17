UPDATE stamps
SET stamps.RateShop = 0
FROM StampsProfile stamps
INNER JOIN shippingprofile ship
	ON ship.ShippingProfileID = stamps.ShippingProfileID
WHERE ship.ShipmentTypePrimary = 1