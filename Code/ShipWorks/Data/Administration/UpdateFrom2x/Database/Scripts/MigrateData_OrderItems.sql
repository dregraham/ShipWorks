
-- ShipWorks V2 to V3 Database Migration Script, generated by CodeSmith
-- For table v2m_OrderItems

-- operational variables
DECLARE 
    @sStoreType int,
    @newOrderID bigint,
    @newOrderItemID bigint,
	@itemIsManual bit,
    @workCounter int,
	@itemLabel varchar(100)
    
-- source table variables
DECLARE
    -- @MasterDatabase will be generated at runtime
    -- @IsArchive will be generated at runtime    
    @sOrderItemID int, 
    @sRowVersion timestamp, 
    @sOrderID int, 
    @sOrdinal int, 
    @sName nvarchar(300), 
    @sCode nvarchar(300), 
    @sSKU nvarchar(100), 
    @sThumbnail nvarchar(350), 
    @sImage nvarchar(max), 
    @sUnitPrice money, 
    @sUnitCost money, 
    @sWeight float, 
    @sQuantity float, 
    @seBayItemID bigint, 
    @seBayTransID bigint, 
    @seBaySellingManagerProductName nvarchar(80), 
    @seBaySellingManagerProductPart nvarchar(80), 
    @seBaySellingManagerRecord int, 
    @sMivaLineID int, 
    @sMivaProductCode nvarchar(300), 
    @sStatus nvarchar(50), 
    @sYahooProductID nvarchar(255), 
    @sLocation nvarchar(100), 
    @sISBN nvarchar(30), 
    @sUPC nvarchar(30), 
    @sChannelAdvisorSiteName varchar(50), 
    @sChannelAdvisorBuyerID varchar(80), 
    @sChannelAdvisorAuctionID varchar(50), 
    @sAmazonOrderItemCode bigint, 
    @sAmazonASIN varchar(32), 
    @sChannelAdvisorClassification nvarchar(30), 
    @sAmazonConditionNote nvarchar(255) 

-- target table variables
DECLARE
    @tOrderItemID bigint, 
    @tRowVersion timestamp, 
    @tOrderID bigint, 
    @tName nvarchar(300), 
    @tCode nvarchar(300), 
    @tSKU nvarchar(100), 
    @tISBN nvarchar(30), 
    @tUPC nvarchar(30), 
    @tDescription nvarchar(max), 
    @tLocation nvarchar(255), 
    @tImage nvarchar(max), 
    @tThumbnail nvarchar(max), 
    @tUnitPrice money, 
    @tUnitCost money, 
    @tWeight float, 
    @tQuantity float, 
    @tLocalStatus nvarchar(255), 
    @tIsManual bit 

-- Track Progress
SET @workCounter = 0

-- the cursor for cycling through the source table
DECLARE workCursor CURSOR FORWARD_ONLY FOR
SELECT TOP 1000
    [OrderItemID],
    [RowVersion],
    [OrderID],
    [Ordinal],
    [Name],
    [Code],
    [SKU],
    [Thumbnail],
    [Image],
    [UnitPrice],
    [UnitCost],
    [Weight],
    [Quantity],
    [eBayItemID],
    [eBayTransID],
    [eBaySellingManagerProductName],
    [eBaySellingManagerProductPart],
    [eBaySellingManagerRecord],
    [MivaLineID],
    [MivaProductCode],
    [Status],
    [YahooProductID],
    [Location],
    [ISBN],
    [UPC],
    [ChannelAdvisorSiteName],
    [ChannelAdvisorBuyerID],
    [ChannelAdvisorAuctionID],
    [AmazonOrderItemCode],
    [AmazonASIN],
    [ChannelAdvisorClassification],
    [AmazonConditionNote]
    FROM v2m_OrderItems

-- open the source table cursor
OPEN workCursor

-- populate source table variables from the source cursor
FETCH NEXT FROM workCursor
INTO
    @sOrderItemID,
    @sRowVersion,
    @sOrderID,
    @sOrdinal,
    @sName,
    @sCode,
    @sSKU,
    @sThumbnail,
    @sImage,
    @sUnitPrice,
    @sUnitCost,
    @sWeight,
    @sQuantity,
    @seBayItemID,
    @seBayTransID,
    @seBaySellingManagerProductName,
    @seBaySellingManagerProductPart,
    @seBaySellingManagerRecord,
    @sMivaLineID,
    @sMivaProductCode,
    @sStatus,
    @sYahooProductID,
    @sLocation,
    @sISBN,
    @sUPC,
    @sChannelAdvisorSiteName,
    @sChannelAdvisorBuyerID,
    @sChannelAdvisorAuctionID,
    @sAmazonOrderItemCode,
    @sAmazonASIN,
    @sChannelAdvisorClassification,
    @sAmazonConditionNote
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @workCounter = @workCounter + 1
    
	SET @newOrderID = dbo.v2m_TranslateKey(@sOrderID, 0 /* Order */)
	
	-- query up through Order to Store to find the store type
	SELECT @sStoreType = s.TypeCode FROM {MASTERDATABASE}.dbo.Store s,
									     {MASTERDATABASE}.dbo.[Order] o
									WHERE o.StoreID = s.StoreID
									AND o.OrderID = @newOrderID

	-- If the order is manual, we know the item is
	SELECT @itemIsManual = IsManual FROM {MASTERDATABASE}.dbo.[Order] WHERE OrderID = @newOrderID;

	-- If the Order is not manual, the item still might be.  Try a little harder to detect that
	IF (@itemIsManual = 0)
	BEGIN
	
		-- eBay real items would definitely have an item id
		IF (@sStoreType = 1 AND @seBayItemID <= 0)
			SET @itemIsManual = 1

		-- Yahoo items would definitely have a product id
		IF (@sStoreType = 2 AND LEN(@sYahooProductID) = 0)
			SET @itemIsManual = 1

		-- I think all amazon items would have an item code
		IF (@sStoreType = 10 AND @sAmazonOrderItemCode <= 0)
			SET @itemIsManual = 1

	END
    -- 
    -- Custom stuff here
    -- 
    SET @tName = @sName
    SET @tCode = @sCode
    SET @tSKU = @sSKU
    SET @tISBN = @sISBN
    SET @tUPC = @sUPC
    SET @tDescription = N''
    SET @tLocation = @sLocation
    SET @tImage = @sImage
    SET @tThumbnail = @sThumbnail
    SET @tUnitPrice = @sUnitPrice
    SET @tUnitCost = @sUnitCost
    SET @tWeight = @sWeight
    SET @tQuantity = @sQuantity
    SET @tLocalStatus = @sStatus
    SET @tIsManual = @itemIsManual -- If the order is manual, we know the item is.  If the order is not, the item still might be... but we've reall got no way to tell.
    
    INSERT INTO {MASTERDATABASE}.dbo.OrderItem  (
	    [OrderID],
	    [Name],
	    [Code],
	    [SKU],
	    [ISBN],
	    [UPC],
	    [Description],
	    [Location],
	    [Image],
	    [Thumbnail],
	    [UnitPrice],
	    [UnitCost],
	    [Weight],
	    [Quantity],
	    [LocalStatus],
	    [IsManual]
    )
    VALUES
    (
	    @newOrderID,
	    @tName,
	    @tCode,
	    @tSKU,
	    @tISBN,
	    @tUPC,
	    @tDescription,
	    @tLocation,
	    @tImage,
	    @tThumbnail,
	    @tUnitPrice,
	    @tUnitCost,
	    @tWeight,
	    @tQuantity,
	    @tLocalStatus,
	    @tIsManual
    )             
    
    -- get the new key
    SET @newOrderItemID = @@IDENTITY
    
    -- record it
	exec dbo.v2m_RecordKey @sOrderItemID, 4 /* OrderItem */, @newOrderItemID

	-- record an object label		
	SET @itemLabel = LEFT(@tName, 100)
	EXEC dbo.v2m_RecordObjectLabel @newOrderItemID, 13, @newOrderID, @itemLabel
	
	-- No need for store-specific data if its manual
	IF (@itemIsManual = 0)
	BEGIN

		-- store-specific order item data
		-- ebay
		IF (@sStoreType = 1)
		BEGIN
			-- IN V3, many values were moved from the order level to the order item level.  During conversion
			-- these values are in an interim table called v2m_EbayTemp.  Selecting those values to insert here.
			INSERT INTO {MASTERDATABASE}.dbo.EbayOrderItem  (
				[OrderItemID],
				[OrderID],
				[EbayItemID],
				[EbayTransactionID],
				[SellingManagerProductName],
				[SellingManagerProductPart],
				[SellingManagerRecord],
				[EffectiveCheckoutStatus],
				[EffectivePaymentMethod],
				[PaymentStatus],
				[PaymentMethod],
				[CheckoutStatus],
				[CompleteStatus],
				[SellerPaidStatus],
				[FeedbackLeftType],
				[FeedbackLeftComments],
				[FeedbackReceivedType],
				[FeedbackReceivedComments],
				[MyEbayPaid],
				[MyEbayShipped],
				[PayPalTransactionID],
				[PayPalAddressStatus]
			)
			SELECT 
				@newOrderItemID, 
				@newOrderID, 
				@seBayItemID,
				@seBayTransID,
				@seBaySellingManagerProductName,
				@seBaySellingManagerProductPart,
				@seBaySellingManagerRecord,
				-1,	-- EffectiveCheckoutStatus 
				-1,  -- EffectivePaymentMethod
				PaymentStatus,
				PaymentMethod,
				CheckoutStatus,
				CompleteStatus,
				SellerPaidStatus,
				FeedbackLeftType,
				FeedbackLeftComments,
				FeedbackReceivedType,
				FeedbackReceivedComments,
				MyEBayPaid,
				MyEbayShipped,
				PayPalTransactionID,
				PayPalAddressStatus
			FROM v2m_EbayTemp
			WHERE OrderID = @sOrderID
		END
	
		-- Yahoo
		IF (@sStoreType = 2)
		BEGIN
			INSERT INTO {MASTERDATABASE}.dbo.YahooOrderItem  (
				[OrderItemID],
				[YahooProductID]
			)
			VALUES
			(
				@newOrderItemID,
				@sYahooProductID
			)             
		END
	
		-- ChannelAdvisor
		IF (@sStoreType = 7)
		BEGIN

			DECLARE @sChannelAdvisorDistributionCenter nvarchar(80)

			SELECT @sChannelAdvisorDistributionCenter = DistributionCenter
			FROM {MASTERDATABASE}.dbo.v2m_ChannelAdvisorOrder
			WHERE OrderID = @newOrderID

			INSERT INTO {MASTERDATABASE}.dbo.ChannelAdvisorOrderItem  (
				[OrderItemID],
				[SiteName],
				[BuyerID],
				[SalesSourceID],
				[Classification],
				[DistributionCenter]	
			)
			VALUES
			(
				@newOrderItemID,
				@sChannelAdvisorSiteName,
				@sChannelAdvisorBuyerID,
				@sChannelAdvisorAuctionID,
				@sChannelAdvisorClassification,
				@sChannelAdvisorDistributionCenter
			)             
		END
	
		-- Amazon
		IF (@sStoreType = 10)
		BEGIN
			INSERT INTO {MASTERDATABASE}.dbo.AmazonOrderItem  (
				[OrderItemID],
				[AmazonOrderItemCode],
				[ASIN],
				[ConditionNote]
			)
			VALUES
			(
				@newOrderItemID,
				@sAmazonOrderItemCode,
				@sAmazonASIN,
				@sAmazonConditionNote
			)   
		END

		-- Infopia
		IF (@sStoreType = 8)
		BEGIN
			INSERT INTO {MASTERDATABASE}.dbo.InfopiaOrderItem  (
				[OrderItemID],
				[Marketplace],
				[MarketplaceItemID],
				[BuyerID]
			)
			VALUES
			(
				@newOrderItemID,
				@sChannelAdvisorSiteName,
				@sChannelAdvisorAuctionID,
				SubString(@sChannelAdvisorBuyerID, 0, 50)
			)   
		END

	END
	
	-- remove the order item
	DELETE FROM dbo.v2m_OrderItems WHERE OrderItemID = @sOrderItemID

-- fetch next row from source table
FETCH NEXT FROM workCursor
INTO
    @sOrderItemID,
    @sRowVersion,
    @sOrderID,
    @sOrdinal,
    @sName,
    @sCode,
    @sSKU,
    @sThumbnail,
    @sImage,
    @sUnitPrice,
    @sUnitCost,
    @sWeight,
    @sQuantity,
    @seBayItemID,
    @seBayTransID,
    @seBaySellingManagerProductName,
    @seBaySellingManagerProductPart,
    @seBaySellingManagerRecord,
    @sMivaLineID,
    @sMivaProductCode,
    @sStatus,
    @sYahooProductID,
    @sLocation,
    @sISBN,
    @sUPC,
    @sChannelAdvisorSiteName,
    @sChannelAdvisorBuyerID,
    @sChannelAdvisorAuctionID,
    @sAmazonOrderItemCode,
    @sAmazonASIN,
    @sChannelAdvisorClassification,
    @sAmazonConditionNote
END
CLOSE workCursor
DEALLOCATE workCursor

-- data migration "protocol" demands we return the number of rows/work completed
SELECT @workCounter as WorkCompleted