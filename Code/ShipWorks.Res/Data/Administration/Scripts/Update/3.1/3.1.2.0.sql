SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
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
PRINT N'Altering [dbo].[FilterNodeContent]'
GO
ALTER TABLE [dbo].[FilterNodeContent] ALTER COLUMN [ColumnMask] [varbinary] (75) NOT NULL
GO
PRINT N'Altering [dbo].[FilterNodeContentDirty]'
GO
ALTER TABLE [dbo].[FilterNodeContentDirty] ALTER COLUMN [ColumnsUpdated] [varbinary] (75) NOT NULL
GO
PRINT N'Altering [dbo].[FilterNodeUpdateCustomer]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCustomer] ALTER COLUMN [ColumnsUpdated] [varbinary] (75) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateCustomer] on [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Altering [dbo].[FilterNodeUpdateItem]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateItem] ALTER COLUMN [ColumnsUpdated] [varbinary] (75) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateItem] on [dbo].[FilterNodeUpdateItem]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Altering [dbo].[FilterNodeUpdateOrder]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateOrder] ALTER COLUMN [ColumnsUpdated] [varbinary] (75) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateOrder] on [dbo].[FilterNodeUpdateOrder]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Altering [dbo].[FilterNodeUpdatePending]'
GO
ALTER TABLE [dbo].[FilterNodeUpdatePending] ALTER COLUMN [ColumnMask] [varbinary] (75) NOT NULL
GO
PRINT N'Altering [dbo].[FilterNodeUpdateShipment]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateShipment] ALTER COLUMN [ColumnsUpdated] [varbinary] (75) NOT NULL
GO
PRINT N'Creating index [IX_FilterNodeUpdateShipment] on [dbo].[FilterNodeUpdateShipment]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment] ([ObjectID], [ColumnsUpdated]) INCLUDE ([ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO
