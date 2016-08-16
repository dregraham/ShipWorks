SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonASIN]'
GO
ALTER TABLE [dbo].[AmazonASIN] DROP
CONSTRAINT [FK_AmazonASIN_AmazonStore]
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] DROP
CONSTRAINT [FK_AmazonStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] DROP CONSTRAINT [PK_AmazonStore]
GO
PRINT N'Rebuilding [dbo].[AmazonStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_AmazonStore]
(
[StoreID] [bigint] NOT NULL,
[AmazonApi] [int] NOT NULL,
[SellerCentralUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellerCentralPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantName] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantToken] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccessKeyID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Cookie] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CookieExpires] [datetime] NOT NULL,
[CookieWaitUntil] [datetime] NOT NULL,
[Certificate] [varbinary] (2048) NULL,
[WeightDownloads] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_AmazonStore]([StoreID], [AmazonApi], [SellerCentralUsername], [SellerCentralPassword], [MerchantName], [MerchantToken], [AccessKeyID], [Cookie], [CookieExpires], [CookieWaitUntil], [Certificate], [WeightDownloads], [MerchantID], [MarketplaceID]) SELECT [StoreID], 0, [SellerCentralUsername], [SellerCentralPassword], [MerchantName], [MerchantToken], [AccessKeyID], [Cookie], [CookieExpires], [CookieWaitUntil], [Certificate], [WeightDownloads], '', '' FROM [dbo].[AmazonStore]
GO
DROP TABLE [dbo].[AmazonStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_AmazonStore]', N'AmazonStore'
GO
PRINT N'Creating primary key [PK_AmazonStore] on [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD CONSTRAINT [PK_AmazonStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[AmazonMwsApiLimit]'
GO
CREATE TABLE [dbo].[AmazonMwsApiLimit]
(
[MerchantID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MwsApiCall] [int] NOT NULL,
[CallsRemain] [int] NOT NULL,
[LastIncremented] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonMwsApiLimit] on [dbo].[AmazonMwsApiLimit]'
GO
ALTER TABLE [dbo].[AmazonMwsApiLimit] ADD CONSTRAINT [PK_AmazonMwsApiLimit] PRIMARY KEY CLUSTERED  ([MerchantID], [MwsApiCall])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonASIN]'
GO
ALTER TABLE [dbo].[AmazonASIN] ADD
CONSTRAINT [FK_AmazonASIN_AmazonStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[AmazonStore] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD
CONSTRAINT [FK_AmazonStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO