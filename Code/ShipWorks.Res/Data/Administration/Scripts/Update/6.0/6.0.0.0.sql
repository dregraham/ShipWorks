SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD
[ChannelOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_ChannelOrderID] DEFAULT (''),
[ShipByDate] [datetime] NULL
GO
PRINT N'Altering [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD
[Brand] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Brand] DEFAULT (''),
[MPN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_MPN] DEFAULT ('')
GO
UPDATE OrderItem 
SET OrderItem.MPN = ChannelAdvisorOrderItem.MPN
FROM OrderItem 
INNER JOIN ChannelAdvisorOrderItem ON OrderItem.OrderItemID = ChannelAdvisorOrderItem.OrderItemID
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP
COLUMN [MPN]
GO
