SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [FK_FedExPackage_FedExShipment]
GO
PRINT N'Dropping constraints from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [PK_FedExPackage]
GO
PRINT N'Dropping constraints from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_Insured]
GO
PRINT N'Dropping index [IX_FedExPackage_ShipmentID] from [dbo].[FedExPackage]'
GO
DROP INDEX [IX_FedExPackage_ShipmentID] ON [dbo].[FedExPackage]
GO
PRINT N'Rebuilding [dbo].[FedExPackage]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FedExPackage]
(
[FedExPackageID] [bigint] NOT NULL IDENTITY(1061, 1000),
[ShipmentID] [bigint] NOT NULL,
[Weight] [float] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[SkidPieces] [int] NOT NULL,
[Insurance] [bit] NOT NULL CONSTRAINT [DF_FedExPackage_Insured] DEFAULT ((0)),
[InsuranceValue] [money] NOT NULL,
[InsurancePennyOne] [bit] NOT NULL,
[DeclaredValue] [money] NOT NULL,
[TrackingNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PriorityAlert] [bit] NOT NULL,
[PriorityAlertEnhancementType] [int] NOT NULL,
[PriorityAlertDetailContent] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DryIceWeight] [float] NOT NULL,
[ContainsAlcohol] [bit] NOT NULL,
[DangerousGoodsEnabled] [bit] NOT NULL,
[DangerousGoodsType] [int] NOT NULL,
[DangerousGoodsAccessibilityType] [int] NOT NULL,
[DangerousGoodsCargoAircraftOnly] [bit] NOT NULL,
[DangerousGoodsEmergencyContactPhone] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DangerousGoodsOfferor] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DangerousGoodsPackagingCount] [int] NOT NULL,
[HazardousMaterialNumber] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HazardousMaterialClass] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HazardousMaterialProperName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HazardousMaterialPackingGroup] [int] NOT NULL,
[HazardousMaterialQuantityValue] [float] NOT NULL,
[HazardousMaterialQuanityUnits] [int] NOT NULL,
[HazardousMaterialTechnicalName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExPackage] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_FedExPackage]([FedExPackageID], [ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [SkidPieces], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber], [PriorityAlert], [PriorityAlertEnhancementType], [PriorityAlertDetailContent], [DryIceWeight], [ContainsAlcohol], [DangerousGoodsEnabled], [DangerousGoodsType], [DangerousGoodsAccessibilityType], [DangerousGoodsCargoAircraftOnly], [DangerousGoodsEmergencyContactPhone], [DangerousGoodsOfferor], [DangerousGoodsPackagingCount], [HazardousMaterialNumber], [HazardousMaterialClass], [HazardousMaterialProperName], [HazardousMaterialPackingGroup], [HazardousMaterialQuantityValue], [HazardousMaterialQuanityUnits], [HazardousMaterialTechnicalName]) SELECT [FedExPackageID], [ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [SkidPieces], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber], [PriorityAlert], [PriorityAlertEnhancementType], [PriorityAlertDetailContent], [DryIceWeight], [ContainsAlcohol], [DangerousGoodsEnabled], [DangerousGoodsType], [DangerousGoodsAccessibilityType], [DangerousGoodsCargoAircraftOnly], [DangerousGoodsEmergencyContactPhone], [DangerousGoodsOfferor], [DangerousGoodsPackagingCount], [HazardousMaterialNumber], [HazardousMaterialClass], [HazardousMaterialProperName], [HazardousMaterialPackingGroup], [HazardousMaterialQuantityValue], [HazardousMaterialQuanityUnits], '' FROM [dbo].[FedExPackage]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExPackage] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[FedExPackage]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_FedExPackage]', RESEED, @idVal)
GO
DROP TABLE [dbo].[FedExPackage]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FedExPackage]', N'FedExPackage'
GO
PRINT N'Creating primary key [PK_FedExPackage] on [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD CONSTRAINT [PK_FedExPackage] PRIMARY KEY CLUSTERED  ([FedExPackageID])
GO
PRINT N'Creating index [IX_FedExPackage_ShipmentID] on [dbo].[FedExPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_FedExPackage_ShipmentID] ON [dbo].[FedExPackage] ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE
GO