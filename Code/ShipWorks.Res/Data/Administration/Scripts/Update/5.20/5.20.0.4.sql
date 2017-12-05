PRINT N'Altering [dbo].[ProStoresOrder].[IX_ProStoresOrder_ConfirmationNumber]'
GO
CREATE NONCLUSTERED INDEX [IX_ProStoresOrder_ConfirmationNumber] 
	ON [dbo].[ProStoresOrder] ( [ConfirmationNumber] ASC )
GO
PRINT N'Altering [dbo].[ProStoresOrderSearch].[IX_ProStoresOrderSearch_ConfirmationNumber]'
GO
CREATE NONCLUSTERED INDEX [IX_ProStoresOrderSearch_ConfirmationNumber] 
	ON [dbo].[ProStoresOrderSearch] ( [ConfirmationNumber] ASC )
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrder].[CustomOrderIdentifier]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrder_CustomerOrderIdentifier_OrderID]
	ON [dbo].[ChannelAdvisorOrder] ([CustomOrderIdentifier], [OrderID])
GO
PRINT N'Altering [dbo].[[ChannelAdvisorOrderSearch]].[[IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier]]'
GO
DROP INDEX [IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier] ON dbo.[ChannelAdvisorOrderSearch]
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrderSearch].[IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID]
	ON [dbo].[ChannelAdvisorOrderSearch] ([CustomOrderIdentifier], [OrderID])
GO
PRINT N'Altering [dbo].[AmazonOrderSearch].[IX_AmazonOrderSearch_AmazonOrderID]'
GO
DROP INDEX [IX_AmazonOrderSearch_AmazonOrderID] ON dbo.[AmazonOrderSearch]
GO
PRINT N'Altering [dbo].[AmazonOrderSearch].[IX_AmazonOrderSearch_AmazonOrderID_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_AmazonOrderSearch_AmazonOrderID_OrderID] ON [dbo].[AmazonOrderSearch] ([AmazonOrderID], [OrderID])
GO
PRINT N'Altering [dbo].[EbayOrderSearch].[IX_EbayOrderSearch_EbayBuyerID]'
GO
DROP INDEX [IX_EbayOrderSearch_EbayBuyerID] ON dbo.[EbayOrderSearch]
GO
PRINT N'Altering [dbo].[EbayOrderSearch].[IX_EbayOrderSearch_EbayBuyerID_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_EbayOrderSearch_EbayBuyerID_OrderID] ON [dbo].[EbayOrderSearch] ([EbayBuyerID], [OrderID])
GO
PRINT N'Altering [dbo].[EbayOrderItem].[IX_EbayOrderItem_SellingManagerRecord_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_EbayOrderItem_SellingManagerRecord_OrderID] ON [dbo].[EbayOrderItem]
	([SellingManagerRecord] ASC, [OrderID] ASC)
GO
PRINT N'Altering [dbo].[GrouponOrderSearch].[IX_GrouponOrderSearch_GrouponOrderID]'
GO
DROP INDEX [IX_GrouponOrderSearch_GrouponOrderID] ON dbo.[GrouponOrderSearch]
GO
PRINT N'Altering [dbo].[GrouponOrderSearch].[IX_GrouponOrderSearch_GrouponOrderID_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_GrouponOrderSearch_GrouponOrderID_OrderID] ON [dbo].[GrouponOrderSearch] ([GrouponOrderID], [OrderID])
GO
PRINT N'Altering [dbo].[GrouponOrderSearch].[IX_GrouponOrderSearch_ParentOrderID]'
GO
DROP INDEX [IX_GrouponOrderSearch_ParentOrderID] ON dbo.[GrouponOrderSearch]
GO
PRINT N'Altering [dbo].[GrouponOrderSearch].[IX_GrouponOrderSearch_ParentOrderID_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_GrouponOrderSearch_ParentOrderID_OrderID] ON [dbo].[GrouponOrderSearch] ([ParentOrderID], [OrderID])
GO
PRINT N'Altering [dbo].[LemonStandOrderSearch].[IX_LemonStandOrderSearch_LemonStandOrderID]'
GO
DROP INDEX [IX_LemonStandOrderSearch_LemonStandOrderID] ON dbo.[LemonStandOrderSearch]
GO
PRINT N'Altering [dbo].[LemonStandOrderSearch].[IX_LemonStandOrderSearch_LemonStandOrderID_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_LemonStandOrderSearch_LemonStandOrderID_OrderID] ON [dbo].[LemonStandOrderSearch] ([LemonStandOrderID], [OrderID])
GO
PRINT N'Altering [dbo].[SearsOrder].[IX_SearsOrder_PoNumber_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_SearsOrder_PoNumber_OrderID] ON [dbo].[SearsOrder] ([PoNumber], [OrderID])
GO
PRINT N'Altering [dbo].[SearsOrderSearch].[IX_SearsOrderSearch_PoNumber_OrderID]'
GO
CREATE NONCLUSTERED INDEX [IX_SearsOrderSearch_PoNumber_OrderID] ON [dbo].[SearsOrderSearch] ([PoNumber], [OrderID])
GO