SET XACT_ABORT ON


-- This script will remove all print jobs older than a certain date
-- 
DECLARE @deleteOlderThan      DATETIME,
        @oldCount             INT,
        @batchSize            INT,
        @printImageResourceID BIGINT,
        @printReferenceKey    VARCHAR(250),
        @imageData            VARBINARY(MAX),
        @imageChecksum        BINARY(32),
        @eplResourceID        BIGINT,
        @eplReferenceKey      VARCHAR(250),
        @eplData              VARBINARY(MAX),
        @eplChecksum          BINARY(32),
		@endProcDate			  DATETIME

SELECT @imageData = convert(VARBINARY(MAX), 0x1F8B0800000000000400ECBD07601C499625262F6DCA7B7F4AF54AD7E074A10880601324D8904010ECC188CDE692EC1D69472329AB2A81CA6556655D661640CCED9DBCF7DE7BEFBDF7DE7BEFBDF7BA3B9D4E27F7DFFF3F5C6664016CF6CE4ADAC99E2180AAC81F3F7E7C1F3F22DECC8B265DD5C5B24D7FBA9AA4F3AC492779BE4C57EBFA229FA593EBF4F5BC587DB7AADF3669B69CA5D47A59A565B5BCC8EB34BBCC8A329B94F9F8FF090000FFFF56DD225D47000000),
	   @imageChecksum = convert(BINARY, 0x6F7E431AD96DBB63B335CDEF1DBB696661C32506A1B967D13727395C73462CCC),
	   @eplData = convert(VARBINARY(MAX), 'Purged'),
	   @eplChecksum = convert(BINARY, 'Purged'),
	   @deleteOlderThan = '{CUTOFFDATE}', --	yyyy-mm-dd
	   @endProcDate = '{ENDPROCDATE}' -- yyyy-mm-dd hh:mm

	   SELECT getdate()

--create dummy resource reccord for non-thermal print jobs
IF NOT EXISTS (
			   SELECT 1
				   FROM
					   [Resource] r
				   WHERE
					   r.Data = @imageData)
BEGIN
	INSERT INTO [Resource] ([Data],
							[Checksum],
							[Compressed],
							[Filename])
		VALUES
			(@imageData,
			 @imageChecksum,
			 1,
			 '__PrintCleanup.swr');

	SET @printImageResourceID = @@IDENTITY
END
ELSE
BEGIN
	-- we've run it once, so just locate the resource id we are redirecting to
	SELECT @printImageResourceID = ResourceID
		FROM
			[Resource] r
		WHERE
			r.Data = @imageData
END

SELECT @printReferenceKey = '#' + convert(VARCHAR(250), @printImageResourceID)

--create dummy resource record for Thermal
IF NOT EXISTS (
			   SELECT 1
				   FROM
					   [Resource] r
				   WHERE
					   r.Data = @eplData)
BEGIN
	INSERT INTO [Resource] ([Data],
							[Checksum],
							[Compressed],
							[Filename])
		VALUES
			(@eplData,
			 @eplChecksum,
			 1,
			 '__PrintCleanup_Thermal.swr');

	SET @eplResourceID = @@IDENTITY
END
ELSE
BEGIN
	-- we've run it once, so just locate the resource id we are redirecting to
	SELECT @eplResourceID = ResourceID
		FROM
			[Resource] r
		WHERE
			r.Data = @eplData
END
SELECT @eplReferenceKey = '#' + convert(VARCHAR(250), @eplResourceID)

-- if this is the first time we've run this, figure out which resources must go
IF object_id('tempdb..#printJobWorking') IS NULL
BEGIN
	-- create temp tables
	CREATE TABLE #printJobWorking(
		PrintResultID BIGINT,
		ContentResourceID BIGINT,
		IsThermal BIT
	)


	-- find all of the old print jobs to wipe out
	INSERT INTO #printJobWorking
		SELECT p.PrintResultID,
			   p.ContentResourceID,
			   CASE p.TemplateType
				   WHEN 3 THEN
					   1
				   ELSE
					   0
			   END
			FROM
				PrintResult p
				INNER JOIN ObjectReference AS or1 ON or1.ObjectReferenceID = p.ContentResourceID
			WHERE
				p.PrintDate < @deleteOlderThan
				AND or1.ObjectID NOT IN (@printImageResourceID, @eplResourceID)

	CREATE INDEX IX_PrintJobWorking ON #printJobWorking (PrintResultID)
END

-- see if there's any actual work to do
IF EXISTS (
		   SELECT 1
			   FROM
				   #printJobWorking)
BEGIN
	DECLARE @currentBatch TABLE(
		PrintResultID BIGINT,
		ContentResourceID BIGINT,
		IsThermal BIT
	)

	INSERT INTO @currentBatch
		SELECT TOP 100 PrintResultID,
					   R.ContentResourceID,
					   R.IsThermal
			FROM
				#printJobWorking R

	WHILE @@ROWCOUNT > 0 AND getdate()< @endProcDate
	BEGIN
		--Wrap edits in transaction
		BEGIN TRANSACTION
		/* Upate ObjectReference refrerenced by EmailOutbound columnPlainPartResourceID
			so they point to to @printImageResourceID */
		UPDATE o
			SET
				ObjectID = CASE b.ISThermal
					WHEN 1 THEN
						@eplResourceID
					ELSE
						@printImageResourceID
				END,
				o.ReferenceKey = CASE b.ISThermal
					WHEN 1 THEN
						@eplReferenceKey
					ELSE
						@printReferenceKey
				END
			FROM
				@currentBatch AS b
				INNER JOIN ObjectReference o ON o.ObjectReferenceID = b.ContentResourceID

		/* Delete ObjectReference not explicitly pointed at by PrintResult (and therefore updated to point to @printImageResourceID */
		DELETE or1
			FROM
				ObjectReference or1
				INNER JOIN @currentBatch b ON b.PrintResultID = or1.ConsumerID AND b.ContentResourceID != or1.ObjectReferenceID

		--Delete items in the batch from the #printJobWorking table
		DELETE pjw
			FROM
				#printJobWorking AS pjw
				INNER JOIN @currentBatch AS cb ON cb.PrintResultID = pjw.PrintResultID

		COMMIT TRANSACTION

		--grab the next batch before ending loop
		DELETE @currentBatch
		INSERT INTO @currentBatch
			SELECT TOP 100 *
				FROM
					#printJobWorking R
	END
END

DROP TABLE #printJobWorking