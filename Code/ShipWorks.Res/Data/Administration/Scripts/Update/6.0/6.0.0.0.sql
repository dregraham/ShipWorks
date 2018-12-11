SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD
[ChannelOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_ChannelOrderID] DEFAULT (''),
[ShipByDate] [datetime] NULL,
[Custom1] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom1] DEFAULT (''),
[Custom2] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom2] DEFAULT (''),
[Custom3] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom3] DEFAULT (''),
[Custom4] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom4] DEFAULT (''),
[Custom5] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom5] DEFAULT ('')
GO
PRINT N'Altering [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD
[Brand] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Brand] DEFAULT (''),
[MPN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_MPN] DEFAULT (''),
[Custom1] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom1] DEFAULT (''),
[Custom2] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom2] DEFAULT (''),
[Custom3] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom3] DEFAULT (''),
[Custom4] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom4] DEFAULT (''),
[Custom5] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom5] DEFAULT ('')
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
