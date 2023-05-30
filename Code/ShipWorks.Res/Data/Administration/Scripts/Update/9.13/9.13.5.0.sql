PRINT N'Altering [dbo].[OrderSearch]'
GO

IF COL_LENGTH(N'[dbo].[OrderSearch]', N'[OriginalChannelOrderID]') IS NULL
BEGIN
	ALTER TABLE [dbo].OrderSearch ADD [OriginalChannelOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
END
GO
