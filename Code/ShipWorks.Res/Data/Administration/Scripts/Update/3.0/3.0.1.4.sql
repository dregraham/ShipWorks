SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[GenericStore]'
GO
ALTER TABLE [dbo].[GenericStore] DROP
CONSTRAINT [FK_GenericStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] DROP
CONSTRAINT [FK_MagentoStore_GenericStore]
GO
PRINT N'Dropping foreign keys from [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] DROP
CONSTRAINT [FK_MivaStore_GenericStore]
GO
PRINT N'Dropping constraints from [dbo].[GenericStore]'
GO
ALTER TABLE [dbo].[GenericStore] DROP CONSTRAINT [PK_GenericStore]
GO
PRINT N'Dropping index [IX_Auto_BillCompany] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillCompany] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillCountryCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillCountryCode] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillEmail] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillEmail] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillLastName] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillLastName] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillPostalCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillPostalCode] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillStateProvCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_RollupNoteCount] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_RollupOrderCount] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_RollupOrderCount] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_RollupOrderTotal] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_RollupOrderTotal] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipCompany] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipCompany] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipCountryCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipEmail] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipEmail] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipLastName] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipLastName] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipPostalCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipStateProvCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Customer]
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
PRINT N'Dropping index [IX_Auto_BillLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_CustomerID] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_CustomerID] ON [dbo].[Order]
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
PRINT N'Dropping index [IX_Auto_RollupItemName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemSKU] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupNoteCount] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order]
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
PRINT N'Dropping index [IX_Auto_ShipLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_StoreID] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_StoreID] ON [dbo].[Order]
GO
PRINT N'Altering [dbo].[Order]'
GO
PRINT N'Creating index [IX_Auto_BillCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Order] ([BillCompany]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order] ([BillCountryCode]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Order] ([BillEmail]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Order] ([BillLastName]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order] ([BillPostalCode]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order] ([BillStateProvCode]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_CustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_CustomerID] ON [dbo].[Order] ([CustomerID]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_LocalStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_LocalStatus] ON [dbo].[Order] ([LocalStatus]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OnlineLastModified] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineLastModified] ON [dbo].[Order] ([OnlineLastModified]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OnlineStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order] ([OnlineStatus]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderDate] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate] ON [dbo].[Order] ([OrderDate]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderNumber] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumber] ON [dbo].[Order] ([OrderNumber]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderNumberComplete] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order] ([OrderNumberComplete]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderTotal] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderTotal] ON [dbo].[Order] ([OrderTotal]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RequestedShipping] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order] ([RequestedShipping]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order] ([RollupItemCode]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order] ([RollupItemCount]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemSKU] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupNoteCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order] ([RollupNoteCount]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Order] ([ShipCompany]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order] ([ShipCountryCode]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Order] ([ShipEmail]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Order] ([ShipLastName]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order] ([ShipStateProvCode]) INCLUDE ([IsManual], [StoreID])
GO
PRINT N'Creating index [IX_Auto_StoreID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_StoreID] ON [dbo].[Order] ([StoreID]) INCLUDE ([IsManual])
GO
ALTER TABLE [dbo].[Order] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Audit]'
GO
ALTER TABLE [dbo].[Audit] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ENABLE CHANGE_TRACKING
GO
PRINT N'Rebuilding [dbo].[GenericStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_GenericStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleVersion] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePlatform] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleDeveloper] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineStoreCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StatusCodes] [xml] NOT NULL,
[DownloadPageSize] [int] NOT NULL,
[RequestTimeout] [int] NOT NULL,
[DownloadStrategy] [int] NOT NULL,
[OnlineStatusSupport] [int] NOT NULL,
[OnlineStatusDataType] [int] NOT NULL,
[OnlineCustomerSupport] [bit] NOT NULL,
[OnlineCustomerDataType] [int] NOT NULL,
[OnlineShipmentDetails] [bit] NOT NULL,
[HttpExpect100Continue] [bit] NOT NULL,
[ResponseEncoding] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_GenericStore]([StoreID], [Username], [Password], [ModuleUrl], [ModuleVersion], [ModulePlatform], [ModuleDeveloper], [OnlineStoreCode], [StatusCodes], [DownloadPageSize], [RequestTimeout], [DownloadStrategy], [OnlineStatusSupport], [OnlineStatusDataType], [OnlineCustomerSupport], [OnlineCustomerDataType], [OnlineShipmentDetails], [HttpExpect100Continue], [ResponseEncoding]) SELECT [StoreID], [Username], [Password], [ModuleUrl], [ModuleVersion], [ModulePlatform], [ModuleDeveloper], [OnlineStoreCode], [StatusCodes], [DownloadPageSize], [RequestTimeout], [DownloadStrategy], [OnlineStatusSupport], [OnlineStatusDataType], [OnlineCustomerSupport], [OnlineCustomerDataType], [OnlineShipmentDetails], [HttpExpect100Continue], 0 FROM [dbo].[GenericStore]
GO
DROP TABLE [dbo].[GenericStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_GenericStore]', N'GenericStore'
GO
PRINT N'Creating primary key [PK_GenericStore] on [dbo].[GenericStore]'
GO
ALTER TABLE [dbo].[GenericStore] ADD CONSTRAINT [PK_GenericStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Altering [dbo].[Note]'
GO
ALTER TABLE [dbo].[Note] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Customer]'
GO
PRINT N'Creating index [IX_Auto_BillCompany] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Customer] ([BillCompany])
GO
PRINT N'Creating index [IX_Auto_BillCountryCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Customer] ([BillCountryCode])
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Customer] ([BillEmail])
GO
PRINT N'Creating index [IX_Auto_BillLastName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Customer] ([BillLastName])
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Customer] ([BillPostalCode])
GO
PRINT N'Creating index [IX_Auto_BillStateProvCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Customer] ([BillStateProvCode])
GO
PRINT N'Creating index [IX_Auto_RollupNoteCount] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Customer] ([RollupNoteCount])
GO
PRINT N'Creating index [IX_Auto_RollupOrderCount] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderCount] ON [dbo].[Customer] ([RollupOrderCount])
GO
PRINT N'Creating index [IX_Auto_RollupOrderTotal] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderTotal] ON [dbo].[Customer] ([RollupOrderTotal])
GO
PRINT N'Creating index [IX_Auto_ShipCompany] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Customer] ([ShipCompany])
GO
PRINT N'Creating index [IX_Auto_ShipCountryCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Customer] ([ShipCountryCode])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Customer] ([ShipEmail])
GO
PRINT N'Creating index [IX_Auto_ShipLastName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Customer] ([ShipLastName])
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Customer] ([ShipPostalCode])
GO
PRINT N'Creating index [IX_Auto_ShipStateProvCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Customer] ([ShipStateProvCode])
GO
ALTER TABLE [dbo].[Customer] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[PrintResult]'
GO
ALTER TABLE [dbo].[PrintResult] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ENABLE CHANGE_TRACKING
GO
PRINT N'Adding foreign keys to [dbo].[GenericStore]'
GO
ALTER TABLE [dbo].[GenericStore] ADD
CONSTRAINT [FK_GenericStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] ADD
CONSTRAINT [FK_MagentoStore_GenericStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericStore] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] ADD
CONSTRAINT [FK_MivaStore_GenericStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericStore] ([StoreID])
GO

