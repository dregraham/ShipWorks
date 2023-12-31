SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Setting Equaship Shipments to None'
GO
UPDATE Shipment
SET ShipmentType = 99
WHERE shipmenttype = 10
GO
PRINT N'Deleting Equaship Profiles'
GO
DELETE ShippingProfile
WHERE ShipmentType = 10
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[EquaShipAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping foreign keys from [dbo].[EquaShipProfile]'
GO
ALTER TABLE [dbo].[EquaShipProfile] DROP CONSTRAINT [FK_EquashipProfile_ShippingProfile]
GO
PRINT N'Dropping foreign keys from [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] DROP CONSTRAINT [FK_EquashipShipment_Shipment]
GO
PRINT N'Dropping constraints from [dbo].[EquaShipProfile]'
GO
ALTER TABLE [dbo].[EquaShipProfile] DROP CONSTRAINT [PK_EquashipProfile]
GO
PRINT N'Dropping constraints from [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] DROP CONSTRAINT [PK_EquashipShipment]
GO
PRINT N'Dropping constraints from [dbo].[EquaShipAccount]'
GO
ALTER TABLE [dbo].[EquaShipAccount] DROP CONSTRAINT [PK_EquahipAccount]
GO
PRINT N'Dropping [dbo].[EquaShipAccount]'
GO
DROP TABLE [dbo].[EquaShipAccount]
GO
PRINT N'Dropping [dbo].[EquaShipShipment]'
GO
DROP TABLE [dbo].[EquaShipShipment]
GO
PRINT N'Dropping [dbo].[EquaShipProfile]'
GO
DROP TABLE [dbo].[EquaShipProfile]
GO