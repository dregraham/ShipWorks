PRINT N'Creating index [IX_ChannelAdvisorOrder_IsPrime] on [dbo].[ChannelAdvisorOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrder_IsPrime] ON [dbo].[ChannelAdvisorOrder]([IsPrime])
GO