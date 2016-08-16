EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'OtherShipment', 'COLUMN', N'InsuranceType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'InsuranceType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'InsuranceType'
GO
-- Marker
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[ShippingProfile] DISABLE CHANGE_TRACKING
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[Shipment] DISABLE CHANGE_TRACKING
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[EndiciaAccount] DISABLE CHANGE_TRACKING
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
PRINT N'Dropping foreign keys from [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] DROP
CONSTRAINT [FK_UpsProfile_ShippingProfile]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] DROP
CONSTRAINT [FK_UpsProfilePackage_UpsProfile]
GO
PRINT N'Dropping foreign keys from [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] DROP
CONSTRAINT [FK_WorldShipShipment_UpsShipment]
GO
PRINT N'Dropping foreign keys from [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] DROP
CONSTRAINT [FK_WorldShipPackage_WorldShipShipment]
GO
PRINT N'Dropping constraints from [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] DROP CONSTRAINT [PK_EndiciaAccount]
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
PRINT N'Dropping constraints from [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] DROP CONSTRAINT [PK_UpsProfile]
GO
PRINT N'Dropping constraints from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT [PK_UpsShipment]
GO
PRINT N'Dropping constraints from [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] DROP CONSTRAINT [PK_WorldShipPackage]
GO
PRINT N'Dropping constraints from [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] DROP CONSTRAINT [DF_WorldShipPackage_Length]
GO
PRINT N'Dropping index [IX_Shipment_OrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment]
GO

-- BN: Moved up
PRINT N'Altering [dbo].[ShippingProfile]'
GO
ALTER TABLE [dbo].[ShippingProfile] ADD
[InsuranceType] [int] NULL
GO
ALTER TABLE [dbo].[ShippingProfile] ENABLE CHANGE_TRACKING
GO

-- Update the default for the default profiles
UPDATE ShippingProfile SET InsuranceType = 0 WHERE ShipmentTypePrimary = 1
GO

-- Update the default InsuranceType before the columns get wiped from the other tables
UPDATE ShippingProfile 
  SET ShippingProfile.InsuranceType = p.InsuranceType
  FROM FedExProfile p
  WHERE ShippingProfile.ShippingProfileID = p.ShippingProfileID
GO
UPDATE ShippingProfile 
  SET ShippingProfile.InsuranceType = p.InsuranceType
  FROM OtherProfile p
  WHERE ShippingProfile.ShippingProfileID = p.ShippingProfileID
GO
UPDATE ShippingProfile 
  SET ShippingProfile.InsuranceType = p.InsuranceType
  FROM PostalProfile p
  WHERE ShippingProfile.ShippingProfileID = p.ShippingProfileID
GO
UPDATE ShippingProfile 
  SET ShippingProfile.InsuranceType = p.InsuranceType
  FROM UpsProfile p
  WHERE ShippingProfile.ShippingProfileID = p.ShippingProfileID
GO

-- BN: Moved up
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
[InsuranceType] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] ON
GO

-- BN: Edited
INSERT INTO [dbo].[tmp_rg_xx_Shipment]([ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [TrackingNumber], [CustomsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], [InsuranceType]) SELECT [ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [TrackingNumber], [CustomsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], 0 FROM [dbo].[Shipment]
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

-- Copy data from derived tables down to base
UPDATE Shipment 
   SET Shipment.InsuranceType = s.InsuranceType 
   FROM FedExShipment s 
   WHERE Shipment.ShipmentID = s.ShipmentID AND Shipment.ShipmentType = 6
GO
UPDATE Shipment 
   SET Shipment.InsuranceType = s.InsuranceType 
   FROM OtherShipment s 
   WHERE Shipment.ShipmentID = s.ShipmentID and Shipment.ShipmentType = 5
GO
UPDATE Shipment 
   SET Shipment.InsuranceType = s.InsuranceType 
   FROM PostalShipment s 
   WHERE Shipment.ShipmentID = s.ShipmentID and Shipment.ShipmentType IN (2, 3, 4, 9)
GO
UPDATE Shipment 
   SET Shipment.InsuranceType = s.InsuranceType 
   FROM UpsShipment s 
   WHERE Shipment.ShipmentID = s.ShipmentID and Shipment.ShipmentType IN (0, 1)
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
[InsuranceAgreement] [bit] NOT NULL,
[FedExUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExMaskAccount] [bit] NOT NULL,
[FedExLimitValue] [bit] NOT NULL,
[FedExThermal] [bit] NOT NULL,
[FedExThermalType] [int] NOT NULL,
[FedExThermalDocTab] [bit] NOT NULL,
[FedExThermalDocTabType] [int] NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsThermal] [bit] NOT NULL,
[UpsThermalType] [int] NOT NULL,
[EndiciaThermal] [bit] NOT NULL,
[EndiciaThermalType] [int] NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EndiciaThermalDocTab] [bit] NOT NULL,
[EndiciaThermalDocTabType] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_EndiciaThermalDocTabType] DEFAULT ((0)),
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
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [WorldShipLaunch], [WorldShipServices], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [WorldShipLaunch], '', [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType] FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Rebuilding [dbo].[WorldShipPackage]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_WorldShipPackage]
(
[UpsPackageID] [bigint] NOT NULL,
[ShipmentID] [bigint] NOT NULL,
[PackageType] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Weight] [float] NOT NULL,
[ReferenceNumber] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferenceNumber2] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodAmount] [money] NOT NULL,
[CodCashOnly] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DeliveryConfirmation] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DeliveryConfirmationSignature] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DeliveryConfirmationAdult] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Length] [int] NOT NULL CONSTRAINT [DF_WorldShipPackage_Length] DEFAULT (''),
[Width] [int] NOT NULL,
[Height] [int] NOT NULL
)
GO

