SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO


PRINT N'Altering [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] DROP
COLUMN [Street3]
GO


PRINT N'Dropping constraints from [dbo].[FedExEndOfDayClose]'
GO
ALTER TABLE [dbo].[FedExEndOfDayClose] DROP CONSTRAINT [PK_FedExEndOfDayClose]
GO
PRINT N'Dropping index [IX_FedExEndOfDayClose_CloseDate] from [dbo].[FedExEndOfDayClose]'
GO
DROP INDEX [IX_FedExEndOfDayClose_CloseDate] ON [dbo].[FedExEndOfDayClose]
GO
PRINT N'Rebuilding [dbo].[FedExEndOfDayClose]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FedExEndOfDayClose]
(
[FedExEndOfDayCloseID] [bigint] NOT NULL IDENTITY(1065, 1000),
[FedExAccountID] [bigint] NOT NULL,
[AccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CloseDate] [datetime] NOT NULL,
[IsSmartPost] [bit] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExEndOfDayClose] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_FedExEndOfDayClose]([FedExEndOfDayCloseID], [FedExAccountID], [AccountNumber], [CloseDate], IsSmartPost) SELECT [FedExEndOfDayCloseID], [FedExAccountID], [AccountNumber], [CloseDate], 0 FROM [dbo].[FedExEndOfDayClose]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExEndOfDayClose] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[FedExEndOfDayClose]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_FedExEndOfDayClose]', RESEED, @idVal)
GO
DROP TABLE [dbo].[FedExEndOfDayClose]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FedExEndOfDayClose]', N'FedExEndOfDayClose'
GO
PRINT N'Creating primary key [PK_FedExEndOfDayClose] on [dbo].[FedExEndOfDayClose]'
GO
ALTER TABLE [dbo].[FedExEndOfDayClose] ADD CONSTRAINT [PK_FedExEndOfDayClose] PRIMARY KEY CLUSTERED  ([FedExEndOfDayCloseID])
GO
PRINT N'Creating index [IX_FedExEndOfDayClose_CloseDate] on [dbo].[FedExEndOfDayClose]'
GO
CREATE NONCLUSTERED INDEX [IX_FedExEndOfDayClose_CloseDate] ON [dbo].[FedExEndOfDayClose] ([CloseDate]) INCLUDE ([FedExAccountID])
GO


PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping foreign keys from [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP
CONSTRAINT [FK_ShipmentCustomsItem_Shipment]
GO
PRINT N'Dropping constraints from [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP CONSTRAINT [PK_ShipmentCustomsItem]
GO
PRINT N'Rebuilding [dbo].[ShipmentCustomsItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShipmentCustomsItem]
(
[ShipmentCustomsItemID] [bigint] NOT NULL IDENTITY(1051, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentID] [bigint] NOT NULL,
[Description] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Quantity] [float] NOT NULL,
[Weight] [float] NOT NULL,
[UnitValue] [money] NOT NULL,
[CountryOfOrigin] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HarmonizedCode] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NumberOfPieces] [int] NOT NULL,
[UnitPriceAmount] [money] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ShipmentCustomsItem] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_ShipmentCustomsItem]([ShipmentCustomsItemID], [ShipmentID], [Description], [Quantity], [Weight], [UnitValue], [CountryOfOrigin], [HarmonizedCode], NumberOfPieces, UnitPriceAmount) SELECT [ShipmentCustomsItemID], [ShipmentID], [Description], [Quantity], [Weight], [UnitValue], [CountryOfOrigin], [HarmonizedCode], 0, 0 FROM [dbo].[ShipmentCustomsItem]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ShipmentCustomsItem] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[ShipmentCustomsItem]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_ShipmentCustomsItem]', RESEED, @idVal)
GO
DROP TABLE [dbo].[ShipmentCustomsItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShipmentCustomsItem]', N'ShipmentCustomsItem'
GO
PRINT N'Creating primary key [PK_ShipmentCustomsItem] on [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD CONSTRAINT [PK_ShipmentCustomsItem] PRIMARY KEY CLUSTERED  ([ShipmentCustomsItemID])
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ENABLE CHANGE_TRACKING
GO
PRINT N'Adding foreign keys to [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD
CONSTRAINT [FK_ShipmentCustomsItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO


PRINT N'Dropping foreign keys from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP
CONSTRAINT [FK_FedExPackage_FedExShipment]
GO
PRINT N'Dropping constraints from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [PK_FedExPackage]
GO
PRINT N'Dropping constraints from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [DF_FedExPackage_Insured]
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
[HazardousMaterialQuanityUnits] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FedExPackage] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_FedExPackage]([FedExPackageID], [ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [SkidPieces], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber],
[PriorityAlert],
[PriorityAlertEnhancementType],
[PriorityAlertDetailContent],
[DryIceWeight],
[ContainsAlcohol],
[DangerousGoodsEnabled],
[DangerousGoodsType],
[DangerousGoodsAccessibilityType],
[DangerousGoodsCargoAircraftOnly],
[DangerousGoodsEmergencyContactPhone],
[DangerousGoodsOfferor],
[DangerousGoodsPackagingCount],
[HazardousMaterialNumber],
[HazardousMaterialClass],
[HazardousMaterialProperName],
[HazardousMaterialPackingGroup],
[HazardousMaterialQuantityValue],
[HazardousMaterialQuanityUnits]
) SELECT [FedExPackageID], [ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [SkidPieces], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber],
0,
0,
'',
0,
0,
0,
0,
0,
0,
'',
'',
'',
'',
'',
'',
0,0,0
 FROM [dbo].[FedExPackage]
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
PRINT N'Adding foreign keys to [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD
CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE
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
PRINT N'Dropping constraints from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [PK_FedExShipment]
GO
PRINT N'Rebuilding [dbo].[FedExShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FedExShipment]
(
[ShipmentID] [bigint] NOT NULL,
[FedExAccountID] [bigint] NOT NULL,
[MasterFormID] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Service] [int] NOT NULL,
[Signature] [int] NOT NULL,
[PackagingType] [int] NOT NULL,
[NonStandardContainer] [bit] NOT NULL,
[ReferenceCustomer] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferenceInvoice] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferencePO] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorTransportType] [int] NOT NULL,
[PayorTransportName] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorTransportAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorDutiesType] [int] NOT NULL,
[PayorDutiesAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorDutiesName] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorDutiesCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[HomeDeliveryType] [int] NOT NULL,
[HomeDeliveryInstructions] [varchar] (74) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HomeDeliveryDate] [datetime] NOT NULL,
[HomeDeliveryPhone] [varchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FreightInsidePickup] [bit] NOT NULL,
[FreightInsideDelivery] [bit] NOT NULL,
[FreightBookingNumber] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FreightLoadAndCount] [int] NOT NULL,
[EmailNotifyBroker] [int] NOT NULL,
[EmailNotifySender] [int] NOT NULL,
[EmailNotifyRecipient] [int] NOT NULL,
[EmailNotifyOther] [int] NOT NULL,
[EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifyMessage] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodEnabled] [bit] NOT NULL,
[CodAmount] [money] NOT NULL,
[CodPaymentType] [int] NOT NULL,
[CodAddFreight] [bit] NOT NULL,
[CodOriginID] [bigint] NOT NULL,
[CodFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTrackingNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTrackingFormID] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTIN] [nvarchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodChargeBasis] [int] NOT NULL,
[CodAccountNumber] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerEnabled] [bit] NOT NULL,
[BrokerAccount] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerPhoneExtension] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsAdmissibilityPackaging] [int] NOT NULL,
[CustomsRecipientTIN] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [bit] NOT NULL,
[CustomsDocumentsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsExportFilingOption] [int] NOT NULL,
[CustomsAESEEI] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsRecipientIdentificationType] [int] NOT NULL,
[CustomsRecipientIdentificationValue] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsOptionsType] [int] NOT NULL,
[CustomsOptionsDesription] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoice] [bit] NOT NULL,
[CommercialInvoiceTermsOfSale] [int] NOT NULL,
[CommercialInvoicePurpose] [int] NOT NULL,
[CommercialInvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoiceFreight] [money] NOT NULL,
[CommercialInvoiceInsurance] [money] NOT NULL,
[CommercialInvoiceOther] [money] NOT NULL,
[CommercialInvoiceReference] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterOfRecord] [bit] NOT NULL,
[ImporterAccount] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterTIN] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterPostalCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostIndicia] [int] NOT NULL,
[SmartPostEndorsement] [int] NOT NULL,
[SmartPostConfirmation] [bit] NOT NULL,
[SmartPostCustomerManifest] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostHubID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DropoffType] [int] NOT NULL,
[OriginResidentialDetermination] [int] NOT NULL,
[FedExHoldAtLocationEnabled] [bit] NOT NULL,
[HoldLocationId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldLocationType] [int] NULL,
[HoldContactId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPersonName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldTitle] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldCompanyName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPhoneNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPhoneExtension] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPagerNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldFaxNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldEmailAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStreet1] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStreet2] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStreet3] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldCity] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStateOrProvinceCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldUrbanizationCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldCountryCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldResidential] [bit] NULL,
[CustomsNaftaEnabled] [bit] NOT NULL,
[CustomsNaftaPreferenceType] [int] NOT NULL,
[CustomsNaftaDeterminationCode] [int] NOT NULL,
[CustomsNaftaProducerId] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsNaftaNetCostMethod] [int] NOT NULL,
[ReturnType] [int] NOT NULL,
[RmaNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RmaReason] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnSaturdayPickup] [bit] NOT NULL,
[TrafficInArmsLicenseNumber] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternationalControlledExportDetailType] [int] NOT NULL,
[InternationalControlledExportDetailForeignTradeZoneCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternationalControlledExportDetailEntryNumber] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternationalControlledExportDetailLicenseOrPermitNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternationalControlledExportDetailLicenseOrPermitExpirationDate] [datetime] NULL,
[WeightUnitType] [int] NOT NULL,
[LinearUnitType] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_FedExShipment]([ShipmentID], [FedExAccountID], [MasterFormID], [Service], [Signature], [PackagingType], [NonStandardContainer], [ReferenceCustomer], [ReferenceInvoice], [ReferencePO], [PayorTransportType], [PayorTransportAccount], [PayorDutiesType], [PayorDutiesAccount], [SaturdayDelivery], [HomeDeliveryType], [HomeDeliveryInstructions], [HomeDeliveryDate], [HomeDeliveryPhone], [FreightInsidePickup], [FreightInsideDelivery], [FreightBookingNumber], [FreightLoadAndCount], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyMessage], [CodEnabled], [CodAmount], [CodPaymentType], [CodAddFreight], [CodOriginID], [CodFirstName], [CodLastName], [CodCompany], [CodStreet1], [CodStreet2], [CodStreet3], [CodCity], [CodStateProvCode], [CodPostalCode], [CodPhone], [CodTrackingNumber], [CodTrackingFormID], [BrokerEnabled], [BrokerAccount], [BrokerFirstName], [BrokerLastName], [BrokerCompany], [BrokerStreet1], [BrokerStreet2], [BrokerStreet3], [BrokerCity], [BrokerStateProvCode], [BrokerPostalCode], [BrokerCountryCode], [BrokerPhone], [CustomsAdmissibilityPackaging], [CustomsRecipientTIN], [CustomsDocumentsOnly], [CustomsDocumentsDescription], [CommercialInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [ImporterOfRecord], [ImporterAccount], [ImporterTIN], [ImporterFirstName], [ImporterLastName], [ImporterCompany], [ImporterStreet1], [ImporterStreet2], [ImporterStreet3], [ImporterCity], [ImporterStateProvCode], [ImporterPostalCode], [ImporterCountryCode], [ImporterPhone], [SmartPostIndicia], [SmartPostEndorsement], [SmartPostConfirmation], [SmartPostCustomerManifest], [SmartPostHubID],
[PayorTransportName],
[PayorDutiesName],
[PayorDutiesCountryCode],
[EmailNotifyBroker],
[CodCountryCode],
[CodTIN],
[CodChargeBasis],
[CodAccountNumber],
[BrokerPhoneExtension],
[BrokerEmail],
[CustomsExportFilingOption],
[CustomsAESEEI],
[CustomsRecipientIdentificationType],
[CustomsRecipientIdentificationValue],
[CustomsOptionsType],
[CustomsOptionsDesription],
[CommercialInvoiceReference],
[DropoffType],
[OriginResidentialDetermination],
[FedExHoldAtLocationEnabled],


[CustomsNaftaEnabled],
[CustomsNaftaPreferenceType],
[CustomsNaftaDeterminationCode],
[CustomsNaftaProducerId],
[CustomsNaftaNetCostMethod],

[ReturnType],
[RmaNumber],
[RmaReason],
[ReturnSaturdayPickup],
[TrafficInArmsLicenseNumber],
[InternationalControlledExportDetailType],
[InternationalControlledExportDetailForeignTradeZoneCode],
[InternationalControlledExportDetailEntryNumber],
[InternationalControlledExportDetailLicenseOrPermitNumber],
[WeightUnitType],
[LinearUnitType]
) SELECT [ShipmentID], [FedExAccountID], [MasterFormID], [Service], [Signature], [PackagingType], [NonStandardContainer], [ReferenceCustomer], [ReferenceInvoice], [ReferencePO], [PayorTransportType], [PayorTransportAccount], [PayorDutiesType], [PayorDutiesAccount], [SaturdayDelivery], [HomeDeliveryType], [HomeDeliveryInstructions], [HomeDeliveryDate], [HomeDeliveryPhone], [FreightInsidePickup], [FreightInsideDelivery], [FreightBookingNumber], [FreightLoadAndCount], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyMessage], [CodEnabled], [CodAmount], [CodPaymentType], [CodAddFreight], [CodOriginID], [CodFirstName], [CodLastName], [CodCompany], [CodStreet1], [CodStreet2], [CodStreet3], [CodCity], [CodStateProvCode], [CodPostalCode], [CodPhone], [CodTrackingNumber], [CodTrackingFormID], [BrokerEnabled], [BrokerAccount], [BrokerFirstName], [BrokerLastName], [BrokerCompany], [BrokerStreet1], [BrokerStreet2], [BrokerStreet3], [BrokerCity], [BrokerStateProvCode], [BrokerPostalCode], [BrokerCountryCode], [BrokerPhone], [CustomsAdmissibilityPackaging], [CustomsRecipientTIN], [CustomsDocumentsOnly], [CustomsDocumentsDescription], [CommercialInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [ImporterOfRecord], [ImporterAccount], [ImporterTIN], [ImporterFirstName], [ImporterLastName], [ImporterCompany], [ImporterStreet1], [ImporterStreet2], [ImporterStreet3], [ImporterCity], [ImporterStateProvCode], [ImporterPostalCode], [ImporterCountryCode], [ImporterPhone], [SmartPostIndicia], [SmartPostEndorsement], [SmartPostConfirmation], [SmartPostCustomerManifest], [SmartPostHubID],
'',
'',
'',
0,
'',
'',
0,
'',
'',
'',
0,
'',
0,
'',
0,
'',
'',
0,
0,
0,


0,
0,
0,
'',
0,

0,
'',
'',
0,
'',
0,
'',
'',
'',
'',
''

 FROM [dbo].[FedExShipment]
GO
DROP TABLE [dbo].[FedExShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FedExShipment]', N'FedExShipment'
GO
PRINT N'Creating primary key [PK_FedExShipment] on [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [PK_FedExShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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


PRINT N'Altering [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD
[EmailNotifyBroker] [int] NULL,
[DropoffType] [int] NULL,
[OriginResidentialDetermination] [int] NULL,
[PayorTransportName] [nchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnType] [int] NULL,
[RmaNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RmaReason] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnSaturdayPickup] [bit] NULL
GO
PRINT N'Altering [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD
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
[HazardousMaterialQuanityUnits] [int] NULL
GO








