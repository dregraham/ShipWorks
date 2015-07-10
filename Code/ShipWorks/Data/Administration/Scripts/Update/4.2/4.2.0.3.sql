SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [PK_ShippingSettings]
GO
PRINT N'Rebuilding [dbo].[ShippingSettings]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShippingSettings]
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
[FedExExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsInsuranceProvider] [int] NOT NULL,
[UpsInsurancePennyOne] [bit] NOT NULL,
[UpsExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EndiciaThermalDocTab] [bit] NOT NULL,
[EndiciaThermalDocTabType] [int] NOT NULL,
[EndiciaAutomaticExpress1] [bit] NOT NULL,
[EndiciaAutomaticExpress1Account] [bigint] NOT NULL,
[EndiciaInsuranceProvider] [int] NOT NULL,
[EndiciaExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WorldShipLaunch] [bit] NOT NULL,
[UspsAutomaticExpress1] [bit] NOT NULL,
[UspsAutomaticExpress1Account] [bigint] NOT NULL,
[UspsInsuranceProvider] [int] NOT NULL,
[UspsExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Express1EndiciaCustomsCertify] [bit] NOT NULL,
[Express1EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Express1EndiciaThermalDocTab] [bit] NOT NULL,
[Express1EndiciaThermalDocTabType] [int] NOT NULL,
[Express1EndiciaSingleSource] [bit] NOT NULL,
[Express1EndiciaExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnTracInsuranceProvider] [int] NOT NULL,
[OnTracInsurancePennyOne] [bit] NOT NULL,
[OnTracExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[iParcelInsuranceProvider] [int] NOT NULL,
[iParcelInsurancePennyOne] [bit] NOT NULL,
[iParcelExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Express1UspsSingleSource] [bit] NOT NULL,
[Express1UspsExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UpsMailInnovationsEnabled] [bit] NOT NULL,
[WorldShipMailInnovationsEnabled] [bit] NOT NULL,
[BestRateExcludedShipmentTypes] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WebToolsExcludedServiceTypes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipSenseEnabled] [bit] NOT NULL,
[ShipSenseUniquenessXml] [xml] NOT NULL,
[ShipSenseProcessedShipmentID] [bigint] NOT NULL,
[ShipSenseEndShipmentID] [bigint] NOT NULL,
[AutoCreateShipments] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [UspsAutomaticExpress1], [UspsAutomaticExpress1Account], [UspsInsuranceProvider], [Express1EndiciaCustomsCertify], [Express1EndiciaCustomsSigner], [Express1EndiciaThermalDocTab], [Express1EndiciaThermalDocTabType], [Express1EndiciaSingleSource], [OnTracInsuranceProvider], [OnTracInsurancePennyOne], [iParcelInsuranceProvider], [iParcelInsurancePennyOne], [Express1UspsSingleSource], [UpsMailInnovationsEnabled], [WorldShipMailInnovationsEnabled], [BestRateExcludedShipmentTypes], [ShipSenseEnabled], [ShipSenseUniquenessXml], [ShipSenseProcessedShipmentID], [ShipSenseEndShipmentID], [AutoCreateShipments], [FedExExcludedServiceTypes], [UpsExcludedServiceTypes], [EndiciaExcludedServiceTypes], [UspsExcludedServiceTypes], [Express1EndiciaExcludedServiceTypes], [OnTracExcludedServiceTypes], [iParcelExcludedServiceTypes], [Express1UspsExcludedServiceTypes], [WebToolsExcludedServiceTypes]) 
SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [UspsAutomaticExpress1], [UspsAutomaticExpress1Account], [UspsInsuranceProvider], [Express1EndiciaCustomsCertify], [Express1EndiciaCustomsSigner], [Express1EndiciaThermalDocTab], [Express1EndiciaThermalDocTabType], [Express1EndiciaSingleSource], [OnTracInsuranceProvider], [OnTracInsurancePennyOne], [iParcelInsuranceProvider], [iParcelInsurancePennyOne], [Express1UspsSingleSource], [UpsMailInnovationsEnabled], [WorldShipMailInnovationsEnabled], [BestRateExcludedShipmentTypes], [ShipSenseEnabled], [ShipSenseUniquenessXml], [ShipSenseProcessedShipmentID], [ShipSenseEndShipmentID], [AutoCreateShipments], '', '', '', '', '', '', '', '', '' FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO