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
PRINT N'Creating [dbo].[EquaShipProfile]'
GO
CREATE TABLE [dbo].[EquaShipProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[EquaShipAccountID] [bigint] NULL,
[Service] [int] NULL,
[PackageType] [int] NULL,
[ReferenceNumber] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShippingNotes] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Weight] [float] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWidth] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[DeclaredValue] [money] NULL,
[EmailNotification] [bit] NULL,
[SaturdayDelivery] [bit] NULL,
[Confirmation] [int] NULL
)
GO
PRINT N'Creating primary key [PK_EquashipProfile] on [dbo].[EquaShipProfile]'
GO
ALTER TABLE [dbo].[EquaShipProfile] ADD CONSTRAINT [PK_EquashipProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[EquaShipShipment]'
GO
CREATE TABLE [dbo].[EquaShipShipment]
(
[ShipmentID] [bigint] NOT NULL,
[EquaShipAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[PackageType] [int] NOT NULL,
[ReferenceNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShippingNotes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[DeclaredValue] [money] NOT NULL,
[EmailNotification] [bit] NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[Confirmation] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EquashipShipment] on [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] ADD CONSTRAINT [PK_EquashipShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[EquaShipAccount]'
GO
CREATE TABLE [dbo].[EquaShipAccount]
(
[EquaShipAccountID] [bigint] NOT NULL IDENTITY(1067, 1000),
[RowVersion] [timestamp] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_EquahipAccount] on [dbo].[EquaShipAccount]'
GO
ALTER TABLE [dbo].[EquaShipAccount] ADD CONSTRAINT [PK_EquahipAccount] PRIMARY KEY CLUSTERED  ([EquaShipAccountID])
GO
ALTER TABLE [dbo].[EquaShipAccount] ENABLE CHANGE_TRACKING
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
[EquaShipThermal] [bit] NOT NULL,
[EquaShipThermalType] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], [EquaShipThermal], [EquaShipThermalType]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], 0, 0 FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Adding foreign keys to [dbo].[EquaShipProfile]'
GO
ALTER TABLE [dbo].[EquaShipProfile] ADD
CONSTRAINT [FK_EquashipProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] ADD
CONSTRAINT [FK_EquashipShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DeclaredValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'Description'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DimsProfileID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DimsWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'EquaShipAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'InsuranceValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'119', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'PackageType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'ReferenceNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'118', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'ShippingNotes'
GO
