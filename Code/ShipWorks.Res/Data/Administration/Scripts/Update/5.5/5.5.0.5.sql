PRINT N'Dropping foreign keys from [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] DROP CONSTRAINT [FK_FilterNodeContentDetail_FilterNodeContent]
GO
PRINT N'Dropping index [IX_FilterNodeCountDetail] from [dbo].[FilterNodeContentDetail]'
GO
DROP INDEX [IX_FilterNodeCountDetail] ON [dbo].[FilterNodeContentDetail]
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
PRINT N'Creating primary key [PK_FilterNodeContentDetail] on [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] ADD CONSTRAINT [PK_FilterNodeContentDetail] PRIMARY KEY CLUSTERED  ([FilterNodeContentID], [ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateCustomer] on [dbo].[FilterNodeUpdateCustomer]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCustomer] ADD CONSTRAINT [PK_FilterNodeUpdateCustomer] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateItem] on [dbo].[FilterNodeUpdateItem]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateItem] ADD CONSTRAINT [PK_FilterNodeUpdateItem] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateOrder] on [dbo].[FilterNodeUpdateOrder]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateOrder] ADD CONSTRAINT [PK_FilterNodeUpdateOrder] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateShipment] on [dbo].[FilterNodeUpdateShipment]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateShipment] ADD CONSTRAINT [PK_FilterNodeUpdateShipment] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Adding foreign keys to [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] ADD CONSTRAINT [FK_FilterNodeContentDetail_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID]) ON DELETE CASCADE
GO