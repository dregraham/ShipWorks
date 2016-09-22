disable trigger FilterDirtyOrder ON [Order];
disable trigger OrderAuditTrigger ON [Order];
disable trigger OrderLabelTrigger ON [Order];
disable trigger OrderRollupTrigger ON [Order];
disable trigger OrderShipmentShipSenseStatusTrigger ON [Order];
disable trigger OrderShipSenseRecognitionStatusTrigger ON [Order];
GO
UPDATE orderTable
SET orderTable.RollupItemQuantity = ISNULL((SELECT SUM(Quantity) FROM [OrderItem] oi WHERE orderTable.OrderID = oi.OrderID), 0)
FROM [Order] AS orderTable
WHERE orderTable.RollupItemQuantity IS NULL
GO
enable trigger FilterDirtyOrder ON [Order];
enable trigger OrderAuditTrigger ON [Order];
enable trigger OrderLabelTrigger ON [Order];
enable trigger OrderRollupTrigger ON [Order];
enable trigger OrderShipmentShipSenseStatusTrigger ON [Order];
enable trigger OrderShipSenseRecognitionStatusTrigger ON [Order];
GO