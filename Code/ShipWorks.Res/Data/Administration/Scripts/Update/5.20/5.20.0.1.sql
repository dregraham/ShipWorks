PRINT N'Altering [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL  CONSTRAINT [DF_ChannelAdvisorStore_DownloadModifiedNumberOfDaysBack] DEFAULT (1)
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT [DF_ChannelAdvisorStore_DownloadModifiedNumberOfDaysBack]
GO