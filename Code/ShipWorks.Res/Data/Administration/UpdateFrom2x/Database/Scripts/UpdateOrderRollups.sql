UPDATE TOP(1000) dbo.[Order] 
SET RollupItemCount = (SELECT COUNT(*) FROM dbo.OrderItem WHERE dbo.OrderItem.OrderID = dbo.[Order].OrderID),
    RollupItemName = (SELECT CASE COUNT(*) WHEN 1 THEN MAX(Name) ELSE NULL END FROM dbo.OrderItem WHERE dbo.OrderItem.OrderID = dbo.[Order].OrderID),
	RollupItemCode = (SELECT CASE COUNT(*) WHEN 1 THEN MAX(Code) ELSE NULL END FROM dbo.OrderItem WHERE dbo.OrderItem.OrderID = dbo.[Order].OrderID),
	RollupItemSKU = (SELECT CASE COUNT(*) WHEN 1 THEN MAX(SKU) ELSE NULL END FROM dbo.OrderItem WHERE dbo.OrderItem.OrderID = dbo.[Order].OrderID),
	RollupItemLocation = (SELECT CASE COUNT(*) WHEN 1 THEN MAX(Location) ELSE NULL END FROM dbo.OrderItem WHERE dbo.OrderItem.OrderID = dbo.[Order].OrderID),
	RollupItemQuantity = (SELECT CASE COUNT(*) WHEN 1 THEN SUM(Quantity) ELSE NULL END FROM dbo.OrderItem WHERE dbo.OrderItem.OrderID = dbo.[Order].OrderID),
	RollupItemTotalWeight = (SELECT COALESCE(SUM(Weight), 0) FROM dbo.OrderItem WHERE dbo.OrderItem.OrderID = dbo.[Order].OrderID),
	RollupNoteCount = (SELECT COUNT(*) FROM dbo.Note WHERE ObjectID = dbo.[Order].OrderID OR ObjectID = dbo.[Order].CustomerID)
WHERE RollupItemCount IS NULL

SELECT @@ROWCOUNT as WorkCompleted