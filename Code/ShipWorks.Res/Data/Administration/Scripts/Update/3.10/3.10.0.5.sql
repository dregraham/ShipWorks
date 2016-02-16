SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT[FK_ChannelAdvisorOrderItem_OrderItem]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [PK_ChannelAdvisorOrderItem]
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[MarketplaceName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceStoreName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceBuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceSalesID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Classification] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DistributionCenter] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HarmonizedCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsFBA] [bit] NOT NULL,
[MPN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]([OrderItemID], [MarketplaceName], [MarketplaceStoreName], [MarketplaceBuyerID], [MarketplaceSalesID], [Classification], [DistributionCenter], [HarmonizedCode], [IsFBA], [MPN]) 
	SELECT  [OrderItemID], [MarketplaceName], '',
			[MarketplaceBuyerID], [MarketplaceSalesID], [Classification], [DistributionCenter], [HarmonizedCode], [IsFBA], [MPN] 
	FROM [dbo].[ChannelAdvisorOrderItem]
GO
DROP TABLE [dbo].[ChannelAdvisorOrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]', N'ChannelAdvisorOrderItem'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderItem] on [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrderItem_MarketPlaceName] on [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_MarketPlaceName] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceName])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrderItem_MarketplaceStoreName] on [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_MarketplaceStoreName] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceStoreName]) INCLUDE ([MarketplaceBuyerID], [MarketplaceSalesID])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrderItem_MarketplaceBuyerID] on [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_MarketplaceBuyerID] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceBuyerID])
GO
PRINT N'Creating index [IX_ChannelAdvisorOrderItem_MarketplaceSalesID] on [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ChannelAdvisorOrderItem_MarketplaceSalesID] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceSalesID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO












