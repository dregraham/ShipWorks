SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[Store] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] DROP
CONSTRAINT [FK_AmazonStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[AmeriCommerceStore]'
GO
ALTER TABLE [dbo].[AmeriCommerceStore] DROP
CONSTRAINT [FK_AmeriCommerceStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP
CONSTRAINT [FK_ChannelAdvisorStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] DROP
CONSTRAINT [FK_Download_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] DROP
CONSTRAINT [FK_EbayStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] DROP
CONSTRAINT [FK_GenericFileStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP
CONSTRAINT [FK_GenericModuleStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] DROP
CONSTRAINT [FK_InfopiaStore_InfopiaStore]
GO
PRINT N'Dropping foreign keys from [dbo].[MarketplaceAdvisorStore]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorStore] DROP
CONSTRAINT [FK_MarketworksStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[NetworkSolutionsStore]'
GO
ALTER TABLE [dbo].[NetworkSolutionsStore] DROP
CONSTRAINT [FK_NetworkSolutionsStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] DROP
CONSTRAINT [FK_Order_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] DROP
CONSTRAINT [FK_OrderMotionStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[PayPalStore]'
GO
ALTER TABLE [dbo].[PayPalStore] DROP
CONSTRAINT [FK_PayPalStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ProStoresStore]'
GO
ALTER TABLE [dbo].[ProStoresStore] DROP
CONSTRAINT [FK_ProStoresStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] DROP
CONSTRAINT [FK_StoreShopSite_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[StatusPreset]'
GO
ALTER TABLE [dbo].[StatusPreset] DROP
CONSTRAINT [FK_StatusPreset_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] DROP
CONSTRAINT [FK_VolusionStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] DROP
CONSTRAINT [FK_YahooStore_Store]
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
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AutoDownload] [bit] NOT NULL,
[AutoDownloadMinutes] [int] NOT NULL,
[AutoDownloadOnlyAway] [bit] NOT NULL,
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
INSERT INTO [dbo].[tmp_rg_xx_Store]([StoreID], [License], [Edition], [TypeCode], [Enabled], [SetupComplete], [StoreName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], [Website], [AutoDownload], [AutoDownloadMinutes], [AutoDownloadOnlyAway], [ComputerDownloadPolicy], [DefaultEmailAccountID], [ManualOrderPrefix], [ManualOrderPostfix], [InitialDownloadDays], [InitialDownloadOrder]) SELECT [StoreID], [License], [Edition], [TypeCode], [Enabled], [SetupComplete], [StoreName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], [Website], [AutoDownload], [AutoDownloadMinutes], [AutoDownloadOnlyAway], '', [DefaultEmailAccountID], [ManualOrderPrefix], [ManualOrderPostfix], [InitialDownloadDays], [InitialDownloadOrder] FROM [dbo].[Store]
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
PRINT N'Adding foreign keys to [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD
CONSTRAINT [FK_AmazonStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[AmeriCommerceStore]'
GO
ALTER TABLE [dbo].[AmeriCommerceStore] ADD
CONSTRAINT [FK_AmeriCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD
CONSTRAINT [FK_ChannelAdvisorStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] ADD
CONSTRAINT [FK_Download_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD
CONSTRAINT [FK_EbayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD
CONSTRAINT [FK_GenericFileStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD
CONSTRAINT [FK_GenericModuleStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] ADD
CONSTRAINT [FK_InfopiaStore_InfopiaStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorStore]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorStore] ADD
CONSTRAINT [FK_MarketworksStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsStore]'
GO
ALTER TABLE [dbo].[NetworkSolutionsStore] ADD
CONSTRAINT [FK_NetworkSolutionsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD
CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] ADD
CONSTRAINT [FK_OrderMotionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[PayPalStore]'
GO
ALTER TABLE [dbo].[PayPalStore] ADD
CONSTRAINT [FK_PayPalStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresStore]'
GO
ALTER TABLE [dbo].[ProStoresStore] ADD
CONSTRAINT [FK_ProStoresStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[StatusPreset]'
GO
ALTER TABLE [dbo].[StatusPreset] ADD
CONSTRAINT [FK_StatusPreset_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] ADD
CONSTRAINT [FK_StoreShopSite_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] ADD
CONSTRAINT [FK_VolusionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD
CONSTRAINT [FK_YahooStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
