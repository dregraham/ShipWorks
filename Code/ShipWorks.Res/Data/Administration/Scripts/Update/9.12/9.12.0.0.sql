PRINT N'Altering [dbo].[Store]'
GO

IF COL_LENGTH(N'[dbo].[Store]', N'ContinuationToken') IS NULL
BEGIN
	ALTER TABLE [dbo].[Store] ADD [ContinuationToken] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
END
GO

PRINT N'Altering [dbo].[AmazonStore]'
GO
IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ContinuationToken' AND [object_id] = OBJECT_ID(N'AmazonStore'))
BEGIN
	UPDATE [dbo].[Store]
	SET [dbo].[Store].[ContinuationToken]=[dbo].[AmazonStore].[ContinuationToken]
	FROM [dbo].[AmazonStore] 
	WHERE [dbo].[Store].[StoreID]=[dbo].[AmazonStore].[StoreID]
	
	ALTER TABLE [dbo].[AmazonStore] DROP COLUMN [ContinuationToken]
END
GO
