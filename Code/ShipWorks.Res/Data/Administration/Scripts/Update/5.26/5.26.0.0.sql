PRINT N'Updating [dbo].[Order] indexes'
GO
IF EXISTS(SELECT * FROM sys.indexes
			WHERE [name] = 'IX_Auto_StoreID'
				AND [object_id] = OBJECT_ID('Order'))
	DROP INDEX [IX_Auto_StoreID] ON [dbo].[Order];
GO

IF NOT EXISTS(SELECT * FROM sys.indexes
			WHERE [name] = 'IX_Order_StoreIDIsManual'
				AND [object_id] = OBJECT_ID('Order'))
	CREATE NONCLUSTERED INDEX [IX_Order_StoreIDIsManual] ON [dbo].[Order] ([StoreID] ASC, [IsManual] ASC) INCLUDE ([OrderDate], [OrderNumber])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes
			WHERE [name] = 'IX_OrderSearch_StoreIDIsManual'
				AND [object_id] = OBJECT_ID('OrderSearch'))
	CREATE NONCLUSTERED INDEX [IX_OrderSearch_StoreIDIsManual] ON [dbo].[OrderSearch] ([StoreID] ASC, [IsManual] ASC) INCLUDE ([OrderNumber])
GO
