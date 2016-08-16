SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping constraints from [dbo].[FilterNodeUpdateCheckpoint]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCheckpoint] DROP CONSTRAINT [PK_FilterNodeUpdateCheckpoint]
GO
PRINT N'Dropping foreign keys from [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] DROP
CONSTRAINT [FK_FilterNode_FilterNodeContent]
GO
PRINT N'Dropping foreign keys from [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] DROP
CONSTRAINT [FK_FilterNodeContentDetail_FilterNodeContent]
GO
PRINT N'Dropping constraints from [dbo].[FilterNodeContent]'
GO
ALTER TABLE [dbo].[FilterNodeContent] DROP CONSTRAINT [PK_FilterNodeContent]
GO
PRINT N'Dropping index [IX_FilterNodeCountDirty] from [dbo].[FilterNodeContentDirty]'
GO
DROP INDEX [IX_FilterNodeCountDirty] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateCustomer] from [dbo].[FilterNodeUpdateCustomer]'
GO
DROP INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateItem] from [dbo].[FilterNodeUpdateItem]'
GO
DROP INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateOrder] from [dbo].[FilterNodeUpdateOrder]'
GO
DROP INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder]
GO
PRINT N'Dropping index [IX_FilterNodeUpdateShipment] from [dbo].[FilterNodeUpdateShipment]'
GO
DROP INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment]
GO
PRINT N'Rebuilding [dbo].[FilterNodeContent]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeContent]
(
[FilterNodeContentID] [bigint] NOT NULL IDENTITY(1014, 1000),
[RowVersion] [timestamp] NOT NULL,
[CountVersion] [bigint] NOT NULL,
[Status] [smallint] NOT NULL,
[InitialCalculation] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UpdateCalculation] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ColumnMask] [varbinary] (50) NOT NULL,
[JoinMask] [int] NOT NULL,
[Cost] [int] NOT NULL,
[Count] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FilterNodeContent] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_FilterNodeContent]([FilterNodeContentID], [CountVersion], [Status], [InitialCalculation], [UpdateCalculation], [ColumnMask], [JoinMask], [Cost], [Count]) SELECT [FilterNodeContentID], [CountVersion], [Status], [InitialCalculation], [UpdateCalculation], 0x00, 0, [Cost], [Count] FROM [dbo].[FilterNodeContent]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FilterNodeContent] OFF
GO
DECLARE @idVal INT
SELECT @idVal = IDENT_CURRENT(N'FilterNodeContent')
DBCC CHECKIDENT(tmp_rg_xx_FilterNodeContent, RESEED, @idVal)
GO
DROP TABLE [dbo].[FilterNodeContent]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeContent]', N'FilterNodeContent'
GO
PRINT N'Creating primary key [PK_FilterNodeContent] on [dbo].[FilterNodeContent]'
GO
ALTER TABLE [dbo].[FilterNodeContent] ADD CONSTRAINT [PK_FilterNodeContent] PRIMARY KEY CLUSTERED  ([FilterNodeContentID])
GO
PRINT N'Rebuilding [dbo].[FilterNodeContentDirty]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeContentDirty]
(
[FilterNodeContentDirtyID] [bigint] NOT NULL IDENTITY(1, 1),
[RowVersion] [timestamp] NOT NULL,
[ObjectID] [bigint] NOT NULL,
[ParentID] [bigint] NULL,
[ObjectType] [int] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (50) NOT NULL
)
GO
DROP TABLE [dbo].[FilterNodeContentDirty]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeContentDirty]', N'FilterNodeContentDirty'
GO
PRINT N'Creating primary key [PK_FilterNodeContentDirty] on [dbo].[FilterNodeContentDirty]'
GO
ALTER TABLE [dbo].[FilterNodeContentDirty] ADD CONSTRAINT [PK_FilterNodeContentDirty] PRIMARY KEY CLUSTERED  ([FilterNodeContentDirtyID])
GO
PRINT N'Creating index [IX_FilterNodeCountDirty] on [dbo].[FilterNodeContentDirty]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeCountDirty] ON [dbo].[FilterNodeContentDirty] ([RowVersion])
GO
PRINT N'Rebuilding [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeUpdateCustomer]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (50) NOT NULL
)
GO
DROP TABLE [dbo].[FilterNodeUpdateCustomer]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeUpdateCustomer]', N'FilterNodeUpdateCustomer'
GO
PRINT N'Creating index [IX_FilterNodeUpdateCustomer] on [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Rebuilding [dbo].[FilterNodeUpdateItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeUpdateItem]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (50) NOT NULL
)
GO
DROP TABLE [dbo].[FilterNodeUpdateItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeUpdateItem]', N'FilterNodeUpdateItem'
GO
PRINT N'Creating index [IX_FilterNodeUpdateItem] on [dbo].[FilterNodeUpdateItem]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Rebuilding [dbo].[FilterNodeUpdateOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeUpdateOrder]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (50) NOT NULL
)
GO
DROP TABLE [dbo].[FilterNodeUpdateOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeUpdateOrder]', N'FilterNodeUpdateOrder'
GO
PRINT N'Creating index [IX_FilterNodeUpdateOrder] on [dbo].[FilterNodeUpdateOrder]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Rebuilding [dbo].[FilterNodeUpdatePending]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeUpdatePending]
(
[FilterNodeContentID] [bigint] NOT NULL,
[FilterTarget] [int] NOT NULL,
[ColumnMask] [varbinary] (50) NOT NULL,
[JoinMask] [int] NOT NULL,
[Position] [int] NOT NULL
)
GO
DROP TABLE [dbo].[FilterNodeUpdatePending]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeUpdatePending]', N'FilterNodeUpdatePending'
GO
PRINT N'Rebuilding [dbo].[FilterNodeUpdateShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeUpdateShipment]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (50) NOT NULL
)
GO
DROP TABLE [dbo].[FilterNodeUpdateShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeUpdateShipment]', N'FilterNodeUpdateShipment'
GO
PRINT N'Creating index [IX_FilterNodeUpdateShipment] on [dbo].[FilterNodeUpdateShipment]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Adding foreign keys to [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] ADD
CONSTRAINT [FK_FilterNode_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] ADD
CONSTRAINT [FK_FilterNodeContentDetail_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID]) ON DELETE CASCADE
GO
PRINT N'Altering [dbo].[SystemData]'
GO
ALTER TABLE [dbo].[SystemData] DROP
COLUMN [FilterSqlVersion]
GO
PRINT N'Rebuilding [dbo].[FilterNodeUpdateCheckpoint]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNodeUpdateCheckpoint]
(
[CheckpointID] [bigint] NOT NULL IDENTITY(1070, 1000),
[MaxDirtyID] [bigint] NOT NULL,
[DirtyCount] [int] NOT NULL,
[State] [int] NOT NULL,
[Duration] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FilterNodeUpdateCheckpoint] ON
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FilterNodeUpdateCheckpoint] OFF
GO
DECLARE @idVal INT
SELECT @idVal = IDENT_CURRENT(N'FilterNodeUpdateCheckpoint')
DBCC CHECKIDENT(tmp_rg_xx_FilterNodeUpdateCheckpoint, RESEED, @idVal)
GO
DROP TABLE [dbo].[FilterNodeUpdateCheckpoint]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNodeUpdateCheckpoint]', N'FilterNodeUpdateCheckpoint'
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateCheckpoint] on [dbo].[FilterNodeUpdateCheckpoint]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCheckpoint] ADD CONSTRAINT [PK_FilterNodeUpdateCheckpoint] PRIMARY KEY CLUSTERED  ([CheckpointID])
GO


PRINT N'Updating SuperUser UserName'
GO
UPDATE [User] SET Username = '_@system' WHERE UserID = 1027309002
GO