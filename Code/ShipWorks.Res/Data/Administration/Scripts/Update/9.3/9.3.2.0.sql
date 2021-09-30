PRINT N'Altering [dbo].[Shipment]'
GO
IF COL_LENGTH(N'[dbo].[Shipment]', N'IossTaxId') IS NULL
ALTER TABLE [dbo].[Shipment] ADD [IossTaxId] [nvarchar] (25) NULL
GO

PRINT N'Altering [dbo].[FedexShipment]'
GO
IF COL_LENGTH(N'[dbo].[FedexShipment]', N'TinType') IS NULL
ALTER TABLE [dbo].[FedexShipment] ADD [TinType] [int] NULL
GO

PRINT N'Altering [dbo].[FedexProfile]'
GO
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[FedexShipment] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'TinType') IS NULL
ALTER TABLE [dbo].[FedexShipment] ADD [TinType] [int] NULL
GO