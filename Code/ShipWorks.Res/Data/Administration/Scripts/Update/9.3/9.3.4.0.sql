-- Forcing schema update
PRINT N'ALTERING [dbo].[UpsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientTINType] [int] NULL 
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientType') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientType] [int] NULL 
GO
UPDATE upsShipment
SET upsShipment.[CustomsRecipientTINType] = 3 
FROM UpsShipment upsShipment
WHERE upsShipment.[CustomsRecipientTINType] IS NULL AND upsShipment.[CustomsRecipientType] IS NULL
GO

PRINT N'ALTERING [dbo].[UpsProfile]'
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientTINType] [int] NULL
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientType') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientType] [int] NULL
GO
UPDATE upsProfile
SET upsProfile.[CustomsRecipientTIN] = '', upsProfile.[CustomsRecipientTINType] = 0, upsProfile.[CustomsRecipientType] = 0
from UpsProfile upsProfile
INNER JOIN ShippingProfile shipProfile
    on upsProfile.ShippingProfileID = shipProfile.ShippingProfileID
where shipProfile.ShipmentTypePrimary=1 AND upsProfile.[CustomsRecipientTINType] IS NULL AND upsProfile.[CustomsRecipientTIN] IS NULL