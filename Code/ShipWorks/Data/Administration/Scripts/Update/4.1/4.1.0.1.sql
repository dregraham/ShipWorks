SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_Order_ShipAddressValidationStatus] from [dbo].[Order]'
GO
DROP INDEX [IX_Order_ShipAddressValidationStatus] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Shipment_ProcessedOrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment]
GO
PRINT N'Creating index [IX_Order_ShipAddressValidationStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_ShipAddressValidationStatus] ON [dbo].[Order] ([ShipAddressValidationStatus] DESC) INCLUDE ([OrderDate])
GO
PRINT N'Creating index [IX_Shipment_ProcessedOrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC) INCLUDE ([OrderID], [Voided])
GO
