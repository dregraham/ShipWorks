SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[Shipment] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping foreign keys from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP
CONSTRAINT [FK_FedExPackage_FedExShipment]
GO
PRINT N'Dropping foreign keys from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP
CONSTRAINT [FK_FedExShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] DROP
CONSTRAINT [FK_OtherShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP
CONSTRAINT [FK_PostalShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP
CONSTRAINT [FK_ShipmentCustomsItem_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP
CONSTRAINT [FK_UpsShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP
CONSTRAINT [FK_Shipment_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] DROP
CONSTRAINT [FK_UpsPackage_UpsShipment]
GO
PRINT N'Dropping constraints from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [PK_FedExPackage]
GO
PRINT N'Dropping constraints from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [PK_Shipment]
GO
PRINT N'Dropping constraints from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [IX_Shipment_Other]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [PK_ShippingSettings]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_EndiciaThermalDocTabType]
GO
PRINT N'Dropping constraints from [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] DROP CONSTRAINT [PK_UpsPackage]
GO
PRINT N'Dropping index [IX_Shipment_OrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment]
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
[Express1ThermalDocTabType] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], InsurancePolicy, InsuranceLastAgreed, [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], FedExInsuranceProvider, FedExInsurancePennyOne, [UpsAccessKey], [UpsThermal], [UpsThermalType], UpsInsuranceProvider, UpsInsurancePennyOne, [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], '', NULL, [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], 1, 0, [UpsAccessKey], [UpsThermal], [UpsThermalType], 1, 0, [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType] FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
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
[TrackingNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExPackage] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_FedExPackage]([FedExPackageID], [ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [SkidPieces], Insurance, [InsuranceValue], InsurancePennyOne, [DeclaredValue], [TrackingNumber]) SELECT [FedExPackageID], [ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [SkidPieces], 0, [InsuranceValue], 0, [DeclaredValue], [TrackingNumber] FROM [dbo].[FedExPackage]
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

PRINT N'Altering [dbo].[ShippingProfile]'
GO

ALTER TABLE [dbo].[ShippingProfile] ADD
[Insurance] [bit] NULL,
[InsuranceInitialValueSource] [int] NULL,
[InsuranceInitialValueAmount] [money] NULL,
[ReturnShipment] [bit] NULL
GO

-- Set the defaults for the primary profile
UPDATE [dbo].ShippingProfile
	SET 
		[Insurance] = 0,
		[InsuranceInitialValueSource] = 0,
		[InsuranceInitialValueAmount] = 0,
		[ReturnShipment] = 0
	WHERE ShipmentTypePrimary = 1
GO

-- If they had any type of insurance selected before, set Insurance to on
UPDATE ShippingProfile
  SET Insurance = CASE WHEN InsuranceType = 0 THEN 0 ELSE 1 END,
      InsuranceInitialValueSource = 0,
	  InsuranceInitialValueAmount = 0
  WHERE InsuranceType IS NOT NULL
GO

ALTER TABLE [dbo].[ShippingProfile] DROP
COLUMN [InsuranceType]
GO

PRINT N'Altering [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] DROP
COLUMN [DeclaredValue]
GO
PRINT N'Rebuilding [dbo].[Shipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Shipment]
(
[ShipmentID] [bigint] NOT NULL IDENTITY(1031, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[ShipmentType] [int] NOT NULL,
[ContentWeight] [float] NOT NULL,
[TotalWeight] [float] NOT NULL,
[Processed] [bit] NOT NULL,
[ProcessedDate] [datetime] NULL,
[ShipDate] [datetime] NOT NULL,
[ShipmentCost] [money] NOT NULL,
[Voided] [bit] NOT NULL,
[VoidedDate] [datetime] NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsGenerated] [bit] NOT NULL,
[CustomsValue] [money] NOT NULL,
[ThermalType] [int] NULL,
[ShipFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipMiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipCompany] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ResidentialDetermination] [int] NOT NULL,
[ResidentialResult] [bit] NOT NULL,
[OriginOriginID] [bigint] NOT NULL,
[OriginFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginMiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginCompany] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnShipment] [bit] NOT NULL,
[Insurance] [bit] NOT NULL,
[InsuranceProvider] [int] NOT NULL,
[ShipNameParseStatus] [int] NOT NULL,
[ShipUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginNameParseStatus] [int] NOT NULL,
[OriginUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Shipment]([ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [TrackingNumber], [CustomsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], Insurance, InsuranceProvider, [ShipNameParseStatus], [ShipUnparsedName], [OriginNameParseStatus], [OriginUnparsedName]) SELECT [ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [TrackingNumber], [CustomsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], CASE InsuranceType WHEN 0 THEN 0 ELSE 1 END, CASE InsuranceType WHEN 2 THEN 2 ELSE 1 END, [ShipNameParseStatus], [ShipUnparsedName], [OriginNameParseStatus], [OriginUnparsedName] FROM [dbo].[Shipment]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Shipment]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Shipment]', RESEED, @idVal)
GO
DROP TABLE [dbo].[Shipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Shipment]', N'Shipment'
GO
PRINT N'Creating primary key [PK_Shipment] on [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_Shipment_OrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment] ([OrderID])
GO
ALTER TABLE [dbo].[Shipment] ENABLE CHANGE_TRACKING
GO

PRINT N'Rebuilding [dbo].[UpsPackage]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UpsPackage]
(
[UpsPackageID] [bigint] NOT NULL IDENTITY(1063, 1000),
[ShipmentID] [bigint] NOT NULL,
[PackagingType] [int] NOT NULL,
[Weight] [float] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[Insurance] [bit] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[InsurancePennyOne] [bit] NOT NULL,
[DeclaredValue] [money] NOT NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UpsPackage] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_UpsPackage]([UpsPackageID], [ShipmentID], [PackagingType], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], Insurance, [InsuranceValue], InsurancePennyOne, DeclaredValue, [TrackingNumber], [UspsTrackingNumber]) SELECT [UpsPackageID], [ShipmentID], [PackagingType], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], 0, [InsuranceValue], 0, 0, [TrackingNumber], [UspsTrackingNumber] FROM [dbo].[UpsPackage]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UpsPackage] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[UpsPackage]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_UpsPackage]', RESEED, @idVal)
GO
DROP TABLE [dbo].[UpsPackage]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UpsPackage]', N'UpsPackage'
GO
PRINT N'Creating primary key [PK_UpsPackage] on [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [PK_UpsPackage] PRIMARY KEY CLUSTERED  ([UpsPackageID])
GO
PRINT N'Adding constraints to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [IX_Shipment_Other] UNIQUE NONCLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD
CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] ADD
CONSTRAINT [FK_OtherShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD
CONSTRAINT [FK_PostalShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD
CONSTRAINT [FK_ShipmentCustomsItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD
CONSTRAINT [FK_UpsShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD
CONSTRAINT [FK_UpsPackage_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ContentWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsGenerated'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_addextendedproperty N'AuditName', N'Insurance', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'OriginCountry', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginOriginID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'OriginState', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'111', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditName', N'Residential \ Commercial', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialResult'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipCountry', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'7', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipDate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentCost'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'103', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'TotalWeight'
GO

PRINT N'Altering [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] ADD
	[MagentoConnect] [BIT] NOT NULL CONSTRAINT DF_MagentoStore_MagentoConnect DEFAULT 0
GO
ALTER TABLE [dbo].[MagentoStore] DROP CONSTRAINT DF_MagentoStore_MagentoConnect
GO

-- Update 'Insurance' bit now on each FedEx package
UPDATE FedExPackage
  SET Insurance = 1
  WHERE (SELECT s.Insurance FROM Shipment s WHERE s.ShipmentID = FedExPackage.ShipmentID) = 1
GO

-- Update 'Insurance' bit now on each UPS package
UPDATE UpsPackage
  SET Insurance = 1
  WHERE (SELECT s.Insurance FROM Shipment s WHERE s.ShipmentID = UpsPackage.ShipmentID) = 1
GO

-- Update new UPS DeclaredValue field to be what actually got sent as DeclaredValue
UPDATE UpsPackage
  SET DeclaredValue = InsuranceValue
  WHERE (SELECT s.InsuranceProvider FROM Shipment s WHERE s.ShipmentID = UpsPackage.ShipmentID) = 2
GO