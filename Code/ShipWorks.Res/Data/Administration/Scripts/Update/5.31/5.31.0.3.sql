PRINT N'Altering [dbo].[UserSettings]'
GO
IF COL_LENGTH(N'[dbo].[UserSettings]', N'OrderLookupLayout') IS NULL
ALTER TABLE [dbo].[UserSettings] ADD[OrderLookupLayout] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO