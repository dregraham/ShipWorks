PRINT N'Altering [dbo].[PostalProfile]'
GO
IF COL_LENGTH(N'[dbo].[PostalProfile]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[PostalProfile] ADD [CustomsRecipientTin] [nvarchar] (14) NULL
GO
UPDATE postalProfile
SET postalProfile.[CustomsRecipientTIN] = ''
FROM [PostalProfile] postalProfile
INNER JOIN ShippingProfile shipProfile
    ON postalProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE shipProfile.ShipmentTypePrimary = 1 AND postalProfile.[CustomsRecipientTIN] IS NULL
GO

PRINT N'Altering [dbo].[PostalShipment]'
GO
IF COL_LENGTH(N'[dbo].[PostalShipment]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[PostalShipment] ADD [CustomsRecipientTin] [nvarchar] (14) NULL
GO