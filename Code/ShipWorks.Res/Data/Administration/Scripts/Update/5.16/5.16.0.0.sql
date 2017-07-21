PRINT N'Adding [RefreshToken] to [dbo].[ChannelAdvisorStore]'
GO
IF COL_LENGTH(N'[dbo].[ChannelAdvisorStore]', N'RefreshToken') IS NULL
BEGIN
	ALTER TABLE [dbo].[ChannelAdvisorStore]
		Add [RefreshToken] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ChannelAdvisorStore_RefreshToken] DEFAULT ('')

	ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT [DF_ChannelAdvisorStore_RefreshToken]
END

GO

PRINT N'Altering [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
[DistributionCenterID] [bigint] NOT NULL CONSTRAINT [DF_ChannelAdvisorOrderItem_DistributionCenterID] DEFAULT ((-1))

ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [DF_ChannelAdvisorOrderItem_DistributionCenterID]
GO

PRINT N'Creating index [IX_ChannelAdvisorOrderItem_DistributionCenterID] on [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_DistributionCenterID] ON [dbo].[ChannelAdvisorOrderItem] ([DistributionCenterID])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrderItem_DistributionCenter] on [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_DistributionCenter] ON [dbo].[ChannelAdvisorOrderItem] ([DistributionCenter])
GO