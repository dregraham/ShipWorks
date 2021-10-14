PRINT N'Altering [dbo].[FedexShipment]'
GO
IF COL_LENGTH(N'[dbo].[FedexShipment]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[FedexShipment] ADD [CustomsRecipientTINType] [int] NULL
GO

UPDATE fedexShipment
SET fedexShipment.[CustomsRecipientTINType] = 0 
FROM FedexShipment fedexShipment
WHERE fedexShipment.[CustomsRecipientTINType] IS NULL
GO

PRINT N'Altering [dbo].[FedexProfile]'
GO
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[FedexProfile] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[FedexProfile] ADD [CustomsRecipientTINType] [int] NULL
GO

UPDATE fedexProfile
SET fedexProfile.[CustomsRecipientTINType] = 0 , fedexProfile.[CustomsRecipientTIN] = ''
FROM FedexProfile fedexProfile
INNER JOIN ShippingProfile shipProfile
    ON fedexProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE shipProfile.ShipmentTypePrimary = 1 AND fedexProfile.[CustomsRecipientTINType] IS NULL AND fedexProfile.[CustomsRecipientTIN] IS NULL
GO