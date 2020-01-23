PRINT N'Altering [dbo].[Store]'
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('Store') AND [name] = 'ShipEngineOrderSourceID')
BEGIN
	ALTER TABLE Store
		ADD ShipEngineOrderSourceID UNIQUEIDENTIFIER NULL
END
GO
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('Store') AND [name] = 'ShipEngineAccountID')
BEGIN
	ALTER TABLE Store
		ADD ShipEngineAccountID NVARCHAR(50) NULL
END
GO