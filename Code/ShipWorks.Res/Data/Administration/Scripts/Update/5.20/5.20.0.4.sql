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
