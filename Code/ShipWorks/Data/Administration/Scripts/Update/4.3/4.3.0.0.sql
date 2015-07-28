SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

PRINT N'Altering [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD
[NoPostage] [bit] NOT NULL CONSTRAINT [DF_PostalShipment_NoPostage] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] ADD
[NoPostage] [bit] NOT NULL CONSTRAINT [DF_PostalProfile_NoPostage] DEFAULT ((0))
GO
UPDATE PostalShipment
	SET PostalShipment.NoPostage = EndiciaShipment.NoPostage
	FROM EndiciaShipment
	WHERE PostalShipment.ShipmentId = EndiciaShipment.ShipmentId
GO
UPDATE PostalProfile
	SET PostalProfile.NoPostage = EndiciaProfile.NoPostage
	FROM EndiciaProfile
	WHERE PostalProfile.ShippingProfileId = EndiciaProfile.ShippingProfileId
GO
PRINT N'Altering [dbo].[EndiciaProfile]'
GO
ALTER TABLE [dbo].[EndiciaProfile] DROP
COLUMN [NoPostage]
GO
PRINT N'Altering [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] DROP
COLUMN [NoPostage]
GO