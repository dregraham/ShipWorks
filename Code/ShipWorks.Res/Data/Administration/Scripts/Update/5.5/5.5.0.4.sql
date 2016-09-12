/***   Shipment   ***/
PRINT N'Creating index [IX_Shipment_ShipDate] on [dbo].[Shipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Shipment_ShipDate' AND object_id = OBJECT_ID(N'[dbo].[Shipment]'))
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipDate] ON [dbo].[Shipment] ([ShipDate]) INCLUDE ([OrderID])
GO

PRINT N'Creating index [IX_Shipment_ShipmentType] on [dbo].[Shipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Shipment_ShipmentType' AND object_id = OBJECT_ID(N'[dbo].[Shipment]'))
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipmentType] ON [dbo].[Shipment] ([ShipmentType]) INCLUDE ([OrderID])
GO

PRINT N'Dropping index [IX_Shipment_ProcessedOrderID] from [dbo].[Shipment]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Shipment_ProcessedOrderID' AND object_id = OBJECT_ID(N'[dbo].[Shipment]'))
DROP INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment]
GO

PRINT N'Creating index [IX_Shipment_ProcessedOrderID] on [dbo].[Shipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Shipment_ProcessedOrderID' AND object_id = OBJECT_ID(N'[dbo].[Shipment]'))
CREATE NONCLUSTERED INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC, [ProcessedDate]) INCLUDE ([OrderID], [Voided])
GO

PRINT N'Creating index [IX_Shipment_ReturnShipment] on [dbo].[Shipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Shipment_ReturnShipment' AND object_id = OBJECT_ID(N'[dbo].[Shipment]'))
CREATE NONCLUSTERED INDEX [IX_Shipment_ReturnShipment] ON [dbo].[Shipment] ([ReturnShipment]) INCLUDE ([OrderID])
GO

/***   FedEx Shipment   ***/
PRINT N'Creating index [IX_FedExShipment_Service] on [dbo].[FedExShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FedExShipment_Service' AND object_id = OBJECT_ID(N'[dbo].[FedExShipment]'))
CREATE NONCLUSTERED INDEX [IX_FedExShipment_Service] ON [dbo].[FedExShipment] ([Service])
GO
PRINT N'Creating index [IX_FedExShipment_PackagingType] on [dbo].[FedExShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FedExShipment_PackagingType' AND object_id = OBJECT_ID(N'[dbo].[FedExShipment]'))
CREATE NONCLUSTERED INDEX [IX_FedExShipment_PackagingType] ON [dbo].[FedExShipment] ([PackagingType])
GO

/***   iParcel Shipment   ***/
PRINT N'Creating index [IX_IParcelShipment_Service] on [dbo].[iParcelShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_IParcelShipment_Service' AND object_id = OBJECT_ID(N'[dbo].[iParcelShipment]'))
CREATE NONCLUSTERED INDEX [IX_IParcelShipment_Service] ON [dbo].[iParcelShipment] ([Service])
GO

/***   OnTrac Shipment   ***/
PRINT N'Creating index [IX_OnTracShipment_Service] on [dbo].[OnTracShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OnTracShipment_Service' AND object_id = OBJECT_ID(N'[dbo].[OnTracShipment]'))
CREATE NONCLUSTERED INDEX [IX_OnTracShipment_Service] ON [dbo].[OnTracShipment] ([Service])
GO
PRINT N'Creating index [IX_OnTracShipment_PackagingType] on [dbo].[OnTracShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OnTracShipment_PackagingType' AND object_id = OBJECT_ID(N'[dbo].[OnTracShipment]'))
CREATE NONCLUSTERED INDEX [IX_OnTracShipment_PackagingType] ON [dbo].[OnTracShipment] ([PackagingType])
GO

/***   Postal Shipment   ***/
PRINT N'Creating index [IX_PostalShipment_Service] on [dbo].[PostalShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_PostalShipment_Service' AND object_id = OBJECT_ID(N'[dbo].[PostalShipment]'))
CREATE NONCLUSTERED INDEX [IX_PostalShipment_Service] ON [dbo].[PostalShipment] ([Service])
GO
PRINT N'Creating index [IX_PostalShipment_PackagingType] on [dbo].[PostalShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_PostalShipment_PackagingType' AND object_id = OBJECT_ID(N'[dbo].[PostalShipment]'))
CREATE NONCLUSTERED INDEX [IX_PostalShipment_PackagingType] ON [dbo].[PostalShipment] ([PackagingType])
GO
PRINT N'Creating index [IX_PostalShipment_Confirmation] on [dbo].[PostalShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_PostalShipment_Confirmation' AND object_id = OBJECT_ID(N'[dbo].[PostalShipment]'))
CREATE NONCLUSTERED INDEX [IX_PostalShipment_Confirmation] ON [dbo].[PostalShipment] ([Confirmation])
GO

/***   Ups Shipment   ***/
PRINT N'Creating index [IX_UpsShipment_Service] on [dbo].[UpsShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UpsShipment_Service' AND object_id = OBJECT_ID(N'[dbo].[UpsShipment]'))
CREATE NONCLUSTERED INDEX [IX_UpsShipment_Service] ON [dbo].[UpsShipment] ([Service])
GO
PRINT N'Creating index [IX_UpsShipment_DeliveryConfirmation] on [dbo].[UpsShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UpsShipment_DeliveryConfirmation' AND object_id = OBJECT_ID(N'[dbo].[UpsShipment]'))
CREATE NONCLUSTERED INDEX [IX_UpsShipment_DeliveryConfirmation] ON [dbo].[UpsShipment] ([DeliveryConfirmation])
GO
PRINT N'Creating index [IX_UpsPackage_PackagingType] on [dbo].[UpsPackage]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UpsPackage_PackagingType' AND object_id = OBJECT_ID(N'[dbo].[UpsPackage]'))
CREATE NONCLUSTERED INDEX [IX_UpsPackage_PackagingType] ON [dbo].[UpsPackage] ([PackagingType])
GO