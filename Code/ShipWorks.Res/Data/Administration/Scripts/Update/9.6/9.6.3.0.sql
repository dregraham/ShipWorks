PRINT N'ALTERING [dbo].[AmazonStore]'
GO
IF COL_LENGTH(N'[dbo].[AmazonStore]', N'ContinuationToken') IS NULL
ALTER TABLE [dbo].[AmazonStore] ADD [ContinuationToken] [nvarchar] (2048) NULL
GO