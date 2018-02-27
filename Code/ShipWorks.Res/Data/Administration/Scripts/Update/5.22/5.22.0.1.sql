SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping constraints from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_FilterNodeContentDirty]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]', 'U'))
ALTER TABLE [dbo].[FilterNodeContentDirty] DROP CONSTRAINT [PK_FilterNodeContentDirty]
GO
PRINT N'Dropping index [SW_FilterNodeContentDirty_FilterNodeContentDirtyID] from [dbo].[FilterNodeContentDirty]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'SW_FilterNodeContentDirty_FilterNodeContentDirtyID' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
DROP INDEX [SW_FilterNodeContentDirty_FilterNodeContentDirtyID] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Creating primary key [PK_FilterNodeContentDirty_1] on [dbo].[FilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_FilterNodeContentDirty_1' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
ALTER TABLE [dbo].[FilterNodeContentDirty] ADD CONSTRAINT [PK_FilterNodeContentDirty_1] PRIMARY KEY CLUSTERED  ([FilterNodeContentDirtyID])
GO
PRINT N'Creating index [IX_FilterNodeContentDirty_FilterNodeContentDirtyID] on [dbo].[FilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContentDirty_FilterNodeContentDirtyID' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
CREATE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_FilterNodeContentDirtyID] ON [dbo].[FilterNodeContentDirty] ([ObjectID], [ComputerID], [ColumnsUpdated]) INCLUDE ([ObjectType], [ParentID])
GO
