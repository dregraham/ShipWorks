PRINT N'Creating Shipment.[IX_SWDefault_Shipment_ProcessedVoidedOnlineShipmentIDShipmentType] index'
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Shipment]') 
                                 AND name = N'IX_SWDefault_Shipment_ProcessedVoidedOnlineShipmentIDShipmentType')
BEGIN
	DROP INDEX [dbo].[Shipment].[IX_SWDefault_Shipment_ProcessedVoidedOnlineShipmentIDShipmentType]
END	
GO	
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ProcessedVoidedOnlineShipmentIDShipmentType] ON [dbo].[Shipment]
(
	[Processed] ASC,
	[Voided] ASC,
	[OnlineShipmentID] ASC,
	[ShipmentType] ASC
) ON [PRIMARY]
GO
