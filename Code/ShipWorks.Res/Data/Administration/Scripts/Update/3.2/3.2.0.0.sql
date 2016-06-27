
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP
CONSTRAINT [FK_ChannelAdvisorOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [PK_ChannelAdvisorOrder]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [PK_ChannelAdvisorOrderItem]
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrder]
(
[OrderID] [bigint] NOT NULL,
[CustomOrderIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ResellerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineShippingStatus] [int] NOT NULL,
[OnlineCheckoutStatus] [int] NOT NULL,
[OnlinePaymentStatus] [int] NOT NULL,
[FlagStyle] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FlagDescription] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FlagType] [int] NOT NULL,
[MarketplaceNames] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrder]([OrderID], [CustomOrderIdentifier], [ResellerID], [OnlineShippingStatus], [OnlineCheckoutStatus], [OnlinePaymentStatus], FlagStyle, FlagDescription, FlagType, MarketplaceNames) SELECT [OrderID], [CustomOrderIdentifier], [ResellerID], [OnlineShippingStatus], [OnlineCheckoutStatus], [OnlinePaymentStatus], '', '', 0, '' FROM [dbo].[ChannelAdvisorOrder]
GO
DROP TABLE [dbo].[ChannelAdvisorOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrder]', N'ChannelAdvisorOrder'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrder] on [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [PK_ChannelAdvisorOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[MarketplaceName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceBuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceSalesID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Classification] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DistributionCenter] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HarmonizedCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]([OrderItemID], MarketplaceName, [MarketplaceBuyerID], MarketplaceSalesID, [Classification], [DistributionCenter], [HarmonizedCode]) SELECT [OrderItemID], SiteName, [BuyerID], SalesSourceID, [Classification], [DistributionCenter], [HarmonizedCode] FROM [dbo].[ChannelAdvisorOrderItem]
GO
DROP TABLE [dbo].[ChannelAdvisorOrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]', N'ChannelAdvisorOrderItem'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderItem] on [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Altering [dbo].[DownloadDetail]'
GO
ALTER TABLE [dbo].[DownloadDetail] ADD
[ShopifyOrderId] [bigint] NULL
GO
PRINT N'Creating [dbo].[EtsyOrder]'
GO
CREATE TABLE [dbo].[EtsyOrder]
(
[OrderID] [bigint] NOT NULL,
[WasPaid] [bit] NOT NULL,
[WasShipped] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EtsyOrder] on [dbo].[EtsyOrder]'
GO
ALTER TABLE [dbo].[EtsyOrder] ADD CONSTRAINT [PK_EtsyOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[EtsyStore]'
GO
CREATE TABLE [dbo].[EtsyStore]
(
[StoreID] [bigint] NOT NULL,
[EtsyShopID] [bigint] NOT NULL,
[EtsyLogin] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EtsyStoreName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OAuthToken] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OAuthTokenSecret] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_EtsyStore] on [dbo].[EtsyStore]'
GO
ALTER TABLE [dbo].[EtsyStore] ADD CONSTRAINT [PK_EtsyStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[NeweggOrder]'
GO
CREATE TABLE [dbo].[NeweggOrder]
(
[OrderID] [bigint] NOT NULL,
[InvoiceNumber] [bigint] NULL,
[RefundAmount] [money] NULL,
[IsAutoVoid] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_NeweggOrder] on [dbo].[NeweggOrder]'
GO
ALTER TABLE [dbo].[NeweggOrder] ADD CONSTRAINT [PK_NeweggOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[NeweggOrderItem]'
GO
CREATE TABLE [dbo].[NeweggOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[SellerPartNumber] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NeweggItemNumber] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ManufacturerPartNumber] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShippingStatusID] [int] NULL,
[ShippingStatusDescription] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QuantityShipped] [int] NULL
)
GO
PRINT N'Creating primary key [PK_NeweggOrderItem] on [dbo].[NeweggOrderItem]'
GO
ALTER TABLE [dbo].[NeweggOrderItem] ADD CONSTRAINT [PK_NeweggOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[NeweggStore]'
GO
CREATE TABLE [dbo].[NeweggStore]
(
[StoreID] [bigint] NOT NULL,
[SellerID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SecretKey] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_NeweggStore] on [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] ADD CONSTRAINT [PK_NeweggStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[ShopifyOrder]'
GO
CREATE TABLE [dbo].[ShopifyOrder]
(
[OrderID] [bigint] NOT NULL,
[ShopifyOrderID] [bigint] NOT NULL,
[FulfillmentStatusCode] [int] NOT NULL,
[PaymentStatusCode] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyOrder] on [dbo].[ShopifyOrder]'
GO
ALTER TABLE [dbo].[ShopifyOrder] ADD CONSTRAINT [PK_ShopifyOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[ShopifyOrderItem]'
GO
CREATE TABLE [dbo].[ShopifyOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ShopifyOrderItemId] [bigint] NOT NULL,
[ShopifyProductId] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyOrderItem] on [dbo].[ShopifyOrderItem]'
GO
ALTER TABLE [dbo].[ShopifyOrderItem] ADD CONSTRAINT [PK_ShopifyOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[ShopifyStore]'
GO
CREATE TABLE [dbo].[ShopifyStore]
(
[StoreID] [bigint] NOT NULL,
[ShopifyShopUrlName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyShopDisplayName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyAccessToken] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyStore] on [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD CONSTRAINT [PK_ShopifyStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD
CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[EtsyOrder]'
GO
ALTER TABLE [dbo].[EtsyOrder] ADD
CONSTRAINT [FK_EtsyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EtsyStore]'
GO
ALTER TABLE [dbo].[EtsyStore] ADD
CONSTRAINT [FK_EtsyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggOrder]'
GO
ALTER TABLE [dbo].[NeweggOrder] ADD
CONSTRAINT [FK_NeweggOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggOrderItem]'
GO
ALTER TABLE [dbo].[NeweggOrderItem] ADD
CONSTRAINT [FK_NeweggOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] ADD
CONSTRAINT [FK_NeweggStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyOrder]'
GO
ALTER TABLE [dbo].[ShopifyOrder] ADD
CONSTRAINT [FK_ShopifyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyOrderItem]'
GO
ALTER TABLE [dbo].[ShopifyOrderItem] ADD
CONSTRAINT [FK_ShopifyOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD
CONSTRAINT [FK_ShopifyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO

PRINT N'Creating [dbo].[BuyDotComStore]'
GO
CREATE TABLE [dbo].[BuyDotComStore]
(
[StoreID] [bigint] NOT NULL,
[FtpUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_BuyComStore] on [dbo].[BuyDotComStore]'
GO
ALTER TABLE [dbo].[BuyDotComStore] ADD CONSTRAINT [PK_BuyComStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[BuyDotComOrderItem]'
GO
CREATE TABLE [dbo].[BuyDotComOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ListingID] [int] NOT NULL,
[ReferenceID] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Shipping] [money] NOT NULL,
[Tax] [money] NOT NULL,
[Commission] [money] NOT NULL,
[ItemFee] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_BuyDotComOrderItem] on [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] ADD CONSTRAINT [PK_BuyDotComOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO

PRINT N'Adding foreign keys to [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] ADD
CONSTRAINT [FK_BuyDotComOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[BuyDotComStore]'
GO
ALTER TABLE [dbo].[BuyDotComStore] ADD
CONSTRAINT [FK_BuyComStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO


PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'121', 'SCHEMA', N'dbo', 'TABLE', N'EtsyOrder', 'COLUMN', N'WasPaid'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'120', 'SCHEMA', N'dbo', 'TABLE', N'EtsyOrder', 'COLUMN', N'WasShipped'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'122', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'FulfillmentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Fulfillment Status', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'FulfillmentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'121', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'PaymentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Fulfillment Status', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'PaymentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyAccessToken'
GO
EXEC sp_addextendedproperty N'AuditName', N'Access Token', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyAccessToken'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyShopUrlName'
GO
EXEC sp_addextendedproperty N'AuditName', N'Shop Name', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyShopUrlName'
GO


/*--------------
  Rollup CA Marketplace names into the parent Order table
----------------*/

PRINT N'Preparing to upgrade ChannelAdvisor orders...'
GO
SELECT o.OrderID, COALESCE(LEFT(Marketplaces, LEN(Marketplaces) - 1), '') as MarketplaceNames INTO #MarketplaceNames
FROM ChannelAdvisorOrder o
CROSS APPLY (
	SELECT MarketplaceName + ','
	FROM OrderItem oi INNER JOIN ChannelAdvisorOrderItem caoi ON caoi.OrderItemID = oi.OrderItemID
	WHERE oi.OrderID = o.OrderID
	FOR XML PATH('') )
temp ( Marketplaces )
GO

PRINT N'Upgrading ChannelAdvisor orders...'
GO

DECLARE @orderID BIGINT
DECLARE @marketplaceNames varchar(500)
DECLARE @counter int

SET @counter = 1

-- create cursor
DECLARE workCursor CURSOR FOR
SELECT * FROM #MarketplaceNames
OPEN workCursor

FETCH NEXT FROM workCursor
INTO
	@orderID,
	@marketplaceNames
WHILE @@FETCH_STATUS = 0
BEGIN

	UPDATE ChannelAdvisorOrder
		SET MarketplaceNames = @marketplaceNames
		WHERE ChannelAdvisorOrder.OrderID = @orderID

	SET @counter = @counter + 1

	FETCH NEXT FROM workCursor
	INTO
	@orderID,
	@marketplaceNames
END

CLOSE workCursor
DEALLOCATE workCursor

