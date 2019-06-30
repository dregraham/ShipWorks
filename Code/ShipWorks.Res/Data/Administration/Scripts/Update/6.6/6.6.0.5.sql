PRINT N'Altering [dbo].[Download]'
GO

IF COL_LENGTH(N'[dbo].[Download]', N'BatchID') IS NULL
ALTER TABLE [dbo].[Download] ADD [BatchID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Download_BatchID] DEFAULT NEWID()
GO

IF OBJECT_ID('[dbo].[DF_Download_BatchID]', 'D') IS NOT NULL
ALTER TABLE [dbo].[Download] DROP CONSTRAINT [DF_Download_BatchID]
GO

