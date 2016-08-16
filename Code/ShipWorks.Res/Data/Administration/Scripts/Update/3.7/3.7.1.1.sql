SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] DROP CONSTRAINT[FK_ShopifyStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] DROP CONSTRAINT [PK_ShopifyStore]
GO
PRINT N'Rebuilding [dbo].[ShopifyStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShopifyStore]
(
[StoreID] [bigint] NOT NULL,
[ShopifyShopUrlName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyShopDisplayName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyAccessToken] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyRequestedShippingOption] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShopifyStore]([StoreID], [ShopifyShopUrlName], [ShopifyShopDisplayName], [ShopifyAccessToken], [ShopifyRequestedShippingOption]) 
SELECT [StoreID], [ShopifyShopUrlName], [ShopifyShopDisplayName], [ShopifyAccessToken], 0 FROM [dbo].[ShopifyStore]
GO
DROP TABLE [dbo].[ShopifyStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShopifyStore]', N'ShopifyStore'
GO
PRINT N'Creating primary key [PK_ShopifyStore] on [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD CONSTRAINT [PK_ShopifyStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD CONSTRAINT [FK_ShopifyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyAccessToken'
GO
EXEC sp_addextendedproperty N'AuditName', N'Access Token', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyAccessToken'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyShopUrlName'
GO
EXEC sp_addextendedproperty N'AuditName', N'Shop Name', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyShopUrlName'
GO
