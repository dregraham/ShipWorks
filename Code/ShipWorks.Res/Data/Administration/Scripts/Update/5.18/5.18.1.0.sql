PRINT N'Altering [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD
[AmazonVATS] [bit] NOT NULL CONSTRAINT [DF_AmazonStore_AmazonVATS] DEFAULT (0)
GO
PRINT N'Dropping constraints from [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] DROP CONSTRAINT [DF_AmazonStore_AmazonVATS]
GO