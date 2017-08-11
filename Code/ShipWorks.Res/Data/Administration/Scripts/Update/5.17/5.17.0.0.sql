
PRINT N'Creating table to [dbo].[EtsyOrderItem]'
GO
CREATE TABLE [dbo].[EtsyOrderItem](
	[OrderItemID] [bigint] NOT NULL,
	[TransactionID] [int] NOT NULL,
	[ListingID] [int] NOT NULL
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
SELECT [OrderItemID], [Code]=(CASE
when ISNUMERIC([Code]) <> 0 then [Code]
when ISNUMERIC([Code]) <> 1 then '' 
END),  
[SKU]= (CASE
when ISNUMERIC([SKU]) <> 0 then [SKU]
when ISNUMERIC([SKU]) <> 1 then '' 
END)
FROM [dbo].[OrderItem] 
INNER JOIN [Order] ON [Order].[OrderID] = [OrderItem].[OrderID]
INNER JOIN [Store] ON [Store].[StoreID] = [Order].[StoreID]
WHERE [TypeCode] = 33
AND [OrderItemID] NOT IN (SELECT [OrderItemID] FROM [EtsyOrderItem])
GO