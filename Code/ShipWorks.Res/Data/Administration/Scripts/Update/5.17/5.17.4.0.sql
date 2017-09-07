PRINT N'Altering [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD
[HarmonizedCode] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_HarmonizedCode] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_HarmonizedCode]
GO
PRINT N'Copying data from ChannelAdvisor.HarmonizedCode to OrderItem.HarmonizedCode'
GO

UPDATE OrderItem	
SET OrderItem.HarmonizedCode = ChannelAdvisorOrderItem.HarmonizedCode
FROM OrderItem
INNER JOIN ChannelAdvisorOrderItem ON OrderItem.OrderItemID = ChannelAdvisorOrderItem.OrderItemID

GO  
PRINT N'Altering [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP
COLUMN [HarmonizedCode]
GO