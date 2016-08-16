SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT * FROM sys.all_columns INNER JOIN sys.tables ON sys.all_columns.object_id = sys.tables.object_id 
						WHERE sys.tables.name = 'ChannelAdvisorOrderItem' AND sys.all_columns.name = 'MPN')
BEGIN
	PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrderItem]'
END
GO
IF NOT EXISTS (SELECT * FROM sys.all_columns INNER JOIN sys.tables ON sys.all_columns.object_id = sys.tables.object_id 
						WHERE sys.tables.name = 'ChannelAdvisorOrderItem' AND sys.all_columns.name = 'MPN')
BEGIN
	ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT[FK_ChannelAdvisorOrderItem_OrderItem]
END
GO
IF NOT EXISTS (SELECT * FROM sys.all_columns INNER JOIN sys.tables ON sys.all_columns.object_id = sys.tables.object_id 
						WHERE sys.tables.name = 'ChannelAdvisorOrderItem' AND sys.all_columns.name = 'MPN')
BEGIN
	PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrderItem]'
END
GO
IF NOT EXISTS (SELECT * FROM sys.all_columns INNER JOIN sys.tables ON sys.all_columns.object_id = sys.tables.object_id 
						WHERE sys.tables.name = 'ChannelAdvisorOrderItem' AND sys.all_columns.name = 'MPN')
BEGIN
	ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [PK_ChannelAdvisorOrderItem]
END
GO
IF NOT EXISTS (SELECT * FROM sys.all_columns INNER JOIN sys.tables ON sys.all_columns.object_id = sys.tables.object_id 
						WHERE sys.tables.name = 'ChannelAdvisorOrderItem' AND sys.all_columns.name = 'MPN')
BEGIN
	PRINT N'Rebuilding [dbo].[ChannelAdvisorOrderItem]'
END
GO
IF NOT EXISTS (SELECT * FROM sys.all_columns INNER JOIN sys.tables ON sys.all_columns.object_id = sys.tables.object_id 
						WHERE sys.tables.name = 'ChannelAdvisorOrderItem' AND sys.all_columns.name = 'MPN')
BEGIN
	CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]
	(
	[OrderItemID] [bigint] NOT NULL,
	[MarketplaceName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MarketplaceBuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MarketplaceSalesID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Classification] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DistributionCenter] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[HarmonizedCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IsFBA] [bit] NOT NULL,
	[MPN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	)
END
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tmp_rg_xx_ChannelAdvisorOrderItem')
BEGIN
	INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]([OrderItemID], [MarketplaceName], [MarketplaceBuyerID], [MarketplaceSalesID], [Classification], [DistributionCenter], [HarmonizedCode], [IsFBA], [MPN]) SELECT [OrderItemID], [MarketplaceName], [MarketplaceBuyerID], [MarketplaceSalesID], [Classification], [DistributionCenter], [HarmonizedCode], [IsFBA], '' FROM [dbo].[ChannelAdvisorOrderItem]
END
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tmp_rg_xx_ChannelAdvisorOrderItem')
BEGIN
	DROP TABLE [dbo].[ChannelAdvisorOrderItem]
END
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tmp_rg_xx_ChannelAdvisorOrderItem')
BEGIN
	EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]', N'ChannelAdvisorOrderItem'
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_ChannelAdvisorOrderItem')
BEGIN
	PRINT N'Creating primary key [PK_ChannelAdvisorOrderItem] on [dbo].[ChannelAdvisorOrderItem]'
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_ChannelAdvisorOrderItem')
BEGIN
	ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ChannelAdvisorOrderItem_OrderItem')
BEGIN
	PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ChannelAdvisorOrderItem_OrderItem')
BEGIN
	ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
END
GO