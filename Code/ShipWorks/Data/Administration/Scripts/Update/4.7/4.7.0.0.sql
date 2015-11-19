PRINT N'Altering [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD
[Url] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__YahooOrderI__Url__382F5661] DEFAULT ('')
GO