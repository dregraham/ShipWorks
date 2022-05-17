PRINT N'Altering [dbo].[Store]';
GO

IF COL_LENGTH(N'[dbo].[Store]', N'OrderSourceID') IS NULL
	ALTER TABLE [dbo].[Store] ADD [OrderSourceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
GO

IF OBJECT_ID(N'[dbo].[PlatformStore]', 'u') IS NOT NULL
BEGIN

	PRINT N'Moving OrderSourceIDs to [dbo].[Store]';
	UPDATE [dbo].[Store] 
	SET [dbo].[Store].[OrderSourceID] = [dbo].[PlatformStore].[OrderSourceID]
	FROM [dbo].[Store] 
	INNER JOIN [dbo].[PlatformStore] 
	ON [dbo].[Store].[StoreID] = [dbo].[PlatformStore].[StoreID];

	PRINT N'Dropping [dbo].[PlatformStore]';
	DROP TABLE [dbo].[PlatformStore];
END
GO