﻿PRINT N'Updating [dbo].[ShippingSettings]'
GO
UPDATE ShippingSettings
SET	FedExUsername = 'bHUvxLnP6tCSYYdX',
	FedExPassword = 'QQ8YrYjqw7X/ES6teoiW2a7ziL0YY4hy2cfDxbi9ozg='
	WHERE ISNULL(FedExUsername,'') != ''
GO

PRINT N'Altering [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD
	[BatteryMaterial] [int] NOT NULL CONSTRAINT [DF_FedExPackage_BatteryMaterial] DEFAULT (0),
	[BatteryPacking] [int] NOT NULL CONSTRAINT [DF_FedExPackage_BatteryPacking] DEFAULT (0),
	[BatteryRegulatorySubtype] [int] NOT NULL CONSTRAINT [DF_FedExPackage_BatteryRegulatorySubtype] DEFAULT (0)
GO
PRINT N'Dropping constraints from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_BatteryMaterial]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_BatteryPacking]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_BatteryRegulatorySubtype]
GO
PRINT N'Altering [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD
	[BatteryMaterial] [int] NULL,
	[BatteryPacking] [int] NULL,
	[BatteryRegulatorySubtype] [int] NULL
GO