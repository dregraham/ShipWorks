PRINT N'Dropping Order Triggers'
GO
IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[FilterDirtyOrder]'))
	DROP TRIGGER [dbo].[FilterDirtyOrder]
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderAuditTrigger]'))
	DROP TRIGGER [dbo].[OrderAuditTrigger]
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderLabelTrigger]'))
	DROP TRIGGER [dbo].[OrderLabelTrigger]
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderRollupTrigger]'))
	DROP TRIGGER [dbo].[OrderRollupTrigger]
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderShipmentShipSenseStatusTrigger]'))
	DROP TRIGGER [dbo].[OrderShipmentShipSenseStatusTrigger]
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderShipSenseRecognitionStatusTrigger]'))
	DROP TRIGGER [dbo].[OrderShipSenseRecognitionStatusTrigger]
GO

PRINT N'Updating Order Table'
GO
UPDATE orderTable
SET orderTable.RollupItemQuantity = ISNULL((SELECT SUM(Quantity) FROM [OrderItem] oi WHERE orderTable.OrderID = oi.OrderID), 0)
FROM [Order] AS orderTable
WHERE orderTable.RollupItemQuantity IS NULL
GO
