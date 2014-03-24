CREATE TABLE [dbo].[ShopifyStore] (
    [StoreID]                BIGINT         NOT NULL,
    [ShopifyShopUrlName]     NVARCHAR (100) NOT NULL,
    [ShopifyShopDisplayName] NVARCHAR (100) NOT NULL,
    [ShopifyAccessToken]     NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ShopifyStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_ShopifyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyStore', @level2type = N'COLUMN', @level2name = N'ShopifyShopUrlName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Shop Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyStore', @level2type = N'COLUMN', @level2name = N'ShopifyShopUrlName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyStore', @level2type = N'COLUMN', @level2name = N'ShopifyAccessToken';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Access Token', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyStore', @level2type = N'COLUMN', @level2name = N'ShopifyAccessToken';

