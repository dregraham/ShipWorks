PRINT N'Altering [dbo].[UspsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UspsShipment]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[UspsShipment] ADD [CustomsRecipientTin] [nvarchar] (25) NULL
GO

PRINT N'Altering [dbo].[ShippingProfile]'
GO
IF COL_LENGTH(N'[dbo].[ShippingProfile]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[ShippingProfile] ADD [CustomsRecipientTin] [nvarchar] (25) NULL
GO

PRINT N'Altering [dbo].[PostalProfile]'
GO
IF COL_LENGTH(N'[dbo].[PostalProfile]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[PostalProfile] ADD [CustomsRecipientTin] [nvarchar] (25) NULL
GO

PRINT N'Altering [dbo].[PostalShipment]'
GO
IF COL_LENGTH(N'[dbo].[PostalShipment]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[PostalShipment] ADD [CustomsRecipientTin] [nvarchar] (25) NULL
GO