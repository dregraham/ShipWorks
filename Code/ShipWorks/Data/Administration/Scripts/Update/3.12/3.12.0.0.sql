﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[Shipment] DISABLE CHANGE_TRACKING
GO
ALTER TABLE [dbo].[Store] DISABLE CHANGE_TRACKING
GO
ALTER TABLE [dbo].[Order] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ContentWeight'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsGenerated'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsValue'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginNameParseStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginOriginID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialResult'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipDate'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentCost'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'TotalWeight'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillNameParseStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'CustomerID'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'CustomerID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OnlineCustomerID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OnlineStatusCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderNumber'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderNumberComplete'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderTotal'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
PRINT N'Dropping foreign keys from [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] DROP CONSTRAINT [FK_BestRateShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [FK_FedExShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[InsurancePolicy]'
GO
ALTER TABLE [dbo].[InsurancePolicy] DROP CONSTRAINT [FK_InsurancePolicy_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] DROP CONSTRAINT [FK_iParcelShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] DROP CONSTRAINT [FK_OnTracShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] DROP CONSTRAINT [FK_OtherShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP CONSTRAINT [FK_PostalShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP CONSTRAINT [FK_ShipmentCustomsItem_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT [FK_UpsShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_Order]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_ProcessedUser]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_ProcessedComputer]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_VoidedUser]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_VoidedComputer]
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] DROP CONSTRAINT [FK_AmazonStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[AmeriCommerceStore]'
GO
ALTER TABLE [dbo].[AmeriCommerceStore] DROP CONSTRAINT [FK_AmeriCommerceStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] DROP CONSTRAINT [FK_BigCommerceStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[BuyDotComStore]'
GO
ALTER TABLE [dbo].[BuyDotComStore] DROP CONSTRAINT [FK_BuyComStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT [FK_ChannelAdvisorStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] DROP CONSTRAINT [FK_Download_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] DROP CONSTRAINT [FK_EbayStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[EtsyStore]'
GO
ALTER TABLE [dbo].[EtsyStore] DROP CONSTRAINT [FK_EtsyStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] DROP CONSTRAINT [FK_GenericFileStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT [FK_GenericModuleStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] DROP CONSTRAINT [FK_InfopiaStore_InfopiaStore]
GO
PRINT N'Dropping foreign keys from [dbo].[MarketplaceAdvisorStore]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorStore] DROP CONSTRAINT [FK_MarketworksStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[NetworkSolutionsStore]'
GO
ALTER TABLE [dbo].[NetworkSolutionsStore] DROP CONSTRAINT [FK_NetworkSolutionsStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] DROP CONSTRAINT [FK_NeweggStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_Order_Store]
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_Order_Customer]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] DROP CONSTRAINT [FK_OrderMotionStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[PayPalStore]'
GO
ALTER TABLE [dbo].[PayPalStore] DROP CONSTRAINT [FK_PayPalStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ProStoresStore]'
GO
ALTER TABLE [dbo].[ProStoresStore] DROP CONSTRAINT [FK_ProStoresStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] DROP CONSTRAINT [FK_SearsStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] DROP CONSTRAINT [FK_ShopifyStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[StatusPreset]'
GO
ALTER TABLE [dbo].[StatusPreset] DROP CONSTRAINT [FK_StatusPreset_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] DROP CONSTRAINT [FK_StoreShopSite_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] DROP CONSTRAINT [FK_ThreeDCartStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] DROP CONSTRAINT [FK_VolusionStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] DROP CONSTRAINT [FK_YahooStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] DROP CONSTRAINT [FK_AmazonOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [FK_ChannelAdvisorOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ClickCartProOrder]'
GO
ALTER TABLE [dbo].[ClickCartProOrder] DROP CONSTRAINT [FK_ClickCartProOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] DROP CONSTRAINT [FK_CommerceInterfaceOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [FK_EbayOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[EtsyOrder]'
GO
ALTER TABLE [dbo].[EtsyOrder] DROP CONSTRAINT [FK_EtsyOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[MagentoOrder]'
GO
ALTER TABLE [dbo].[MagentoOrder] DROP CONSTRAINT [FK_MagentoOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[MarketplaceAdvisorOrder]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrder] DROP CONSTRAINT [FK_MarketworksOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[NetworkSolutionsOrder]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrder] DROP CONSTRAINT [FK_NetworkSolutionsOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[NeweggOrder]'
GO
ALTER TABLE [dbo].[NeweggOrder] DROP CONSTRAINT [FK_NeweggOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] DROP CONSTRAINT [FK_OrderCharge_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] DROP CONSTRAINT [FK_OrderMotionOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] DROP CONSTRAINT [FK_OrderPaymentDetail_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[PayPalOrder]'
GO
ALTER TABLE [dbo].[PayPalOrder] DROP CONSTRAINT [FK_PayPalOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ProStoresOrder]'
GO
ALTER TABLE [dbo].[ProStoresOrder] DROP CONSTRAINT [FK_ProStoresOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[SearsOrder]'
GO
ALTER TABLE [dbo].[SearsOrder] DROP CONSTRAINT [FK_SearsOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ShopifyOrder]'
GO
ALTER TABLE [dbo].[ShopifyOrder] DROP CONSTRAINT [FK_ShopifyOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[YahooOrder]'
GO
ALTER TABLE [dbo].[YahooOrder] DROP CONSTRAINT [FK_YahooOrder_Order]
GO
PRINT N'Dropping constraints from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [PK_Shipment]
GO
PRINT N'Dropping constraints from [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] DROP CONSTRAINT [PK_Store]
GO
PRINT N'Dropping constraints from [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [PK_Order]
GO
PRINT N'Dropping index [IX_FilterNodeContentDirty_ColumnsUpdated] from [dbo].[FilterNodeContentDirty]'
GO
DROP INDEX [IX_FilterNodeContentDirty_ColumnsUpdated] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_FilterNodeContentDirty_ParentIDObjectType] from [dbo].[FilterNodeContentDirty]'
GO
DROP INDEX [IX_FilterNodeContentDirty_ParentIDObjectType] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateOrder] from [dbo].[FilterNodeUpdateOrder]'
GO
DROP INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateShipment] from [dbo].[FilterNodeUpdateShipment]'
GO
DROP INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateCustomer] from [dbo].[FilterNodeUpdateCustomer]'
GO
DROP INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateItem] from [dbo].[FilterNodeUpdateItem]'
GO
DROP INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem]
GO
PRINT N'Dropping index [IX_Shipment_OrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Shipment_OrderID_ShipSenseStatus] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_OrderID_ShipSenseStatus] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Shipment_ProcessedOrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Shipment_RequestedLabelFormat] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_RequestedLabelFormat] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Shipment_ActualLabelFormat] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ActualLabelFormat] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Store_StoreName] from [dbo].[Store]'
GO
DROP INDEX [IX_Store_StoreName] ON [dbo].[Store]
GO
PRINT N'Dropping index [IX_OnlineLastModified_StoreID_IsManual] from [dbo].[Order]'
GO
DROP INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_StoreID] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_StoreID] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_CustomerID] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_CustomerID] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderNumber] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderNumber] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderNumberComplete] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderDate] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderDate] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OrderTotal] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OrderTotal] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_LocalStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_LocalStatus] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_OnlineCustomerID] from [dbo].[Order]'
GO
DROP INDEX [IX_OnlineCustomerID] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_OnlineStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RequestedShipping] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillFirstName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillFirstName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillCompany] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillCompany] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillCountryCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillEmail] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillEmail] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipLastName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipLastName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipCompany] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipCompany] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStateProvCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipCountryCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipEmail] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipEmail] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemCount] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemSKU] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupNoteCount] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipSenseHashKey] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipSenseHashKey] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipSenseRecognitionStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipSenseRecognitionStatus] ON [dbo].[Order]
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
[BillEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillAddressValidationSuggestionCount] [int] NOT NULL,
[BillAddressValidationStatus] [int] NOT NULL,
[BillAddressValidationError] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillResidentialStatus] [int] NOT NULL,
[BillPOBox] [int] NOT NULL,
[BillUSTerritory] [int] NOT NULL,
[BillMilitaryAddress] [int] NOT NULL,
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
[ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipAddressValidationSuggestionCount] [int] NOT NULL,
[ShipAddressValidationStatus] [int] NOT NULL,
[ShipAddressValidationError] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipResidentialStatus] [int] NOT NULL,
[ShipPOBox] [int] NOT NULL,
[ShipUSTerritory] [int] NOT NULL,
[ShipMilitaryAddress] [int] NOT NULL,
[RollupItemCount] [int] NOT NULL,
[RollupItemName] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemSKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemLocation] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemQuantity] [float] NULL,
[RollupItemTotalWeight] [float] NOT NULL,
[RollupNoteCount] [int] NOT NULL,
[BillNameParseStatus] [int] NOT NULL,
[BillUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipNameParseStatus] [int] NOT NULL,
[ShipUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipSenseHashKey] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
[ShipSenseRecognitionStatus] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Order] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Order]([OrderID], [StoreID], [CustomerID], [OrderNumber], [OrderNumberComplete], [OrderDate], [OrderTotal], [LocalStatus], [IsManual], [OnlineLastModified], [OnlineCustomerID], [OnlineStatus], [OnlineStatusCode], [RequestedShipping], [BillFirstName], [BillMiddleName], [BillLastName], [BillCompany], [BillStreet1], [BillStreet2], [BillStreet3], [BillCity], [BillStateProvCode], [BillPostalCode], [BillCountryCode], [BillPhone], [BillFax], [BillEmail], [BillWebsite], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipFax], [ShipEmail], [ShipWebsite], [RollupItemCount], [RollupItemName], [RollupItemCode], [RollupItemSKU], [RollupItemLocation], [RollupItemQuantity], [RollupItemTotalWeight], [RollupNoteCount], [BillNameParseStatus], [BillUnparsedName], [ShipNameParseStatus], [ShipUnparsedName], [ShipSenseHashKey], [ShipSenseRecognitionStatus],
	[BillAddressValidationError], [BillAddressValidationStatus], [BillAddressValidationSuggestionCount], [BillMilitaryAddress], [BillPOBox], [BillResidentialStatus], [BillUSTerritory], [ShipAddressValidationError], [ShipAddressValidationStatus], [ShipAddressValidationSuggestionCount], [ShipMilitaryAddress], [ShipPOBox], [ShipResidentialStatus], [ShipUSTerritory])
