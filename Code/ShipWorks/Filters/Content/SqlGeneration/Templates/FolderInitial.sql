DECLARE @started DATETIME
SET @started = GETDATE()

INSERT INTO FilterNodeContentDetail (FilterNodeContentID, ObjectID)
  SELECT DISTINCT @filterNodeContentID, ObjectID
  FROM FilterNodeContentDetail
  WHERE FilterNodeContentID IN ({0})

UPDATE FilterNodeContent
  SET [Status] = {1},
      [Cost] = DATEDIFF(millisecond, @started, GETDATE()),
      [Count] = @@rowcount,
      [CountVersion] = 1
  WHERE FilterNodeContentID = @filterNodeContentID
