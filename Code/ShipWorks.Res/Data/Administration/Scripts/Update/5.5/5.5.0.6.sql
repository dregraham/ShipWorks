SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] DROP CONSTRAINT [FK_OdbcStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] DROP CONSTRAINT [PK_OdbcStore]
GO
PRINT N'Rebuilding [dbo].[OdbcStore]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_OdbcStore]
(
[StoreID] [bigint] NOT NULL,
[ImportConnectionString] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImportMap] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImportStrategy] [int] NOT NULL,
[ImportColumnSourceType] [int] NOT NULL,
[ImportColumnSource] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImportOrderItemStrategy] [int] NOT NULL,
[UploadStrategy] [int] NOT NULL,
[UploadMap] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UploadColumnSourceType] [int] NOT NULL,
[UploadColumnSource] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UploadConnectionString] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[RG_Recovery_1_OdbcStore]([StoreID], [ImportConnectionString], [ImportMap], [ImportStrategy], [ImportColumnSourceType], [ImportColumnSource],
	[ImportOrderItemStrategy], 
	[UploadStrategy], [UploadMap], [UploadColumnSourceType], [UploadColumnSource], [UploadConnectionString]) 
SELECT [StoreID], [ImportConnectionString], [ImportMap], [ImportStrategy], [ImportColumnSourceType], [ImportColumnSource],
	0, 
	[UploadStrategy], [UploadMap], [UploadColumnSourceType], [UploadColumnSource], [UploadConnectionString] FROM [dbo].[OdbcStore]
GO
DROP TABLE [dbo].[OdbcStore]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_OdbcStore]', N'OdbcStore', N'OBJECT'
GO
PRINT N'Creating primary key [PK_OdbcStore] on [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] ADD CONSTRAINT [PK_OdbcStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] ADD CONSTRAINT [FK_OdbcStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO