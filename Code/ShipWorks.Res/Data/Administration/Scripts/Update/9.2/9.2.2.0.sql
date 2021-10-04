PRINT N'Altering [dbo].[Store]'
GO
IF COL_LENGTH(N' [dbo].[Store]', N'ManagedInHub') IS NULL
ALTER TABLE [dbo].[Store] ADD
[ManagedInHub] [bit] NULL
GO