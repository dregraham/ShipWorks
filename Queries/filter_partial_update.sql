--CREATE TABLE #DirtyObjects (ObjectID bigint NOT NULL)
--DELETE FilterNodeCountDirty OUTPUT deleted.ObjectID INTO #DirtyObjects

DECLARE @filterNodeCountID bigint
DECLARE @countChange int
DECLARE @param1 money
DECLARE @param2 money

SET @filterNodeCountID = 10
SET @param1 = 300.00
SET @param2 = 500.00

DELETE FilterNodeCountDetail
  WHERE FilterNodeCountID = @filterNodeCountID
    AND
		ObjectID IN 
		(
			SELECT ObjectID FROM #DirtyObjects
		) 
	AND
	    ObjectID NOT IN
		(
			SELECT o.OrderID
			   FROM [Order] o INNER JOIN #DirtyObjects d ON o.OrderID = d.ObjectID
			   WHERE 
				  (
					 (o.OrderTotal > @param1 AND o.OrderTotal < @param2)
				  )
		 )

SET @countChange = -@@rowCount;

INSERT INTO FilterNodeCountDetail (FilterNodeCountID, ObjectID)
	SELECT @filterNodeCountID, o.OrderID
	   FROM [Order] o INNER JOIN #DirtyObjects d ON o.OrderID = d.ObjectID
	   WHERE 
		  (
			 (o.OrderTotal > @param1 AND o.OrderTotal < @param2)
		  )
          AND NOT EXISTS (SELECT * FROM FilterNodeCountDetail WHERE ObjectID = o.OrderID AND FilterNodeCountID = @filterNodeCountID)

SET @countChange = @countChange + @@rowcount

UPDATE FilterNodeCount
  SET [Status] = 1,
      [StatusDate] = GETUTCDATE(),
      [Count] = @countChange
  WHERE FilterNodeCountID = @filterNodeCountID