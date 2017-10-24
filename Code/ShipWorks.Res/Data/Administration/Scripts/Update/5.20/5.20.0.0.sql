PRINT N'Updating [dbo].[ShippingSettings]'
GO
UPDATE ShippingSettings
SET	FedExUsername = 'MFG2EvMKBLcxcCsk',
	FedExPassword = 'nF4kG4o3/NwRrGa+QhLZtw95OnmtqNMr6mhhziyFEYE='
	WHERE ISNULL(FedExUsername,'') != ''
GO