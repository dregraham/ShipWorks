﻿-- Forcing schema update
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
UPDATE ups
SET ups.[CustomsRecipientTIN] = '', ups.[CustomsRecipientTINType] = 0, ups.[CustomsRecipientType] = 0
from UpsProfile ups
INNER JOIN ShippingProfile ship
    on ups.ShippingProfileID = ship.ShippingProfileID
where ship.ShipmentTypePrimary=1