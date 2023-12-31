SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD
[CostCenter] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IrregularIndicator] [int] NULL,
[Cn22Number] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
 
UPDATE [dbo].[UpsShipment] SET [CostCenter] = '', [IrregularIndicator] = 0, [Cn22Number] = ''

ALTER TABLE [dbo].[UpsShipment]
	ALTER COLUMN [CostCenter] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	
ALTER TABLE [dbo].[UpsShipment]
	ALTER COLUMN [IrregularIndicator] [int] NOT NULL
	
ALTER TABLE [dbo].[UpsShipment]
	ALTER COLUMN [Cn22Number] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL


PRINT N'Altering [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD
[CostCenter] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IrregularIndicator] [int] NULL,
[Cn22Number] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[UpsMailInnovationsEnabled] [bit] NULL,
[WorldShipMailInnovationsEnabled] [bit] NULL
GO

-- Default the values to false
UPDATE [dbo].[ShippingSettings]
	set [UpsMailInnovationsEnabled] = 0, [WorldShipMailInnovationsEnabled] = 0

-- Now update the new ShippingSettings columns based on previous [WorldShipServices] column
UPDATE [dbo].[ShippingSettings]
	set [UpsMailInnovationsEnabled] = 1
	where [WorldShipServices] like '%0%'
	
UPDATE [dbo].[ShippingSettings]
	set [WorldShipMailInnovationsEnabled] = 1
	where [WorldShipServices] like '%0%'

-- Now make them not nullable
ALTER TABLE [dbo].[ShippingSettings]
	ALTER COLUMN [UpsMailInnovationsEnabled] [bit] NOT NULL

ALTER TABLE [dbo].[ShippingSettings]
	ALTER COLUMN [WorldShipMailInnovationsEnabled] [bit] NOT NULL

-- Now delete the old column
ALTER TABLE [dbo].[ShippingSettings] DROP
COLUMN [WorldShipServices]
GO

PRINT N'Updating primary UPS Profile'
GO
UPDATE [dbo].UpsProfile
SET 
	[CostCenter] = '',
	[IrregularIndicator] = 0,
	[Cn22Number] = ''
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (0,1) AND ShipmentTypePrimary = 1)


