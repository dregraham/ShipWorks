PRINT N'Altering [dbo].[Store]'
GO
IF COL_LENGTH(N'[dbo].[Store]', N'ManagedInHub') IS NULL
ALTER TABLE [dbo].[Store] ADD
	[ManagedInHub] [bit] NOT NULL CONSTRAINT [DF_Store_ManagedInHub] DEFAULT (0)
GO