DECLARE @filterNodeCountID bigint
DECLARE @param1 money

SET @filterNodeCountID = 10
SET @param1 = 1010000.00

INSERT INTO FilterNodeCountDetail (FilterNodeCountID, ObjectID)
	SELECT @filterNodeCountID, o.OrderID
	FROM [Order] o
	   WHERE 
		  (
			 (o.OrderTotal > @param1)
		  )

UPDATE FilterNodeCount
  SET [Status] = 1,
      [StatusDate] = GETUTCDATE(),
      [Count] = @@rowcount
  WHERE FilterNodeCountID = @filterNodeCountID