PRINT N'Altering [dbo].[Product]'
GO
IF COL_LENGTH(N'[dbo].[Product]', N'UploadToWarehouseNeeded') IS NULL
	ALTER TABLE [dbo].[Product] ADD [UploadToWarehouseNeeded] [bit] NOT NULL CONSTRAINT [DF_Product_UploadToWarehouseNeeded] DEFAULT (1)

PRINT N'Altering [dbo].[Store]'
GO
IF COL_LENGTH(N'[dbo].[Store]', N'WarehouseStoreID') IS NULL
	ALTER TABLE [dbo].Store ADD [WarehouseStoreID] [uniqueidentifier] NULL
GO