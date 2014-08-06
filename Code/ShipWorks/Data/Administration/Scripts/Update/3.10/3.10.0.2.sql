﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] DROP CONSTRAINT[FK_ChannelAdvisorStore_Store]
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
[DownloadCriteria] [smallint] NOT NULL,
[ProfileID] [int] NOT NULL,
[AttributesToDownload] [xml] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorStore]([StoreID], [AccountKey], [DownloadCriteria], [ProfileID],[AttributesToDownload]) 
	SELECT [StoreID], [AccountKey], [DownloadCriteria], [ProfileID], '<x>some array that doesnt exist yet.  update the update script.</x>'
	FROM [dbo].[ChannelAdvisorStore]
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

