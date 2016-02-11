SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] DROP
CONSTRAINT [FK_EndiciaShipment_PostalShipment]
GO
PRINT N'Dropping foreign keys from [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP
CONSTRAINT [FK_PostalShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP
CONSTRAINT [FK_StampsShipment_PostalShipment]
GO
PRINT N'Dropping constraints from [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] DROP CONSTRAINT [PK_EndiciaShipment]
GO
PRINT N'Dropping constraints from [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP CONSTRAINT [PK_PostalShipment]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [PK_ShippingSettings]
GO
PRINT N'Altering [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] ALTER COLUMN [CommerceInterfaceOrderNumber] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Rebuilding [dbo].[PostalShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_PostalShipment]
(
[ShipmentID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[Confirmation] [int] NOT NULL,
[PackagingType] [int] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[NonRectangular] [bit] NOT NULL,
[NonMachinable] [bit] NOT NULL,
[CustomsContentType] [int] NOT NULL,
[CustomsContentDescription] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceValue] [money] NOT NULL,
[ExpressSignatureWaiver] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_PostalShipment]([ShipmentID], [Service], [Confirmation], [PackagingType], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [NonRectangular], [NonMachinable], [CustomsContentType], [CustomsContentDescription], [InsuranceValue], [ExpressSignatureWaiver]) SELECT [ShipmentID], [Service], [Confirmation], [PackagingType], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [NonRectangular], [NonMachinable], [CustomsContentType], [CustomsContentDescription], [InsuranceValue], 0 FROM [dbo].[PostalShipment]
GO
DROP TABLE [dbo].[PostalShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_PostalShipment]', N'PostalShipment'
GO
PRINT N'Creating primary key [PK_PostalShipment] on [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [PK_PostalShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[EndiciaShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EndiciaShipment]
(
[ShipmentID] [bigint] NOT NULL,
[EndiciaAccountID] [bigint] NOT NULL,
[OriginalEndiciaAccountID] [bigint] NULL,
[StealthPostage] [bit] NOT NULL,
[NoPostage] [bit] NOT NULL,
[ReferenceID] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RubberStamp1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RubberStamp2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RubberStamp3] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TransactionID] [int] NULL,
[RefundFormID] [int] NULL,
[ScanFormID] [bigint] NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_EndiciaShipment]([ShipmentID], [EndiciaAccountID], [StealthPostage], [NoPostage], [ReferenceID], [RubberStamp1], [RubberStamp2], [RubberStamp3], [TransactionID], [RefundFormID], [ScanFormID]) SELECT [ShipmentID], [EndiciaAccountID], [StealthPostage], [NoPostage], [ReferenceID], [RubberStamp1], [RubberStamp2], [RubberStamp3], [TransactionID], [RefundFormID], [ScanFormID] FROM [dbo].[EndiciaShipment]
GO
DROP TABLE [dbo].[EndiciaShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EndiciaShipment]', N'EndiciaShipment'
GO
PRINT N'Creating primary key [PK_EndiciaShipment] on [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [PK_EndiciaShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], [Express1SingleSource], [EquaShipThermal], [EquaShipThermalType]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], 0, 0, [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType], 0, [EquaShipThermal], [EquaShipThermalType] FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD
CONSTRAINT [FK_EndiciaShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD
CONSTRAINT [FK_PostalShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD
CONSTRAINT [FK_StampsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'EndiciaAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'RefundFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'ScanFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'TransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'105', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'Confirmation'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'CustomsContentDescription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'CustomsContentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'DimsProfileID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'DimsWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'InsuranceValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'106', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'PackagingType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'104', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'Service'
GO


PRINT N'Altering [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] ADD
[ExpressSignatureWaiver] [bit] NULL
GO

UPDATE [dbo].PostalProfile
SET ExpressSignatureWaiver = 0
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (2,3,4,9) AND ShipmentTypePrimary = 1)