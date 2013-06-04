DECLARE @started DATETIME
SET @started = GETDATE()

IF OBJECT_ID('tempdb.dbo.#Matches') IS NOT NULL
  DROP TABLE #Matches

SELECT {0}.{1} as ObjectID INTO #Matches
   FROM {2}
   {3}
   
DELETE FilterNodeContentDetail WHERE FilterNodeContentID = @filterNodeContentID

INSERT INTO FilterNodeContentDetail (FilterNodeContentID, ObjectID)
	SELECT @filterNodeContentID, ObjectID
	FROM #Matches
	   
UPDATE FilterNodeContent
  SET [Status] = {4},
      [Cost] = DATEDIFF(millisecond, @started, GETDATE()),
      [Count] = @@rowcount,
      [CountVersion] = 1
  WHERE FilterNodeContentID = @filterNodeContentID
