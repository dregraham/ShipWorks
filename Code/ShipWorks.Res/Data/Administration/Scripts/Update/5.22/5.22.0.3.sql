PRINT N'Creating GenericModuleOrder rows that correspond to GenericModuleStore orders'
GO
INSERT INTO GenericModuleOrder (OrderID, AmazonOrderID, IsFBA, IsPrime, IsSameDay)
SELECT MagentoOrder.OrderId,'',0,2,0
FROM MagentoOrder
WHERE OrderID NOT IN (SELECT OrderID FROM GenericModuleOrder)
GO

PRINT N'Creating MagentoOrder foreign key'
GO
ALTER TABLE [dbo].[MagentoOrder]  WITH CHECK ADD CONSTRAINT [FK_MagentoOrder_GenericModuleOrder] FOREIGN KEY([OrderID])
REFERENCES [dbo].[GenericModuleOrder] ([OrderID])
GO
ALTER TABLE [dbo].[MagentoOrder] CHECK CONSTRAINT [FK_MagentoOrder_GenericModuleOrder]
GO
ALTER TABLE [dbo].[MagentoOrder] DROP CONSTRAINT [FK_MagentoOrder_Order]
GO
