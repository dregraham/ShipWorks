SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Altering [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] ADD
[Authentication] [int] NOT NULL CONSTRAINT [DF_ShopSiteStore_AuthenticationType] DEFAULT ((0)),
[OauthClientID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShopSiteStore_ClientID] DEFAULT (''),
[OauthSecretKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShopSiteStore_SecretKey] DEFAULT (''),
[Identifier] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShopSiteStore_AuthorizationUrl] DEFAULT (''),
[AuthorizationCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShopSiteStore_AuthorizationCode] DEFAULT ('')
GO
EXEC sp_rename N'[dbo].[ShopSiteStore].[CgiUrl]', N'ApiUrl', N'COLUMN'
GO