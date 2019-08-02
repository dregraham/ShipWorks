PRINT N'Altering [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] ALTER COLUMN [Identifier] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