SELECT [OrderID], [StoreID], [CustomerID], [OrderNumber], [OrderNumberComplete], [OrderDate], [OrderTotal], [LocalStatus], [IsManual], [OnlineLastModified], [OnlineCustomerID], [OnlineStatus], [OnlineStatusCode], [RequestedShipping], [BillFirstName], [BillMiddleName], [BillLastName], [BillCompany], [BillStreet1], [BillStreet2], [BillStreet3], [BillCity], [BillStateProvCode], [BillPostalCode], [BillCountryCode], [BillPhone], [BillFax], [BillEmail], [BillWebsite], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipFax], [ShipEmail], [ShipWebsite], [RollupItemCount], [RollupItemName], [RollupItemCode], [RollupItemSKU], [RollupItemLocation], [RollupItemQuantity], [RollupItemTotalWeight], [RollupNoteCount], [BillNameParseStatus], [BillUnparsedName], [ShipNameParseStatus], [ShipUnparsedName], [ShipSenseHashKey], [ShipSenseRecognitionStatus],
	'', 0, 0, 0, 0, 0, 0, '', 0, 0, 0, 0, 0, 0
FROM [dbo].[Order]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Order] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Order]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Order]', RESEED, @idVal)
GO
DROP TABLE [dbo].[Order]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Order]', N'Order'
GO
PRINT N'Creating primary key [PK_Order] on [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_OnlineLastModified_StoreID_IsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order] ([OnlineLastModified] DESC, [StoreID], [IsManual])
GO
PRINT N'Creating index [IX_Auto_StoreID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_StoreID] ON [dbo].[Order] ([StoreID]) INCLUDE ([IsManual])
GO
PRINT N'Creating index [IX_Auto_CustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_CustomerID] ON [dbo].[Order] ([CustomerID])
GO
PRINT N'Creating index [IX_Auto_OrderNumber] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumber] ON [dbo].[Order] ([OrderNumber])
GO
PRINT N'Creating index [IX_Auto_OrderNumberComplete] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order] ([OrderNumberComplete])
GO
PRINT N'Creating index [IX_Auto_OrderDate] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate] ON [dbo].[Order] ([OrderDate])
GO
PRINT N'Creating index [IX_Auto_OrderTotal] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderTotal] ON [dbo].[Order] ([OrderTotal])
GO
PRINT N'Creating index [IX_Auto_LocalStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_LocalStatus] ON [dbo].[Order] ([LocalStatus])
GO
PRINT N'Creating index [IX_OnlineCustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_OnlineCustomerID] ON [dbo].[Order] ([OnlineCustomerID])
GO
PRINT N'Creating index [IX_Auto_OnlineStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order] ([OnlineStatus])
GO
PRINT N'Creating index [IX_Auto_RequestedShipping] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order] ([RequestedShipping])
GO
PRINT N'Creating index [IX_Auto_BillFirstName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillFirstName] ON [dbo].[Order] ([BillFirstName])
GO
PRINT N'Creating index [IX_Auto_BillLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Order] ([BillLastName])
GO
PRINT N'Creating index [IX_Auto_BillCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Order] ([BillCompany])
GO
PRINT N'Creating index [IX_Auto_BillStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order] ([BillStateProvCode])
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order] ([BillPostalCode])
GO
PRINT N'Creating index [IX_Auto_BillCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order] ([BillCountryCode])
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Order] ([BillEmail])
GO
PRINT N'Creating index [IX_Auto_ShipLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Order] ([ShipLastName])
GO
PRINT N'Creating index [IX_Auto_ShipCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Order] ([ShipCompany])
GO
PRINT N'Creating index [IX_Auto_ShipStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order] ([ShipStateProvCode])
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode])
GO
PRINT N'Creating index [IX_Auto_ShipCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order] ([ShipCountryCode])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Order] ([ShipEmail])
GO
PRINT N'Creating index [IX_Auto_RollupItemCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order] ([RollupItemCount])
GO
PRINT N'Creating index [IX_Auto_RollupItemName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName])
GO
PRINT N'Creating index [IX_Auto_RollupItemCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order] ([RollupItemCode])
GO
PRINT N'Creating index [IX_Auto_RollupItemSKU] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU])
GO
PRINT N'Creating index [IX_Auto_RollupNoteCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order] ([RollupNoteCount])
GO
PRINT N'Creating index [IX_Auto_ShipSenseHashKey] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipSenseHashKey] ON [dbo].[Order] ([ShipSenseHashKey])
GO
PRINT N'Creating index [IX_Auto_ShipSenseRecognitionStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipSenseRecognitionStatus] ON [dbo].[Order] ([ShipSenseRecognitionStatus])
GO
ALTER TABLE [dbo].[Order] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Order]'
GO
PRINT N'Rebuilding [dbo].[Store]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Store]
(
[StoreID] [bigint] NOT NULL IDENTITY(1005, 1000),
[RowVersion] [timestamp] NOT NULL,
[License] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Edition] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TypeCode] [int] NOT NULL,
[Enabled] [bit] NOT NULL,
[SetupComplete] [bit] NOT NULL,
[StoreName] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Fax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AutoDownload] [bit] NOT NULL,
[AutoDownloadMinutes] [int] NOT NULL,
[AutoDownloadOnlyAway] [bit] NOT NULL,
[AddressValidationSetting] [int] NOT NULL,
[ComputerDownloadPolicy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultEmailAccountID] [bigint] NOT NULL,
[ManualOrderPrefix] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ManualOrderPostfix] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InitialDownloadDays] [int] NULL,
[InitialDownloadOrder] [bigint] NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Store] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Store]([StoreID], [License], [Edition], [TypeCode], [Enabled], [SetupComplete], [StoreName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], [Website], [AutoDownload], [AutoDownloadMinutes], [AutoDownloadOnlyAway], [ComputerDownloadPolicy], [DefaultEmailAccountID], [ManualOrderPrefix], [ManualOrderPostfix], [InitialDownloadDays], [InitialDownloadOrder],
	[AddressValidationSetting]) 
