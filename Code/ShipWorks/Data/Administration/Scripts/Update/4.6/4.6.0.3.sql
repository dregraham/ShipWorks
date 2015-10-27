SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_Shipment_ProcessedOrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment]
GO
PRINT N'Creating index [IX_Shipment_ProcessedOrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC) INCLUDE ([OrderID], [Voided]) WITH (FILLFACTOR=75)
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_Shipment_ShipAddressValidationStatus] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ShipAddressValidationStatus] ON [dbo].[Shipment]
GO
PRINT N'Creating index [IX_Shipment_ShipAddressValidationStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipAddressValidationStatus] ON [dbo].[Shipment] ([ShipAddressValidationStatus] DESC) INCLUDE ([OrderID], [Processed], [Voided])
GO