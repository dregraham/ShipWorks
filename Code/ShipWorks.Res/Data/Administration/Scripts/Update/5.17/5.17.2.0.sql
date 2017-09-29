PRINT N'Creating table to [dbo].[EtsyOrderItem]' 
GO 
CREATE TABLE [dbo].[EtsyOrderItem]( 
     [OrderItemID] [bigint] NOT NULL, 
     [TransactionID] [nvarchar] (300) NOT NULL, 
     [ListingID] [nvarchar] (100) NOT NULL 
 CONSTRAINT [PK_EtsyOrderItem] PRIMARY KEY CLUSTERED  
( 
     [OrderItemID] ASC 
) ON [PRIMARY] 
) ON [PRIMARY] 
GO 
 
ALTER TABLE [dbo].[EtsyOrderItem]  WITH CHECK ADD CONSTRAINT [FK_EtsyOrderItem_OrderItem] FOREIGN KEY([OrderItemID]) 
REFERENCES [dbo].[OrderItem] ([OrderItemID]) 
GO 
 
ALTER TABLE [dbo].[EtsyOrderItem] CHECK CONSTRAINT [FK_EtsyOrderItem_OrderItem] 
GO 
 
INSERT INTO [dbo].[EtsyOrderItem]  
            ([OrderItemID], [TransactionID], [ListingID]) 
     SELECT [OrderItemID], [Code], [SKU]
     FROM [dbo].[OrderItem]  
     INNER JOIN [Order] ON [Order].[OrderID] = [OrderItem].[OrderID] 
     INNER JOIN [Store] ON [Store].[StoreID] = [Order].[StoreID] 
     WHERE [TypeCode] = 33 
     AND [OrderItemID] NOT IN (SELECT [OrderItemID] FROM [EtsyOrderItem]) 
GO 
 
ALTER TABLE [EtsyOrderItem] ALTER COLUMN [TransactionID] [nvarchar] (300) NOT NULL 
GO 
ALTER TABLE [EtsyOrderItem] ALTER COLUMN [ListingID] [nvarchar] (100) NOT NULL 
GO