

PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD
[CombineSplitStatus] [int] NOT NULL CONSTRAINT [DF_Order_CombineSplitStatus] DEFAULT ((0))
GO
PRINT N'Creating index [IX_Order_CombineSplitStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_CombineSplitStatus] ON [dbo].[Order] ([CombineSplitStatus])
GO
PRINT N'Altering [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] ADD
[DialogSettings] [xml] NULL
GO
PRINT N'Creating [dbo].[OrderSearch]'
GO
CREATE TABLE [dbo].[OrderSearch]
(
[OrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[StoreID] [bigint] NOT NULL,
[OrderNumber] [bigint] NOT NULL,
[OrderNumberComplete] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderSearch] on [dbo].[OrderSearch]'
GO
ALTER TABLE [dbo].[OrderSearch] ADD CONSTRAINT [PK_OrderSearch] PRIMARY KEY CLUSTERED  ([OrderSearchID])
GO
PRINT N'Creating index [IX_OrderSearch_OrderNumber] on [dbo].[OrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderSearch_OrderNumber] ON [dbo].[OrderSearch] ([OrderNumber]) INCLUDE ([OrderID], [StoreID])
GO
PRINT N'Creating index [IX_OrderSearch_OrderNumberComplete] on [dbo].[OrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderSearch_OrderNumberComplete] ON [dbo].[OrderSearch] ([OrderNumberComplete]) INCLUDE ([OrderID], [StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderSearch]'
GO
ALTER TABLE [dbo].[OrderSearch] ADD CONSTRAINT [FK_OrderSearch_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderSearch] ADD CONSTRAINT [FK_OrderSearch_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating [dbo].[AmazonOrderSearch]'
GO
CREATE TABLE [dbo].[AmazonOrderSearch]
(
[AmazonOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[AmazonOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonOrderSearch] on [dbo].[AmazonOrderSearch]'
GO
ALTER TABLE [dbo].[AmazonOrderSearch] ADD CONSTRAINT [PK_AmazonOrderSearch] PRIMARY KEY CLUSTERED  ([AmazonOrderSearchID])
GO
PRINT N'Creating index [IX_AmazonOrderSearch_AmazonOrderID] on [dbo].[AmazonOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_AmazonOrderSearch_AmazonOrderID] ON [dbo].[AmazonOrderSearch] ([AmazonOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[ChannelAdvisorOrderSearch]'
GO
CREATE TABLE [dbo].[ChannelAdvisorOrderSearch]
(
[ChannelAdvisorOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[CustomOrderIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderSearch] on [dbo].[ChannelAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderSearch] ADD CONSTRAINT [PK_ChannelAdvisorOrderSearch] PRIMARY KEY CLUSTERED  ([ChannelAdvisorOrderSearchID])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier] on [dbo].[ChannelAdvisorOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier] ON [dbo].[ChannelAdvisorOrderSearch] ([CustomOrderIdentifier]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[ClickCartProOrderSearch]'
GO
CREATE TABLE [dbo].[ClickCartProOrderSearch]
(
[ClickCartProOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ClickCartProOrderID] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ClickCartProOrderSearch] on [dbo].[ClickCartProOrderSearch]'
GO
ALTER TABLE [dbo].[ClickCartProOrderSearch] ADD CONSTRAINT [PK_ClickCartProOrderSearch] PRIMARY KEY CLUSTERED  ([ClickCartProOrderSearchID])
GO
PRINT N'Creating index [IX_ClickCartProOrderSearch_ClickCartProOrderID] on [dbo].[ClickCartProOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_ClickCartProOrderSearch_ClickCartProOrderID] ON [dbo].[ClickCartProOrderSearch] ([ClickCartProOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[CommerceInterfaceOrderSearch]'
GO
CREATE TABLE [dbo].[CommerceInterfaceOrderSearch]
(
[CommerceInterfaceOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[CommerceInterfaceOrderNumber] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_CommerceInterfaceOrderSearch] on [dbo].[CommerceInterfaceOrderSearch]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrderSearch] ADD CONSTRAINT [PK_CommerceInterfaceOrderSearch] PRIMARY KEY CLUSTERED  ([CommerceInterfaceOrderSearchID])
GO
PRINT N'Creating index [IX_CommerceInterfaceOrderSearch_CommerceInterfaceOrderNumber] on [dbo].[CommerceInterfaceOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_CommerceInterfaceOrderSearch_CommerceInterfaceOrderNumber] ON [dbo].[CommerceInterfaceOrderSearch] ([CommerceInterfaceOrderNumber]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[EbayOrderSearch]'
GO
CREATE TABLE [dbo].[EbayOrderSearch]
(
[EbayOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[EbayBuyerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellingManagerRecord] [int] NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EbayOrderSearch] on [dbo].[EbayOrderSearch]'
GO
ALTER TABLE [dbo].[EbayOrderSearch] ADD CONSTRAINT [PK_EbayOrderSearch] PRIMARY KEY CLUSTERED  ([EbayOrderSearchID])
GO
PRINT N'Creating index [IX_EbayOrderSearch_EbayBuyerID] on [dbo].[EbayOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_EbayOrderSearch_EbayBuyerID] ON [dbo].[EbayOrderSearch] ([EbayBuyerID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[GrouponOrderSearch]'
GO
CREATE TABLE [dbo].[GrouponOrderSearch]
(
[GrouponOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[GrouponOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_GrouponOrderSearch] on [dbo].[GrouponOrderSearch]'
GO
ALTER TABLE [dbo].[GrouponOrderSearch] ADD CONSTRAINT [PK_GrouponOrderSearch] PRIMARY KEY CLUSTERED  ([GrouponOrderSearchID])
GO
PRINT N'Creating index [IX_GrouponOrderSearch_GrouponOrderID] on [dbo].[GrouponOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_GrouponOrderSearch_GrouponOrderID] ON [dbo].[GrouponOrderSearch] ([GrouponOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[LemonStandOrderSearch]'
GO
CREATE TABLE [dbo].[LemonStandOrderSearch]
(
[LemonStandOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[LemonStandOrderID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_LemonStandOrderSearch] on [dbo].[LemonStandOrderSearch]'
GO
ALTER TABLE [dbo].[LemonStandOrderSearch] ADD CONSTRAINT [PK_LemonStandOrderSearch] PRIMARY KEY CLUSTERED  ([LemonStandOrderSearchID])
GO
PRINT N'Creating index [IX_LemonStandOrderSearch_LemonStandOrderID] on [dbo].[LemonStandOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_LemonStandOrderSearch_LemonStandOrderID] ON [dbo].[LemonStandOrderSearch] ([LemonStandOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[MagentoOrderSearch]'
GO
CREATE TABLE [dbo].[MagentoOrderSearch]
(
[MagentoOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[MagentoOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MagentoOrderSearch] on [dbo].[MagentoOrderSearch]'
GO
ALTER TABLE [dbo].[MagentoOrderSearch] ADD CONSTRAINT [PK_MagentoOrderSearch] PRIMARY KEY CLUSTERED  ([MagentoOrderSearchID])
GO
PRINT N'Creating index [IX_MagentoOrderSearch_MagentoOrderID] on [dbo].[MagentoOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_MagentoOrderSearch_MagentoOrderID] ON [dbo].[MagentoOrderSearch] ([MagentoOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[MarketplaceAdvisorOrderSearch]'
GO
CREATE TABLE [dbo].[MarketplaceAdvisorOrderSearch]
(
[MarketplaceAdvisorOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[InvoiceNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellerOrderNumber] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MarketplaceAdvisorOrderSearch] on [dbo].[MarketplaceAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrderSearch] ADD CONSTRAINT [PK_MarketplaceAdvisorOrderSearch] PRIMARY KEY CLUSTERED  ([MarketplaceAdvisorOrderSearchID])
GO
PRINT N'Creating [dbo].[NetworkSolutionsOrderSearch]'
GO
CREATE TABLE [dbo].[NetworkSolutionsOrderSearch]
(
[NetworkSolutionsOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[NetworkSolutionsOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_NetworkSolutionsOrderSearch] on [dbo].[NetworkSolutionsOrderSearch]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrderSearch] ADD CONSTRAINT [PK_NetworkSolutionsOrderSearch] PRIMARY KEY CLUSTERED  ([NetworkSolutionsOrderSearchID])
GO
PRINT N'Creating [dbo].[OrderMotionOrderSearch]'
GO
CREATE TABLE [dbo].[OrderMotionOrderSearch]
(
[OrderMotionOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[OrderMotionShipmentID] [int] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderMotionOrderSearch] on [dbo].[OrderMotionOrderSearch]'
GO
ALTER TABLE [dbo].[OrderMotionOrderSearch] ADD CONSTRAINT [PK_OrderMotionOrderSearch] PRIMARY KEY CLUSTERED  ([OrderMotionOrderSearchID])
GO
PRINT N'Creating [dbo].[PayPalOrderSearch]'
GO
CREATE TABLE [dbo].[PayPalOrderSearch]
(
[PayPalOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[TransactionID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PayPalOrderSearch] on [dbo].[PayPalOrderSearch]'
GO
ALTER TABLE [dbo].[PayPalOrderSearch] ADD CONSTRAINT [PK_PayPalOrderSearch] PRIMARY KEY CLUSTERED  ([PayPalOrderSearchID])
GO
PRINT N'Creating [dbo].[ProStoresOrderSearch]'
GO
CREATE TABLE [dbo].[ProStoresOrderSearch]
(
[ProStoresOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ConfirmationNumber] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProStoresOrderSearch] on [dbo].[ProStoresOrderSearch]'
GO
ALTER TABLE [dbo].[ProStoresOrderSearch] ADD CONSTRAINT [PK_ProStoresOrderSearch] PRIMARY KEY CLUSTERED  ([ProStoresOrderSearchID])
GO
PRINT N'Creating [dbo].[SearsOrderSearch]'
GO
CREATE TABLE [dbo].[SearsOrderSearch]
(
[SearsOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[PoNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsOrderSearch] on [dbo].[SearsOrderSearch]'
GO
ALTER TABLE [dbo].[SearsOrderSearch] ADD CONSTRAINT [PK_SearsOrderSearch] PRIMARY KEY CLUSTERED  ([SearsOrderSearchID])
GO
PRINT N'Creating [dbo].[ShopifyOrderSearch]'
GO
CREATE TABLE [dbo].[ShopifyOrderSearch]
(
[ShopifyOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ShopifyOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyOrderSearch] on [dbo].[ShopifyOrderSearch]'
GO
ALTER TABLE [dbo].[ShopifyOrderSearch] ADD CONSTRAINT [PK_ShopifyOrderSearch] PRIMARY KEY CLUSTERED  ([ShopifyOrderSearchID])
GO
PRINT N'Creating [dbo].[ThreeDCartOrderSearch]'
GO
CREATE TABLE [dbo].[ThreeDCartOrderSearch]
(
[ThreeDCartOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ThreeDCartOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrderSearch] on [dbo].[ThreeDCartOrderSearch]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderSearch] ADD CONSTRAINT [PK_ThreeDCartOrderSearch] PRIMARY KEY CLUSTERED  ([ThreeDCartOrderSearchID])
GO
PRINT N'Creating [dbo].[WalmartOrderSearch]'
GO
CREATE TABLE [dbo].[WalmartOrderSearch]
(
[WalmartOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[PurchaseOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartOrderSearch] on [dbo].[WalmartOrderSearch]'
GO
ALTER TABLE [dbo].[WalmartOrderSearch] ADD CONSTRAINT [PK_WalmartOrderSearch] PRIMARY KEY CLUSTERED  ([WalmartOrderSearchID])
GO
PRINT N'Creating index [IX_WalmartOrderSearch_PurchaseOrderID] on [dbo].[WalmartOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_WalmartOrderSearch_PurchaseOrderID] ON [dbo].[WalmartOrderSearch] ([PurchaseOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[YahooOrderSearch]'
GO
CREATE TABLE [dbo].[YahooOrderSearch]
(
[YahooOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[YahooOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_YahooOrderSearch] on [dbo].[YahooOrderSearch]'
GO
ALTER TABLE [dbo].[YahooOrderSearch] ADD CONSTRAINT [PK_YahooOrderSearch] PRIMARY KEY CLUSTERED  ([YahooOrderSearchID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrderSearch]'
GO
ALTER TABLE [dbo].[AmazonOrderSearch] ADD CONSTRAINT [FK_AmazonOrderSearch_AmazonOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[AmazonOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderSearch] ADD CONSTRAINT [FK_ChannelAdvisorOrderSearch_ChannelAdvisorOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ChannelAdvisorOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ClickCartProOrderSearch]'
GO
ALTER TABLE [dbo].[ClickCartProOrderSearch] ADD CONSTRAINT [FK_ClickCartProOrderSearch_ClickCartProOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ClickCartProOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[CommerceInterfaceOrderSearch]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrderSearch] ADD CONSTRAINT [FK_CommerceInterfaceOrderSearch_CommerceInterfaceOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[CommerceInterfaceOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderSearch]'
GO
ALTER TABLE [dbo].[EbayOrderSearch] ADD CONSTRAINT [FK_EbayOrderSearch_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[GrouponOrderSearch]'
GO
ALTER TABLE [dbo].[GrouponOrderSearch] ADD CONSTRAINT [FK_GrouponOrderSearch_GrouponOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[GrouponOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[LemonStandOrderSearch]'
GO
ALTER TABLE [dbo].[LemonStandOrderSearch] ADD CONSTRAINT [FK_LemonStandOrderSearch_LemonStandOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[LemonStandOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[MagentoOrderSearch]'
GO
ALTER TABLE [dbo].[MagentoOrderSearch] ADD CONSTRAINT [FK_MagentoOrderSearch_MagentoOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[MagentoOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrderSearch] ADD CONSTRAINT [FK_MarketplaceAdvisorOrderSearch_MarketplaceAdvisorOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[MarketplaceAdvisorOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsOrderSearch]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrderSearch] ADD CONSTRAINT [FK_NetworkSolutionsOrderSearch_NetworkSolutionsOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[NetworkSolutionsOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionOrderSearch]'
GO
ALTER TABLE [dbo].[OrderMotionOrderSearch] ADD CONSTRAINT [FK_OrderMotionOrderSearch_OrderMotionOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[OrderMotionOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PayPalOrderSearch]'
GO
ALTER TABLE [dbo].[PayPalOrderSearch] ADD CONSTRAINT [FK_PayPalOrderSearch_PayPalOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[PayPalOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresOrderSearch]'
GO
ALTER TABLE [dbo].[ProStoresOrderSearch] ADD CONSTRAINT [FK_ProStoresOrderSearch_ProStoresOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ProStoresOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[SearsOrderSearch]'
GO
ALTER TABLE [dbo].[SearsOrderSearch] ADD CONSTRAINT [FK_SearsOrderSearch_SearsOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[SearsOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyOrderSearch]'
GO
ALTER TABLE [dbo].[ShopifyOrderSearch] ADD CONSTRAINT [FK_ShopifyOrderSearch_ShopifyOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ShopifyOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrderSearch]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderSearch] ADD CONSTRAINT [FK_ThreeDCartOrderSearch_ThreeDCartOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ThreeDCartOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WalmartOrderSearch]'
GO
ALTER TABLE [dbo].[WalmartOrderSearch] ADD CONSTRAINT [FK_WalmartOrderSearch_WalmartOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[WalmartOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrderSearch]'
GO
ALTER TABLE [dbo].[YahooOrderSearch] ADD CONSTRAINT [FK_YahooOrderSearch_YahooOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[YahooOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding OriginalOrderID to [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD [OriginalOrderID] BIGINT NOT NULL CONSTRAINT [DF_OrderItem_OriginalOrderID] DEFAULT ((0))
GO
UPDATE [dbo].[OrderItem] SET OriginalOrderID = OrderID
GO
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_OriginalOrderID]
GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_OriginalOrderID] ON [dbo].[OrderItem] ([OriginalOrderID] ASC)
GO


