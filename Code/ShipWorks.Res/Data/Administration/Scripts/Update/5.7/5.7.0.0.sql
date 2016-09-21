disable trigger FilterDirtyOrder ON [Order];
disable trigger OrderAuditTrigger ON [Order];
disable trigger OrderLabelTrigger ON [Order];
disable trigger OrderRollupTrigger ON [Order];
disable trigger OrderShipmentShipSenseStatusTrigger ON [Order];
disable trigger OrderShipSenseRecognitionStatusTrigger ON [Order];
GO
UPDATE orderTable
SET orderTable.RollupItemQuantity = (Select SUM(Quantity) from [OrderItem] oi WHERE orderTable.OrderID = oi.OrderID)
From [Order] as orderTable;
GO
enable trigger FilterDirtyOrder ON [Order];
enable trigger OrderAuditTrigger ON [Order];
enable trigger OrderLabelTrigger ON [Order];
enable trigger OrderRollupTrigger ON [Order];
enable trigger OrderShipmentShipSenseStatusTrigger ON [Order];
enable trigger OrderShipSenseRecognitionStatusTrigger ON [Order];
GO