UPDATE fp
SET	fp.EmailNotifyBroker = 0,
	fp.DropoffType = 0,
	fp.OriginResidentialDetermination = 0,
	fp.PayorTransportName = '',
	fp.ReturnType = 0, 
	fp.RmaNumber='',
	fp.RmaReason='',
	fp.ReturnSaturdayPickup=0
FROM FedExProfile fp
INNER JOIN ShippingProfile sp
	ON fp.ShippingProfileID = sp.ShippingProfileID
WHERE sp.shipmentTypePrimary = 1






















PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCompany'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerFirstName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerLastName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAddFreight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD Amount', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCompany'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodFirstName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodLastName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodOriginID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTrackingFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTrackingNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceFreight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceInsurance'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoicePurpose'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceTermsOfSale'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAdmissibilityPackaging'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsDescription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsOnly'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientTIN'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyMessage'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyOtherAddress'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyRecipient'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifySender'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FedExAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightBookingNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsideDelivery'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsidePickup'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightLoadAndCount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryDate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryInstructions'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'113', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Home Delivery', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterAccount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCompany'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterFirstName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterLastName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterOfRecord'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterTIN'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'MasterFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'109', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PackagingType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Account', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesAccount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'110', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees To', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transportation Account', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportAccount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'110', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transporation To', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'108', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'114', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'Signature'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostConfirmation'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostCustomerManifest'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostEndorsement'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostHubID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostIndicia'
GO


