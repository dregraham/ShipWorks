PRINT N'Altering [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD
[UspsPackageID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'Updating primary UPS Profile'
GO
UPDATE [dbo].UpsProfile
SET 
	[UspsPackageID] = ''
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (0,1) AND ShipmentTypePrimary = 1)

PRINT N'Altering [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD
[UspsPackageID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'Updating USPS PackageID for UPS Shipments'
UPDATE [UpsShipment] set [UspsPackageID] = ''

PRINT N'Altering [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ALTER COLUMN [UspsPackageID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO



