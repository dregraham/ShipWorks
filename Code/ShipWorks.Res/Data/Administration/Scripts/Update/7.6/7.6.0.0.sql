SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[RakutenOrder]'
GO
IF OBJECT_ID(N'[dbo].[RakutenOrder]', 'U') IS NULL
CREATE TABLE [dbo].[RakutenOrder]
(
[OrderID] [bigint] NOT NULL,
[RakutenOrderID] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RakutenPackageID] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_RakutenOrder] on [dbo].[RakutenOrder]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_RakutenOrder' AND object_id = OBJECT_ID(N'[dbo].[RakutenOrder]'))
ALTER TABLE [dbo].[RakutenOrder] ADD CONSTRAINT [PK_RakutenOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[RakutenOrderItem]'
GO
IF OBJECT_ID(N'[dbo].[RakutenOrderItem]', 'U') IS NULL
CREATE TABLE [dbo].[RakutenOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[RakutenOrderItemID] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Discount] [money] NOT NULL,
[ItemTotal] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_RakutenOrderItem] on [dbo].[RakutenOrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_RakutenOrderItem' AND object_id = OBJECT_ID(N'[dbo].[RakutenOrderItem]'))
ALTER TABLE [dbo].[RakutenOrderItem] ADD CONSTRAINT [PK_RakutenOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[RakutenOrderSearch]'
GO
IF OBJECT_ID(N'[dbo].[RakutenOrderSearch]', 'U') IS NULL
CREATE TABLE [dbo].[RakutenOrderSearch]
(
[RakutenOrderSearchID] [bigint] IDENTITY(1,1) NOT NULL,
[OrderID] [bigint] NOT NULL,
[RakutenOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL,
)
GO
PRINT N'Creating primary key [PK_RakutenOrderSearch] on [dbo].[RakutenOrderSearch]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_RakutenOrderSearch' AND object_id = OBJECT_ID(N'[dbo].[RakutenOrderSearch]'))
ALTER TABLE [dbo].[RakutenOrderSearch] ADD CONSTRAINT [PK_RakutenOrderSearch] PRIMARY KEY CLUSTERED  ([RakutenOrderSearchID])
GO
PRINT N'Creating [dbo].[RakutenStore]'
GO
IF OBJECT_ID(N'[dbo].[RakutenStore]', 'U') IS NULL
CREATE TABLE [dbo].[RakutenStore]
(
[StoreID] [bigint] NOT NULL,
[AuthKey] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceID] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopURL] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_RakutenStore] on [dbo].[RakutenStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_RakutenStore' AND object_id = OBJECT_ID(N'[dbo].[RakutenStore]'))
ALTER TABLE [dbo].[RakutenStore] ADD CONSTRAINT [PK_RakutenStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[RakutenOrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RakutenOrderItem_OrderItem]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[RakutenOrderItem]', 'U'))
ALTER TABLE [dbo].[RakutenOrderItem] ADD CONSTRAINT [FK_RakutenOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[RakutenOrder]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RakutenOrder_Order]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[RakutenOrder]', 'U'))
ALTER TABLE [dbo].[RakutenOrder] ADD CONSTRAINT [FK_RakutenOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[RakutenOrderSearch]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RakutenOrderSearch_RakutenOrder]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[RakutenOrderSearch]', 'U'))
ALTER TABLE [dbo].[RakutenOrderSearch] ADD CONSTRAINT [FK_RakutenOrderSearch_RakutenOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[RakutenOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[RakutenStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RakutenStore_Store]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[RakutenStore]', 'U'))
ALTER TABLE [dbo].[RakutenStore] ADD CONSTRAINT [FK_RakutenStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO