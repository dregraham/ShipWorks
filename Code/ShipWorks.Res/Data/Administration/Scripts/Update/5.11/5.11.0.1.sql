SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[WalmartOrder]'
GO
CREATE TABLE [dbo].[WalmartOrder]
(
[OrderID] [bigint] NOT NULL,
[PurchaseOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomerOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EstimatedDeliveryDate] [datetime] NOT NULL,
[EstimatedShipDate] [datetime] NOT NULL,
[RequestedShippingMethodCode] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartOrder] on [dbo].[WalmartOrder]'
GO
ALTER TABLE [dbo].[WalmartOrder] ADD CONSTRAINT [PK_WalmartOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_Auto_PurchaseOrderId] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_PurchaseOrderId] ON [dbo].[WalmartOrder] ([PurchaseOrderID])
GO
PRINT N'Creating index [IX_Auto_CustomerOrderId] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_CustomerOrderId] ON [dbo].[WalmartOrder] ([CustomerOrderID])
GO
PRINT N'Creating index [IX_Auto_EstimatedDeliveryDate] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_EstimatedDeliveryDate] ON [dbo].[WalmartOrder] ([EstimatedDeliveryDate])
GO
PRINT N'Creating index [IX_Auto_EstimatedShipDate] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_EstimatedShipDate] ON [dbo].[WalmartOrder] ([EstimatedShipDate])
GO
PRINT N'Creating index [IX_Auto_RequestedShippingMethodCode] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShippingMethodCode] ON [dbo].[WalmartOrder] ([RequestedShippingMethodCode])
GO
PRINT N'Creating [dbo].[WalmartOrderItem]'
GO
CREATE TABLE [dbo].[WalmartOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[LineNumber] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartOrderItem] on [dbo].[WalmartOrderItem]'
GO
ALTER TABLE [dbo].[WalmartOrderItem] ADD CONSTRAINT [PK_WalmartOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[WalmartStore]'
GO
CREATE TABLE [dbo].[WalmartStore]
(
[StoreID] [bigint] NOT NULL,
[ConsumerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PrivateKey] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ChannelType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartStore] on [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].[WalmartStore] ADD CONSTRAINT [PK_WalmartStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[WalmartOrderItem]'
GO
ALTER TABLE [dbo].[WalmartOrderItem] ADD CONSTRAINT [FK_WalmartOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[WalmartOrder]'
GO
ALTER TABLE [dbo].[WalmartOrder] ADD CONSTRAINT [FK_WalmartOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].[WalmartStore] ADD CONSTRAINT [FK_WalmartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO