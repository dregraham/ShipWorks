SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling Amazon shipping carrier in [dbo].[ShippingSettings] for existing customers'
GO
UPDATE [ShippingSettings] set [Excluded] = [Excluded] + ',16'
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [FK_ChannelAdvisorOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT [FK_ChannelAdvisorStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_Order_Store]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [PK_ChannelAdvisorOrder]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT [PK_ChannelAdvisorStore]
GO
PRINT N'Dropping constraints from [dbo].[PostalShipment]'
GO
IF EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[PostalShipment]') AND name = N'DF_PostalProfile_NoPostage')
BEGIN
	ALTER TABLE [dbo].[PostalShipment] DROP CONSTRAINT [DF_PostalProfile_NoPostage]
END
GO
PRINT N'Dropping index [IX_ChannelAdvisorOrder_OnlineShippingStatus] from [dbo].[ChannelAdvisorOrder]'
GO
DROP INDEX [IX_ChannelAdvisorOrder_OnlineShippingStatus] ON [dbo].[ChannelAdvisorOrder]
GO
PRINT N'Dropping index [IX_Order_ShipAddressValidationStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Order_ShipAddressValidationStatus] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Order_DestinationCommercial] from [dbo].[Order]'
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = N'IX_Order_DestinationCommercial')
BEGIN
	DROP INDEX [IX_Order_DestinationCommercial] ON [dbo].[Order]
END
GO
PRINT N'Dropping index [IX_Order_DestinationResidential] from [dbo].[Order]'
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = N'IX_Order_DestinationResidential')
BEGIN
	DROP INDEX [IX_Order_DestinationResidential] ON [dbo].[Order]
END
GO
PRINT N'Dropping index [IX_Store_OrderNumberComplete_IsManual] from [dbo].[Order]'
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = N'IX_Store_OrderNumberComplete_IsManual')
BEGIN
	DROP INDEX [IX_Store_OrderNumberComplete_IsManual] ON [dbo].[Order]
END
GO
PRINT N'Dropping index [IX_Shipment_ProcessedOrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Shipment_ShipAddressValidationStatus] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ShipAddressValidationStatus] ON [dbo].[Shipment]
GO
PRINT N'Creating [dbo].[AmazonShipment]'
GO
CREATE TABLE [dbo].[AmazonShipment]
(
[ShipmentID] [bigint] NOT NULL,
[CarrierName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_CarrierName] DEFAULT (''),
[ShippingServiceName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ShippingServiceName] DEFAULT (''),
[ShippingServiceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ShippingServiceId] DEFAULT (''),
[ShippingServiceOfferID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ShippingServiceOfferId] DEFAULT (''),
[InsuranceValue] [money] NOT NULL CONSTRAINT [DF_AmazonShipment_InsuranceValue] DEFAULT ((0)),
[DimsProfileID] [bigint] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsProfileID] DEFAULT ((0)),
[DimsLength] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsLength] DEFAULT ((0)),
[DimsWidth] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsWidth] DEFAULT ((0)),
[DimsHeight] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsHeight] DEFAULT ((0)),
[DimsWeight] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsWeight] DEFAULT ((0)),
[DimsAddWeight] [bit] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsAddWeight] DEFAULT ((0)),
[DeliveryExperience] [int] NOT NULL CONSTRAINT [DF_AmazonShipment_DeliveryExperience] DEFAULT ((0)),
[DeclaredValue] [money] NULL,
[AmazonUniqueShipmentID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_AmazonShipment] on [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ADD CONSTRAINT [PK_AmazonShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Altering [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD
[AmazonShippingToken] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonStore_AmazonShippingToken] DEFAULT (N'hlkH7XeEA5GehLVXyC7ZpSjpPQVzOciXTvoFKlnobEJblNvBlX/dasYvkTYKkOQfL5Oy6kfyeZI=')
GO
PRINT N'Creating [dbo].[AmazonProfile]'
GO
CREATE TABLE [dbo].[AmazonProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[DeliveryExperience] [int] NULL,
[Weight] [float] NULL
)
GO
PRINT N'Creating primary key [PK_AmazonProfile] on [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [PK_AmazonProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrder]
(
[OrderID] [bigint] NOT NULL,
[CustomOrderIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ResellerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineShippingStatus] [int] NOT NULL,
[OnlineCheckoutStatus] [int] NOT NULL,
[OnlinePaymentStatus] [int] NOT NULL,
[FlagStyle] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FlagDescription] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FlagType] [int] NOT NULL,
[MarketplaceNames] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsPrime] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrder]([OrderID], [CustomOrderIdentifier], [ResellerID], [OnlineShippingStatus], [OnlineCheckoutStatus], [OnlinePaymentStatus], [FlagStyle], [FlagDescription], [FlagType], [MarketplaceNames], [IsPrime])
SELECT [ChannelAdvisorOrder].[OrderID], [CustomOrderIdentifier], [ResellerID], [OnlineShippingStatus], [OnlineCheckoutStatus], [OnlinePaymentStatus], [FlagStyle], [FlagDescription], [FlagType], [MarketplaceNames],
	CASE
		WHEN RequestedShipping LIKE '%Amazon%' AND RequestedShipping LIKE '%Prime%' THEN 1
		WHEN RequestedShipping = '' THEN 0
		ELSE 2
	END FROM [dbo].[ChannelAdvisorOrder] LEFT JOIN [dbo].[Order] ON [dbo].[ChannelAdvisorOrder].[OrderId] = [dbo].[Order].[OrderId]
GO
DROP TABLE [dbo].[ChannelAdvisorOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrder]', N'ChannelAdvisorOrder'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrder] on [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [PK_ChannelAdvisorOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrder_OnlineShippingStatus] on [dbo].[ChannelAdvisorOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrder_OnlineShippingStatus] ON [dbo].[ChannelAdvisorOrder] ([OnlineShippingStatus])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrder_IsPrime] on [dbo].[ChannelAdvisorOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrder_IsPrime] ON [dbo].[ChannelAdvisorOrder] ([IsPrime])
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorStore]
(
[StoreID] [bigint] NOT NULL,
[AccountKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProfileID] [int] NOT NULL,
[AttributesToDownload] [xml] NOT NULL,
[ConsolidatorAsUsps] [bit] NOT NULL,
[AmazonMerchantID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonAuthToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonApiRegion] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonShippingToken] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorStore]([StoreID], [AccountKey], [ProfileID], [AttributesToDownload], [ConsolidatorAsUsps], [AmazonMerchantID], [AmazonAuthToken], [AmazonApiRegion], [AmazonShippingToken])
SELECT [StoreID], [AccountKey], [ProfileID], [AttributesToDownload], [ConsolidatorAsUsps], '', '', '', 'hlkH7XeEA5GehLVXyC7ZpSjpPQVzOciXTvoFKlnobEJblNvBlX/dasYvkTYKkOQfL5Oy6kfyeZI=' FROM [dbo].[ChannelAdvisorStore]
GO
DROP TABLE [dbo].[ChannelAdvisorStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorStore]', N'ChannelAdvisorStore'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorStore] on [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD CONSTRAINT [PK_ChannelAdvisorStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrderItem_Classification] on [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_Classification] ON [dbo].[ChannelAdvisorOrderItem] ([Classification])
GO
PRINT N'Creating index [IX_Order_ShipAddressValidationStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_ShipAddressValidationStatus] ON [dbo].[Order] ([ShipAddressValidationStatus] DESC) INCLUDE ([OrderDate])
GO
PRINT N'Creating index [IX_Shipment_ProcessedOrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC) INCLUDE ([OrderID], [Voided])
GO
PRINT N'Creating index [IX_Shipment_ShipAddressValidationStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipAddressValidationStatus] ON [dbo].[Shipment] ([ShipAddressValidationStatus] DESC) INCLUDE ([OrderID], [Processed], [Voided])
GO
PRINT N'Adding constraints to [dbo].[PostalShipment]'
GO
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[PostalShipment]') AND name = N'DF_PostalShipment_NoPostage')
BEGIN
	ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [DF_PostalShipment_NoPostage] DEFAULT ((0)) FOR [NoPostage]
END
GO
PRINT N'Adding foreign keys to [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ADD CONSTRAINT [FK_AmazonShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [FK_AmazonProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD CONSTRAINT [FK_ChannelAdvisorStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'CarrierName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DeclaredValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'129', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DeliveryExperience'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsAddWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsProfileID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'InsuranceValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'ShippingServiceName'
GO