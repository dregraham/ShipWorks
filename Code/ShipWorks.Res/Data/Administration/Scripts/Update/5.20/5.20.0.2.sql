CREATE NONCLUSTERED INDEX [SW_EbayOrderItem_EbayItemID_EbayTransactionID]
	ON [dbo].[EbayOrderItem] ([EbayItemID],[EbayTransactionID])
GO
CREATE NONCLUSTERED INDEX [SW_EbayOrderItem_EffectiveCheckoutStatus_EbayOrderItemID]
	ON [dbo].[EbayOrderItem] ([EffectiveCheckoutStatus], [OrderItemID])
GO
CREATE INDEX [SW_FilterNodeContentDirty_FilterNodeContentDirtyID] 
	ON [dbo].[FilterNodeContentDirty] (FilterNodeContentDirtyID)
GO