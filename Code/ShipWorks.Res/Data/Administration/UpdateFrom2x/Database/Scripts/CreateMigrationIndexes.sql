CREATE NONCLUSTERED INDEX [v2m_IX_ArchiveLogs]
ON [dbo].[ArchiveLogs] ([Type],[TargetKey])
GO

CREATE NONCLUSTERED INDEX [v2m_IX_MigrationKeys]
ON [dbo].[v2m_MigrationKeys] ([KeyTypeCode])
INCLUDE ([OriginalKey],[NewKey])
GO

CREATE NONCLUSTERED INDEX [v2m_IX_UpsPackages]
ON [dbo].[v2m_UpsPackages] ([ShipmentID])
GO

CREATE NONCLUSTERED INDEX [v2m_IX_FedexPackages]
ON [dbo].[v2m_FedexPackages] ([ShipmentID])
GO

IF OBJECT_ID(N'Order') IS NOT NULL 
CREATE NONCLUSTERED INDEX [v2m_IX_Orders]
ON [dbo].[Order] (StoreID, OrderNumber)
INCLUDE ([OrderID])
GO

IF OBJECT_ID(N'Order') IS NOT NULL 
CREATE NONCLUSTERED INDEX [v2m_IX_Orders2]
ON [dbo].[Order] ([StoreID],[IsManual],[OrderNumberComplete])
INCLUDE ([OrderNumber])
GO

IF OBJECT_ID(N'OrderItem') IS NOT NULL
CREATE NONCLUSTERED INDEX [v2m_IX_OrderItem]
ON [dbo].[OrderItem] ([OrderID])
GO

IF OBJECT_ID(N'[EbayOrderItem]') IS NOT NULL
CREATE NONCLUSTERED INDEX [v2m_IX_EbayOrderItem_OrderID]
ON [dbo].[EbayOrderItem] ([OrderID])
GO

IF OBJECT_ID(N'[Order]') IS NOT NULL
CREATE NONCLUSTERED INDEX [v2m_IX_Order_CustomerID]
ON [dbo].[Order] ([CustomerID])
GO

IF OBJECT_ID(N'[Order]') IS NOT NULL
CREATE NONCLUSTERED INDEX [v2m_IX_Order_RollupItemCount] 
ON dbo.[Order] (RollupItemCount)
GO

IF OBJECT_ID(N'[EbayOrder]') IS NOT NULL
CREATE NONCLUSTERED INDEX [v2m_IX_EbayOrder_RollupItemCount] 
ON dbo.[EbayOrder] (RollupEbayItemCount)
GO

IF OBJECT_ID(N'[Customer]') IS NOT NULL
CREATE NONCLUSTERED INDEX [v2m_IX_Customer_RollupOrderCount] 
ON dbo.[Customer] (RollupOrderCount)
GO

IF OBJECT_ID(N'[Note]') IS NOT NULL
CREATE NONCLUSTERED INDEX [v2m_IX_Note_ObjectID] 
ON dbo.[Note] (ObjectID)
GO