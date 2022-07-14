PRINT N'ALTERING [dbo].[Store]'
GO

IF COL_LENGTH(N'[dbo].[Store]', N'PlatformAmazonCarrierId') IS NULL
	ALTER TABLE [dbo].[Store] ADD [PlatformAmazonCarrierID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
GO