-- BN: Edited
INSERT INTO [dbo].[tmp_rg_xx_WorldShipPackage]([UpsPackageID], [ShipmentID], [PackageType], [Weight], [ReferenceNumber], [ReferenceNumber2], [CodOption], [CodAmount], [CodCashOnly], [DeliveryConfirmation], [DeliveryConfirmationSignature], [DeliveryConfirmationAdult], [Length], [Width], [Height]) SELECT [UpsPackageID], [ShipmentID], [PackageType], [Weight], [ReferenceNumber], '', [CodOption], [CodAmount], [CodCashOnly], [DeliveryConfirmation], [DeliveryConfirmationSignature], [DeliveryConfirmationAdult], [Length], [Width], [Height] FROM [dbo].[WorldShipPackage]
GO
DROP TABLE [dbo].[WorldShipPackage]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_WorldShipPackage]', N'WorldShipPackage'
GO
PRINT N'Creating primary key [PK_WorldShipPackage] on [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD CONSTRAINT [PK_WorldShipPackage] PRIMARY KEY CLUSTERED  ([UpsPackageID])
GO
PRINT N'Altering [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] DROP
COLUMN [InsuranceType]
GO
PRINT N'Altering [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP
COLUMN [InsuranceType]
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP
COLUMN [InsuranceType]
GO
PRINT N'Altering [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] DROP
COLUMN [InsuranceType]
GO
PRINT N'Altering [dbo].[OtherProfile]'
GO
ALTER TABLE [dbo].[OtherProfile] DROP
COLUMN [InsuranceType]
GO
PRINT N'Altering [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] DROP
COLUMN [InsuranceType]
GO
PRINT N'Rebuilding [dbo].[UpsShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UpsShipment]
(
[ShipmentID] [bigint] NOT NULL,
[UpsAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[CodEnabled] [bit] NOT NULL,
[CodAmount] [money] NOT NULL,
[CodPaymentType] [int] NOT NULL,
[DeliveryConfirmation] [int] NOT NULL,
[ReferenceNumber] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferenceNumber2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorType] [int] NOT NULL,
[PayorAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifySender] [int] NOT NULL,
[EmailNotifyRecipient] [int] NOT NULL,
[EmailNotifyOther] [int] NOT NULL,
[EmailNotifyOtherAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifyFrom] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifySubject] [int] NOT NULL,
[EmailNotifyMessage] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [bit] NOT NULL,
[CustomsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoice] [bit] NOT NULL,
[CommercialInvoiceTermsOfSale] [int] NOT NULL,
[CommercialInvoicePurpose] [int] NOT NULL,
[CommercialInvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoiceFreight] [money] NOT NULL,
[CommercialInvoiceInsurance] [money] NOT NULL,
[CommercialInvoiceOther] [money] NOT NULL,
[WorldShipStatus] [int] NOT NULL,
[PublishedCharges] [money] NOT NULL,
[NegotiatedRate] [bit] NOT NULL,
[ReturnService] [int] NOT NULL,
[ReturnUndeliverableEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnContents] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO

