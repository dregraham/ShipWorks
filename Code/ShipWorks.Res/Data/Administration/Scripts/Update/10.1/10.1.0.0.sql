PRINT N'Updating [dbo].[PostalProfile] due to removal of two USPS services'
GO

UPDATE PostalProfile
SET Service = 16
FROM PostalProfile
         INNER JOIN ShippingProfile shipProfile
                    ON PostalProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE shipProfile.ShipmentType = 15 AND PostalProfile.Service = 13
GO

UPDATE PostalProfile
SET Service = 16
FROM PostalProfile
         INNER JOIN ShippingProfile shipProfile
                    ON PostalProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE shipProfile.ShipmentType = 2 AND PostalProfile.Service = 13
GO

UPDATE PostalProfile
SET Service = 16
FROM PostalProfile
         INNER JOIN ShippingProfile shipProfile
                    ON PostalProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE shipProfile.ShipmentType = 15 AND PostalProfile.Service = 1 AND PostalProfile.PackagingType = 0
GO

UPDATE PostalProfile
SET Service = 16
FROM PostalProfile
        INNER JOIN ShippingProfile shipProfile
                    ON PostalProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE shipProfile.ShipmentType = 2 AND PostalProfile.Service = 1 AND PostalProfile.PackagingType = 0
GO