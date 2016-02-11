UPDATE TOP(1000) dbo.[Customer] 
	SET RollupNoteCount = (SELECT COUNT(*) FROM dbo.Note WHERE ObjectID = dbo.[Customer].CustomerID OR ObjectID IN (SELECT o.OrderID FROM [Order] o WHERE o.CustomerID = dbo.[Customer].CustomerID)),
	    RollupOrderCount = (SELECT COUNT(*) FROM dbo.[Order] WHERE dbo.[Order].CustomerID = dbo.[Customer].CustomerID),
	    RollupOrderTotal = (SELECT COALESCE(SUM(OrderTotal), 0) FROM dbo.[Order] WHERE dbo.[Order].CustomerID = dbo.[Customer].CustomerID)
WHERE RollupOrderCount IS NULL

SELECT @@ROWCOUNT as WorkCompleted