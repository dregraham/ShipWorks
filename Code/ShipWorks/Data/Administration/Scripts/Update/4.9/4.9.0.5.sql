-- OrderItemAttribute
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_OrderItemAttribute_OrderItemID] from [dbo].[OrderItemAttribute]'
GO
DROP INDEX [IX_OrderItemAttribute_OrderItemID] ON [dbo].[OrderItemAttribute]
GO
PRINT N'Creating index [IX_OrderItemAttribute_OrderItemID] on [dbo].[OrderItemAttribute]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrderItemAttribute_OrderItemID] ON [dbo].[OrderItemAttribute] ([OrderItemID] ASC,[OrderItemAttributeID] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO