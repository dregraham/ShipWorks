PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'AuditEnabled') IS NULL
	ALTER TABLE [dbo].[Configuration] ADD [AuditEnabled] [bit] NOT NULL CONSTRAINT [DF_Configuration_AuditEnabled] DEFAULT ((1))
GO


