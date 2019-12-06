PRINT N'Altering [dbo].[ChannelAdvisorStore]'
GO
IF COL_LENGTH(N'[dbo].[ChannelAdvisorStore]', N'ExcludeFBA') IS NULL
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD[ExcludeFBA] [bit] NOT NULL CONSTRAINT [DF_ChannelAdvisorStore_ExcludeFBA] DEFAULT ((1))
GO