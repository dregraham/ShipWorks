SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[BigCommerceStore]'
GO
IF COL_LENGTH(N'[dbo].[BigCommerceStore]', N'BigCommerceAuthentication') IS NULL
ALTER TABLE [dbo].[BigCommerceStore] ADD[BigCommerceAuthentication] [int] NOT NULL CONSTRAINT [DF_BigCommerceStore_BigCommerceAuthentication] DEFAULT ((1))
IF COL_LENGTH(N'[dbo].[BigCommerceStore]', N'OauthClientId') IS NULL
ALTER TABLE [dbo].[BigCommerceStore] ADD[OauthClientId] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_BigCommerceStore_OauthClientId] DEFAULT ('')
IF COL_LENGTH(N'[dbo].[BigCommerceStore]', N'OauthToken') IS NULL
ALTER TABLE [dbo].[BigCommerceStore] ADD[OauthToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_BigCommerceStore_OauthToken] DEFAULT ('')
GO
PRINT N'Creating extended properties'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'BigCommerceAuthentication'))
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'BigCommerceAuthentication'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'BigCommerceAuthentication'))
EXEC sp_addextendedproperty N'AuditName', N'BigCommerce Authentication Type', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'BigCommerceAuthentication'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthClientId'))
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthClientId'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthClientId'))
EXEC sp_addextendedproperty N'AuditName', N'OAuth Client ID', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthClientId'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthToken'))
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthToken'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthToken'))
EXEC sp_addextendedproperty N'AuditName', N'OAuth Token', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'OauthToken'
GO

UPDATE BigCommerceStore SET BigCommerceAuthentication = 0
GO