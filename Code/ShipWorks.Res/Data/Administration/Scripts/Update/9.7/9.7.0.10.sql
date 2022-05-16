PRINT N'ALTERING [dbo].[DhlEcommerceAccount]'
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceAccount]', N'AncillaryEndorsement') IS NOT NULL
	ALTER TABLE dbo.DhlEcommerceAccount
		DROP CONSTRAINT DF_DhlEcommerceAccount_AncillaryEndorsement
	GO
	ALTER TABLE dbo.DhlEcommerceAccount
		DROP COLUMN AncillaryEndorsement
GO