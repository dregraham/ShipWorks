DECLARE @started DATETIME
SET @started = GETDATE()

DECLARE @isArchive BIT;
IF (EXISTS(SELECT * FROM [Configuration] WHERE CONVERT(NVARCHAR(MAX), ArchivalSettingsXml) = '<ArchivalSettings/>'))
BEGIN
	SET @isArchive = 0
END
ELSE
BEGIN
	SET @isArchive = 1
END

IF OBJECT_ID('tempdb.dbo.#Removed') IS NOT NULL
  DROP TABLE #Removed

IF OBJECT_ID('tempdb.dbo.#Added') IS NOT NULL
  DROP TABLE #Added

DECLARE @deleteCount int
DECLARE @insertCount int

SET @deleteCount = 0
SET @insertCount = 0

CREATE TABLE #Removed
(
	ObjectID bigint NOT NULL
);

CREATE TABLE #Added
(
	ObjectID bigint NOT NULL
);

SET XACT_ABORT ON;

BEGIN TRAN;

DELETE FilterNodeContentDetail
     OUTPUT deleted.ObjectID INTO #Removed
  WHERE FilterNodeContentID = @filterNodeContentID
    AND
		ObjectID IN 
		(
			SELECT ObjectID FROM #DirtyObjects
		) 
	AND
	    ObjectID NOT IN
		(
		  SELECT ObjectID
			  FROM FilterNodeContentDetail
			  WHERE FilterNodeContentID IN ({0})
		 )

SET @deleteCount = @@rowCount;
   
IF (@isArchive = 0)
BEGIN
	INSERT INTO ActionQueue(ActionID, ActionName, TriggerComputerID, ComputerLimitedList, ObjectID, Status, NextStep)
	   SELECT t.ActionID, '', TopComputer.ComputerID, CASE t.ComputerLimitedType WHEN 0 THEN '' When 1 Then CAST(TopComputer.ComputerID as varchar(150)) ELSE t.ComputerLimitedList END, r.ObjectID, 0, 0
	   FROM ActionFilterTrigger t, #Removed r CROSS APPLY (SELECT MAX(ComputerID) as ComputerID FROM #DirtyObjects WHERE r.ObjectID = ObjectID) AS TopComputer
	   WHERE t.FilterNodeID = @filterNodeID AND
			 t.Direction = 0
END

INSERT INTO FilterNodeContentDetail (FilterNodeContentID, ObjectID)
		OUTPUT inserted.ObjectID INTO #Added
	  SELECT @filterNodeContentID, cd.ObjectID
	  FROM FilterNodeContentDetail cd
	  WHERE cd.FilterNodeContentID IN ({0})
          AND cd.ObjectID IN (SELECT ObjectID FROM #DirtyObjects)

SET @insertCount = @@rowcount

IF (@isArchive = 0)
BEGIN
	INSERT INTO ActionQueue(ActionID, ActionName, TriggerComputerID, ComputerLimitedList, ObjectID, Status, NextStep)
	   SELECT t.ActionID, '', TopComputer.ComputerID, CASE t.ComputerLimitedType WHEN 0 THEN '' When 1 Then CAST(TopComputer.ComputerID as varchar(150)) ELSE t.ComputerLimitedList END, a.ObjectID, 0, 0
	   FROM ActionFilterTrigger t, #Added a CROSS APPLY (SELECT MAX(ComputerID) as ComputerID  FROM #DirtyObjects WHERE a.ObjectID = ObjectID) AS TopComputer
	   WHERE t.FilterNodeID = @filterNodeID AND
			 t.Direction = 1
END

DECLARE @countChanged int
SET @countChanged = 0

IF (@deleteCount > 0 OR @insertCount > 0)
BEGIN
    SET @countChanged = 1
END

UPDATE FilterNodeContent
  SET [Status] = {1},
      [Cost] = DATEDIFF(millisecond, @started, GETDATE()),
      [Count] = [Count] + (@insertCount - @deleteCount),
      [CountVersion] = [CountVersion] + @countChanged
  WHERE FilterNodeContentID = @filterNodeContentID
  
COMMIT;

SET XACT_ABORT OFF;
