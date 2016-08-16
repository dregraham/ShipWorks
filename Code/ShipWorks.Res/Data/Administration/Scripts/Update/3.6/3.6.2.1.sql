PRINT N'Defaulting UPS Profile ShipmentChargeType to Receiver'
GO

UPDATE UpsProfile SET ShipmentChargeType = 1 WHERE ShipmentChargeType IS NULL

GO
