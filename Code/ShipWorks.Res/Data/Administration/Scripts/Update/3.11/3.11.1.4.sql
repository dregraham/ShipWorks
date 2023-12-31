SET NUMERIC_ROUNDABORT OFF 
GO 
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON 
GO 
PRINT N'Creating [dbo].[GrouponOrder]' 
GO 
CREATE TABLE [dbo].[GrouponOrder] 
( 
[OrderID] [bigint] NOT NULL, 
[GrouponOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) 
GO 
PRINT N'Creating primary key [PK_GrouponOrder] on [dbo].[GrouponOrder]' 
GO 
ALTER TABLE [dbo].[GrouponOrder] ADD CONSTRAINT [PK_GrouponOrder] PRIMARY KEY CLUSTERED  ([OrderID]) 
GO 
PRINT N'Creating [dbo].[GrouponOrderItem]' 
GO 
CREATE TABLE [dbo].[GrouponOrderItem] 
( 
[OrderItemID] [bigint] NOT NULL, 
[Permalink] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, 
[ChannelSKUProvided] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, 
[FulfillmentLineItemID] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, 
[BomSKU] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, 
[GrouponLineItemID] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) 
GO 
PRINT N'Creating primary key [PK_GrouponOrderItem] on [dbo].[GrouponOrderItem]' 
GO 
ALTER TABLE [dbo].[GrouponOrderItem] ADD CONSTRAINT [PK_GrouponOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID]) 
GO 
PRINT N'Creating [dbo].[GrouponStore]' 
GO 
CREATE TABLE [dbo].[GrouponStore] 
( 
[StoreID] [bigint] NOT NULL, 
[SupplierID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, 
[Token] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) 
GO 
PRINT N'Creating primary key [PK_GrouponStore] on [dbo].[GrouponStore]' 
GO 
ALTER TABLE [dbo].[GrouponStore] ADD CONSTRAINT [PK_GrouponStore] PRIMARY KEY CLUSTERED  ([StoreID]) 
GO 
PRINT N'Adding foreign keys to [dbo].[GrouponOrder]' 
GO 
ALTER TABLE [dbo].[GrouponOrder] ADD CONSTRAINT [FK_GrouponOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID]) 
GO 
PRINT N'Adding foreign keys to [dbo].[GrouponOrderItem]' 
GO 
ALTER TABLE [dbo].[GrouponOrderItem] ADD CONSTRAINT [FK_GrouponOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID]) 
GO 
PRINT N'Adding foreign keys to [dbo].[GrouponStore]' 
GO 
ALTER TABLE [dbo].[GrouponStore] ADD CONSTRAINT [FK_GrouponStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) 
GO