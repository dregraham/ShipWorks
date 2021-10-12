-- Forcing schema update
PRINT N'ALTERING [dbo].[UpsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientTINType] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'CustomsRecipientType') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [CustomsRecipientType] [nvarchar] (24) NULL
GO

PRINT N'ALTERING [dbo].[UpsProfile]'
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientTINType] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[UpsProfile]', N'CustomsRecipientType') IS NULL
ALTER TABLE [dbo].[UpsProfile] ADD [CustomsRecipientType] [nvarchar] (24) NULL
GO