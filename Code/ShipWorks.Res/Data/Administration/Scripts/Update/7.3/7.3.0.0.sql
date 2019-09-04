PRINT N'Altering [dbo].[OrderItem]'
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'OrderItem' AND COLUMN_NAME = 'HubItemID')
BEGIN
	ALTER TABLE OrderItem
		ADD HubItemID NVARCHAR(50) NOT NULL CONSTRAINT [DF_OrderItem_HubItemID] DEFAULT ('')
END
GO

PRINT N'Dropping constraints from [dbo].[OrderItem]'
GO
IF EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('OrderItem') AND name = 'DF_OrderItem_HubItemID')
	ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_HubItemID]
GO
