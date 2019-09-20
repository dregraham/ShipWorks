PRINT N'Altering [dbo].[ShopifyStore]'
GO
IF COL_LENGTH(N'[dbo].[ShopifyStore]', N'ShopifyFulfillmentLocation') IS NULL
ALTER TABLE [dbo].[ShopifyStore] ADD[ShopifyFulfillmentLocation] [bigint] NOT NULL CONSTRAINT [DF_ShopifyStore_ShopifyFulfillmentLocation] DEFAULT ((0))
GO

PRINT N'Altering [dbo].[OdbcStore]'
GO
IF COL_LENGTH(N'[dbo].[OdbcStore]', N'WarehouseLastModified') IS NULL
ALTER TABLE [dbo].[OdbcStore] ADD[WarehouseLastModified] [datetime2] NULL
GO