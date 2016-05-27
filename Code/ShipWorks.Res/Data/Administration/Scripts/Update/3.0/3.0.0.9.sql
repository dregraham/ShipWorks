SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP
CONSTRAINT [FK_ChannelAdvisorOrder_Order]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [PK_ChannelAdvisorOrder]
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrder]
(
[OrderID] [bigint] NOT NULL,
[CustomOrderIdentifier] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ResellerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineShippingStatus] [int] NOT NULL CONSTRAINT [DF_ChannelAdvisorOrder_OnlineShippingStatus] DEFAULT ((0)),
[OnlineCheckoutStatus] [int] NOT NULL CONSTRAINT [DF_ChannelAdvisorOrder_OnlineCheckoutStatus] DEFAULT ((0)),
[OnlinePaymentStatus] [int] NOT NULL CONSTRAINT [DF_ChannelAdvisorOrder_OnlinePaymentStatus] DEFAULT ((0))
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrder]([OrderID], [CustomOrderIdentifier], [ResellerID]) SELECT [OrderID], [CustomOrderIdentifier], [ResellerID] FROM [dbo].[ChannelAdvisorOrder]
GO
DROP TABLE [dbo].[ChannelAdvisorOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrder]', N'ChannelAdvisorOrder'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrder] on [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [PK_ChannelAdvisorOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD
CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [DF_ChannelAdvisorOrder_OnlineShippingStatus]
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [DF_ChannelAdvisorOrder_OnlineCheckoutStatus]
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] DROP CONSTRAINT [DF_ChannelAdvisorOrder_OnlinePaymentStatus]
GO