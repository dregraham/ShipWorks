﻿-- Forcing schema update
PRINT N'ALTERING [dbo].[UpsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientTIN] [nvarchar] (35) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientTINType] [int] NULL 
GO
UPDATE upsShipment
SET upsShipment.[CustomsRecipientTINType] = 0 
FROM UpsShipment upsShipment
WHERE upsShipment.[CustomsRecipientTINType] IS NULL
GO

PRINT N'ALTERING [dbo].[UpsProfile]'
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientTIN] [nvarchar] (35) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientTINType] [int] NULL
GO
UPDATE upsProfile
SET upsProfile.[CustomsRecipientTIN] = '', upsProfile.[CustomsRecipientTINType] = 0
from UpsProfile upsProfile
INNER JOIN ShippingProfile shipProfile
    on upsProfile.ShippingProfileID = shipProfile.ShippingProfileID
where shipProfile.ShipmentTypePrimary=1 AND upsProfile.[CustomsRecipientTINType] IS NULL
GO

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