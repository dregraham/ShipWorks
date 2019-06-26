PRINT N'Altering [dbo].[Store]'
GO
IF COL_LENGTH(N'[dbo].[Store]', N'WarehouseStoreID') IS NULL
	ALTER TABLE [dbo].Store ADD [WarehouseStoreID] [uniqueidentifier] NULL
GO