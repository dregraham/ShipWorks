UPDATE TOP(1000) dbo.eBayOrder
SET RollupeBayItemCount = (SELECT COUNT(*) FROM dbo.eBayOrderItem WHERE dbo.eBayOrderItem.OrderID = dbo.eBayOrder.OrderID),
	RollupEffectiveCheckoutStatus = (SELECT CASE COUNT(DISTINCT(EffectiveCheckoutStatus)) WHEN 1 THEN MAX(EffectiveCheckoutStatus) ELSE NULL END FROM dbo.eBayOrderItem WHERE dbo.eBayOrderItem.OrderID = dbo.eBayOrder.OrderID),
	RollupEffectivePaymentMethod = (SELECT CASE COUNT(DISTINCT(EffectivePaymentMethod)) WHEN 1 THEN MAX(EffectivePaymentMethod) ELSE NULL END FROM dbo.eBayOrderItem WHERE dbo.eBayOrderItem.OrderID = dbo.eBayOrder.OrderID),
	RollupFeedbackLeftType = (SELECT CASE COUNT(DISTINCT(FeedbackLeftType)) WHEN 1 THEN MAX(FeedbackLeftType) ELSE NULL END FROM dbo.eBayOrderItem WHERE dbo.eBayOrderItem.OrderID = dbo.eBayOrder.OrderID),
	RollupFeedbackLeftComments = (SELECT CASE COUNT(DISTINCT(FeedbackLeftComments)) WHEN 1 THEN MAX(FeedbackLeftComments) ELSE NULL END FROM dbo.EbayOrderItem WHERE dbo.eBayOrderItem.OrderID = dbo.eBayOrder.OrderID),
	RollupFeedbackReceivedType = (SELECT CASE COUNT(DISTINCT(FeedbackReceivedType)) WHEN 1 THEN MAX(FeedbackReceivedType) ELSE NULL END FROM dbo.EbayOrderItem WHERE dbo.eBayORderItem.OrderID = dbo.eBayOrder.OrderID),
	RollupFeedbackReceivedComments = (SELECT CASE COUNT(DISTINCT(FeedbackReceivedComments)) WHEN 1 THEN MAX(FeedbackReceivedComments) ELSE NULL END FROM Dbo.EbayOrderItem WHERE dbo.EbayOrderItem.OrderID = dbo.EbayOrder.OrderID),
	RollupPayPalAddressStatus = (SELECT CASE COUNT(DISTINCT(PayPalAddressStatus)) WHEN 1 THEN MAX(PayPalAddressStatus) ELSE NULL END FROM Dbo.EbayOrderItem WHERE dbo.EbayOrderItem.OrderID = dbo.EbayOrder.OrderID),
	RollupSellingManagerRecord = (SELECT CASE COUNT(DISTINCT(SellingManagerRecord)) WHEN 1 THEN MAX(SellingManagerRecord) ELSE NULL END FROM Dbo.EbayOrderItem WHERE dbo.EbayOrderItem.OrderID = dbo.EbayOrder.OrderID)
WHERE RollupeBayItemCount IS NULL

SELECT @@ROWCOUNT as WorkCompleted