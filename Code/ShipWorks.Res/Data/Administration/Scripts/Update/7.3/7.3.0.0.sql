PRINT N'Altering [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD
[ShopifyFulfillmentLocation] [bigint] NOT NULL CONSTRAINT [DF_ShopifyStore_ShopifyFulfillmentLocation] DEFAULT ((0))
GO