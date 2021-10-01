PRINT N'Altering [dbo].[FedexShipment]'
GO
IF COL_LENGTH(N'[dbo].[FedexShipment]', N'TinType') IS NULL
ALTER TABLE [dbo].[FedexShipment] ADD [CustomsRecipientTinType] [int] NULL
GO

PRINT N'Altering [dbo].[FedexProfile]'
GO
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[FedexProfile] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[FedexProfile]', N'TinType') IS NULL
ALTER TABLE [dbo].[FedexProfile] ADD [CustomsRecipientTinType] [int] NULL
GO