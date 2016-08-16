SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping index [IX_DownloadDetail_AmazonOrderID] from [dbo].[DownloadDetail]'
GO
DROP INDEX [IX_DownloadDetail_AmazonOrderID] ON [dbo].[DownloadDetail]
GO
PRINT N'Dropping index [IX_DownloadDetail_Ebay] from [dbo].[DownloadDetail]'
GO
DROP INDEX [IX_DownloadDetail_Ebay] ON [dbo].[DownloadDetail]
GO
PRINT N'Altering [dbo].[DownloadDetail]'
GO

ALTER TABLE [dbo].[DownloadDetail] ADD
[ExtraBigIntData1] [bigint] NULL,
[ExtraBigIntData2] [bigint] NULL,
[ExtraBigIntData3] [bigint] NULL,
[ExtraStringData1] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'Migrating [dbo].[DownloadDetail]'
GO


UPDATE [dbo].[DownloadDetail]
   SET ExtraBigIntData1 = COALESCE(EbayOrderID, NetworkSolutionsOrderID, OrderMotionShipmentID, ShopifyOrderID),
	   ExtraBigIntData2 = EbayItemID,
	   ExtraBigIntData3 = EbayTransactionID,
	   ExtraStringData1 = COALESCE(AmazonOrderID, PayPalTransactionID, YahooOrderID, ClickCartProOrderID, GenericOrderNumberComplete)
GO


ALTER TABLE [dbo].[DownloadDetail] DROP
COLUMN [AmazonOrderID],
COLUMN [EbayOrderID],
COLUMN [EbayItemID],
COLUMN [EbayTransactionID],
COLUMN [PayPalTransactionID],
COLUMN [YahooOrderID],
COLUMN [NetworkSolutionsOrderID],
COLUMN [OrderMotionShipmentID],
COLUMN [ClickCartProOrderID],
COLUMN [GenericOrderNumberComplete],
COLUMN [ShopifyOrderId]
GO


PRINT N'Creating index [IX_DownloadDetail_BigIntIndex] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_BigIntIndex] ON [dbo].[DownloadDetail] ([ExtraBigIntData1], [ExtraBigIntData2], [ExtraBigIntData3])
GO
PRINT N'Creating index [IX_DownloadDetail_String] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_String] ON [dbo].[DownloadDetail] ([ExtraStringData1])
GO


PRINT N'Creating [dbo].[SearsOrder]'
GO
CREATE TABLE [dbo].[SearsOrder]
(
[OrderID] [bigint] NOT NULL,
[PoNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PoNumberWithDate] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationID] [int] NOT NULL,
[Commission] [money] NOT NULL,
[CustomerPickup] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsOrder] on [dbo].[SearsOrder]'
GO
ALTER TABLE [dbo].[SearsOrder] ADD CONSTRAINT [PK_SearsOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[SearsOrderItem]'
GO
CREATE TABLE [dbo].[SearsOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[LineNumber] [int] NOT NULL,
[ItemID] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Commission] [money] NOT NULL,
[Shipping] [money] NOT NULL,
[OnlineStatus] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsOrderItem] on [dbo].[SearsOrderItem]'
GO
ALTER TABLE [dbo].[SearsOrderItem] ADD CONSTRAINT [PK_SearsOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[SearsStore]'
GO
CREATE TABLE [dbo].[SearsStore]
(
[StoreID] [bigint] NOT NULL,
[Email] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsStore] on [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] ADD CONSTRAINT [PK_SearsStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[SearsOrder]'
GO
ALTER TABLE [dbo].[SearsOrder] ADD
CONSTRAINT [FK_SearsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[SearsOrderItem]'
GO
ALTER TABLE [dbo].[SearsOrderItem] ADD
CONSTRAINT [FK_SearsOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] ADD
CONSTRAINT [FK_SearsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
