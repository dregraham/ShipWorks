PRINT N'ALTERING [dbo].[DhlEcommerceShipment]'
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'AncillaryEndorsement') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD [AncillaryEndorsement] [nvarchar](50) NOT NULL DEFAULT ((''))
GO

PRINT N'ALTERING [dbo].[DhlEcommerceProfile]'
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceProfile]', N'AncillaryEndorsement') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceProfile] ADD [AncillaryEndorsement] [nvarchar](50) NULL
GO