SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[ShopSiteStore]'
GO
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreShopSite_Store]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ShopSiteStore]', 'U'))
ALTER TABLE [dbo].[ShopSiteStore] DROP CONSTRAINT [FK_StoreShopSite_Store]
GO
PRINT N'Dropping constraints from [dbo].[ShopSiteStore]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_StoreShopSite]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[ShopSiteStore]', 'U'))
ALTER TABLE [dbo].[ShopSiteStore] DROP CONSTRAINT [PK_StoreShopSite]
GO

PRINT N'Rebuilding [dbo].[ShopSiteStore]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_ShopSiteStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RequireSSL] [bit] NOT NULL,
[DownloadPageSize] [int] NOT NULL,
[RequestTimeout] [int] NOT NULL,
[ShopSiteAuthentication] [int] NOT NULL,
[OauthClientID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OauthSecretKey] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OauthAuthorizationCode] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Identifier] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[RG_Recovery_1_ShopSiteStore]
		  ([StoreID], [Username], [Password], [ApiUrl], [RequireSSL], [DownloadPageSize], [RequestTimeout], [ShopSiteAuthentication], [OauthClientID], [OauthSecretKey], [OauthAuthorizationCode], [Identifier]) 
	SELECT [StoreID], [Username], [Password], [CgiUrl], [RequireSSL], [DownloadPageSize], [RequestTimeout], 0, '', '', '', '' 
	FROM [dbo].[ShopSiteStore]
GO
DROP TABLE [dbo].[ShopSiteStore]
GO
IF (OBJECT_ID(N'[dbo].[RG_Recovery_1_ShopSiteStore]', 'U') IS NOT NULL) AND (OBJECT_ID(N'[dbo].[ShopSiteStore]', 'U') IS NULL)
EXEC sp_rename N'[dbo].[RG_Recovery_1_ShopSiteStore]', N'ShopSiteStore', N'OBJECT'
GO
PRINT N'Creating primary key [PK_StoreShopSite] on [dbo].[ShopSiteStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_StoreShopSite' AND object_id = OBJECT_ID(N'[dbo].[ShopSiteStore]'))
ALTER TABLE [dbo].[ShopSiteStore] ADD CONSTRAINT [PK_StoreShopSite] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopSiteStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreShopSite_Store]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ShopSiteStore]', 'U'))
ALTER TABLE [dbo].[ShopSiteStore] ADD CONSTRAINT [FK_StoreShopSite_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO