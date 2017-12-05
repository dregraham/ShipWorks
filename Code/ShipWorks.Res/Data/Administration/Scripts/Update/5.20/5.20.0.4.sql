CREATE NONCLUSTERED INDEX [IX_ProStoresOrder_ConfirmationNumber] 
	ON [dbo].[ProStoresOrder] ( [ConfirmationNumber] ASC )
GO
CREATE NONCLUSTERED INDEX [IX_ProStoresOrderSearch_ConfirmationNumber] 
	ON [dbo].[ProStoresOrderSearch] ( [ConfirmationNumber] ASC )
GO

CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrder_CustomerOrderIdentifier_OrderID]
	ON [dbo].[ChannelAdvisorOrder] ([CustomOrderIdentifier], [OrderID])
GO

DROP INDEX [IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier] ON dbo.[ChannelAdvisorOrderSearch]
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID]
	ON [dbo].[ChannelAdvisorOrderSearch] ([CustomOrderIdentifier], [OrderID])
GO

DROP INDEX [IX_AmazonOrderSearch_AmazonOrderID] ON dbo.[AmazonOrderSearch]
GO
CREATE NONCLUSTERED INDEX [IX_AmazonOrderSearch_AmazonOrderID_OrderID] ON [dbo].[AmazonOrderSearch] ([AmazonOrderID], [OrderID])
GO

DROP INDEX [IX_EbayOrderSearch_EbayBuyerID] ON dbo.[EbayOrderSearch]
GO
CREATE NONCLUSTERED INDEX [IX_EbayOrderSearch_EbayBuyerID_OrderID] ON [dbo].[EbayOrderSearch] ([EbayBuyerID], [OrderID])
GO

CREATE NONCLUSTERED INDEX [IX_EbayOrderItem_SellingManagerRecord_OrderID] ON [dbo].[EbayOrderItem]
	([SellingManagerRecord] ASC, [OrderID] ASC)
GO

DROP INDEX [IX_GrouponOrderSearch_GrouponOrderID] ON dbo.[GrouponOrderSearch]
GO
CREATE NONCLUSTERED INDEX [IX_GrouponOrderSearch_GrouponOrderID_OrderID] ON [dbo].[GrouponOrderSearch] ([GrouponOrderID], [OrderID])
GO

DROP INDEX [IX_GrouponOrderSearch_ParentOrderID] ON dbo.[GrouponOrderSearch]
GO
CREATE NONCLUSTERED INDEX [IX_GrouponOrderSearch_ParentOrderID_OrderID] ON [dbo].[GrouponOrderSearch] ([ParentOrderID], [OrderID])
GO

DROP INDEX [IX_LemonStandOrderSearch_LemonStandOrderID] ON dbo.[LemonStandOrderSearch]
GO
CREATE NONCLUSTERED INDEX [IX_LemonStandOrderSearch_LemonStandOrderID_OrderID] ON [dbo].[LemonStandOrderSearch] ([LemonStandOrderID], [OrderID])
GO

CREATE NONCLUSTERED INDEX [IX_SearsOrder_PoNumber_OrderID] ON [dbo].[SearsOrder] ([PoNumber], [OrderID])
GO

CREATE NONCLUSTERED INDEX [IX_SearsOrderSearch_PoNumber_OrderID] ON [dbo].[SearsOrderSearch] ([PoNumber], [OrderID])
GO