SELECT [StoreID], [License], [Edition], [TypeCode], [Enabled], [SetupComplete], [StoreName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], [Website], [AutoDownload], [AutoDownloadMinutes], [AutoDownloadOnlyAway], [ComputerDownloadPolicy], [DefaultEmailAccountID], [ManualOrderPrefix], [ManualOrderPostfix], [InitialDownloadDays], [InitialDownloadOrder],
	1
FROM [dbo].[Store]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Store] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Store]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Store]', RESEED, @idVal)
GO
DROP TABLE [dbo].[Store]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Store]', N'Store'
GO
PRINT N'Creating primary key [PK_Store] on [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] ADD CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating index [IX_Store_StoreName] on [dbo].[Store]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Store_StoreName] ON [dbo].[Store] ([StoreName])
GO
ALTER TABLE [dbo].[Store] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Store]'
GO
PRINT N'Rebuilding [dbo].[Shipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Shipment]
(
[ShipmentID] [bigint] NOT NULL IDENTITY(1031, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[ShipmentType] [int] NOT NULL,
[ContentWeight] [float] NOT NULL,
[TotalWeight] [float] NOT NULL,
[Processed] [bit] NOT NULL,
[ProcessedDate] [datetime] NULL,
[ProcessedUserID] [bigint] NULL,
[ProcessedComputerID] [bigint] NULL,
[ShipDate] [datetime] NOT NULL,
[ShipmentCost] [money] NOT NULL,
[Voided] [bit] NOT NULL,
[VoidedDate] [datetime] NULL,
[VoidedUserID] [bigint] NULL,
[VoidedComputerID] [bigint] NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsGenerated] [bit] NOT NULL,
[CustomsValue] [money] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL,
[ActualLabelFormat] [int] NULL,
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
[ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipAddressValidationSuggestionCount] [int] NOT NULL,
[ShipAddressValidationStatus] [int] NOT NULL,
[ShipAddressValidationError] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipResidentialStatus] [int] NOT NULL,
[ShipPOBox] [int] NOT NULL,
[ShipUSTerritory] [int] NOT NULL,
[ShipMilitaryAddress] [int] NOT NULL,
[ResidentialDetermination] [int] NOT NULL,
[ResidentialResult] [bit] NOT NULL,
[OriginOriginID] [bigint] NOT NULL,
[OriginFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginMiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginCompany] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnShipment] [bit] NOT NULL,
[Insurance] [bit] NOT NULL,
[InsuranceProvider] [int] NOT NULL,
[ShipNameParseStatus] [int] NOT NULL,
[ShipUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginNameParseStatus] [int] NOT NULL,
[OriginUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BestRateEvents] [tinyint] NOT NULL,
[ShipSenseStatus] [int] NOT NULL,
[ShipSenseChangeSets] [xml] NOT NULL,
[ShipSenseEntry] [varbinary] (max) NOT NULL,
[OnlineShipmentID] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Shipment]([ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ProcessedUserID], [ProcessedComputerID], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [VoidedUserID], [VoidedComputerID], [TrackingNumber], [CustomsGenerated], [CustomsValue], [RequestedLabelFormat], [ActualLabelFormat], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], [Insurance], [InsuranceProvider], [ShipNameParseStatus], [ShipUnparsedName], [OriginNameParseStatus], [OriginUnparsedName], [BestRateEvents], [ShipSenseStatus], [ShipSenseChangeSets], [ShipSenseEntry], [OnlineShipmentID],
	[ShipAddressValidationError], [ShipAddressValidationStatus], [ShipAddressValidationSuggestionCount], [ShipMilitaryAddress], [ShipPOBox], [ShipResidentialStatus], [ShipUSTerritory])  
