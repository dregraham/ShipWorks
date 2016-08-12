
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

/*  Modify OrderItem table indexes  */
PRINT N'Creating index [IX_OrderItem_Code] on [dbo].[OrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OrderItem_Code' AND object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
CREATE NONCLUSTERED INDEX [IX_OrderItem_Code] ON [dbo].[OrderItem] ([Code]) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_OrderItem_Name] on [dbo].[OrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OrderItem_Name' AND object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
CREATE NONCLUSTERED INDEX [IX_OrderItem_Name] ON [dbo].[OrderItem] ([Name]) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_OrderItem_Quantity] on [dbo].[OrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OrderItem_Quantity' AND object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
CREATE NONCLUSTERED INDEX [IX_OrderItem_Quantity] ON [dbo].[OrderItem] ([Quantity]) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_OrderItem_Sku] on [dbo].[OrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OrderItem_Sku' AND object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
CREATE NONCLUSTERED INDEX [IX_OrderItem_Sku] ON [dbo].[OrderItem] ([SKU]) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_OrderItem_Weight] on [dbo].[OrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OrderItem_Weight' AND object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
CREATE NONCLUSTERED INDEX [IX_OrderItem_Weight] ON [dbo].[OrderItem] ([Weight]) INCLUDE ([OrderID])
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