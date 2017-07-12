PRINT N'Adding [RefreshToken] to [dbo].[ChannelAdvisorStore]'
GO
IF COL_LENGTH(N'[dbo].[ChannelAdvisorStore]', N'RefreshToken') IS NULL
BEGIN
	ALTER TABLE [dbo].[ChannelAdvisorStore]
		Add [RefreshToken] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ChannelAdvisorStore_RefreshToken] DEFAULT ('')

	ALTER TABLE [dbo].[ChannelAdvisorStore] DROP DEFAULT
END