SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Removing [dbo].[AmazonAccount]'
GO
DROP TABLE AmazonAccount
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT [FK_ChannelAdvisorStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT [PK_ChannelAdvisorStore]
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorStore]
(
[StoreID] [bigint] NOT NULL,
[AccountKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProfileID] [int] NOT NULL,
[AttributesToDownload] [xml] NOT NULL,
[ConsolidatorAsUsps] [bit] NOT NULL,
[AmazonMerchantID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonAuthToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonApiRegion] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonShippingToken] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorStore]([StoreID], [AccountKey], [ProfileID], [AttributesToDownload], [ConsolidatorAsUsps], [AmazonMerchantID], [AmazonAuthToken], [AmazonApiRegion], [AmazonShippingToken]) 
SELECT [StoreID], [AccountKey], [ProfileID], [AttributesToDownload], [ConsolidatorAsUsps], '', '', '', 'EQz7Xib9kyERqbpYh2kanA3PEQW9w8fJuqnqv2NvwnKbskplppGJhWdhMSNtviyV0ydWlVaqjGVf2i1bFdcGC0AEZ7oIt/Ef6Ylk4ND+JhohkEzm5QOu+r/YLUSW1pNpDjs3edufn//xPGrxbvHb1y/HiKB4xop2oarh3PNoYaZIyIbQYJAv75H7btreUkEkiY4QBu2CwwOH5yjb58nELqPjn4dKJ6LN5PZ7+dRuxfT9ccV7rAj411vUieNtZlcTRkLyK+NDZu0AslwnC3/1wgJmdta8fXzH5sYtPBHIAN2HSzugrrguODS0iy5w9o8m' FROM [dbo].[ChannelAdvisorStore]
GO
DROP TABLE [dbo].[ChannelAdvisorStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorStore]', N'ChannelAdvisorStore'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorStore] on [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD CONSTRAINT [PK_ChannelAdvisorStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD CONSTRAINT [FK_ChannelAdvisorStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO