SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[OverstockOrder]'
GO
IF OBJECT_ID(N'[dbo].[OverstockOrder]', 'U') IS NULL
CREATE TABLE [dbo].[OverstockOrder]
(
[OrderID] [bigint] NOT NULL,
[OverstockOrderID] [bigint] NOT NULL,
[ChannelName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WarehouseName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_OverstockOrder] on [dbo].[OverstockOrder]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_OverstockOrder' AND object_id = OBJECT_ID(N'[dbo].[OverstockOrder]'))
ALTER TABLE [dbo].[OverstockOrder] ADD CONSTRAINT [PK_OverstockOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[OverstockOrderItem]'
GO
IF OBJECT_ID(N'[dbo].[OverstockOrderItem]', 'U') IS NULL
CREATE TABLE [dbo].[OverstockOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ChannelLineId] [int] NULL,
[LineId] [int] NULL,
[ItemID] [int] NULL
)
GO
PRINT N'Creating primary key [PK_OverstockOrderItem] on [dbo].[OverstockOrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_OverstockOrderItem' AND object_id = OBJECT_ID(N'[dbo].[OverstockOrderItem]'))
ALTER TABLE [dbo].[OverstockOrderItem] ADD CONSTRAINT [PK_OverstockOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[OverstockOrderSearch]'
GO
IF OBJECT_ID(N'[dbo].[OverstockOrderSearch]', 'U') IS NULL
CREATE TABLE [dbo].[OverstockOrderSearch]
(
[OverstockOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[OverstockOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OverstockOrderSearch] on [dbo].[OverstockOrderSearch]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_OverstockOrderSearch' AND object_id = OBJECT_ID(N'[dbo].[OverstockOrderSearch]'))
ALTER TABLE [dbo].[OverstockOrderSearch] ADD CONSTRAINT [PK_OverstockOrderSearch] PRIMARY KEY CLUSTERED  ([OverstockOrderSearchID])
GO
PRINT N'Creating [dbo].[OverstockStore]'
GO
IF OBJECT_ID(N'[dbo].[OverstockStore]', 'U') IS NULL
CREATE TABLE [dbo].[OverstockStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_OverstockStore] on [dbo].[OverstockStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_OverstockStore' AND object_id = OBJECT_ID(N'[dbo].[OverstockStore]'))
ALTER TABLE [dbo].[OverstockStore] ADD CONSTRAINT [PK_OverstockStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OverstockOrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OverstockOrderItem_OrderItem]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[OverstockOrderItem]', 'U'))
ALTER TABLE [dbo].[OverstockOrderItem] ADD CONSTRAINT [FK_OverstockOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[OverstockOrderSearch]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OverstockOrderSearch_OverstockOrder]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[OverstockOrderSearch]', 'U'))
ALTER TABLE [dbo].[OverstockOrderSearch] ADD CONSTRAINT [FK_OverstockOrderSearch_OverstockOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[OverstockOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OverstockOrder]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OverstockOrder_Order]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[OverstockOrder]', 'U'))
ALTER TABLE [dbo].[OverstockOrder] ADD CONSTRAINT [FK_OverstockOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OverstockStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OverstockStore_Store]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[OverstockStore]', 'U'))
ALTER TABLE [dbo].[OverstockStore] ADD CONSTRAINT [FK_OverstockStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OverstockOrder]') AND name = N'IX_OverstockOrder_ChannelName')
CREATE NONCLUSTERED INDEX [IX_OverstockOrder_ChannelName] ON [dbo].[OverstockOrder]
(
	[ChannelName] ASC
) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OverstockOrder]') AND name = N'IX_OverstockOrder_OverstockOrderID')
CREATE NONCLUSTERED INDEX [IX_OverstockOrder_OverstockOrderID] ON [dbo].[OverstockOrder]
(
	[OverstockOrderID] ASC
) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OverstockOrder]') AND name = N'IX_OverstockOrder_WarehouseName')
CREATE NONCLUSTERED INDEX [IX_OverstockOrder_WarehouseName] ON [dbo].[OverstockOrder]
(
	[WarehouseName] ASC
) ON [PRIMARY]
GO
