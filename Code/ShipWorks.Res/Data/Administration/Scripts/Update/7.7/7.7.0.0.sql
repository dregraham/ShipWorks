PRINT N'Altering [dbo].[Store]'
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('Store') AND [name] = 'ShipEngineOrderSourceID')
BEGIN
	ALTER TABLE FedExProfile
		ADD ShipEngineOrderSourceID UNIQUEIDENTIFIER NULL
END
GO

