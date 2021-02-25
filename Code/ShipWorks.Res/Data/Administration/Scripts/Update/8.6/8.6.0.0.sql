GO
PRINT N'Creating [dbo].[PlatformStore]'
GO
IF OBJECT_ID(N'[dbo].[PlatformStore]', 'U') IS NULL
CREATE TABLE [dbo].[PlatformStore]
(
[StoreID] [bigint] NOT NULL,
[OrderSourceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_PlatformStore] on [dbo].[PlatformStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_PlatformStore]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[PlatformStore]', 'U'))
ALTER TABLE [dbo].[PlatformStore] ADD CONSTRAINT [PK_PlatformStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[PlatformStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PlatformStore_Store]','F') AND parent_object_id = OBJECT_ID(N'[dbo].[PlatformStore]', 'U'))
ALTER TABLE [dbo].[PlatformStore] ADD CONSTRAINT [FK_PlatformStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
