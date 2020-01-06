PRINT N'Altering [dbo].[ProductVariant]'
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('ProductVariant') AND [name] = 'FNSku')
BEGIN
	ALTER TABLE ProductVariant
		ADD FNSku [nvarchar](300) NULL
END
GO

IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('ProductVariant') AND [name] = 'EAN')
BEGIN
	ALTER TABLE ProductVariant
		ADD EAN [nvarchar](30) NULL
END
GO
