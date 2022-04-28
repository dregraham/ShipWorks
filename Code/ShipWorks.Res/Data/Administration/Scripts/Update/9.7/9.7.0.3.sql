PRINT N'ALTERING [dbo].[DhlEcommerceShipment]'
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'AncillaryEndorsement') IS NOT NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] DROP COLUMN [AncillaryEndorsement]
GO

PRINT N'ALTERING [dbo].[DhlEcommerceProfile]'
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceProfile]', N'AncillaryEndorsement') IS NOT NULL
	ALTER TABLE [dbo].[DhlEcommerceProfile] DROP COLUMN [AncillaryEndorsement]
GO