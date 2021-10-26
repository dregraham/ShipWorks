PRINT N'Altering [dbo].[PostalProfile]'
GO
IF COL_LENGTH(N'[dbo].[PostalProfile]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[PostalProfile] ADD [CustomsRecipientTin] [nvarchar] (24) NULL
GO

PRINT N'Altering [dbo].[PostalShipment]'
GO
IF COL_LENGTH(N'[dbo].[PostalShipment]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[PostalShipment] ADD [CustomsRecipientTin] [nvarchar] (24) NULL
GO