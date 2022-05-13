PRINT N'ALTERING [dbo].[DhlEcommerceShipment]'
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'AncillaryEndorsement') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD [AncillaryEndorsement] [int] NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_AncillaryEndorsement] DEFAULT ((0))
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'StampsTransactionID') IS NOT NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] DROP COLUMN [StampsTransactionID]
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'ScanFormBatchID') IS NOT NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] DROP COLUMN [ScanFormBatchID]
GO

PRINT N'ALTERING [dbo].[DhlEcommerceAccount]'
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceAccount]', N'AncillaryEndorsement') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceAccount] ADD [AncillaryEndorsement] [int] NOT NULL CONSTRAINT [DF_DhlEcommerceAccount_AncillaryEndorsement] DEFAULT ((0))
GO

PRINT N'ALTERING [dbo].[DhlEcommerceProfile]'
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceProfile]', N'AncillaryEndorsement') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceProfile] ADD [AncillaryEndorsement] [int] NULL 
GO