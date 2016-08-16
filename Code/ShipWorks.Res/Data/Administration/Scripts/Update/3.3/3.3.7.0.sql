ALTER TABLE dbo.EndiciaAccount ADD
	ScanFormAddressSource int NOT NULL CONSTRAINT DF_EndiciaAccount_ScanFormAddressSource DEFAULT 0
GO

ALTER TABLE dbo.EndiciaAccount
	DROP CONSTRAINT DF_EndiciaAccount_ScanFormAddressSource
GO