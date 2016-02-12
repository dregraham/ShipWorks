/*
Run this script on:

        INDY\Interapptive.ShipWorksSchema    -  This database will be modified

to synchronize it with:

        INDY\INTERAPPTIVE.ShipWorksSchema

You are recommended to back up your database before running this script

Script created by SQL Compare version 8.1.2 from Red Gate Software Ltd at 5/24/2010 11:12:35 AM

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] DROP
CONSTRAINT [FK_AmazonOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP
CONSTRAINT [FK_ChannelAdvisorOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ClickCartProOrder]'
GO
ALTER TABLE [dbo].[ClickCartProOrder] DROP
CONSTRAINT [FK_ClickCartProOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] DROP
CONSTRAINT [FK_CommerceInterfaceOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP
CONSTRAINT [FK_EbayOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[MagentoOrder]'
GO
ALTER TABLE [dbo].[MagentoOrder] DROP
CONSTRAINT [FK_MagentoOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[MarketplaceAdvisorOrder]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrder] DROP
CONSTRAINT [FK_MarketworksOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[NetworkSolutionsOrder]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrder] DROP
CONSTRAINT [FK_NetworkSolutionsOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] DROP
CONSTRAINT [FK_OrderCharge_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] DROP
CONSTRAINT [FK_OrderItem_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] DROP
CONSTRAINT [FK_OrderMotionOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] DROP
CONSTRAINT [FK_OrderPaymentDetail_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[PayPalOrder]'
GO
ALTER TABLE [dbo].[PayPalOrder] DROP
CONSTRAINT [FK_PayPalOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ProStoresOrder]'
GO
ALTER TABLE [dbo].[ProStoresOrder] DROP
CONSTRAINT [FK_ProStoresOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP
CONSTRAINT [FK_Shipment_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[YahooOrder]'
GO
ALTER TABLE [dbo].[YahooOrder] DROP
CONSTRAINT [FK_YahooOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] DROP
CONSTRAINT [FK_Order_Store],
CONSTRAINT [FK_Order_Customer]
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] DROP
CONSTRAINT [FK_AmazonOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] DROP
CONSTRAINT [FK_EbayOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[InfopiaOrderItem]'
GO
ALTER TABLE [dbo].[InfopiaOrderItem] DROP
CONSTRAINT [FK_InfopiaOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] DROP
CONSTRAINT [FK_OrderItemAttribute_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] DROP
CONSTRAINT [FK_YahooOrderItem_OrderItem]
GO
PRINT N'Dropping constraints from [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [PK_Order]
GO
PRINT N'Dropping constraints from [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [PK_OrderItem]
GO
PRINT N'Dropping index [IX_Auto_BillCity] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillCity] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillCompany] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillCompany] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillCountryCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillEmail] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillEmail] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillFax] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillFax] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillFirstName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillFirstName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillMiddleName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillMiddleName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillPhone] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillPhone] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStreet1] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStreet1] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStreet2] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStreet2] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStreet3] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStreet3] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillWebsite] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillWebsite] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_CustomerID] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_CustomerID] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_IsManual] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_IsManual] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_LocalStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_LocalStatus] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OnlineLastModified] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OnlineLastModified] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OnlineStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderDate] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderDate] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderNumber] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderNumber] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderNumberComplete] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderTotal] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderTotal] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RequestedShipping] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemCount] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItems] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItems] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupOrders] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_RollupOrders] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_RollupItemLocation] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemLocation] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemQuantity] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemQuantity] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemSKU] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupNoteCount] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipCity] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipCity] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipCompany] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipCompany] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipCountryCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipEmail] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipEmail] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipFax] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipFax] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipFirstName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipFirstName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipMiddleName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipPhone] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipPhone] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet1] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStreet1] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet2] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStreet2] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet3] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStreet3] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipWebsite] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipWebsite] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_StoreID] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_StoreID] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_OnlineLastModified_StoreID_IsManual] from [dbo].[Order]'
GO
DROP INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_OnlineCustomerID] from [dbo].[Order]'
GO
DROP INDEX [IX_OnlineCustomerID] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_OrderItem_OrderID] from [dbo].[OrderItem]'
GO
DROP INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem]
GO
PRINT N'Rebuilding [dbo].[Order]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Order]
(
[OrderID] [bigint] NOT NULL IDENTITY(1006, 1000),
[RowVersion] [timestamp] NOT NULL,
[StoreID] [bigint] NOT NULL,
[CustomerID] [bigint] NOT NULL,
[OrderNumber] [bigint] NOT NULL,
[OrderNumberComplete] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrderDate] [datetime] NOT NULL,
[OrderTotal] [money] NOT NULL,
[LocalStatus] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL,
[OnlineLastModified] [datetime] NOT NULL,
[OnlineCustomerID] [sql_variant] NULL,
[OnlineStatus] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineStatusCode] [sql_variant] NULL,
[RequestedShipping] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillMiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCompany] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipMiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipCompany] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RollupItemCount] [int] NOT NULL,
[RollupItemName] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemSKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemLocation] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemQuantity] [float] NULL,
[RollupItemTotalWeight] [float] NOT NULL,
[RollupNoteCount] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Order] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Order]([OrderID], [StoreID], [CustomerID], [OrderNumber], [OrderNumberComplete], [OrderDate], [OrderTotal], [LocalStatus], [IsManual], [OnlineLastModified], [OnlineCustomerID], [OnlineStatus], [OnlineStatusCode], [RequestedShipping], [BillFirstName], [BillMiddleName], [BillLastName], [BillCompany], [BillStreet1], [BillStreet2], [BillStreet3], [BillCity], [BillStateProvCode], [BillPostalCode], [BillCountryCode], [BillPhone], [BillFax], [BillEmail], [BillWebsite], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipFax], [ShipEmail], [ShipWebsite], [RollupItemCount], [RollupItemName], [RollupItemCode], [RollupItemSKU], [RollupItemLocation], [RollupItemQuantity], [RollupItemTotalWeight], [RollupNoteCount]) SELECT [OrderID], [StoreID], [CustomerID], [OrderNumber], [OrderNumberComplete], [OrderDate], [OrderTotal], [LocalStatus], [IsManual], [OnlineLastModified], [OnlineCustomerID], [OnlineStatus], [OnlineStatusCode], [RequestedShipping], [BillFirstName], [BillMiddleName], [BillLastName], [BillCompany], [BillStreet1], [BillStreet2], [BillStreet3], [BillCity], [BillStateProvCode], [BillPostalCode], [BillCountryCode], [BillPhone], [BillFax], [BillEmail], [BillWebsite], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipFax], [ShipEmail], [ShipWebsite], [RollupItemCount], [RollupItemName], [RollupItemCode], [RollupItemSKU], [RollupItemLocation], [RollupItemQuantity], 0, [RollupNoteCount] FROM [dbo].[Order]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Order] OFF
GO
DECLARE @idVal INT
SELECT @idVal = IDENT_CURRENT(N'Order')
DBCC CHECKIDENT(tmp_rg_xx_Order, RESEED, @idVal)
GO
DROP TABLE [dbo].[Order]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Order]', N'Order'
GO
PRINT N'Creating primary key [PK_Order] on [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_Auto_BillCity] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCity] ON [dbo].[Order] ([BillCity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Order] ([BillCompany]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order] ([BillCountryCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Order] ([BillEmail]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillFax] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillFax] ON [dbo].[Order] ([BillFax]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillFirstName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillFirstName] ON [dbo].[Order] ([BillFirstName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Order] ([BillLastName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillMiddleName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillMiddleName] ON [dbo].[Order] ([BillMiddleName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillPhone] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPhone] ON [dbo].[Order] ([BillPhone]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order] ([BillPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order] ([BillStateProvCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStreet1] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet1] ON [dbo].[Order] ([BillStreet1]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStreet2] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet2] ON [dbo].[Order] ([BillStreet2]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStreet3] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet3] ON [dbo].[Order] ([BillStreet3]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillWebsite] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillWebsite] ON [dbo].[Order] ([BillWebsite]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_CustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_CustomerID] ON [dbo].[Order] ([CustomerID]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_IsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_IsManual] ON [dbo].[Order] ([IsManual]) INCLUDE ([RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_LocalStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_LocalStatus] ON [dbo].[Order] ([LocalStatus]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OnlineLastModified] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineLastModified] ON [dbo].[Order] ([OnlineLastModified]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OnlineStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order] ([OnlineStatus]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderDate] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate] ON [dbo].[Order] ([OrderDate]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderNumber] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumber] ON [dbo].[Order] ([OrderNumber]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderNumberComplete] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order] ([OrderNumberComplete]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderTotal] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderTotal] ON [dbo].[Order] ([OrderTotal]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RequestedShipping] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order] ([RequestedShipping]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order] ([RollupItemCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order] ([RollupItemCount]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemLocation] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemLocation] ON [dbo].[Order] ([RollupItemLocation]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemQuantity] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemQuantity] ON [dbo].[Order] ([RollupItemQuantity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemSKU] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemTotalWeight] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemTotalWeight] ON [dbo].[Order] ([RollupItemTotalWeight]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupNoteCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order] ([RollupNoteCount]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCity] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCity] ON [dbo].[Order] ([ShipCity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Order] ([ShipCompany]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order] ([ShipCountryCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Order] ([ShipEmail]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipFax] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipFax] ON [dbo].[Order] ([ShipFax]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipFirstName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipFirstName] ON [dbo].[Order] ([ShipFirstName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Order] ([ShipLastName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipMiddleName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Order] ([ShipMiddleName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipPhone] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPhone] ON [dbo].[Order] ([ShipPhone]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order] ([ShipStateProvCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStreet1] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet1] ON [dbo].[Order] ([ShipStreet1]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStreet2] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet2] ON [dbo].[Order] ([ShipStreet2]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStreet3] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet3] ON [dbo].[Order] ([ShipStreet3]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipWebsite] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipWebsite] ON [dbo].[Order] ([ShipWebsite]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_StoreID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_StoreID] ON [dbo].[Order] ([StoreID]) INCLUDE ([IsManual], [RowVersion])
GO
PRINT N'Creating index [IX_OnlineLastModified_StoreID_IsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order] ([OnlineLastModified] DESC, [StoreID], [IsManual])
GO
PRINT N'Creating index [IX_OnlineCustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_OnlineCustomerID] ON [dbo].[Order] ([OnlineCustomerID])
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [PK_ChannelAdvisorOrderItem]
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[SiteName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SalesSourceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Classification] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DistributionCenter] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]([OrderItemID], [SiteName], [BuyerID], [SalesSourceID], [Classification], [DistributionCenter]) SELECT [OrderItemID], [SiteName], [BuyerID], [SalesSourceID], '', '' FROM [dbo].[ChannelAdvisorOrderItem]
GO
DROP TABLE [dbo].[ChannelAdvisorOrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]', N'ChannelAdvisorOrderItem'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderItem] on [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Rebuilding [dbo].[OrderItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_OrderItem]
(
[OrderItemID] [bigint] NOT NULL IDENTITY(1013, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ISBN] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UPC] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Location] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Image] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Thumbnail] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UnitPrice] [money] NOT NULL,
[UnitCost] [money] NOT NULL,
[Weight] [float] NOT NULL,
[TotalWeight] AS ([Weight]*[Quantity]),
[Quantity] [float] NOT NULL,
[LocalStatus] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_OrderItem] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_OrderItem]([OrderItemID], [OrderID], [Name], [Code], [SKU], [ISBN], [UPC], [Description], [Location], [Image], [Thumbnail], [UnitPrice], [UnitCost], [Weight], [Quantity], [LocalStatus], [IsManual]) SELECT [OrderItemID], [OrderID], [Name], [Code], [SKU], [ISBN], [UPC], [Description], [Location], [Image], [Thumbnail], [UnitPrice], [UnitCost], [Weight], [Quantity], [LocalStatus], [IsManual] FROM [dbo].[OrderItem]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_OrderItem] OFF
GO
DECLARE @idVal INT
SELECT @idVal = IDENT_CURRENT(N'OrderItem')
DBCC CHECKIDENT(tmp_rg_xx_OrderItem, RESEED, @idVal)
GO
DROP TABLE [dbo].[OrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_OrderItem]', N'OrderItem'
GO
PRINT N'Creating primary key [PK_OrderItem] on [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating index [IX_OrderItem_OrderID] on [dbo].[OrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem] ([OrderID])
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [CodCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [BrokerCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [ImporterCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] ALTER COLUMN [Company] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[ShippingOrigin]'
GO
ALTER TABLE [dbo].[ShippingOrigin] ALTER COLUMN [Company] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD
CONSTRAINT [FK_AmazonOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD
CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ClickCartProOrder]'
GO
ALTER TABLE [dbo].[ClickCartProOrder] ADD
CONSTRAINT [FK_ClickCartProOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] ADD
CONSTRAINT [FK_CommerceInterfaceOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD
CONSTRAINT [FK_EbayOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoOrder]'
GO
ALTER TABLE [dbo].[MagentoOrder] ADD
CONSTRAINT [FK_MagentoOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorOrder]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrder] ADD
CONSTRAINT [FK_MarketworksOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsOrder]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrder] ADD
CONSTRAINT [FK_NetworkSolutionsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] ADD
CONSTRAINT [FK_OrderCharge_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD
CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] ADD
CONSTRAINT [FK_OrderMotionOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] ADD
CONSTRAINT [FK_OrderPaymentDetail_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[PayPalOrder]'
GO
ALTER TABLE [dbo].[PayPalOrder] ADD
CONSTRAINT [FK_PayPalOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresOrder]'
GO
ALTER TABLE [dbo].[ProStoresOrder] ADD
CONSTRAINT [FK_ProStoresOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrder]'
GO
ALTER TABLE [dbo].[YahooOrder] ADD
CONSTRAINT [FK_YahooOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD
CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]),
CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] ADD
CONSTRAINT [FK_AmazonOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] ADD
CONSTRAINT [FK_EbayOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaOrderItem]'
GO
ALTER TABLE [dbo].[InfopiaOrderItem] ADD
CONSTRAINT [FK_InfopiaOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] ADD
CONSTRAINT [FK_OrderItemAttribute_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD
CONSTRAINT [FK_YahooOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'BillCountry', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'BillState', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'CustomerID'
GO
EXEC sp_addextendedproperty N'AuditName', N'Customer', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'CustomerID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OnlineCustomerID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OnlineStatusCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderNumber'
GO
EXEC sp_addextendedproperty N'AuditName', N'Order Number', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderNumberComplete'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderTotal'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipCountry', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
EXEC sp_addextendedproperty N'AuditName', N'Store', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'UnitCost'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'UnitPrice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'Weight'
GO

UPDATE dbo.[Order] 
  SET RollupItemTotalWeight = (SELECT COALESCE(SUM(i.Quantity * i.Weight), 0) FROM OrderItem i WHERE i.OrderID = [Order].OrderID)
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[FedExAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] DROP CONSTRAINT [PK_FedExAccount]
GO
PRINT N'Rebuilding [dbo].[FedExAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FedExAccount]
(
[FedExAccountID] [bigint] NOT NULL IDENTITY(1055, 1000),
[RowVersion] [timestamp] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccountNumber] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SignatureRelease] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MeterNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostHubList] [xml] NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_FedExAccount]([FedExAccountID], [Description], [AccountNumber], [SignatureRelease], [MeterNumber], [SmartPostHubList], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website]) SELECT [FedExAccountID], [Description], [AccountNumber], [SignatureRelease], [MeterNumber], [SmartPostHubList], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website] FROM [dbo].[FedExAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExAccount] OFF
GO
DROP TABLE [dbo].[FedExAccount]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FedExAccount]', N'FedExAccount'
GO
PRINT N'Creating primary key [PK_FedExAccount] on [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] ADD CONSTRAINT [PK_FedExAccount] PRIMARY KEY CLUSTERED  ([FedExAccountID])
GO
ALTER TABLE [dbo].[FedExAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[EndiciaAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] DROP CONSTRAINT [PK_EndiciaAccount]
GO
PRINT N'Rebuilding [dbo].[EndiciaAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EndiciaAccount]
(
[EndiciaAccountID] [bigint] NOT NULL IDENTITY(1066, 1000),
[AccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignupConfirmation] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WebPassword] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiInitialPassword] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserPassword] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccountType] [int] NOT NULL,
[TestAccount] [bit] NOT NULL,
[CreatedByShipWorks] [bit] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Fax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EndiciaAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_EndiciaAccount]([EndiciaAccountID], [AccountNumber], [SignupConfirmation], [WebPassword], [ApiInitialPassword], [ApiUserPassword], [AccountType], [TestAccount], [CreatedByShipWorks], [Description], [FirstName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email]) SELECT [EndiciaAccountID], [AccountNumber], [SignupConfirmation], [WebPassword], [ApiInitialPassword], [ApiUserPassword], [AccountType], [TestAccount], [CreatedByShipWorks], [Description], [FirstName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email] FROM [dbo].[EndiciaAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EndiciaAccount] OFF
GO
DROP TABLE [dbo].[EndiciaAccount]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EndiciaAccount]', N'EndiciaAccount'
GO
PRINT N'Creating primary key [PK_EndiciaAccount] on [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] ADD CONSTRAINT [PK_EndiciaAccount] PRIMARY KEY CLUSTERED  ([EndiciaAccountID])
GO
ALTER TABLE [dbo].[EndiciaAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[GenericStore]'
GO
ALTER TABLE [dbo].[GenericStore] ADD
[HttpExpect100Continue] [bit] NOT NULL CONSTRAINT [DF_GenericStore_HttpExpect100Continue] DEFAULT ((1))
GO
ALTER TABLE [dbo].[GenericStore] DROP CONSTRAINT [DF_GenericStore_HttpExpect100Continue]
GO

ALTER TABLE dbo.ShippingSettings
	DROP CONSTRAINT DF_ShippingSettings_ShippingSetupList
GO
ALTER TABLE dbo.ShippingSettings
	DROP CONSTRAINT DF_ShippingSettings_ShippingDefaultType
GO
CREATE TABLE dbo.Tmp_ShippingSettings
	(
	ShippingSettingsID bit NOT NULL,
	Activated varchar(30) NOT NULL,
	Configured varchar(30) NOT NULL,
	Excluded varchar(30) NOT NULL,
	DefaultType int NOT NULL,
	BlankPhoneOption int NOT NULL,
	BlankPhoneNumber nvarchar(16) NOT NULL,
	InsuranceAgreement bit NOT NULL,
	FedExUsername nvarchar(50) NULL,
	FedExPassword nvarchar(50) NULL,
	FedExMaskAccount bit NOT NULL,
	FedExLimitValue bit NOT NULL,
	FedExThermal bit NOT NULL,
	FedExThermalType int NOT NULL,
	FedExThermalDocTab bit NOT NULL,
	FedExThermalDocTabType int NOT NULL,
	UpsAccessKey nvarchar(50) NULL,
	UpsThermal bit NOT NULL,
	UpsThermalType int NOT NULL,
	EndiciaThermal bit NOT NULL,
	EndiciaThermalType int NOT NULL,
	EndiciaCustomsCertify bit NOT NULL,
	EndiciaCustomsSigner nvarchar(100) NOT NULL,
	WorldShipLaunch bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ShippingSettings SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.ShippingSettings)
	 EXEC('INSERT INTO dbo.Tmp_ShippingSettings (ShippingSettingsID, Activated, Configured, Excluded,    DefaultType, BlankPhoneOption, BlankPhoneNumber, InsuranceAgreement, FedExUsername, FedExPassword, FedExMaskAccount, FedExLimitValue, FedExThermal, FedExThermalType, FedExThermalDocTab, FedExThermalDocTabType, UpsAccessKey, UpsThermal, UpsThermalType, EndiciaThermal, EndiciaThermalType, EndiciaCustomsCertify, EndiciaCustomsSigner, WorldShipLaunch)
		                                  SELECT ShippingSettingsID, SetupList, SetupList,  ExcludeList, DefaultType, BlankPhoneOption, BlankPhoneNumber, InsuranceAgreement, FedExUsername, FedExPassword, FedExMaskAccount, FedExLimitValue, FedExThermal, FedExThermalType, FedExThermalDocTab, FedExThermalDocTabType, UpsAccessKey, UpsThermal, UpsThermalType, EndiciaThermal, EndiciaThermalType, EndiciaCustomsCertify, EndiciaCustomsSigner, WorldShipLaunch FROM dbo.ShippingSettings WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.ShippingSettings
GO
EXECUTE sp_rename N'dbo.Tmp_ShippingSettings', N'ShippingSettings', 'OBJECT' 
GO
ALTER TABLE dbo.ShippingSettings ADD CONSTRAINT
	PK_ShippingSettings PRIMARY KEY CLUSTERED 
	(
	ShippingSettingsID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO