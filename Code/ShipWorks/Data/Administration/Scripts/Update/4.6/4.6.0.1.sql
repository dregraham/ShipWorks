PRINT N'Creating index [IX_ChannelAdvisorOrderItem_Classification] ON [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_Classification] ON [dbo].[ChannelAdvisorOrderItem] ([Classification])
GO