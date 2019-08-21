PRINT N'Altering [dbo].[OdbcStore]'
GO
IF COL_LENGTH(N'[dbo].[OdbcStore]', N'WarehouseLastModified') IS NULL
ALTER TABLE [dbo].[OdbcStore] ADD[WarehouseLastModified] [datetime2] NULL
GO