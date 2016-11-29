truncate table [QuickFilterNodeContentDirty]
GO

truncate table [FilterNodeContentDirty]
GO

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_FilterNodeContentDirty_IgnoreDuplicates] from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_IgnoreDuplicates' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
DROP INDEX [IX_FilterNodeContentDirty_IgnoreDuplicates] ON [dbo].[FilterNodeContentDirty]
GO

PRINT N'Dropping index [IX_QuickFilterNodeContentDirty_IgnoreDuplicates] from [dbo].[QuickFilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_IgnoreDuplicates' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
DROP INDEX [IX_QuickFilterNodeContentDirty_IgnoreDuplicates] ON [dbo].[QuickFilterNodeContentDirty]
GO

PRINT N'Creating index [IX_FilterNodeContentDirty_IgnoreDuplicates] on [dbo].[FilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_IgnoreDuplicates' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_IgnoreDuplicates] ON [dbo].[FilterNodeContentDirty] ([ObjectID], [ColumnsUpdated], [ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO

PRINT N'Creating index [IX_QuickFilterNodeContentDirty_IgnoreDuplicates] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_IgnoreDuplicates' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
CREATE UNIQUE NONCLUSTERED INDEX [IX_QuickFilterNodeContentDirty_IgnoreDuplicates] ON [dbo].[QuickFilterNodeContentDirty] ([ObjectID], [ColumnsUpdated], [ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO

PRINT N'Creating index [IX_FilterNodeContentDirty_ParentIDObjectType] on [dbo].[FilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_ParentIDObjectType' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
CREATE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_ParentIDObjectType] ON [dbo].[FilterNodeContentDirty] ([ColumnsUpdated],[ComputerID])
GO

PRINT N'Creating index [IX_QuickFilterNodeContentDirty_ParentIDObjectType] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_ParentIDObjectType' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
CREATE NONCLUSTERED INDEX [IX_QuickFilterNodeContentDirty_ParentIDObjectType] ON [dbo].[QuickFilterNodeContentDirty] ([ColumnsUpdated],[ComputerID])
GO
