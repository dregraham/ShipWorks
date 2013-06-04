DECLARE @filterNodeCountID bigint
SET @filterNodeCountID = 13014

INSERT INTO FilterNodeCountDetail (FilterNodeCountID, ObjectID)
  SELECT DISTINCT @filterNodeCountID, ObjectID
  FROM FilterNodeCountDetail
  WHERE FilterNodeCountID IN (16014)

UPDATE FilterNodeCount
  SET [Status] = 1,
      [StatusDate] = GETUTCDATE(),
      [Count] = @@rowcount
  WHERE FilterNodeCountID = @filterNodeCountID

-----------------------------------------------

-- For this to work, we have to garuntee that updates get done from the bottom of the tree up
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
		  SELECT ObjectID
			  FROM FilterNodeCountDetail
			  WHERE FilterNodeCountID IN (16014)
		 )

DECLARE @countChange int
SET @countChange = -@@rowCount;

INSERT INTO FilterNodeCountDetail (FilterNodeCountID, ObjectID)
	  SELECT DISTINCT @filterNodeCountID, cd.ObjectID
	  FROM FilterNodeCountDetail cd
	  WHERE cd.FilterNodeCountID IN (16014)
          AND cd.ObjectID IN (SELECT ObjectID FROM #DirtyObjects)
          AND NOT EXISTS (SELECT * FROM FilterNodeCountDetail WHERE ObjectID = cd.ObjectID AND FilterNodeCountID = @filterNodeCountID)

SET @countChange = @countChange + @@rowcount

UPDATE FilterNodeCount
  SET [Status] = 1,
      [StatusDate] = GETUTCDATE(),
      [Count] = [Count] + @countChange
  WHERE FilterNodeCountID = @filterNodeCountID
