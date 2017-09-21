PRINT N'Creating table to [dbo].[EtsyOrderItem]' 
GO 
CREATE TABLE [dbo].[EtsyOrderItem]( 
     [OrderItemID] [bigint] NOT NULL, 
     [TransactionID] [bigint] NOT NULL, 
     [ListingID] [bigint] NOT NULL 
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
     SELECT [OrderItemID],  
             (CASE 
                         when ISNUMERIC([Code]) <> 0 and CONVERT(FLOAT, [Code]) BETWEEN -2147483648 AND 2147483647  then convert(bigint, [Code]) 
                         else ''  
                    END),   
             (CASE 
                         when ISNUMERIC([SKU]) <> 0 and CONVERT(FLOAT, [Code]) BETWEEN -2147483648 AND 2147483647  then convert(bigint, [SKU]) 
                         else ''  
                    END) 
     FROM [dbo].[OrderItem]  
     INNER JOIN [Order] ON [Order].[OrderID] = [OrderItem].[OrderID] 
     INNER JOIN [Store] ON [Store].[StoreID] = [Order].[StoreID] 
     WHERE [TypeCode] = 33 
     AND [OrderItemID] NOT IN (SELECT [OrderItemID] FROM [EtsyOrderItem]) 
GO 
 
ALTER TABLE [EtsyOrderItem] ALTER COLUMN [TransactionID] [int] NOT NULL 
GO 
ALTER TABLE [EtsyOrderItem] ALTER COLUMN [ListingID] [int] NOT NULL 
GO