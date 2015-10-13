SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Updating [dbo].[FedExShipment]'
GO
--Add ReferenceFIMS to FedexShipment
ALTER TABLE [FedExShipment]
ADD [ReferenceFIMS] [nvarchar](300) NOT NULL
CONSTRAINT [FedExShipmentReferenceFIMSDefault] DEFAULT ''
GO
PRINT N'Updating [dbo].[FedExProfile]'
GO
--Add ReferenceFIMS to FedexProfile
ALTER TABLE [FedExProfile]
ADD [ReferenceFIMS] [nvarchar](300) NULL
GO
--Set the ReferenceFIMS of existing FIMS shipments to ReferenceCustomer
UPDATE [FedExShipment] SET [ReferenceFIMS] = [ReferenceCustomer] WHERE [Service] = 27
GO
--Set the default profiles ReferenceFIMS to an empty string
UPDATE fp
SET fp.[ReferenceFIMS] = ''
FROM [FedExProfile] fp
	INNER JOIN [ShippingProfile] sp 
	ON fp.[ShippingProfileID] = sp.[ShippingProfileID]
WHERE sp.[ShipmentType] = 6 and sp.[ShipmentTypePrimary] = 1
GO
--Drop the default value constraint from FedExShipment
ALTER TABLE [FedExShipment]
DROP CONSTRAINT [FedExShipmentReferenceFIMSDefault]
GO