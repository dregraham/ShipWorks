SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[PayPalStore]'
GO
IF EXISTS(SELECT *
	FROM sys.all_columns
		WHERE [object_id] = OBJECT_ID('PayPalStore')
			AND [name] = 'ApiCertificate'
			AND [max_length] < 4096)
BEGIN
	ALTER TABLE PayPalStore
		ALTER COLUMN ApiCertificate VARBINARY(4096) NULL
END
GO
PRINT N'Altering [dbo].[EbayStore]'
GO
IF EXISTS(SELECT *
	FROM sys.all_columns
		WHERE [object_id] = OBJECT_ID('EbayStore')
			AND [name] = 'PayPalApiCertificate'
			AND [max_length] < 4096)
BEGIN
	ALTER TABLE EbayStore
		ALTER COLUMN PayPalApiCertificate VARBINARY(4096) NULL
END
GO