SELECT [ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ProcessedUserID], [ProcessedComputerID], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [VoidedUserID], [VoidedComputerID], [TrackingNumber], [CustomsGenerated], [CustomsValue], [RequestedLabelFormat], [ActualLabelFormat], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], [Insurance], [InsuranceProvider], [ShipNameParseStatus], [ShipUnparsedName], [OriginNameParseStatus], [OriginUnparsedName], [BestRateEvents], [ShipSenseStatus], [ShipSenseChangeSets], [ShipSenseEntry], [OnlineShipmentID],
		'', 0, 0, 0, 0, 0, 0
FROM [dbo].[Shipment]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Shipment]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Shipment]', RESEED, @idVal)
GO
DROP TABLE [dbo].[Shipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Shipment]', N'Shipment'
GO
PRINT N'Creating primary key [PK_Shipment] on [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_Shipment_OrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment] ([OrderID])
GO
PRINT N'Creating index [IX_Shipment_OrderID_ShipSenseStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID_ShipSenseStatus] ON [dbo].[Shipment] ([OrderID], [Processed], [ShipSenseStatus])
GO
PRINT N'Creating index [IX_Shipment_ProcessedOrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_Shipment_RequestedLabelFormat] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_RequestedLabelFormat] ON [dbo].[Shipment] ([RequestedLabelFormat])
GO
PRINT N'Creating index [IX_Shipment_ActualLabelFormat] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ActualLabelFormat] ON [dbo].[Shipment] ([ActualLabelFormat])
GO
ALTER TABLE [dbo].[Shipment] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Shipment]'
GO
PRINT N'Altering [dbo].[FilterNodeContent]'
GO
ALTER TABLE [dbo].[FilterNodeContent] ALTER COLUMN [ColumnMask] [varbinary] (100) NOT NULL
GO
PRINT N'Altering [dbo].[FilterNodeContentDirty]'
GO
ALTER TABLE [dbo].[FilterNodeContentDirty] ALTER COLUMN [ColumnsUpdated] [varbinary] (100) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeContentDirty_ColumnsUpdated] on [dbo].[FilterNodeContentDirty]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_ColumnsUpdated] ON [dbo].[FilterNodeContentDirty] ([ColumnsUpdated])
GO
PRINT N'Creating index [IX_FilterNodeContentDirty_ParentIDObjectType] on [dbo].[FilterNodeContentDirty]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_ParentIDObjectType] ON [dbo].[FilterNodeContentDirty] ([ParentID], [ObjectType]) INCLUDE ([ColumnsUpdated], [ComputerID])
GO
PRINT N'Altering [dbo].[FilterNodeUpdateOrder]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateOrder] ALTER COLUMN [ColumnsUpdated] [varbinary] (100) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateOrder] on [dbo].[FilterNodeUpdateOrder]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Altering [dbo].[FilterNodeUpdateShipment]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateShipment] ALTER COLUMN [ColumnsUpdated] [varbinary] (100) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateShipment] on [dbo].[FilterNodeUpdateShipment]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Altering [dbo].[FilterNodeUpdateCustomer]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCustomer] ALTER COLUMN [ColumnsUpdated] [varbinary] (100) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateCustomer] on [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Altering [dbo].[FilterNodeUpdateItem]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateItem] ALTER COLUMN [ColumnsUpdated] [varbinary] (100) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateItem] on [dbo].[FilterNodeUpdateItem]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[ValidatedAddress]'
GO
CREATE TABLE [dbo].[ValidatedAddress]
(
[ValidatedAddressID] [bigint] NOT NULL IDENTITY(1100, 1000),
[ConsumerID] [bigint] NOT NULL,
[AddressPrefix] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsOriginal] [bit] NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ResidentialStatus] [int] NOT NULL,
[POBox] [int] NOT NULL,
[USTerritory] [int] NOT NULL,
[MilitaryAddress] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ValidatedAddress] on [dbo].[ValidatedAddress]'
GO
ALTER TABLE [dbo].[ValidatedAddress] ADD CONSTRAINT [PK_ValidatedAddress] PRIMARY KEY CLUSTERED  ([ValidatedAddressID])
GO
PRINT N'Creating index [IX_ValidatedAddress_ConsumerIDAddressPrefix] on [dbo].[ValidatedAddress]'
GO
CREATE NONCLUSTERED INDEX [IX_ValidatedAddress_ConsumerIDAddressPrefix] ON [dbo].[ValidatedAddress] ([ConsumerID], [AddressPrefix])
GO
PRINT N'Adding foreign keys to [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [FK_BestRateShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[InsurancePolicy]'
GO
ALTER TABLE [dbo].[InsurancePolicy] ADD CONSTRAINT [FK_InsurancePolicy_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] ADD CONSTRAINT [FK_iParcelShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD CONSTRAINT [FK_OnTracShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] ADD CONSTRAINT [FK_OtherShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [FK_PostalShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD CONSTRAINT [FK_ShipmentCustomsItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [FK_UpsShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_ProcessedUser] FOREIGN KEY ([ProcessedUserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_ProcessedComputer] FOREIGN KEY ([ProcessedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_VoidedUser] FOREIGN KEY ([VoidedUserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_VoidedComputer] FOREIGN KEY ([VoidedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD CONSTRAINT [FK_AmazonStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[AmeriCommerceStore]'
GO
ALTER TABLE [dbo].[AmeriCommerceStore] ADD CONSTRAINT [FK_AmeriCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD CONSTRAINT [FK_BigCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[BuyDotComStore]'
GO
ALTER TABLE [dbo].[BuyDotComStore] ADD CONSTRAINT [FK_BuyComStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD CONSTRAINT [FK_ChannelAdvisorStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] ADD CONSTRAINT [FK_Download_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD CONSTRAINT [FK_EbayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[EtsyStore]'
GO
ALTER TABLE [dbo].[EtsyStore] ADD CONSTRAINT [FK_EtsyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD CONSTRAINT [FK_GenericFileStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD CONSTRAINT [FK_GenericModuleStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] ADD CONSTRAINT [FK_InfopiaStore_InfopiaStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorStore]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorStore] ADD CONSTRAINT [FK_MarketworksStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsStore]'
GO
ALTER TABLE [dbo].[NetworkSolutionsStore] ADD CONSTRAINT [FK_NetworkSolutionsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] ADD CONSTRAINT [FK_NeweggStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] ADD CONSTRAINT [FK_OrderMotionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[PayPalStore]'
GO
ALTER TABLE [dbo].[PayPalStore] ADD CONSTRAINT [FK_PayPalStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresStore]'
GO
ALTER TABLE [dbo].[ProStoresStore] ADD CONSTRAINT [FK_ProStoresStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] ADD CONSTRAINT [FK_SearsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD CONSTRAINT [FK_ShopifyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[StatusPreset]'
GO
ALTER TABLE [dbo].[StatusPreset] ADD CONSTRAINT [FK_StatusPreset_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] ADD CONSTRAINT [FK_StoreShopSite_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [FK_ThreeDCartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] ADD CONSTRAINT [FK_VolusionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD CONSTRAINT [FK_YahooStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD CONSTRAINT [FK_AmazonOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ClickCartProOrder]'
GO
ALTER TABLE [dbo].[ClickCartProOrder] ADD CONSTRAINT [FK_ClickCartProOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] ADD CONSTRAINT [FK_CommerceInterfaceOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD CONSTRAINT [FK_EbayOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EtsyOrder]'
GO
ALTER TABLE [dbo].[EtsyOrder] ADD CONSTRAINT [FK_EtsyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoOrder]'
GO
ALTER TABLE [dbo].[MagentoOrder] ADD CONSTRAINT [FK_MagentoOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorOrder]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrder] ADD CONSTRAINT [FK_MarketworksOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsOrder]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrder] ADD CONSTRAINT [FK_NetworkSolutionsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggOrder]'
GO
ALTER TABLE [dbo].[NeweggOrder] ADD CONSTRAINT [FK_NeweggOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] ADD CONSTRAINT [FK_OrderCharge_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] ADD CONSTRAINT [FK_OrderMotionOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] ADD CONSTRAINT [FK_OrderPaymentDetail_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[PayPalOrder]'
GO
ALTER TABLE [dbo].[PayPalOrder] ADD CONSTRAINT [FK_PayPalOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresOrder]'
GO
ALTER TABLE [dbo].[ProStoresOrder] ADD CONSTRAINT [FK_ProStoresOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[SearsOrder]'
GO
ALTER TABLE [dbo].[SearsOrder] ADD CONSTRAINT [FK_SearsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyOrder]'
GO
ALTER TABLE [dbo].[ShopifyOrder] ADD CONSTRAINT [FK_ShopifyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrder]'
GO
ALTER TABLE [dbo].[YahooOrder] ADD CONSTRAINT [FK_YahooOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ContentWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsGenerated'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_addextendedproperty N'AuditName', N'Insurance', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'OriginCountry', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginOriginID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'OriginState', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'111', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditName', N'Residential \ Commercial', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialResult'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipCountry', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'7', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipDate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentCost'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'103', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'TotalWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'BillCountry', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillNameParseStatus'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
EXEC sp_addextendedproperty N'AuditName', N'Store', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
