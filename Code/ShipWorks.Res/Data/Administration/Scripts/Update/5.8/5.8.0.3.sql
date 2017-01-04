PRINT N'Updating [dbo].[ShippingSettings]'
GO
UPDATE ShippingSettings
SET	FedExUsername = 'Y3AiLKySX4L5Vjmd',
	FedExPassword = 'hN6BUfrJeg8pdsADnc/bS7bKlLDM5CFAqjBdqwoM2D4='
	WHERE ISNULL(FedExUsername,'') != ''
GO