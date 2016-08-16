SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
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
[Activated] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Configured] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Excluded] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultType] [int] NOT NULL,
[BlankPhoneOption] [int] NOT NULL,
[BlankPhoneNumber] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsurancePolicy] [nvarchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceLastAgreed] [datetime] NULL,
[FedExUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExMaskAccount] [bit] NOT NULL,
[FedExThermal] [bit] NOT NULL,
[FedExThermalType] [int] NOT NULL,
[FedExThermalDocTab] [bit] NOT NULL,
[FedExThermalDocTabType] [int] NOT NULL,
[FedExInsuranceProvider] [int] NOT NULL,
[FedExInsurancePennyOne] [bit] NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsThermal] [bit] NOT NULL,
[UpsThermalType] [int] NOT NULL,
[UpsInsuranceProvider] [int] NOT NULL,
[UpsInsurancePennyOne] [bit] NOT NULL,
[EndiciaThermal] [bit] NOT NULL,
[EndiciaThermalType] [int] NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EndiciaThermalDocTab] [bit] NOT NULL,
[EndiciaThermalDocTabType] [int] NOT NULL,
[EndiciaAutomaticExpress1] [bit] NOT NULL,
[EndiciaAutomaticExpress1Account] [bigint] NOT NULL,
[EndiciaInsuranceProvider] [int] NOT NULL,
[WorldShipLaunch] [bit] NOT NULL,
[WorldShipServices] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StampsThermal] [bit] NOT NULL,
[StampsThermalType] [int] NOT NULL,
[Express1Thermal] [bit] NOT NULL,
[Express1ThermalType] [int] NOT NULL,
[Express1CustomsCertify] [bit] NOT NULL,
[Express1CustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Express1ThermalDocTab] [bit] NOT NULL,
[Express1ThermalDocTabType] [int] NOT NULL,
[Express1SingleSource] [bit] NOT NULL,
[EquaShipThermal] [bit] NOT NULL,
[EquaShipThermalType] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], [Express1SingleSource], [EquaShipThermal], [EquaShipThermalType]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], 1, [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], [Express1SingleSource], [EquaShipThermal], [EquaShipThermalType] FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
