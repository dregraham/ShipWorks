truncate table [FilterNodeContentDirty]
GO
truncate table [QuickFilterNodeContentDirty]
GO

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping constraints from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_FilterNodeContentDirty]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]', 'U'))
ALTER TABLE [dbo].[FilterNodeContentDirty] DROP CONSTRAINT [PK_FilterNodeContentDirty]
GO
PRINT N'Dropping constraints from [dbo].[QuickFilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_QuickFilterNodeContentDirty]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]', 'U'))
ALTER TABLE [dbo].[QuickFilterNodeContentDirty] DROP CONSTRAINT [PK_QuickFilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_FilterNodeContentDirty_ColumnsUpdated] from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_ColumnsUpdated' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
DROP INDEX [IX_FilterNodeContentDirty_ColumnsUpdated] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_FilterNodeContentDirty_IgnoreDuplicates] from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_IgnoreDuplicates' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
DROP INDEX [IX_FilterNodeContentDirty_IgnoreDuplicates] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_FilterNodeContentDirty_ParentIDObjectType] from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_ParentIDObjectType' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
DROP INDEX [IX_FilterNodeContentDirty_ParentIDObjectType] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_FilterNodeContentDirty_RowVersion] from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_RowVersion' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
DROP INDEX [IX_FilterNodeContentDirty_RowVersion] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_QuickFilterNodeContentDirty_ColumnsUpdated] from [dbo].[QuickFilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_ColumnsUpdated' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
DROP INDEX [IX_QuickFilterNodeContentDirty_ColumnsUpdated] ON [dbo].[QuickFilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_QuickFilterNodeContentDirty_IgnoreDuplicates] from [dbo].[QuickFilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_IgnoreDuplicates' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
DROP INDEX [IX_QuickFilterNodeContentDirty_IgnoreDuplicates] ON [dbo].[QuickFilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_QuickFilterNodeContentDirty_ParentIDObjectType] from [dbo].[QuickFilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_ParentIDObjectType' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
DROP INDEX [IX_QuickFilterNodeContentDirty_ParentIDObjectType] ON [dbo].[QuickFilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_QuickFilterNodeContentDirty_RowVersion] from [dbo].[QuickFilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_RowVersion' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
DROP INDEX [IX_QuickFilterNodeContentDirty_RowVersion] ON [dbo].[QuickFilterNodeContentDirty]
GO
PRINT N'Creating primary key [PK_FilterNodeContentDirty] on [dbo].[FilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_FilterNodeContentDirty' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
ALTER TABLE [dbo].[FilterNodeContentDirty] ADD CONSTRAINT [PK_FilterNodeContentDirty] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated], [ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeContentDirty] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeContentDirty' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
ALTER TABLE [dbo].[QuickFilterNodeContentDirty] ADD CONSTRAINT [PK_QuickFilterNodeContentDirty] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated], [ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
