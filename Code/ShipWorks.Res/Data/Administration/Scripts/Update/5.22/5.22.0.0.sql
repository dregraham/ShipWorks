PRINT N'Creating [dbo].[PackageProfile]'
GO
CREATE TABLE [dbo].[PackageProfile]
(
[PackageProfileID] [bigint] NOT NULL IDENTITY(1104, 1000),
[ShippingProfileID] [bigint] NOT NULL,
[Weight] [float] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[FedExProfilePackageID] [bigint] NULL,
[UpsProfilePackageID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_PackageProfile] on [dbo].[PackageProfile]'
GO
ALTER TABLE [dbo].[PackageProfile] ADD CONSTRAINT [PK_PackageProfile] PRIMARY KEY CLUSTERED  ([PackageProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[PackageProfile]'
GO
ALTER TABLE [dbo].[PackageProfile] ADD CONSTRAINT [FK_PackageProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO

PRINT N'Dropping foreign keys from [dbo].[DhlExpressProfilePackage]'
GO
ALTER TABLE [dbo].[DhlExpressProfilePackage] DROP CONSTRAINT [FK_DhlExpressPackageProfile_DhlExpressProfile]
GO
PRINT N'Dropping foreign keys from [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] DROP CONSTRAINT [FK_FedExProfilePackage_FedExProfile]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] DROP CONSTRAINT [FK_UpsProfilePackage_UpsProfile]
GO

PRINT N'Dropping constraints from [dbo].[DhlExpressProfilePackage]'
GO
ALTER TABLE [dbo].[DhlExpressProfilePackage] DROP CONSTRAINT [PK_DhlExpressPackageProfile]
GO
PRINT N'Dropping constraints from [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] DROP CONSTRAINT [PK_FedExProfilePackage]
GO
PRINT N'Dropping constraints from [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] DROP CONSTRAINT [PK_UpsProfilePackage]
GO


PRINT N'Dropping [dbo].[DhlExpressProfilePackage]'
GO
DROP TABLE [dbo].[DhlExpressProfilePackage]
GO

-- AMAZON
PRINT N'Transfer Amazon profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO [PackageProfile] (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight) 
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM [AmazonProfile]

PRINT N'Drop AmazonProfile dimension and weight columns'
GO
ALTER TABLE AmazonProfile
DROP COLUMN [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
GO
-- AMAZON

-- ASENDIA
PRINT N'Inserting from [AsendiaProfile] into [PackageProfile]'
GO
INSERT INTO [PackageProfile] ([ShippingProfileID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight]) 
SELECT [ShippingProfileID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight]
FROM [AsendiaProfile]

PRINT N'Dropping Weight and Dimensions columns for [AsendiaProfile]'
GO
ALTER TABLE [AsendiaProfile] 
DROP COLUMN [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight]
GO
-- ASENDIA

-- Best Rate
PRINT N'Transfer BestRate profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM BestRateProfile
GO

PRINT N'Drop BestRateProfile dimension and weight columns'
GO
ALTER TABLE BestRateProfile
DROP COLUMN [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
GO
--Best Rate

-- FEDEX
PRINT N'Inserting from [FedExProfilePackage] into [PackageProfile]'
GO
INSERT INTO [PackageProfile] ([ShippingProfileID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [FedExProfilePackageID]) 
SELECT [ShippingProfileID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [FedExProfilePackageID]
FROM [FedExProfilePackage]
GO
PRINT N'Rebuilding [dbo].[FedExProfilePackage]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_FedExProfilePackage]
(
[PackageProfileID] [bigint] NOT NULL,
[PriorityAlert] [bit] NULL,
[PriorityAlertEnhancementType] [int] NULL,
[PriorityAlertDetailContent] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceWeight] [float] NULL,
[ContainsAlcohol] [bit] NULL,
[DangerousGoodsEnabled] [bit] NULL,
[DangerousGoodsType] [int] NULL,
[DangerousGoodsAccessibilityType] [int] NULL,
[DangerousGoodsCargoAircraftOnly] [bit] NULL,
[DangerousGoodsEmergencyContactPhone] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DangerousGoodsOfferor] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DangerousGoodsPackagingCount] [int] NULL,
[HazardousMaterialNumber] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HazardousMaterialClass] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HazardousMaterialProperName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HazardousMaterialPackingGroup] [int] NULL,
[HazardousMaterialQuantityValue] [float] NULL,
[HazardousMaterialQuanityUnits] [int] NULL,
[SignatoryContactName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryTitle] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryPlace] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContainerType] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NumberOfContainers] [int] NULL,
[PackingDetailsCargoAircraftOnly] [bit] NULL,
[PackingDetailsPackingInstructions] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BatteryMaterial] [int] NULL,
[BatteryPacking] [int] NULL,
[BatteryRegulatorySubtype] [int] NULL
)
GO
INSERT INTO [dbo].[RG_Recovery_1_FedExProfilePackage]([PackageProfileID], [PriorityAlert], [PriorityAlertEnhancementType], [PriorityAlertDetailContent], [DryIceWeight], [ContainsAlcohol], [DangerousGoodsEnabled], [DangerousGoodsType], [DangerousGoodsAccessibilityType], [DangerousGoodsCargoAircraftOnly], [DangerousGoodsEmergencyContactPhone], [DangerousGoodsOfferor], [DangerousGoodsPackagingCount], [HazardousMaterialNumber], [HazardousMaterialClass], [HazardousMaterialProperName], [HazardousMaterialPackingGroup], [HazardousMaterialQuantityValue], [HazardousMaterialQuanityUnits], [SignatoryContactName], [SignatoryTitle], [SignatoryPlace], [ContainerType], [NumberOfContainers], [PackingDetailsCargoAircraftOnly], [PackingDetailsPackingInstructions], [BatteryMaterial], [BatteryPacking], [BatteryRegulatorySubtype]) 
SELECT [PackageProfileID], [PriorityAlert], [PriorityAlertEnhancementType], [PriorityAlertDetailContent], [DryIceWeight], [ContainsAlcohol], [DangerousGoodsEnabled], [DangerousGoodsType], [DangerousGoodsAccessibilityType], [DangerousGoodsCargoAircraftOnly], [DangerousGoodsEmergencyContactPhone], [DangerousGoodsOfferor], [DangerousGoodsPackagingCount], [HazardousMaterialNumber], [HazardousMaterialClass], [HazardousMaterialProperName], [HazardousMaterialPackingGroup], [HazardousMaterialQuantityValue], [HazardousMaterialQuanityUnits], [SignatoryContactName], [SignatoryTitle], [SignatoryPlace], [ContainerType], [NumberOfContainers], [PackingDetailsCargoAircraftOnly], [PackingDetailsPackingInstructions], [BatteryMaterial], [BatteryPacking], [BatteryRegulatorySubtype] 
	FROM [dbo].[FedExProfilePackage] fpp
	INNER JOIN PackageProfile as pp  ON fpp.[FedExProfilePackageID] = pp.[FedExProfilePackageID]
