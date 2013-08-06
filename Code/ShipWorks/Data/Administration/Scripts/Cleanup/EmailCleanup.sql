set xact_abort ON

DECLARE @deleteOlderThan         DATETIME,
        @emailResourceID   BIGINT,
        @emailReferenceKey VARCHAR(250)

SET @deleteOlderThan = '{CUTOFFDATE}' --	yyyy-mm-dd

--create dummy resource reccord
IF NOT EXISTS (
			   SELECT 1
				   FROM
					   [Resource]
				   WHERE
					   [Filename] = '__EmailCleanup.swr')
BEGIN
	INSERT INTO [Resource] ([Data],
							[Checksum],
							[Compressed],
							[Filename])
		VALUES
			(0x1F8B0800000000000400ECBD07601C499625262F6DCA7B7F4AF54AD7E074A10880601324D8904010ECC188CDE692EC1D69472329AB2A81CA6556655D661640CCED9DBCF7DE7BEFBDF7DE7BEFBDF7BA3B9D4E27F7DFFF3F5C6664016CF6CE4ADAC99E2180AAC81F3F7E7C1F3F22DECC8B26CD175951A6F3AC492779BE4C57EBFA229FA593EBF4F5BC587DB7AADF3669B69CA5D47259A565B5BCC8EB34BBA477B249998FFF9F000000FFFF68E5632B43000000,
			 0x36C2A3B7068081B3C119CCE74F6CFA8879F078F412414B3A883E1F6B8F81135A,
			 1,
			 '__EmailCleanup.swr');

	SET @emailResourceID = @@IDENTITY
END
ELSE
BEGIN
	-- we've run it once, so just locate the resource id we are redirecting to
	SELECT @emailResourceID = ResourceID
		FROM
			[Resource]
		WHERE
			[Filename] = '__EmailCleanup.swr'
END

SELECT @emailReferenceKey = '#' + convert(VARCHAR(250), @emailResourceID)


-- if this is the first time we've run this, figure out which resources must go
IF object_id('tempdb..#emailsToCleanUp') IS NULL
BEGIN
	-- create temp tables
	CREATE TABLE #emailsToCleanUp(
		EmailOutboundID BIGINT
	)

	-- Select items to cleanup
	INSERT INTO #emailsToCleanUp
		SELECT EmailOutboundID
			FROM
				EmailOutbound AS eo
				INNER JOIN ObjectReference AS or1 ON eo.PlainPartResourceID = or1.ObjectReferenceID
			WHERE
				SentDate < @deleteOlderThan
				AND or1.ObjectID <> @emailResourceID
				


	CREATE INDEX IX_EmailsToCleanUp ON #emailsToCleanUp (EmailOutboundID)
END

IF EXISTS (
		   SELECT 1
			   FROM
				   #emailsToCleanUp)
BEGIN
	DECLARE @batchEmailsToCleanUp TABLE(
		EmailOutboundID BIGINT
	)
	INSERT INTO @batchEmailsToCleanUp
		SELECT TOP 100 EmailOutboundID
			FROM
				#emailsToCleanUp

	WHILE @@ROWCOUNT > 0
	BEGIN
		-- Wrap edits in transaction
		BEGIN TRANSACTION

		/* Upate ObjectReference refrerenced by EmailOutbound columnPlainPartResourceID
			so they point to to @emailResourceID */
		UPDATE o
			SET
				o.ReferenceKey = @emailReferenceKey,
				ObjectID = @emailResourceID
			FROM
				ObjectReference o
				INNER JOIN EmailOutbound e ON o.ConsumerID = e.EmailOutboundID AND o.ObjectReferenceID = PlainPartResourceID
				INNER JOIN [Resource] r ON r.ResourceID = o.ObjectID 
				INNER JOIN @batchEmailsToCleanUp b ON b.EmailOutboundID = e.EmailOutboundID

		/* Update EmailOutbound HtmlPartResourceID to point to null. The PlainPartResourceID will point to the dummy */
		UPDATE eo
			SET
				HtmlPartResourceID = NULL
			FROM
				@batchEmailsToCleanUp AS cu
				INNER JOIN EmailOutbound AS eo ON cu.EmailOutboundID = eo.EmailOutboundID

		/* Delete ObjectReference not explicitly pointed at by EmailOutbound (embedded email images) */
		DELETE o
			FROM
				ObjectReference o
				INNER JOIN EmailOutbound e ON e.EmailOutboundID = o.ConsumerID
				INNER JOIN @batchEmailsToCleanUp b ON b.EmailOutboundID = e.EmailOutboundID
			WHERE
				ObjectID <> @emailResourceID

		/* delete current batch out of temp table so it is not accidentaly reprocessed */
		DELETE tcu
			FROM
				#emailsToCleanUp AS tcu
				INNER JOIN @batchEmailsToCleanUp b ON b.EmailOutboundID = tcu.EmailOutboundID

		COMMIT TRANSACTION


		--grab the next batch before ending loop
		

		DELETE @batchEmailsToCleanUp

		INSERT INTO @batchEmailsToCleanUp
			SELECT TOP 100 *
				FROM
					#emailsToCleanUp R
	END --end while

END --end if at least one row in #emailsToCleanUp

DROP TABLE #emailsToCleanUp