CREATE NONCLUSTERED INDEX [SW_EbayOrderItem_EbayItemID_EbayTransactionID]
       ON [dbo].[EbayOrderItem] ([EbayItemID],[EbayTransactionID])

CREATE NONCLUSTERED INDEX [SW_EbayOrderItem_EffectiveCheckoutStatus_EbayOrderItemID]
       ON [dbo].[EbayOrderItem] ([EffectiveCheckoutStatus], [OrderItemID])

CREATE INDEX [SW_FilterNodeContentDirty_FilterNodeContentDirtyID] on dbo.FilterNodeContentDirty
       (FilterNodeContentDirtyID)
