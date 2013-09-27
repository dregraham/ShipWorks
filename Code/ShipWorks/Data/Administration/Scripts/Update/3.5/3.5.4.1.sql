SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD
[ShipmentChargeType] [int] NULL,
[ShipmentChargeAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentChargePostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentChargeCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

UPDATE UpsShipment
	set [ShipmentChargeType] = 0, [ShipmentChargeAccount] = '', [ShipmentChargePostalCode] = '', [ShipmentChargeCountryCode] = ''

ALTER TABLE UpsShipment
	ALTER COLUMN [ShipmentChargeType] [int] NOT NULL
	
ALTER TABLE UpsShipment
	ALTER COLUMN [ShipmentChargeAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	
ALTER TABLE UpsShipment
	ALTER COLUMN [ShipmentChargePostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	
ALTER TABLE UpsShipment
	ALTER COLUMN [ShipmentChargeCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL

PRINT N'Altering [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD
[ShipmentChargeType] [int] NULL,
[ShipmentChargeAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentChargePostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentChargeCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO