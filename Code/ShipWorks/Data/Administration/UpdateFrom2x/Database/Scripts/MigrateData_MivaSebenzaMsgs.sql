
-- ShipWorks V2 to V3 Database Migration Script, generated by CodeSmith
-- For table v2m_MivaSebenzaMsgs

-- operational variables
DECLARE 
    @workCounter int

-- source table variables
DECLARE
    -- @MasterDatabase will be generated at runtime
    -- @IsArchive will be generated at runtime    
    @sMivaSebenzaMsgID int, 
    @sOrderID int, 
    @sData1 varchar(max), 
    @sData2 varchar(max), 
    @sData3 varchar(max) 

-- Track Progress
SET @workCounter = 0

-- the cursor for cycling through the source table
DECLARE workCursor CURSOR FORWARD_ONLY FOR
SELECT TOP 1000
        [MivaSebenzaMsgID],
        [OrderID],
        [Data1],
        [Data2],
        [Data3]
    FROM v2m_MivaSebenzaMsgs

-- open the source table cursor
OPEN workCursor

-- populate source table variables from the source cursor
FETCH NEXT FROM workCursor
INTO
        @sMivaSebenzaMsgID,
        @sOrderID,
        @sData1,
        @sData2,
        @sData3
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @workCounter = @workCounter + 1
    
    DECLARE 
		@newOrderID bigint,
		@orderDate datetime
    
    -- get the new order id
    SET @newOrderID = dbo.v2m_TranslateKey(@sOrderID, 0 /* order */)
    
    -- get the order date to use on the notes
    SELECT @orderDate = OrderDate FROM {MASTERDATABASE}.dbo.[Order] WHERE OrderID = @newOrderID
    
    -- attach up to 3 notes
    IF (LEN(@sData1) > 0)
		EXEC dbo.v2m_CreateNote @newOrderID, @orderDate, @sData1, 1 /* Download */, 1 /* public */
    
    IF (LEN(@sData2) > 0)
		EXEC dbo.v2m_CreateNote @newOrderID, @orderDate, @sData2, 1 /* Download */, 1 /* public */
		
    IF (LEN(@sData3) > 0)
		EXEC dbo.v2m_CreateNote @newOrderID, @orderDate, @sData3, 1 /* Download */, 1 /* public */
		
		
	-- remove the row
	DELETE FROM dbo.v2m_MivaSebenzaMsgs WHERE MivaSebenzaMsgID = @sMivaSebenzaMsgID
	
-- fetch next row from source table
FETCH NEXT FROM workCursor
INTO
        @sMivaSebenzaMsgID,
        @sOrderID,
        @sData1,
        @sData2,
        @sData3
END
CLOSE workCursor
DEALLOCATE workCursor

-- data migration "protocol" demands we return the number of rows/work completed
SELECT @workCounter as WorkCompleted