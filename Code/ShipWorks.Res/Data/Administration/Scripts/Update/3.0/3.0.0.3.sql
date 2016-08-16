SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping constraints from [dbo].[SystemData]'
GO
ALTER TABLE [dbo].[SystemData] DROP CONSTRAINT [PK_SystemData]
GO
PRINT N'Dropping index [IX_Auto_RollupItemName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemSKU] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order]
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [RollupItemName] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[Order] ALTER COLUMN [RollupItemCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[Order] ALTER COLUMN [RollupItemSKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Creating index [IX_Auto_RollupItemName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order] ([RollupItemCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemSKU] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating [dbo].[FilterNodeUpdateCheckpoint]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateCheckpoint]
(
[CheckpointID] [bigint] NOT NULL IDENTITY(1070, 1000),
[RowVersion] [binary] (8) NOT NULL,
[DirtyCount] [int] NOT NULL,
[State] [int] NOT NULL,
[Duration] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateCheckpoint] on [dbo].[FilterNodeUpdateCheckpoint]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCheckpoint] ADD CONSTRAINT [PK_FilterNodeUpdateCheckpoint] PRIMARY KEY CLUSTERED  ([CheckpointID])
GO
PRINT N'Creating [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateCustomer]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateCustomer] on [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateItem]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateItem]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateItem] on [dbo].[FilterNodeUpdateItem]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateOrder]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateOrder]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateOrder] on [dbo].[FilterNodeUpdateOrder]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdatePending]'
GO
CREATE TABLE [dbo].[FilterNodeUpdatePending]
(
[FilterNodeContentID] [bigint] NOT NULL,
[FilterTarget] [int] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateShipment]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateShipment]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateShipment] on [dbo].[FilterNodeUpdateShipment]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Rebuilding [dbo].[SystemData]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_SystemData]
(
[SystemDataID] [bit] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[DatabaseID] [uniqueidentifier] NOT NULL,
[DateFiltersLastUpdate] [datetime] NOT NULL,
[TemplateVersion] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FilterSqlVersion] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_SystemData]([SystemDataID], [DatabaseID], [DateFiltersLastUpdate], [TemplateVersion], [FilterSqlVersion]) SELECT [SystemDataID], [DatabaseID], [DateFiltersLastUpdate], [TemplateVersion], 1 FROM [dbo].[SystemData]
GO
DROP TABLE [dbo].[SystemData]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_SystemData]', N'SystemData'
GO
PRINT N'Creating primary key [PK_SystemData] on [dbo].[SystemData]'
GO
ALTER TABLE [dbo].[SystemData] ADD CONSTRAINT [PK_SystemData] PRIMARY KEY CLUSTERED  ([SystemDataID])
GO
