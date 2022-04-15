PRINT N'Altering [dbo].[Store]';
GO

IF COL_LENGTH(N'[dbo].[Store]', N'OrderSourceID') IS NULL
	ALTER TABLE [dbo].[Store] ADD [OrderSourceID] [nvarchar] (50) NULL;
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

PRINT N'Altering [dbo].[AmazonStore]';
GO

IF COL_LENGTH(N'[dbo].[AmazonStore]', N'MaxOrderDatePreMigration') IS NULL
	ALTER TABLE [dbo].[AmazonStore] ADD [MaxOrderDatePreMigration] [datetime] NULL;
GO

PRINT N'Setting MaxOrderDatePreMigration on [dbo].[AmazonStore]';
UPDATE [dbo].[AmazonStore] 
SET [dbo].[AmazonStore].[MaxOrderDatePreMigration] = [MaxOrderDate]
FROM [dbo].[AmazonStore]
INNER JOIN 
( 
	SELECT [StoreID], MAX([OrderDate]) AS [MaxOrderDate]
	FROM [dbo].[Order]
	GROUP BY [StoreID]
) [MaxOrderDates] ON [dbo].[AmazonStore].[StoreID] = [MaxOrderDates].[StoreID]
WHERE [dbo].[AmazonStore].[MaxOrderDatePreMigration] IS NULL;