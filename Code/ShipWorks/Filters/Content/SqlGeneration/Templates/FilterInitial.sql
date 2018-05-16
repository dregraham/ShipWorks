if EXISTS
(
	SELECT * FROM FilterNode n
	INNER JOIN FilterSequence s
	ON n.FilterSequenceID = s.FilterSequenceID
	INNER JOIN Filter f
	ON s.FilterID = f.FilterID
	WHERE FilterNodeID = @filterNodeID and f.[State] = 0
)
BEGIN
	UPDATE FilterNodeContent
	  SET [Status] = 1,
		  [Cost] = 0,
		  [Count] = 0,
		  [CountVersion] = 1
	  WHERE FilterNodeContentID = @filterNodeContentID
	  
	DELETE FilterNodeContentDetail WHERE FilterNodeContentID = @filterNodeContentID
END
ELSE
BEGIN
	DECLARE @started DATETIME
	SET @started = GETDATE()

	DELETE FilterNodeContentDetail WHERE FilterNodeContentID = @filterNodeContentID
		
	;WITH TmpMatches as
	(
		SELECT {0}.{1} as ObjectID
		   FROM {2}
		   {3}
    )

	INSERT INTO FilterNodeContentDetail (FilterNodeContentID, ObjectID)
		SELECT @filterNodeContentID, ObjectID
		FROM TmpMatches
	   
	UPDATE FilterNodeContent
	  SET [Status] = {4},
		  [Cost] = DATEDIFF(millisecond, @started, GETDATE()),
		  [Count] = @@rowcount,
		  [CountVersion] = 1
	  WHERE FilterNodeContentID = @filterNodeContentID
END