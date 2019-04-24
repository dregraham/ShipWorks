PRINT N'Altering [dbo].[Product]'
GO
IF COL_LENGTH(N'[dbo].[Product]', N'UploadToWarehouseNeeded') IS NULL
	ALTER TABLE [dbo].[Product] ADD [UploadToWarehouseNeeded] [bit] NOT NULL CONSTRAINT [DF_Product_UploadToWarehouseNeeded] DEFAULT (1)



