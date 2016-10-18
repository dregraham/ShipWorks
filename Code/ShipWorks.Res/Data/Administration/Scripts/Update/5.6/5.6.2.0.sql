
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[QuickFilterNodeContentDirty]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeContentDirty]
(
[FilterNodeContentDirtyID] [bigint] NOT NULL IDENTITY(1, 1),
[RowVersion] [timestamp] NOT NULL,
[ObjectID] [bigint] NOT NULL,
[ParentID] [bigint] NULL,
[ObjectType] [int] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeContentDirty] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeContentDirty' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
ALTER TABLE [dbo].[QuickFilterNodeContentDirty] ADD CONSTRAINT [PK_QuickFilterNodeContentDirty] PRIMARY KEY CLUSTERED  ([FilterNodeContentDirtyID])
GO
PRINT N'Creating index [IX_QuickFilterNodeContentDirty_RowVersion] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_RowVersion' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
CREATE NONCLUSTERED INDEX [IX_QuickFilterNodeContentDirty_RowVersion] ON [dbo].[QuickFilterNodeContentDirty] ([RowVersion])
GO
PRINT N'Creating index [IX_QuickFilterNodeContentDirty_ParentIDObjectType] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_ParentIDObjectType' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
CREATE NONCLUSTERED INDEX [IX_QuickFilterNodeContentDirty_ParentIDObjectType] ON [dbo].[QuickFilterNodeContentDirty] ([ParentID], [ObjectType]) INCLUDE ([ColumnsUpdated], [ComputerID])
GO
PRINT N'Creating index [IX_QuickFilterNodeContentDirty_ColumnsUpdated] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QuickFilterNodeContentDirty_ColumnsUpdated' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
CREATE NONCLUSTERED INDEX [IX_QuickFilterNodeContentDirty_ColumnsUpdated] ON [dbo].[QuickFilterNodeContentDirty] ([ColumnsUpdated])
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCheckpoint]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateCheckpoint]
(
[CheckpointID] [bigint] NOT NULL IDENTITY(1070, 1000),
[MaxDirtyID] [bigint] NOT NULL,
[DirtyCount] [int] NOT NULL,
[State] [int] NOT NULL,
[Duration] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateCheckpoint] on [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateCheckpoint' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCheckpoint]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateCheckpoint] ADD CONSTRAINT [PK_QuickFilterNodeUpdateCheckpoint] PRIMARY KEY CLUSTERED  ([CheckpointID])
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateCustomer]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCustomer]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateCustomer]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateCustomer] on [dbo].[QuickFilterNodeUpdateCustomer]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateCustomer' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCustomer]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateCustomer] ADD CONSTRAINT [PK_QuickFilterNodeUpdateCustomer] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateItem]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateItem]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateItem]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateItem] on [dbo].[QuickFilterNodeUpdateItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateItem' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateItem]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateItem] ADD CONSTRAINT [PK_QuickFilterNodeUpdateItem] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateOrder]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateOrder]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateOrder]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateOrder] on [dbo].[QuickFilterNodeUpdateOrder]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateOrder' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateOrder]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateOrder] ADD CONSTRAINT [PK_QuickFilterNodeUpdateOrder] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdatePending]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdatePending]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdatePending]
(
[FilterNodeContentID] [bigint] NOT NULL,
[FilterTarget] [int] NOT NULL,
[ColumnMask] [varbinary] (100) NOT NULL,
[JoinMask] [int] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateShipment]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateShipment]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateShipment]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateShipment] on [dbo].[QuickFilterNodeUpdateShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateShipment' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateShipment]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateShipment] ADD CONSTRAINT [PK_QuickFilterNodeUpdateShipment] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating index [IX_FilterNodeContent_Status] on [dbo].[FilterNodeContent]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNodeContent_Status' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContent]'))
CREATE NONCLUSTERED INDEX [IX_FilterNodeContent_Status] ON [dbo].[FilterNodeContent] ([Status]) INCLUDE ([ColumnMask], [Cost], [FilterNodeContentID], [JoinMask], [UpdateCalculation])
GO