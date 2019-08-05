PRINT N'Altering [dbo].[ChannelAdvisorStore]' 
GO 
 
IF COL_LENGTH(N'[dbo].[ChannelAdvisorStore]', N'DownloadModifiedNumberOfDaysBack') IS NOT NULL 
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP COLUMN [DownloadModifiedNumberOfDaysBack] 
GO 
 