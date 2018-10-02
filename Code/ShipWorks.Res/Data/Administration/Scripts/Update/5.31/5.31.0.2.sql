SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_ShippingSettings]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[ShippingSettings]', 'U'))
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [PK_ShippingSettings]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'FedExFimsEnabled' AND object_id = OBJECT_ID(N'[dbo].[ShippingSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_ShippingSettings_FedExFimsEnabled]', 'D'))
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_FedExFimsEnabled]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'FedExFimsUsername' AND object_id = OBJECT_ID(N'[dbo].[ShippingSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_ShippingSettings_FedExFimsUsername]', 'D'))
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_FedExFimsUsername]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'FedExFimsPassword' AND object_id = OBJECT_ID(N'[dbo].[ShippingSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_ShippingSettings_FedExFimsPassword]', 'D'))
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_FedExFimsPassword]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ShipmentsLoaderEnsureFiltersLoadedTimeout' AND object_id = OBJECT_ID(N'[dbo].[ShippingSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_ShippingSettings_ShipmentsLoaderEnsureFiltersLoadedTimeout]', 'D'))
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_ShipmentsLoaderEnsureFiltersLoadedTimeout]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ShipmentDateCutoffJson' AND object_id = OBJECT_ID(N'[dbo].[ShippingSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_ShippingSettings_ShipmentDateCutoffJson]', 'D'))
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_ShipmentDateCutoffJson]
GO
PRINT N'Rebuilding [dbo].[ShippingSettings]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_ShippingSettings]
(
[ShippingSettingsID] [bit] NOT NULL,
[Activated] [varchar] (45) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Configured] [varchar] (45) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Excluded] [varchar] (45) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultType] [int] NOT NULL,
[BlankPhoneOption] [int] NOT NULL,
[BlankPhoneNumber] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsurancePolicy] [nvarchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceLastAgreed] [datetime] NULL,
[FedExUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExMaskAccount] [bit] NOT NULL,
[FedExThermalDocTab] [bit] NOT NULL,
[FedExThermalDocTabType] [int] NOT NULL,
[FedExInsuranceProvider] [int] NOT NULL,
[FedExInsurancePennyOne] [bit] NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsInsuranceProvider] [int] NOT NULL,
[UpsInsurancePennyOne] [bit] NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EndiciaThermalDocTab] [bit] NOT NULL,
[EndiciaThermalDocTabType] [int] NOT NULL,
[EndiciaAutomaticExpress1] [bit] NOT NULL,
[EndiciaAutomaticExpress1Account] [bigint] NOT NULL,
[EndiciaInsuranceProvider] [int] NOT NULL,
[WorldShipLaunch] [bit] NOT NULL,
[UspsAutomaticExpress1] [bit] NOT NULL,
[UspsAutomaticExpress1Account] [bigint] NOT NULL,
[UspsInsuranceProvider] [int] NOT NULL,
[Express1EndiciaCustomsCertify] [bit] NOT NULL,
[Express1EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Express1EndiciaThermalDocTab] [bit] NOT NULL,
[Express1EndiciaThermalDocTabType] [int] NOT NULL,
[Express1EndiciaSingleSource] [bit] NOT NULL,
[OnTracInsuranceProvider] [int] NOT NULL,
[OnTracInsurancePennyOne] [bit] NOT NULL,
[iParcelInsuranceProvider] [int] NOT NULL,
[iParcelInsurancePennyOne] [bit] NOT NULL,
[Express1UspsSingleSource] [bit] NOT NULL,
[UpsMailInnovationsEnabled] [bit] NOT NULL,
[WorldShipMailInnovationsEnabled] [bit] NOT NULL,
[BestRateExcludedShipmentTypes] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipSenseEnabled] [bit] NOT NULL,
[ShipSenseUniquenessXml] [xml] NOT NULL,
[ShipSenseProcessedShipmentID] [bigint] NOT NULL,
[ShipSenseEndShipmentID] [bigint] NOT NULL,
[AutoCreateShipments] [bit] NOT NULL,
[FedExFimsEnabled] [bit] NOT NULL CONSTRAINT [DF_ShippingSettings_FedExFimsEnabled] DEFAULT ((0)),
[FedExFimsUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_FedExFimsUsername] DEFAULT (''),
[FedExFimsPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_FedExFimsPassword] DEFAULT (''),
[ShipmentEditLimit] [int] NOT NULL,
[ShipmentsLoaderEnsureFiltersLoadedTimeout] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_ShipmentsLoaderEnsureFiltersLoadedTimeout] DEFAULT ((0)),
[ShipmentDateCutoffJson] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_ShipmentDateCutoffJson] DEFAULT (''),
[ShipEngineApiKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrderLookupFieldLayout] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_OrderLookupFieldLayout] DEFAULT ('')
)
GO
INSERT INTO [dbo].[RG_Recovery_1_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [UspsAutomaticExpress1], [UspsAutomaticExpress1Account], [UspsInsuranceProvider], [Express1EndiciaCustomsCertify], [Express1EndiciaCustomsSigner], [Express1EndiciaThermalDocTab], [Express1EndiciaThermalDocTabType], [Express1EndiciaSingleSource], [OnTracInsuranceProvider], [OnTracInsurancePennyOne], [iParcelInsuranceProvider], [iParcelInsurancePennyOne], [Express1UspsSingleSource], [UpsMailInnovationsEnabled], [WorldShipMailInnovationsEnabled], [BestRateExcludedShipmentTypes], [ShipSenseEnabled], [ShipSenseUniquenessXml], [ShipSenseProcessedShipmentID], [ShipSenseEndShipmentID], [AutoCreateShipments], [FedExFimsEnabled], [FedExFimsUsername], [FedExFimsPassword], [ShipmentEditLimit], [ShipmentsLoaderEnsureFiltersLoadedTimeout], [ShipmentDateCutoffJson], [ShipEngineApiKey], [OrderLookupFieldLayout]) 
	SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [UspsAutomaticExpress1], [UspsAutomaticExpress1Account], [UspsInsuranceProvider], [Express1EndiciaCustomsCertify], [Express1EndiciaCustomsSigner], [Express1EndiciaThermalDocTab], [Express1EndiciaThermalDocTabType], [Express1EndiciaSingleSource], [OnTracInsuranceProvider], [OnTracInsurancePennyOne], [iParcelInsuranceProvider], [iParcelInsurancePennyOne], [Express1UspsSingleSource], [UpsMailInnovationsEnabled], [WorldShipMailInnovationsEnabled], [BestRateExcludedShipmentTypes], [ShipSenseEnabled], [ShipSenseUniquenessXml], [ShipSenseProcessedShipmentID], [ShipSenseEndShipmentID], [AutoCreateShipments], [FedExFimsEnabled], [FedExFimsUsername], [FedExFimsPassword], [ShipmentEditLimit], [ShipmentsLoaderEnsureFiltersLoadedTimeout], [ShipmentDateCutoffJson], [ShipEngineApiKey], '' 
	FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
IF (OBJECT_ID(N'[dbo].[RG_Recovery_1_ShippingSettings]', 'U') IS NOT NULL) AND (OBJECT_ID(N'[dbo].[ShippingSettings]', 'U') IS NULL)
EXEC sp_rename N'[dbo].[RG_Recovery_1_ShippingSettings]', N'ShippingSettings', N'OBJECT'
GO
