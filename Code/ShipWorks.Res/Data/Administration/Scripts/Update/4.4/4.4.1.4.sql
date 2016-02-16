PRINT N'Updating [dbo].[ShippingSettings]'
GO
UPDATE ShippingSettings
SET	FedExUsername = 'L3eNgex1LGADpYBc',
	FedExPassword = 'El2xbZr107yCknuaLrgDdaIOgia+/ts6IBDZqv8GWVU='
	WHERE ISNULL(FedExUsername,'') != ''
GO