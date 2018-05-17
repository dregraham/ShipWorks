SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[MagentoStore]'
GO
IF NOT EXISTS(SELECT *
	FROM sys.all_columns
		WHERE [object_id] = OBJECT_ID('MagentoStore')
			AND [name] = 'UpdateSplitOrderOnlineStatus')
BEGIN
	ALTER TABLE [dbo].[MagentoStore]
		ADD [UpdateSplitOrderOnlineStatus] BIT NOT NULL CONSTRAINT [DF_MagentoStore_UpdateSplitOrderOnlineStatus] DEFAULT(0)
	
	ALTER TABLE [dbo].[MagentoStore] DROP CONSTRAINT [DF_MagentoStore_UpdateSplitOrderOnlineStatus]
END
