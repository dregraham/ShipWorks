PRINT N'Altering [dbo].[ChannelAdvisorStore]'
GO
IF COL_LENGTH(N'[dbo].[ChannelAdvisorStore]', N'DownloadDaysBack') IS NULL
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD [DownloadDaysBack] [tinyint] NOT NULL
GO