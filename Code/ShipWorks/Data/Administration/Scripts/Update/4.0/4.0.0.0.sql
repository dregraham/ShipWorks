-- Add Best Rate Shipment Type to the activated and configured shipment types
update ShippingSettings 
	set Activated = 
		  CASE 
			 WHEN len(Activated) > 0 THEN Activated + ',14'
			 ELSE '14'
		  END,
	Configured = 
		  CASE 
			 WHEN len(Configured) > 0 THEN Configured + ',14'
			 ELSE '14'
		  END
GO

PRINT N'Creating [dbo].[BestRateShipment]'
GO
CREATE TABLE [dbo].[BestRateShipment]
(
[ShipmentID] [bigint] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[ServiceLevel] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_BestRateShipment] on [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [PK_BestRateShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO

-- Create default best rate profile
INSERT INTO [ShipWorksLocal].[dbo].[ShippingProfile] ([Name], [ShipmentType], [ShipmentTypePrimary], [OriginID], [Insurance], [InsuranceInitialValueSource], [InsuranceInitialValueAmount], [ReturnShipment])
VALUES ('Defaults - Best rate', 14, 1, 0, 0, 0, 0.00, 0)
GO

INSERT INTO [ShipWorksLocal].[dbo].[BestRateProfile] ([ShippingProfileID], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [Weight], [TransitDays])
SELECT TOP 1 ShippingProfileID, 0, 0, 0, 0, 0, 0, 0, 0  FROM ShippingProfile WHERE ShipmentType = 14
GO