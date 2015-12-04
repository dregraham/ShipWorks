PRINT N'Altering [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD
[YahooStoreID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
[AccessToken] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
[BackupOrderNumber] [bigint] NULL
GO

PRINT N'Altering [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD
[Url] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT ('')
GO