SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[Store] DISABLE CHANGE_TRACKING
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
PRINT N'Dropping foreign keys from [dbo].[GrouponStore]'
GO
ALTER TABLE [dbo].[GrouponStore] DROP CONSTRAINT [FK_GrouponStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] DROP CONSTRAINT [FK_InfopiaStore_InfopiaStore]
GO
PRINT N'Dropping foreign keys from [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] DROP CONSTRAINT [FK_JetStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[LemonStandStore]'
GO
ALTER TABLE [dbo].[LemonStandStore] DROP CONSTRAINT [FK_LemonStandStore_Store]
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
PRINT N'Dropping foreign keys from [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] DROP CONSTRAINT [FK_OdbcStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_Order_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] DROP CONSTRAINT [FK_OrderMotionStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderSearch]'
GO
ALTER TABLE [dbo].[OrderSearch] DROP CONSTRAINT [FK_OrderSearch_Store]
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
PRINT N'Dropping foreign keys from [dbo].[SparkPayStore]'
GO
ALTER TABLE [dbo].[SparkPayStore] DROP CONSTRAINT [FK_SparkPayStore_Store]
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
PRINT N'Dropping foreign keys from [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].[WalmartStore] DROP CONSTRAINT [FK_WalmartStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] DROP CONSTRAINT [FK_YahooStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] DROP CONSTRAINT [PK_Store]
GO
PRINT N'Dropping index [IX_Store_StoreName] from [dbo].[Store]'
GO
DROP INDEX [IX_Store_StoreName] ON [dbo].[Store]
GO
PRINT N'Rebuilding [dbo].[Store]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_Store]
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
[DomesticAddressValidationSetting] [int] NOT NULL,
[InternationalAddressValidationSetting] [int] NOT NULL CONSTRAINT [DF_Store_InternationalAddressValidationSetting] DEFAULT ((1)),
[ComputerDownloadPolicy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultEmailAccountID] [bigint] NOT NULL,
[ManualOrderPrefix] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ManualOrderPostfix] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InitialDownloadDays] [int] NULL,
[InitialDownloadOrder] [bigint] NULL
)
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_Store] ON
GO
INSERT INTO [dbo].[RG_Recovery_1_Store]([StoreID], [License], [Edition], [TypeCode], [Enabled], [SetupComplete], [StoreName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], [Website], [AutoDownload], [AutoDownloadMinutes], [AutoDownloadOnlyAway], [DomesticAddressValidationSetting], [ComputerDownloadPolicy], [DefaultEmailAccountID], [ManualOrderPrefix], [ManualOrderPostfix], [InitialDownloadDays], [InitialDownloadOrder]) SELECT [StoreID], [License], [Edition], [TypeCode], [Enabled], [SetupComplete], [StoreName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], [Website], [AutoDownload], [AutoDownloadMinutes], [AutoDownloadOnlyAway], [AddressValidationSetting], [ComputerDownloadPolicy], [DefaultEmailAccountID], [ManualOrderPrefix], [ManualOrderPostfix], [InitialDownloadDays], [InitialDownloadOrder] FROM [dbo].[Store]
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_Store] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Store]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[RG_Recovery_1_Store]', RESEED, @idVal)
GO
DROP TABLE [dbo].[Store]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_Store]', N'Store', N'OBJECT'
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
PRINT N'Adding foreign keys to [dbo].[GrouponStore]'
GO
ALTER TABLE [dbo].[GrouponStore] ADD CONSTRAINT [FK_GrouponStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] ADD CONSTRAINT [FK_InfopiaStore_InfopiaStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] ADD CONSTRAINT [FK_JetStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[LemonStandStore]'
GO
ALTER TABLE [dbo].[LemonStandStore] ADD CONSTRAINT [FK_LemonStandStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
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
PRINT N'Adding foreign keys to [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] ADD CONSTRAINT [FK_OdbcStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] ADD CONSTRAINT [FK_OrderMotionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderSearch]'
GO
ALTER TABLE [dbo].[OrderSearch] ADD CONSTRAINT [FK_OrderSearch_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
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
PRINT N'Adding foreign keys to [dbo].[SparkPayStore]'
GO
ALTER TABLE [dbo].[SparkPayStore] ADD CONSTRAINT [FK_SparkPayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
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
PRINT N'Adding foreign keys to [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].[WalmartStore] ADD CONSTRAINT [FK_WalmartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD CONSTRAINT [FK_YahooStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Dropping constraints from [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] DROP CONSTRAINT [DF_Store_InternationalAddressValidationSetting]
GO

-- Mark the address validation status to pending for all orders in the past 5 days
-- that have either no shipments or at least one unprocessed shipment
-- and ShipAddressValidationStatus was set to 9 - WillNotValidate
UPDATE [Order]
SET ShipAddressValidationStatus = 1
FROM [Order] 
LEFT JOIN Shipment ON [Order].OrderId = Shipment.OrderID
WHERE DATEDIFF(d, [Order].OrderDate, GETDATE()) <= 5 
	AND (Shipment.ShipmentID IS NULL OR Shipment.Processed = 0)
	AND ([Order].ShipAddressValidationStatus = 9)
GO

-- Mark the address validation status to pending for all shipments with 
-- orders in the past 5 days that have not been processed 
-- and ShipAddressValidationStatus was set to 9 - WillNotValidate
UPDATE Shipment
SET ShipAddressValidationStatus = 1
FROM Shipment
INNER JOIN [Order] ON [Order].OrderId = Shipment.OrderId
WHERE  DATEDIFF(d, [Order].OrderDate, GETDATE()) <= 5 
	AND Shipment.Processed = 0
	AND Shipment.ShipAddressValidationStatus = 9
GO