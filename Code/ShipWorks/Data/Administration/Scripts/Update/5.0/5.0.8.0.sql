SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping extended properties'
GO
IF EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'))
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
IF EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'))
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
IF EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'))
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO
IF EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'))
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO
PRINT N'Dropping foreign keys from [dbo].[ThreeDCartStore]'
GO
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ThreeDCartStore_Store]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ThreeDCartStore]', 'U'))
ALTER TABLE [dbo].[ThreeDCartStore] DROP CONSTRAINT [FK_ThreeDCartStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[ThreeDCartStore]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_ThreeDCartStore]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[ThreeDCartStore]', 'U'))
ALTER TABLE [dbo].[ThreeDCartStore] DROP CONSTRAINT [PK_ThreeDCartStore]
GO
PRINT N'Rebuilding [dbo].[ThreeDCartStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ThreeDCartStore]
(
[StoreID] [bigint] NOT NULL,
[StoreUrl] [nvarchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserKey] [nvarchar] (65) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TimeZoneID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StatusCodes] [xml] NULL,
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL,
[RestUser] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ThreeDCartStore]([StoreID], [StoreUrl], [ApiUserKey], [TimeZoneID], [StatusCodes], [DownloadModifiedNumberOfDaysBack]) SELECT [StoreID], [StoreUrl], [ApiUserKey], [TimeZoneID], [StatusCodes], [DownloadModifiedNumberOfDaysBack] FROM [dbo].[ThreeDCartStore]
GO
DROP TABLE [dbo].[ThreeDCartStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ThreeDCartStore]', N'ThreeDCartStore'
GO
PRINT N'Creating primary key [PK_ThreeDCartStore] on [dbo].[ThreeDCartStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ThreeDCartStore' AND object_id = OBJECT_ID(N'[dbo].[ThreeDCartStore]'))
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [PK_ThreeDCartStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ThreeDCartStore_Store]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ThreeDCartStore]', 'U'))
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [FK_ThreeDCartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating extended properties'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'))
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'))
EXEC sp_addextendedproperty N'AuditName', N'User Key', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'))
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'))
EXEC sp_addextendedproperty N'AuditName', N'Store URL', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO

PRINT N'Creating [dbo].[ThreeDCartOrder]'
GO
CREATE TABLE [dbo].[ThreeDCartOrder]
(
[OrderID] [bigint] NOT NULL,
[ThreeDCartOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrder] on [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [PK_ThreeDCartOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [FK_ThreeDCartOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
