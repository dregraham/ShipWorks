DECLARE 
	@deleteOlderThan datetime,
	@oldCount int,
	@batchSize int

SET @deleteOlderThan = '{CUTOFFDATE}'	--	yyyy-mm-dd

-- if this is the first time we've run this, figure out which resources must go
IF OBJECT_ID('tempdb..#auditTemp') IS NULL
BEGIN

	SELECT auditID 
	INTO #auditTemp
	FROM Audit
	WHERE Date < @deleteOlderThan
	
	CREATE INDEX IX_ResourceWorking on #auditTemp (auditID)
END

SELECT @oldCount = COUNT(*) FROM #auditTemp
IF @oldCount > 0
BEGIN

	DECLARE @currentBatch TABLE ( auditID bigint )
	DECLARE @deletedAuditIds table ( AuditChangeID bigint )
	
	INSERT INTO @currentBatch
	SELECT TOP 100 *  FROM #auditTemp 
		
	WHILE @@ROWCOUNT>0
	BEGIN
	
		BEGIN TRANSACTION
			DELETE C
			OUTPUT deleted.AuditChangeID into @deletedAuditIds
			FROM @currentBatch B
			INNER JOIN AuditChange C
			ON C.AuditID=B.auditID
			
			DELETE C
			FROM @deletedAuditIds D
			INNER JOIN AuditChangeDetail C
			ON C.AuditChangeID=D.AuditChangeID
			
			DELETE A
			FROM @currentBatch B
			INNER JOIN Audit A
			ON A.AuditID=B.auditID
			
			DELETE T
			FROM #auditTemp T
			INNER JOIN @currentBatch B
				ON T.AuditID=B.auditID
				
		COMMIT TRANSACTION
		
		
		DELETE @currentBatch
		DELETE @deletedAuditIds

		--grab the next batch before ending loop
		--Must be last statement in while loop or the @@rowcount will be an issue
		INSERT INTO @currentBatch
			SELECT TOP 100 *  FROM #auditTemp R		
	END

END

DROP TABLE #auditTemp