GO
DROP TABLE [dbo].[FedExProfilePackage]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_FedExProfilePackage]', N'FedExProfilePackage', N'OBJECT'
GO
PRINT N'Creating primary key [PK_FedExProfilePackage_PackageProfileID] on [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD CONSTRAINT [PK_FedExProfilePackage_PackageProfileID] PRIMARY KEY CLUSTERED  ([PackageProfileID])
GO
PRINT N'Dropping [FedExProfilePackageID] from [ProfilePackage]'
GO
ALTER TABLE [PackageProfile] DROP COLUMN [FedExProfilePackageID]
GO
-- FEDEX

-- iParcelProfilePackage
PRINT N'Transfer iParcel profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM iParcelProfilePackage
GO
PRINT N'Dropping foreign keys from [dbo].[iParcelProfilePackage]'
GO
ALTER TABLE [dbo].[iParcelProfilePackage] DROP CONSTRAINT [FK_iParcelPackageProfile_iParcelProfile]
GO
PRINT N'Dropping constraints from [dbo].[iParcelProfilePackage]'
GO
ALTER TABLE [dbo].[iParcelProfilePackage] DROP CONSTRAINT [PK_iParcelPackageProfile]
GO
PRINT N'Drop iParcelProfilePackage'
GO
DROP TABLE iParcelProfilePackage
GO
-- iParcelProfilePackage

