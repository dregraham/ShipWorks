PRINT N'Altering [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD
	[FreightPackaging] [int] NOT NULL CONSTRAINT [DF_FedExPackage_FreightPackaging] DEFAULT ((0)),
	[FreightPieces] [int] NOT NULL CONSTRAINT [DF_FedExPackage_FreightPieces] DEFAULT ((0))
GO
PRINT N'Dropping constraints from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_FreightPackaging]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_FreightPieces]
GO

PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
	[FreightRole] [int] NOT NULL CONSTRAINT [DF_FedExShipment_FreightRole] DEFAULT ((0)),
	[FreightCollectTerms] [int] NOT NULL CONSTRAINT [DF_FedExShipment_FreightCollectTerms] DEFAULT ((0)),
	[FreightTotalHandlinUnits] [int] NOT NULL CONSTRAINT [DF_FedExShipment_FreightTotalHandlinUnits] DEFAULT ((1)),
	[FreightClass] [int] NOT NULL CONSTRAINT [DF_FedExShipment_FreightClass] DEFAULT ((0)),
	[FreightSpecialServices] [int] NOT NULL CONSTRAINT [DF_FedExShipment_FreightSpecialServices] DEFAULT ((0))
GO

PRINT N'Dropping constraints from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_FreightRole]
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_FreightCollectTerms]
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_FreightTotalHandlinUnits]
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_FreightClass]
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_FreightSpecialServices]
GO
