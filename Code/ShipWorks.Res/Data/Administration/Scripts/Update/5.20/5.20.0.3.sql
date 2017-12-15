PRINT N'Altering [dbo].[OrderItem].[[IX_OrderItem_Code_OrderId]]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_Code_OrderId] ON [dbo].[OrderItem] ([Code], [OrderID])
GO