-- OnTrac
PRINT N'Transfer OnTrac profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM OnTracProfile
GO
PRINT N'Drop OnTracProfile dimension and weight columns'
GO
ALTER TABLE OnTracProfile
DROP COLUMN [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
GO
-- OnTrac

-- Postal
PRINT N'Transfer Postal profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM PostalProfile
GO
PRINT N'Drop PostalProfile dimension and weight columns'
GO
ALTER TABLE PostalProfile
DROP COLUMN [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
GO
-- Postal

-- UPS
PRINT N'Transfer Ups profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO [PackageProfile] (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, UpsProfilePackageID) 
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, UpsProfilePackageID
FROM [UpsProfilePackage]
GO

PRINT N'Rebuilding [dbo].[UpsProfilePackage]'
GO
CREATE TABLE [dbo].[RG_Recovery_2_UpsProfilePackage]
(
[PackageProfileID] [bigint] NOT NULL,
[PackagingType] [int] NULL,
[AdditionalHandlingEnabled] [bit] NULL,
[VerbalConfirmationEnabled] [bit] NULL,
[VerbalConfirmationName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationPhone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationPhoneExtension] [nvarchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceEnabled] [bit] NULL,
[DryIceRegulationSet] [int] NULL,
[DryIceWeight] [float] NULL,
[DryIceIsForMedicalUse] [bit] NULL
)
GO
INSERT INTO [dbo].[RG_Recovery_2_UpsProfilePackage]([PackageProfileID], [PackagingType], [AdditionalHandlingEnabled], [VerbalConfirmationEnabled], [VerbalConfirmationName], [VerbalConfirmationPhone], [VerbalConfirmationPhoneExtension], [DryIceEnabled], [DryIceRegulationSet], [DryIceWeight], [DryIceIsForMedicalUse]) 
SELECT [PackageProfileID], [PackagingType], [AdditionalHandlingEnabled], [VerbalConfirmationEnabled], [VerbalConfirmationName], [VerbalConfirmationPhone], [VerbalConfirmationPhoneExtension], [DryIceEnabled], [DryIceRegulationSet], [DryIceWeight], [DryIceIsForMedicalUse] 
FROM [UpsProfilePackage] AS upp
	INNER JOIN [PackageProfile] AS pp ON upp.UpsProfilePackageID = pp.UpsProfilePackageID
GO
DROP TABLE [dbo].[UpsProfilePackage]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_2_UpsProfilePackage]', N'UpsProfilePackage', N'OBJECT'
GO
PRINT N'Creating primary key [PK_UpsProfilePackage_PackageProfileID] on [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] ADD CONSTRAINT [PK_UpsProfilePackage_PackageProfileID] PRIMARY KEY CLUSTERED  ([PackageProfileID])
GO
PRINT N'Dropping [UpsProfilePackageID] from [ProfilePackage]' 
GO 
ALTER TABLE [PackageProfile] DROP COLUMN [UpsProfilePackageID]
GO
-- UPS

PRINT N'Adding foreign keys to [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD CONSTRAINT [FK_FedExProfilePackage_PackageProfile] FOREIGN KEY ([PackageProfileID]) REFERENCES [dbo].[PackageProfile] ([PackageProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] ADD CONSTRAINT [FK_UpsProfilePackage_PackageProfile] FOREIGN KEY ([PackageProfileID]) REFERENCES [dbo].[PackageProfile] ([PackageProfileID]) ON DELETE CASCADE
GO

PRINT N'Adding foreign keys to [dbo].[OnTracProfile]'
ALTER TABLE [dbo].[OnTracProfile] DROP CONSTRAINT [FK_OnTracProfile_ShippingProfile]
GO
ALTER TABLE [dbo].[OnTracProfile]  WITH CHECK ADD  CONSTRAINT [FK_OnTracProfile_ShippingProfile] FOREIGN KEY([ShippingProfileID])
REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID])
ON DELETE CASCADE
GO
