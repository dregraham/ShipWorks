SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[MagentoStore]'
GO
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MagentoStore_GenericModuleStore]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[MagentoStore]', 'U'))
ALTER TABLE [dbo].[MagentoStore] DROP CONSTRAINT [FK_MagentoStore_GenericModuleStore]
GO
PRINT N'Dropping constraints from [dbo].[MagentoStore]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_MagentoStore]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[MagentoStore]', 'U'))
ALTER TABLE [dbo].[MagentoStore] DROP CONSTRAINT [PK_MagentoStore]
GO
PRINT N'Rebuilding [dbo].[MagentoStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_MagentoStore]
(
[StoreID] [bigint] NOT NULL,
[MagentoTrackingEmails] [bit] NOT NULL,
[MagentoVersion] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_MagentoStore]([StoreID], [MagentoTrackingEmails], [MagentoVersion]) SELECT [StoreID], [MagentoTrackingEmails], [MagentoConnect] FROM [dbo].[MagentoStore]
GO
DROP TABLE [dbo].[MagentoStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_MagentoStore]', N'MagentoStore'
GO
PRINT N'Creating primary key [PK_MagentoStore] on [dbo].[MagentoStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_MagentoStore' AND object_id = OBJECT_ID(N'[dbo].[MagentoStore]'))
ALTER TABLE [dbo].[MagentoStore] ADD CONSTRAINT [PK_MagentoStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MagentoStore_GenericModuleStore]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[MagentoStore]', 'U'))
ALTER TABLE [dbo].[MagentoStore] ADD CONSTRAINT [FK_MagentoStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
GO