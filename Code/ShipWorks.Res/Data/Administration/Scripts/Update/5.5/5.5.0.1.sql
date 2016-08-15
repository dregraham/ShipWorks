PRINT N'Dropping index [IX_Auto_OrderDate] from [dbo].[Order]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Auto_OrderDate' AND object_id = OBJECT_ID(N'[dbo].[Order]'))
DROP INDEX [IX_Auto_OrderDate] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemName] from [dbo].[Order]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Auto_RollupItemName' AND object_id = OBJECT_ID(N'[dbo].[Order]'))
DROP INDEX [IX_Auto_RollupItemName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_StoreID] from [dbo].[Order]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Auto_StoreID' AND object_id = OBJECT_ID(N'[dbo].[Order]'))
DROP INDEX [IX_Auto_StoreID] ON [dbo].[Order]
GO

/*  Modify Order table indexes  */
PRINT N'Creating index [IX_Auto_OrderDate] on [dbo].[Order]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Auto_OrderDate' AND object_id = OBJECT_ID(N'[dbo].[Order]'))
CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate] ON [dbo].[Order] ([OrderDate]) INCLUDE ([IsManual])
GO
PRINT N'Creating index [IX_Auto_RollupItemName] on [dbo].[Order]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Auto_RollupItemName' AND object_id = OBJECT_ID(N'[dbo].[Order]'))
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName], [OrderID])
GO
PRINT N'Creating index [IX_Auto_StoreID] on [dbo].[Order]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Auto_StoreID' AND object_id = OBJECT_ID(N'[dbo].[Order]'))
CREATE NONCLUSTERED INDEX [IX_Auto_StoreID] ON [dbo].[Order] ([StoreID]) INCLUDE ([IsManual], [OrderDate])
GO
PRINT N'Creating index [IX_Order_StoreIDOrderDateLocalStatus] on [dbo].[Order]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Order_StoreIDOrderDateLocalStatus' AND object_id = OBJECT_ID(N'[dbo].[Order]'))
CREATE NONCLUSTERED INDEX [IX_Order_StoreIDOrderDateLocalStatus] ON [dbo].[Order] ([StoreID], [OrderDate], [LocalStatus])
GO