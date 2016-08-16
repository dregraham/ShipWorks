PRINT N'Update ShipmentChargeCountryCode in UpsProfile for PrimaryProfile where the value is not set.'
GO
UPDATE up
SET up.ShipmentChargeCountryCode = 'US'
FROM UpsProfile up
INNER JOIN ShippingProfile sp ON sp.ShippingProfileID = up.ShippingProfileID
WHERE sp.ShipmentTypePrimary = 1 AND ISNULL(up.ShipmentChargeCountryCode, '') = ''
GO