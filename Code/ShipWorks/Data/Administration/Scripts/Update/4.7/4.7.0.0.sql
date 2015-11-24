PRINT N'Altering [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD
[YahooStoreID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__YahooStor__Yahoo__3B0BC30C] DEFAULT (''),
[AccessToken] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__YahooStor__Acces__3BFFE745] DEFAULT (''),
[BackupOrderNumber] [bigint] NULL
GO

PRINT N'Altering [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD
[Url] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT ('')
GO