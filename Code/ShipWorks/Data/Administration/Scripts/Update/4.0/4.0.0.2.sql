SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating index [IX_Order_BillAddressValidationStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_BillAddressValidationStatus] ON [dbo].[Order] ([BillAddressValidationStatus] DESC)
GO
PRINT N'Creating index [IX_Order_BillMilitaryAddress] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_BillMilitaryAddress] ON [dbo].[Order] ([BillMilitaryAddress] DESC)
GO
PRINT N'Creating index [IX_Order_BillPOBox] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_BillPOBox] ON [dbo].[Order] ([BillPOBox] DESC)
GO
PRINT N'Creating index [IX_Order_BillResidentialStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_BillResidentialStatus] ON [dbo].[Order] ([BillResidentialStatus] DESC)
GO
PRINT N'Creating index [IX_Order_BillUSTerritory] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_BillUSTerritory] ON [dbo].[Order] ([BillUSTerritory] DESC)
GO
PRINT N'Creating index [IX_Order_ShipAddressValidationStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_ShipAddressValidationStatus] ON [dbo].[Order] ([ShipAddressValidationStatus] DESC)
GO
PRINT N'Creating index [IX_Auto_ShipFirstName] on [dbo].[Order]'
GO
-- *********************
-- Purposely using If Not Exists here because this index may already exist in some db's and we dont' want
-- to fail on upgrade.
-- *********************
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = N'IX_Auto_ShipFirstName')
CREATE NONCLUSTERED INDEX [IX_Auto_ShipFirstName] ON [dbo].[Order]
(
	[ShipFirstName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
PRINT N'Creating index [IX_Order_ShipMilitaryAddress] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_ShipMilitaryAddress] ON [dbo].[Order] ([ShipMilitaryAddress] DESC)
GO
PRINT N'Creating index [IX_Order_ShipPOBox] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_ShipPOBox] ON [dbo].[Order] ([ShipPOBox] DESC)
GO
PRINT N'Creating index [IX_Order_ShipResidentialStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_ShipResidentialStatus] ON [dbo].[Order] ([ShipResidentialStatus] DESC)
GO
PRINT N'Creating index [IX_Order_ShipUSTerritory] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Order_ShipUSTerritory] ON [dbo].[Order] ([ShipUSTerritory] DESC)
GO
PRINT N'Creating index [IX_Shipment_ShipAddressValidationStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipAddressValidationStatus] ON [dbo].[Shipment] ([ShipAddressValidationStatus] DESC) INCLUDE ([Processed])
GO
PRINT N'Creating index [IX_Shipment_ShipMilitaryAddress] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipMilitaryAddress] ON [dbo].[Shipment] ([ShipMilitaryAddress] DESC)
GO
PRINT N'Creating index [IX_Shipment_ShipPOBox] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipPOBox] ON [dbo].[Shipment] ([ShipPOBox] DESC)
GO
PRINT N'Creating index [IX_Shipment_ShipResidentialStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipResidentialStatus] ON [dbo].[Shipment] ([ShipResidentialStatus] DESC)
GO
PRINT N'Creating index [IX_Shipment_ShipUSTerritory] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ShipUSTerritory] ON [dbo].[Shipment] ([ShipUSTerritory] DESC)
GO
