-- OrderItem
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_OrderItem_OrderID] from [dbo].[OrderItem]'
GO
DROP INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem]
GO
PRINT N'Creating index [IX_OrderItem_OrderID] on [dbo].[OrderItem]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem] ([OrderID] ASC, [OrderItemID] ASC)
GO