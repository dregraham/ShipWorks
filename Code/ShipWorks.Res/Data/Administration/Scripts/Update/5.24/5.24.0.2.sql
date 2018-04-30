PRINT N'Altering [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD
[ShopifyNotifyCustomer] [bit] NOT NULL CONSTRAINT [DF_ShopifyStore_ShopifyNotifyCustomer] DEFAULT (1)
GO

PRINT N'Dropping constraints from [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] DROP CONSTRAINT [DF_ShopifyStore_ShopifyNotifyCustomer]
GO