/*---------------------------------------*
*
*
*/

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
EXEC sp_rename N'[dbo].[FedExShipment].[InternationalControlledExportDetailType]', N'IntlExportDetailType', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[FedExShipment].[InternationalControlledExportDetailForeignTradeZoneCode]', N'IntlExportDetailForeignTradeZoneCode', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[FedExShipment].[InternationalControlledExportDetailEntryNumber]', N'IntlExportDetailEntryNumber', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[FedExShipment].[InternationalControlledExportDetailLicenseOrPermitNumber]', N'IntlExportDetailLicenseOrPermitNumber', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[FedExShipment].[InternationalControlledExportDetailLicenseOrPermitExpirationDate]', N'IntlExportDetailLicenseOrPermitExpirationDate', 'COLUMN'
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'BuyerFeedbackPrivate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'BuyerFeedbackScore'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'CombinedLocally'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEbayItemCount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEffectiveCheckoutStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEffectivePaymentMethod'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackLeftComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackLeftType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackReceivedComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackReceivedType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupPayPalAddressStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupSellingManagerRecord'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'128', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
EXEC sp_addextendedproperty N'AuditName', N'Shipping Method', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerEmail'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhoneExtension'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAccountNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodChargeBasis'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTIN'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceReference'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAESEEI'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsExportFilingOption'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaDeterminationCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'NAFTA Selected', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaNetCostMethod'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaPreferenceType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaProducerId'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsOptionsDesription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsOptionsType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientIdentificationType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientIdentificationValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'123', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Dropoff Type', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyBroker'
GO
EXEC sp_addextendedproperty N'AuditName', N'Hold At Location Selected', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FedExHoldAtLocationEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCompanyName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldContactId'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldEmailAddress'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldFaxNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldLocationId'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldLocationType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPagerNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPersonName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPhoneExtension'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPhoneNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldResidential'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStateOrProvinceCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldTitle'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldUrbanizationCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailEntryNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailForeignTradeZoneCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailLicenseOrPermitExpirationDate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailLicenseOrPermitNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'126', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'LinearUnitType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Dimension Units', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'LinearUnitType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'111', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditName', N'Origin Residential \ Commercial', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Country', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Name', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesName'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transportation Name', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'124', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ReturnType'
GO
EXEC sp_addextendedproperty N'AuditName', N'RMA Number', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaNumber'
GO
EXEC sp_addextendedproperty N'AuditName', N'RMA Reason', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaReason'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'TrafficInArmsLicenseNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'125', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'WeightUnitType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Weight Units', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'WeightUnitType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'iParcelAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'127', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'TrackBySMS'
GO