-- BN: Edited
INSERT INTO [dbo].[tmp_rg_xx_UpsShipment]([ShipmentID], [UpsAccountID], [Service], [SaturdayDelivery], [CodEnabled], [CodAmount], [CodPaymentType], [DeliveryConfirmation], [ReferenceNumber], [ReferenceNumber2], [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [CustomsDocumentsOnly], [CustomsDescription], [CommercialInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [WorldShipStatus], [PublishedCharges], [NegotiatedRate], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents], [UspsTrackingNumber]) SELECT [ShipmentID], [UpsAccountID], [Service], [SaturdayDelivery], [CodEnabled], [CodAmount], [CodPaymentType], [DeliveryConfirmation], [ReferenceNumber], '', [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [CustomsDocumentsOnly], [CustomsDescription], [CommercialInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [WorldShipStatus], [PublishedCharges], [NegotiatedRate], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents], '' FROM [dbo].[UpsShipment]
GO
DROP TABLE [dbo].[UpsShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UpsShipment]', N'UpsShipment'
GO
PRINT N'Creating primary key [PK_UpsShipment] on [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [PK_UpsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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
[InsuranceValue] [money] NOT NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UpsPackage] ON
GO

-- BN: Edited
INSERT INTO [dbo].[tmp_rg_xx_UpsPackage]([UpsPackageID], [ShipmentID], [PackagingType], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [InsuranceValue], [TrackingNumber], [UspsTrackingNumber]) SELECT [UpsPackageID], [ShipmentID], [PackagingType], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [InsuranceValue], [TrackingNumber], '' FROM [dbo].[UpsPackage]
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
PRINT N'Rebuilding [dbo].[UpsProfile]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UpsProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[UpsAccountID] [bigint] NULL,
[Service] [int] NULL,
[SaturdayDelivery] [bit] NULL,
[ResidentialDetermination] [int] NULL,
[DeliveryConfirmation] [int] NULL,
[ReferenceNumber] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReferenceNumber2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorType] [int] NULL,
[PayorAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifySender] [int] NULL,
[EmailNotifyRecipient] [int] NULL,
[EmailNotifyOther] [int] NULL,
[EmailNotifyOtherAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifyFrom] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifySubject] [int] NULL,
[EmailNotifyMessage] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnService] [int] NULL,
[ReturnUndeliverableEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnContents] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_UpsProfile]([ShippingProfileID], [UpsAccountID], [Service], [SaturdayDelivery], [ResidentialDetermination], [DeliveryConfirmation], [ReferenceNumber], [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents]) SELECT [ShippingProfileID], [UpsAccountID], [Service], [SaturdayDelivery], [ResidentialDetermination], [DeliveryConfirmation], [ReferenceNumber], [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents] FROM [dbo].[UpsProfile]
GO
DROP TABLE [dbo].[UpsProfile]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UpsProfile]', N'UpsProfile'
GO
PRINT N'Creating primary key [PK_UpsProfile] on [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD CONSTRAINT [PK_UpsProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Altering [dbo].[WorldShipProcessed]'
GO
ALTER TABLE [dbo].[WorldShipProcessed] ADD
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
ALTER TABLE [dbo].[WorldShipProcessed] ALTER COLUMN [TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Rebuilding [dbo].[EndiciaAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EndiciaAccount]
(
[EndiciaAccountID] [bigint] NOT NULL IDENTITY(1066, 1000),
[EndiciaReseller] [int] NOT NULL,
[AccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignupConfirmation] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WebPassword] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiInitialPassword] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserPassword] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccountType] [int] NOT NULL,
[TestAccount] [bit] NOT NULL,
[CreatedByShipWorks] [bit] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Fax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EndiciaAccount] ON
GO
-- BN: Manually Edited
INSERT INTO [dbo].[tmp_rg_xx_EndiciaAccount]([EndiciaAccountID], [EndiciaReseller], [AccountNumber], [SignupConfirmation], [WebPassword], [ApiInitialPassword], [ApiUserPassword], [AccountType], [TestAccount], [CreatedByShipWorks], [Description], [FirstName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], [MailingPostalCode]) SELECT [EndiciaAccountID], [EndiciaReseller], [AccountNumber], [SignupConfirmation], [WebPassword], [ApiInitialPassword], [ApiUserPassword], [AccountType], [TestAccount], [CreatedByShipWorks], [Description], [FirstName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Fax], [Email], '' FROM [dbo].[EndiciaAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EndiciaAccount] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[EndiciaAccount]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_EndiciaAccount]', RESEED, @idVal)
GO
DROP TABLE [dbo].[EndiciaAccount]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EndiciaAccount]', N'EndiciaAccount'
GO
PRINT N'Creating primary key [PK_EndiciaAccount] on [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] ADD CONSTRAINT [PK_EndiciaAccount] PRIMARY KEY CLUSTERED  ([EndiciaAccountID])
GO
ALTER TABLE [dbo].[EndiciaAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Adding constraints to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [IX_Shipment_Other] UNIQUE NONCLUSTERED  ([ShipmentID])
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
PRINT N'Adding foreign keys to [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD
CONSTRAINT [FK_UpsProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] ADD
CONSTRAINT [FK_UpsProfilePackage_UpsProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[UpsProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ADD
CONSTRAINT [FK_WorldShipShipment_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD
CONSTRAINT [FK_WorldShipPackage_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ContentWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsGenerated'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Insurance', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'OriginCountry', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
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
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'TotalWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD Amount', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceFreight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceInsurance'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoicePurpose'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceTermsOfSale'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CustomsDescription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CustomsDocumentsOnly'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'116', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'DeliveryConfirmation'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyFrom'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyMessage'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyOtherAddress'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyRecipient'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifySender'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifySubject'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'NegotiatedRate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'117', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PublishedCharges'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'115', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'UpsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'WorldShipStatus'
GO


-- Handle the new profile value
PRINT N'Updating primary UPS Worldship Profile'
GO
UPDATE [dbo].UpsProfile
SET 
	ReferenceNumber2 = ''
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (0,1) AND ShipmentTypePrimary = 1)