PRINT N'Altering [dbo].[OrderItem]'
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'OrderItem' AND COLUMN_NAME = 'HubItemID')
BEGIN
	ALTER TABLE OrderItem
		ADD HubItemID NVARCHAR(50) NULL
END
GO
