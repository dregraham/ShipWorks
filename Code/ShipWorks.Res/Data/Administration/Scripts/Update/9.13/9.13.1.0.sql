PRINT N'Altering [dbo].[FedExProfile]'
GO
IF COL_LENGTH(N'[dbo].[FedExProfile]', N'PayorCountryCode') IS NULL
ALTER TABLE [dbo].[FedExProfile] ADD[PayorCountryCode] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
IF COL_LENGTH(N'[dbo].[FedExProfile]', N'PayorPostalCode') IS NULL
ALTER TABLE [dbo].[FedExProfile] ADD[PayorPostalCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'Updating FedEx Primary Profile'
GO

UPDATE fp
SET [PayorCountryCode] = '', [PayorPostalCode] = ''
FROM dbo.FedExProfile fp
INNER JOIN dbo.ShippingProfile p ON p.ShippingProfileID = fp.ShippingProfileID
WHERE [PayorCountryCode] IS NULL AND PayorPostalCode IS NULL AND p.ShipmentTypePrimary = 1
GO