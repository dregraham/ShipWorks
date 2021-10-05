PRINT N'Altering [dbo].[UspsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UspsShipment]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UspsShipment] ADD [CustomsRecipientTIN] [nvarchar] (25) NULL
GO

PRINT N'Altering [dbo].[UspsProfile]'
GO
IF COL_LENGTH(N'[dbo].[UspsProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[UspsProfile] ADD [CustomsRecipientTIN] [nvarchar] (25) NULL
GO