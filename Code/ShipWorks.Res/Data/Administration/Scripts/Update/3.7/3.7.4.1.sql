SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[EndiciaProfile]'
GO
ALTER TABLE [dbo].[EndiciaProfile] ADD
[ScanBasedReturn] [bit] NULL
GO
PRINT N'Altering [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD
[ScanBasedReturn] [bit] NULL
GO

update [dbo].[EndiciaShipment] set [ScanBasedReturn] = 0
GO

ALTER TABLE [dbo].[EndiciaShipment] ALTER COLUMN [ScanBasedReturn] [bit] NOT NULL
GO

PRINT N'Updating primary Endicia Profile'
GO
UPDATE [dbo].[EndiciaProfile]
SET 
	[ScanBasedReturn] = 0
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (2,9) AND ShipmentTypePrimary = 1)