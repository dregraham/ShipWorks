PRINT N'Altering [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
[DistributionCenterName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ChannelAdvisorOrderItem_DistributionCenterName] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [DF_ChannelAdvisorOrderItem_DistributionCenterName]
GO