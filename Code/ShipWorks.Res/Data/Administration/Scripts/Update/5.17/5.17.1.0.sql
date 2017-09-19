SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[JetStore]'
GO
CREATE TABLE [dbo].[JetStore]
(
	[StoreID] [bigint] NOT NULL,
	[ApiUser] [nvarchar](100) NOT NULL,
	[Secret] [nvarchar](100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetStore] on [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] ADD CONSTRAINT [PK_JetStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] ADD CONSTRAINT [FK_JetStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating [dbo].[JetOrder]'
GO
CREATE TABLE [dbo].[JetOrder]
(
[OrderID] [bigint] NOT NULL,
[MerchantOrderId] [nvarchar](50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetOrder] on [dbo].[JetOrder]'
GO
ALTER TABLE [dbo].[JetOrder] ADD CONSTRAINT [PK_JetOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[JetOrderItem]'
GO
CREATE TABLE [dbo].[JetOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[MerchantSku] [nvarchar](50) NOT NULL,
[JetOrderItemID] [nvarchar](50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetOrderItem] on [dbo].[JetOrderItem]'
GO
ALTER TABLE [dbo].[JetOrderItem] ADD CONSTRAINT [PK_JetOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[JetOrderItem]'
GO
ALTER TABLE [dbo].[JetOrderItem] ADD CONSTRAINT [FK_JetOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[JetOrder]'
GO
ALTER TABLE [dbo].[JetOrder] ADD CONSTRAINT [FK_JetOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])