PRINT N'Altering [dbo].[FedExProfile]'
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('FedExProfile') AND [name] = 'CreateCommercialInvoice')
BEGIN
	ALTER TABLE FedExProfile
		ADD CreateCommercialInvoice BIT NULL
END
GO

IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('FedExProfile') AND [name] = 'FileElectronically')
BEGIN
	ALTER TABLE FedExProfile
		ADD FileElectronically BIT NULL
END
GO
