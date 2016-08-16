PRINT N'Creating index [IX_ChannelAdvisorOrder_OnlineStatus] on [dbo].[ChannelAdvisorOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrder_OnlineShippingStatus] ON [dbo].[ChannelAdvisorOrder] ([OnlineShippingStatus])
GO
PRINT N'Creating index [IX_PayPalOrder_PaymentStatus] on [dbo].[PayPalOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_PayPalOrder_PaymentStatus] ON [dbo].[PayPalOrder] ([PaymentStatus])
GO
