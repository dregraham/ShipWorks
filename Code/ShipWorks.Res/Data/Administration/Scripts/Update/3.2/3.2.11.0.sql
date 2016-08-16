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
PRINT N'Creating [dbo].[OnTracProfile]'
GO
CREATE TABLE [dbo].[OnTracProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[OnTracAccountID] [bigint] NULL,
[ResidentialDetermination] [int] NULL,
[Service] [int] NULL,
[SaturdayDelivery] [bit] NULL,
[SignatureRequired] [bit] NULL,
[PackagingType] [int] NULL,
[Weight] [float] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Reference2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Instructions] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_OnTracProfile] on [dbo].[OnTracProfile]'
GO
ALTER TABLE [dbo].[OnTracProfile] ADD CONSTRAINT [PK_OnTracProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[OnTracShipment]'
GO
CREATE TABLE [dbo].[OnTracShipment]
(
[ShipmentID] [bigint] NOT NULL,
[OnTracAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[IsCod] [bit] NOT NULL,
[CodType] [int] NOT NULL,
[CodAmount] [money] NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[SignatureRequired] [bit] NOT NULL,
[PackagingType] [int] NOT NULL,
[Instructions] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Reference2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceValue] [money] NOT NULL,
[InsurancePennyOne] [bit] NOT NULL,
[DeclaredValue] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OnTracShipment] on [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD CONSTRAINT [PK_OnTracShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[OnTracAccount]'
GO
CREATE TABLE [dbo].[OnTracAccount]
(
[OnTracAccountID] [bigint] NOT NULL IDENTITY(1090, 1000),
[RowVersion] [timestamp] NOT NULL,
[AccountNumber] [int] NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (43) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_OnTracAccount] on [dbo].[OnTracAccount]'
GO
ALTER TABLE [dbo].[OnTracAccount] ADD CONSTRAINT [PK_OnTracAccount] PRIMARY KEY CLUSTERED  ([OnTracAccountID])
GO
ALTER TABLE [dbo].[OnTracAccount] ENABLE CHANGE_TRACKING
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
[EquaShipThermalType] [int] NOT NULL,
[OnTracThermal] [bit] NOT NULL,
[OnTracThermalType] [int] NOT NULL,
[OnTracInsuranceProvider] [int] NOT NULL,
[OnTracInsurancePennyOne] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], [Express1SingleSource], [EquaShipThermal], [EquaShipThermalType],
			[OnTracThermal], [OnTracThermalType], [OnTracInsuranceProvider], [OnTracInsurancePennyOne])
SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], [Express1SingleSource], [EquaShipThermal], [EquaShipThermalType],
			0, 0, 1, 0 FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Adding foreign keys to [dbo].[OnTracProfile]'
GO
ALTER TABLE [dbo].[OnTracProfile] ADD
CONSTRAINT [FK_OnTracProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD
CONSTRAINT [FK_OnTracShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
