PRINT N'ALTERING [dbo].[Shipment].IX_SWDefault_Shipment_TrackingStatus'
GO

IF EXISTS (SELECT * FROM sys.indexes 
		   WHERE object_id = OBJECT_ID(N'[dbo].[Shipment]') 
			 AND name = N'IX_SWDefault_Shipment_TrackingStatus')
BEGIN
	DROP INDEX [IX_SWDefault_Shipment_TrackingStatus] ON [dbo].[Shipment]
END

CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_TrackingStatus] ON [dbo].[Shipment]
(
	[TrackingStatus] ASC
)
INCLUDE([ShipmentType],[Processed],[Voided],[TrackingNumber],[TrackingHubTimestamp]) 
GO
