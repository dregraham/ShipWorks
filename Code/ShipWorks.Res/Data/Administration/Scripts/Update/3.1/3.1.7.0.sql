SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD 
	BillNameParseStatus INT NOT NULL CONSTRAINT DF_Order_BillNameParseStatus DEFAULT 1,
	BillUnparsedName NVARCHAR(100) NOT NULL CONSTRAINT DF_Order_BillUnparsedName DEFAULT '', 
	ShipNameParseStatus INT NOT NULL CONSTRAINT DF_Order_ShipNameParseStatus DEFAULT 1,
	ShipUnparsedName NVARCHAR(100) NOT NULL CONSTRAINT DF_Order_ShipUnparsedName DEFAULT ''
GO

ALTER TABLE [dbo].[Order] DROP CONSTRAINT DF_Order_BillNameParseStatus
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT DF_Order_BillUnparsedName
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT DF_Order_ShipNameParseStatus
GO
ALTER TABLE [dbo].[Order] DROP CONSTRAINT DF_Order_ShipUnparsedName
GO

PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
	ShipNameParseStatus INT NOT NULL CONSTRAINT DF_Shipment_ShipNameParseStatus DEFAULT 1,
	ShipUnparsedName NVARCHAR(100) NOT NULL CONSTRAINT DF_Shipment_ShipUnparsedName DEFAULT '',
	OriginNameParseStatus INT NOT NULL CONSTRAINT DF_Shipment_OriginNameParseStatus DEFAULT 1,
	OriginUnparsedName NVARCHAR(100) NOT NULL CONSTRAINT DF_Shipment_OriginUnparsedName DEFAULT ''
GO

ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT DF_Shipment_ShipNameParseStatus
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT DF_Shipment_ShipUnparsedName
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT DF_Shipment_OriginNameParseStatus
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT DF_Shipment_OriginUnparsedName
GO

PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipNameParseStatus'
GO