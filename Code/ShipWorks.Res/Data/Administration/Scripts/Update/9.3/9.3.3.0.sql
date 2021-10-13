PRINT N'Altering [dbo].[FedexShipment]'
GO
IF COL_LENGTH(N'[dbo].[FedexShipment]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[FedexShipment] ADD [CustomsRecipientTINType] [int] NULL
GO

PRINT N'Altering [dbo].[FedexProfile]'
GO
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[FedexProfile] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[FedexProfile] ADD [CustomsRecipientTINType] [int] NULL
GO
UPDATE fedex
SET fedex.[CustomsRecipientTINType] = '0'
FROM FedexProfile fedex
INNER JOIN ShippingProfile ship
    ON fedex.ShippingProfileID = ship.ShippingProfileID
WHERE ship.ShipmentTypePrimary = 1
GO