----------------------------------
-- From 3.0.1.2666 On...
----------------------------------

PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[EndiciaAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] DROP CONSTRAINT [PK_EndiciaAccount]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [PK_ShippingSettings]
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
PRINT N'Dropping index [IX_Auto_BillLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_CustomerID] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_CustomerID] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_LocalStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_LocalStatus] ON [dbo].[Order]
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
PRINT N'Dropping index [IX_Auto_RollupItemName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemSKU] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupNoteCount] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order]
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
PRINT N'Dropping index [IX_Auto_ShipLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OnlineLastModified] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OnlineLastModified] ON [dbo].[Order]
GO
PRINT N'Altering [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ALTER COLUMN [EndiciaAccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Rebuilding [dbo].[EndiciaAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EndiciaAccount]
(
[EndiciaAccountID] [bigint] NOT NULL IDENTITY(1066, 1000),
[EndiciaReseller] [int] NOT NULL,
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
INSERT INTO [dbo].[tmp_rg_xx_EndiciaAccount]([EndiciaAccountID], [EndiciaReseller], [AccountNumber], [SignupConfirmation], [WebPassword], [ApiInitialPassword], [ApiUserPassword], [AccountType], [TestAccount], [CreatedByShipWorks], [Description], [FirstName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email]) SELECT [EndiciaAccountID], 0, [AccountNumber], [SignupConfirmation], [WebPassword], [ApiInitialPassword], [ApiUserPassword], [AccountType], [TestAccount], [CreatedByShipWorks], [Description], [FirstName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email] FROM [dbo].[EndiciaAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EndiciaAccount] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[EndiciaAccount]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_EndiciaAccount]', RESEED, @idVal)
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
PRINT N'Rebuilding [dbo].[ShippingSettings]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShippingSettings]
(
[ShippingSettingsID] [bit] NOT NULL,
[Activated] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Configured] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Excluded] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultType] [int] NOT NULL,
[BlankPhoneOption] [int] NOT NULL,
[BlankPhoneNumber] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceAgreement] [bit] NOT NULL,
[FedExUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExMaskAccount] [bit] NOT NULL,
[FedExLimitValue] [bit] NOT NULL,
[FedExThermal] [bit] NOT NULL,
[FedExThermalType] [int] NOT NULL,
[FedExThermalDocTab] [bit] NOT NULL,
[FedExThermalDocTabType] [int] NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsThermal] [bit] NOT NULL,
[UpsThermalType] [int] NOT NULL,
[EndiciaThermal] [bit] NOT NULL,
[EndiciaThermalType] [int] NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WorldShipLaunch] [bit] NOT NULL,
[StampsThermal] [bit] NOT NULL,
[StampsThermalType] [int] NOT NULL,
[Express1Thermal] [bit] NOT NULL,
[Express1ThermalType] [int] NOT NULL,
[Express1CustomsCertify] [bit] NOT NULL,
[Express1CustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [WorldShipLaunch], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [WorldShipLaunch], [StampsThermal], [StampsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner] FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Creating index [IX_Auto_BillCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Order] ([BillCompany])
GO
PRINT N'Creating index [IX_Auto_BillCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order] ([BillCountryCode])
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Order] ([BillEmail])
GO
PRINT N'Creating index [IX_Auto_BillLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Order] ([BillLastName])
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order] ([BillPostalCode])
GO
PRINT N'Creating index [IX_Auto_BillStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order] ([BillStateProvCode])
GO
PRINT N'Creating index [IX_Auto_CustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_CustomerID] ON [dbo].[Order] ([CustomerID])
GO
PRINT N'Creating index [IX_Auto_LocalStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_LocalStatus] ON [dbo].[Order] ([LocalStatus])
GO
PRINT N'Creating index [IX_Auto_OnlineStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order] ([OnlineStatus])
GO
PRINT N'Creating index [IX_Auto_OrderDate] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate] ON [dbo].[Order] ([OrderDate])
GO
PRINT N'Creating index [IX_Auto_OrderNumber] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumber] ON [dbo].[Order] ([OrderNumber])
GO
PRINT N'Creating index [IX_Auto_OrderNumberComplete] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order] ([OrderNumberComplete])
GO
PRINT N'Creating index [IX_Auto_OrderTotal] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderTotal] ON [dbo].[Order] ([OrderTotal])
GO
PRINT N'Creating index [IX_Auto_RequestedShipping] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order] ([RequestedShipping])
GO
PRINT N'Creating index [IX_Auto_RollupItemCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order] ([RollupItemCode])
GO
PRINT N'Creating index [IX_Auto_RollupItemCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order] ([RollupItemCount])
GO
PRINT N'Creating index [IX_Auto_RollupItemName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName])
GO
PRINT N'Creating index [IX_Auto_RollupItemSKU] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU])
GO
PRINT N'Creating index [IX_Auto_RollupNoteCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order] ([RollupNoteCount])
GO
PRINT N'Creating index [IX_Auto_ShipCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Order] ([ShipCompany])
GO
PRINT N'Creating index [IX_Auto_ShipCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order] ([ShipCountryCode])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Order] ([ShipEmail])
GO
PRINT N'Creating index [IX_Auto_ShipLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Order] ([ShipLastName])
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode])
GO
PRINT N'Creating index [IX_Auto_ShipStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order] ([ShipStateProvCode])
GO
PRINT N'Dropping constraints from [dbo].[FilterNodeContentRemoved]'
GO
ALTER TABLE [dbo].[FilterNodeContentRemoved] DROP CONSTRAINT [DF_ObjectDeletion_DeletionDate]
GO
PRINT N'Dropping index [IX_ObjectDeletion_RowVersion] from [dbo].[FilterNodeContentRemoved]'
GO
DROP INDEX [IX_ObjectDeletion_RowVersion] ON [dbo].[FilterNodeContentRemoved]
GO
PRINT N'Dropping index [IX_ObjectDeletion_Date] from [dbo].[FilterNodeContentRemoved]'
GO
DROP INDEX [IX_ObjectDeletion_Date] ON [dbo].[FilterNodeContentRemoved]
GO
PRINT N'Dropping [dbo].[FilterNodeContentRemoved]'
GO
DROP TABLE [dbo].[FilterNodeContentRemoved]
GO
----------------------------------
-- Retroactively adding this, EmailOutbound started being monitored for changes on 12/27/2010,
-- and it looks like early, early alpha users may not have had this turned on during the initial
-- database setup.
----------------------------------
PRINT N'Altering [dbo].[EmailOutbound]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_Tables t join sys.objects o on o.object_id = t.object_id where o.Name = 'EmailOutbound')
BEGIN
	ALTER TABLE [dbo].[EmailOutbound] ENABLE CHANGE_TRACKING
END
GO