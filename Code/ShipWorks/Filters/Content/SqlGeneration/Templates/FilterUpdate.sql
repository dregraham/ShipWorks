DECLARE @started DATETIME
SET @started = GETDATE()

IF OBJECT_ID('tempdb.dbo.#Matches') IS NOT NULL
  DROP TABLE #Matches

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

SELECT {0}.{1} as ObjectID INTO #Matches
   FROM {2} INNER JOIN #DirtyObjects d ON {0}.{1} = d.ObjectID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0
   {3}
   
CREATE INDEX idx_Matches ON #Matches (ObjectID);

SET XACT_ABORT ON;
   
BEGIN TRAN;
   
DELETE FilterNodeContentDetail
     OUTPUT deleted.ObjectID INTO #Removed
  WHERE FilterNodeContentID = @filterNodeContentID
    AND
		ObjectID IN 
		(
			SELECT ObjectID FROM #DirtyObjects WHERE dbo.BitwiseAnd(ColumnsUpdated, @columnMask) != 0x0
              EXCEPT
            SELECT ObjectID FROM #Matches
		) 

SET @deleteCount = @@rowCount;
   
INSERT INTO ActionQueue(ActionID, ActionName, TriggerComputerID, RunComputerID, ObjectID, Status, NextStep)
   SELECT t.ActionID, '', TopComputer.ComputerID, CASE t.ComputerLimited WHEN 0 THEN NULL ELSE TopComputer.ComputerID END, r.ObjectID, 0, 0
   FROM ActionFilterTrigger t, #Removed r CROSS APPLY (SELECT MAX(ComputerID) as ComputerID  FROM #DirtyObjects WHERE r.ObjectID = ObjectID) AS TopComputer
   WHERE t.FilterNodeID = @filterNodeID AND
         t.Direction = 0

INSERT INTO FilterNodeContentDetail (FilterNodeContentID, ObjectID)
		OUTPUT inserted.ObjectID INTO #Added
	SELECT @filterNodeContentID, m.ObjectID
	   FROM #Matches m

SET @insertCount = @@rowcount

INSERT INTO ActionQueue(ActionID, ActionName, TriggerComputerID, RunComputerID, ObjectID, Status, NextStep)
   SELECT t.ActionID, '', TopComputer.ComputerID, CASE t.ComputerLimited WHEN 0 THEN NULL ELSE TopComputer.ComputerID END, a.ObjectID, 0, 0
   FROM ActionFilterTrigger t, #Added a CROSS APPLY (SELECT MAX(ComputerID) as ComputerID  FROM #DirtyObjects WHERE a.ObjectID = ObjectID) AS TopComputer
   WHERE t.FilterNodeID = @filterNodeID AND
         t.Direction = 1

DECLARE @countChanged int
SET @countChanged = 0

IF (@deleteCount > 0 OR @insertCount > 0)
BEGIN
    SET @countChanged = 1
END

UPDATE FilterNodeContent
  SET [Status] = {4},
      [Cost] = DATEDIFF(millisecond, @started, GETDATE()),
      [Count] = [Count] + (@insertCount - @deleteCount),
      [CountVersion] = [CountVersion] + @countChanged
  WHERE FilterNodeContentID = @filterNodeContentID
  
COMMIT;

SET XACT_ABORT OFF;
