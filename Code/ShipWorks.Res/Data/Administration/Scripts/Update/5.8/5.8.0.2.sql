﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD
[SignatoryContactName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_FedExPackage_SignatoryContactName] DEFAULT (('')),
[SignatoryTitle] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_FedExPackage_SignatoryTitle] DEFAULT (('')),
[SignatoryPlace] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_FedExPackage_SignatoryPlace] DEFAULT (('')),
[AlcoholRecipientType] [int] NOT NULL CONSTRAINT [DF_FedExPackage_AlcoholRecipientType] DEFAULT ((0)),
[ContainerType] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_FedExPackage_ContainerType] DEFAULT (''),
[NumberOfContainers] [int] NOT NULL CONSTRAINT [DF_FedExPackage_NumberOfContainers] DEFAULT ((0)),
[PackingDetailsCargoAircraftOnly] [bit] NOT NULL CONSTRAINT [DF_FedExPackage_PackingDetailsCargoAircraftOnly] DEFAULT ((0)),
[PackingDetailsPackingInstructions] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_FedExPackage_PackingDetailsPackingInstructions] DEFAULT ('')
GO
PRINT N'Dropping FedExPackage Constraints'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_SignatoryContactName]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_SignatoryTitle]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_SignatoryPlace]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_AlcoholRecipientType]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_ContainerType]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_NumberOfContainers]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_PackingDetailsCargoAircraftOnly]
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_PackingDetailsPackingInstructions]
GO
PRINT N'Altering [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD
[SignatoryContactName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryTitle] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryPlace] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContainerType] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NumberOfContainers] [int] NULL,
[PackingDetailsCargoAircraftOnly] [bit] NULL,
[PackingDetailsPackingInstructions] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
[ThirdPartyConsignee] [bit] NOT NULL CONSTRAINT [DF_FedExShipment_ThirdPartyConsignee] DEFAULT ((0)),
[Currency] [int] NULL,
[InternationalTrafficInArmsService] [bit] NULL
GO
PRINT N'Dropping constraints from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_ThirdPartyConsignee]
GO
PRINT N'Altering [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD
[ThirdPartyConsignee] [bit] NULL
GO
PRINT N'Set ThirdParytConsignee to 0 on the default FedEx Profile'
GO
UPDATE fep
set fep.[ThirdPartyConsignee] = 0
FROM dbo.FedExProfile fep	
INNER JOIN dbo.ShippingProfile sp ON sp.ShippingProfileID = fep.ShippingProfileID
WHERE sp.ShipmentTypePrimary = 1
GO