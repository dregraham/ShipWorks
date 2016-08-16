SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT N'Altering [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD
	[ReturnService] INT NOT NULL CONSTRAINT DF_UpsShipment_ReturnService DEFAULT 0,
	[ReturnUndeliverableEmail] nvarchar(50) NOT NULL CONSTRAINT DF_UpsShipment_ReturnUndeliverableEmail DEFAULT '',
	[ReturnContents] nvarchar(300) NOT NULL CONSTRAINT DF_UpsShipment_ReturnContents DEFAULT ''
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT DF_UpsShipment_ReturnService
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT DF_UpsShipment_ReturnUndeliverableEmail
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT DF_UpsShipment_ReturnContents
GO

PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
	ReturnShipment BIT NOT NULL CONSTRAINT DF_Shipment_ReturnShipment DEFAULT 0
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT DF_Shipment_ReturnShipment
GO

PRINT N'Altering [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].UpsProfile ADD
	[ReturnService] INT NULL,
	[ReturnUndeliverableEmail] nvarchar(50) NULL,
	[ReturnContents] nvarchar(300) NULL
GO

PRINT N'Updating primary UPS Profile'
GO
UPDATE [dbo].UpsProfile
SET 
	ReturnService = 0,
	ReturnUndeliverableEmail = '',
	ReturnContents = ''
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (0,1) AND ShipmentTypePrimary = 1)

-- Worldship Shipment ID fix
PRINT N'Altering [dbo].[WorldShipProcessed]'
GO
ALTER TABLE [dbo].[WorldShipProcessed] ALTER COLUMN [ShipmentID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO