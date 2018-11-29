

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[BigCommerceOrderItem]'
GO
CREATE TABLE [dbo].[BigCommerceOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[OrderAddressID] [bigint] NOT NULL,
[OrderProductID] [bigint] NOT NULL,
[ParentOrderProductID] [bigint] NULL,
[IsDigitalItem] [bit] NOT NULL CONSTRAINT [DF_BigCommerceOrderItem_IsDigitalItem] DEFAULT ((0)),
[EventDate] [datetime] NULL,
[EventName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_BigCommerceOrderItem] on [dbo].[BigCommerceOrderItem]'
GO
ALTER TABLE [dbo].[BigCommerceOrderItem] ADD CONSTRAINT [PK_BigCommerceOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO

PRINT N'Creating [dbo].[AmazonServiceType]'
GO
CREATE TABLE [dbo].[AmazonServiceType]
(
[AmazonServiceTypeID] [int] NOT NULL IDENTITY(1, 1),
[ApiValue] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonServiceTypeID] on [dbo].[AmazonServiceType]'
GO
ALTER TABLE [dbo].[AmazonServiceType] ADD CONSTRAINT [PK_AmazonServiceTypeID] PRIMARY KEY CLUSTERED  ([AmazonServiceTypeID])
GO
PRINT N'Creating index [IX_SWDefault_AmazonServiceType_ApiValue] on [dbo].[AmazonServiceType]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_AmazonServiceType_ApiValue] ON [dbo].[AmazonServiceType] ([ApiValue])
GO

PRINT N'Creating [dbo].[FedExPackage]'
GO
CREATE TABLE [dbo].[FedExPackage]
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
[HazardousMaterialQuanityUnits] [int] NOT NULL,
[HazardousMaterialTechnicalName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SignatoryContactName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SignatoryTitle] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SignatoryPlace] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AlcoholRecipientType] [int] NOT NULL,
[ContainerType] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NumberOfContainers] [int] NOT NULL,
[PackingDetailsCargoAircraftOnly] [bit] NOT NULL,
[PackingDetailsPackingInstructions] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BatteryMaterial] [int] NOT NULL,
[BatteryPacking] [int] NOT NULL,
[BatteryRegulatorySubtype] [int] NOT NULL,
[FreightPackaging] [int] NOT NULL,
[FreightPieces] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FedExPackage] on [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD CONSTRAINT [PK_FedExPackage] PRIMARY KEY CLUSTERED  ([FedExPackageID])
GO
PRINT N'Creating index [IX_SWDefault_FedExPackage_ShipmentID] on [dbo].[FedExPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FedExPackage_ShipmentID] ON [dbo].[FedExPackage] ([ShipmentID])
GO
PRINT N'Creating [dbo].[GenericFileStore]'
GO
CREATE TABLE [dbo].[GenericFileStore]
(
[StoreID] [bigint] NOT NULL,
[FileFormat] [int] NOT NULL,
[FileSource] [int] NOT NULL,
[DiskFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpAccountID] [bigint] NULL,
[FtpFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailAccountID] [bigint] NULL,
[EmailFolder] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GenericFileStore_EmailIncomingFolder] DEFAULT (''),
[EmailFolderValidityID] [bigint] NOT NULL,
[EmailFolderLastMessageID] [bigint] NOT NULL,
[EmailOnlyUnread] [bit] NOT NULL,
[NamePatternMatch] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NamePatternSkip] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SuccessAction] [int] NOT NULL,
[SuccessMoveFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ErrorAction] [int] NOT NULL,
[ErrorMoveFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[XmlXsltFileName] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[XmlXsltContent] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FlatImportMap] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_GenericFileStore] on [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD CONSTRAINT [PK_GenericFileStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[ObjectReference]'
GO
CREATE TABLE [dbo].[ObjectReference]
(
[ObjectReferenceID] [bigint] NOT NULL IDENTITY(1030, 1000),
[ConsumerID] [bigint] NOT NULL,
[ReferenceKey] [varchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ObjectReference_ReferenceKey] DEFAULT (''),
[ObjectID] [bigint] NOT NULL,
[Reason] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_ObjectReference] on [dbo].[ObjectReference]'
GO
ALTER TABLE [dbo].[ObjectReference] ADD CONSTRAINT [PK_ObjectReference] PRIMARY KEY CLUSTERED  ([ObjectReferenceID])
GO
PRINT N'Creating index [IX_SWDefault_ObjectReference_ConsumerIDReferenceKey] on [dbo].[ObjectReference]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ObjectReference_ConsumerIDReferenceKey] ON [dbo].[ObjectReference] ([ConsumerID], [ReferenceKey]) INCLUDE ([ObjectID])
GO
PRINT N'Creating index [IX_SWDefault_ObjectReference_ObjectID] on [dbo].[ObjectReference]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ObjectReference_ObjectID] ON [dbo].[ObjectReference] ([ObjectID])
GO
PRINT N'Creating [dbo].[EbayOrder]'
GO
CREATE TABLE [dbo].[EbayOrder]
(
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[EbayBuyerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CombinedLocally] [bit] NOT NULL,
[SelectedShippingMethod] [int] NOT NULL CONSTRAINT [DF__EbayOrder__Selec__2B203F5D] DEFAULT ((0)),
[SellingManagerRecord] [int] NULL,
[GspEligible] [bit] NOT NULL,
[GspFirstName] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspLastName] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspStreet1] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspStreet2] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspCity] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspStateProvince] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspPostalCode] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspCountryCode] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspReferenceID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RollupEbayItemCount] [int] NOT NULL,
[RollupEffectiveCheckoutStatus] [int] NULL,
[RollupEffectivePaymentMethod] [int] NULL,
[RollupFeedbackLeftType] [int] NULL,
[RollupFeedbackLeftComments] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupFeedbackReceivedType] [int] NULL,
[RollupFeedbackReceivedComments] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupPayPalAddressStatus] [int] NULL,
[GuaranteedDelivery] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EbayOrder] on [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD CONSTRAINT [PK_EbayOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_EbayOrder_EbayBuyerID] on [dbo].[EbayOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrder_EbayBuyerID] ON [dbo].[EbayOrder] ([EbayBuyerID])
GO
PRINT N'Creating index [IX_SWDefault_EbayOrder_GuaranteedDelivery] on [dbo].[EbayOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrder_GuaranteedDelivery] ON [dbo].[EbayOrder] ([GuaranteedDelivery])
GO
PRINT N'Creating index [IX_SWDefault_EbayOrder_OrderID_Includes_CheckoutStatus_GspEligible] on [dbo].[EbayOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrder_OrderID_Includes_CheckoutStatus_GspEligible] ON [dbo].[EbayOrder] (	[OrderID] ASC)
	INCLUDE ([RollupEffectiveCheckoutStatus], [GspEligible]) ON [PRIMARY]
GO
PRINT N'Creating [dbo].[WorldShipPackage]'
GO
CREATE TABLE [dbo].[WorldShipPackage]
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
[Height] [int] NOT NULL,
[DeclaredValueAmount] [float] NULL,
[DeclaredValueOption] [nchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CN22GoodsType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CN22Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PostalSubClass] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MIDeliveryConfirmation] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnFrom] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnSubjectLine] [nvarchar] (18) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnMemo] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn1ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn1ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn1Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn2ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn2ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn2Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn3ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn3ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn3Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipperRelease] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AdditionalHandlingEnabled] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationTelephone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceRegulationSet] [nvarchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceWeight] [float] NULL,
[DryIceMedicalPurpose] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceWeightUnitOfMeasure] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_WorldShipPackage] on [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD CONSTRAINT [PK_WorldShipPackage] PRIMARY KEY CLUSTERED  ([UpsPackageID])
GO
PRINT N'Creating index [IX_SWDefault_WorldShipPackage_ShipmentID] on [dbo].[WorldShipPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_WorldShipPackage_ShipmentID] ON [dbo].[WorldShipPackage] ([ShipmentID])
GO
PRINT N'Creating [dbo].[Action]'
GO
CREATE TABLE [dbo].[Action]
(
[ActionID] [bigint] NOT NULL IDENTITY(1040, 1000),
[RowVersion] [timestamp] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Enabled] [bit] NOT NULL,
[ComputerLimitedType] [int] NOT NULL,
[ComputerLimitedList] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StoreLimited] [bit] NOT NULL,
[StoreLimitedList] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TriggerType] [int] NOT NULL,
[TriggerSettings] [xml] NOT NULL,
[TaskSummary] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternalOwner] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_Action] on [dbo].[Action]'
GO
ALTER TABLE [dbo].[Action] ADD CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED  ([ActionID])
GO
ALTER TABLE [dbo].[Action] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Action]'
GO
PRINT N'Creating [dbo].[ActionFilterTrigger]'
GO
CREATE TABLE [dbo].[ActionFilterTrigger]
(
[ActionID] [bigint] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[Direction] [int] NOT NULL,
[ComputerLimitedType] [int] NOT NULL,
[ComputerLimitedList] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ActionFilterTrigger] on [dbo].[ActionFilterTrigger]'
GO
ALTER TABLE [dbo].[ActionFilterTrigger] ADD CONSTRAINT [PK_ActionFilterTrigger] PRIMARY KEY CLUSTERED  ([ActionID])
GO
PRINT N'Creating [dbo].[Computer]'
GO
CREATE TABLE [dbo].[Computer]
(
[ComputerID] [bigint] NOT NULL IDENTITY(1001, 1000),
[RowVersion] [timestamp] NOT NULL,
[Identifier] [uniqueidentifier] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_Computer] on [dbo].[Computer]'
GO
ALTER TABLE [dbo].[Computer] ADD CONSTRAINT [PK_Computer] PRIMARY KEY CLUSTERED  ([ComputerID])
GO
ALTER TABLE [dbo].[Computer] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Computer]'
GO
PRINT N'Creating [dbo].[ActionQueueSelection]'
GO
CREATE TABLE [dbo].[ActionQueueSelection]
(
[ActionQueueSelectionID] [bigint] NOT NULL IDENTITY(1097, 1000),
[ActionQueueID] [bigint] NOT NULL,
[ObjectID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ActionQueueSelection] on [dbo].[ActionQueueSelection]'
GO
ALTER TABLE [dbo].[ActionQueueSelection] ADD CONSTRAINT [PK_ActionQueueSelection] PRIMARY KEY CLUSTERED  ([ActionQueueSelectionID])
GO
PRINT N'Creating index [IX_SWDefault_ActionQueueSelection_ActionQueueID] on [dbo].[ActionQueueSelection]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ActionQueueSelection_ActionQueueID] ON [dbo].[ActionQueueSelection] ([ActionQueueID], [ObjectID])

GO
PRINT N'Creating [dbo].[ActionQueueStep]'
GO
CREATE TABLE [dbo].[ActionQueueStep]
(
[ActionQueueStepID] [bigint] NOT NULL IDENTITY(1043, 1000),
[RowVersion] [timestamp] NOT NULL,
[ActionQueueID] [bigint] NOT NULL,
[StepStatus] [int] NOT NULL,
[StepIndex] [int] NOT NULL,
[StepName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TaskIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TaskSettings] [xml] NOT NULL,
[InputSource] [int] NOT NULL,
[InputFilterNodeID] [bigint] NOT NULL,
[FilterCondition] [bit] NOT NULL,
[FilterConditionNodeID] [bigint] NOT NULL,
[FlowSuccess] [int] NOT NULL,
[FlowSkipped] [int] NOT NULL,
[FlowError] [int] NOT NULL,
[AttemptDate] [datetime] NOT NULL,
[AttemptError] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AttemptCount] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_QueueStep] on [dbo].[ActionQueueStep]'
GO
ALTER TABLE [dbo].[ActionQueueStep] ADD CONSTRAINT [PK_QueueStep] PRIMARY KEY CLUSTERED  ([ActionQueueStepID])
GO
PRINT N'Creating index [IX_SWDefault_ActionQueueStep_ActionQueue] on [dbo].[ActionQueueStep]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ActionQueueStep_ActionQueue] ON [dbo].[ActionQueueStep] ([ActionQueueID], [StepIndex])
GO

PRINT N'Creating [dbo].[ActionTask]'
GO
CREATE TABLE [dbo].[ActionTask]
(
[ActionTaskID] [bigint] NOT NULL IDENTITY(1042, 1000),
[ActionID] [bigint] NOT NULL,
[TaskIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TaskSettings] [xml] NOT NULL,
[StepIndex] [int] NOT NULL,
[InputSource] [int] NOT NULL,
[InputFilterNodeID] [bigint] NOT NULL,
[FilterCondition] [bit] NOT NULL,
[FilterConditionNodeID] [bigint] NOT NULL,
[FlowSuccess] [int] NOT NULL,
[FlowSkipped] [int] NOT NULL,
[FlowError] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ActionTask] on [dbo].[ActionTask]'
GO
ALTER TABLE [dbo].[ActionTask] ADD CONSTRAINT [PK_ActionTask] PRIMARY KEY CLUSTERED  ([ActionTaskID])
GO
ALTER TABLE [dbo].[ActionTask] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[ActionTask]'
GO
PRINT N'Creating [dbo].[AmazonStore]'
GO
CREATE TABLE [dbo].[AmazonStore]
(
[StoreID] [bigint] NOT NULL,
[AmazonApi] [int] NOT NULL,
[AmazonApiRegion] [char](2) NOT NULL,
[SellerCentralUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellerCentralPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantName] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantToken] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccessKeyID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuthToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Cookie] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CookieExpires] [datetime] NOT NULL,
[CookieWaitUntil] [datetime] NOT NULL,
[Certificate] [varbinary] (2048) NULL,
[WeightDownloads] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ExcludeFBA] [bit] NOT NULL,
[DomainName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonVATS] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonStore] on [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD CONSTRAINT [PK_AmazonStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[AmazonASIN]'
GO
CREATE TABLE [dbo].[AmazonASIN]
(
[StoreID] [bigint] NOT NULL,
[SKU] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonASIN] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonASIN] on [dbo].[AmazonASIN]'
GO
ALTER TABLE [dbo].[AmazonASIN] ADD CONSTRAINT [PK_AmazonASIN] PRIMARY KEY CLUSTERED  ([StoreID], [SKU])
GO
PRINT N'Creating [dbo].[Order]'
GO
CREATE TABLE [dbo].[Order]
(
[OrderID] [bigint] NOT NULL IDENTITY(1006, 1000),
[RowVersion] [timestamp] NOT NULL,
[StoreID] [bigint] NOT NULL,
[CustomerID] [bigint] NOT NULL,
[OrderNumber] [bigint] NOT NULL,
[OrderNumberComplete] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrderDate] [datetime] NOT NULL,
[OrderTotal] [money] NOT NULL,
[LocalStatus] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL,
[OnlineLastModified] [datetime2] NOT NULL,
[OnlineCustomerID] [sql_variant] NULL,
[OnlineStatus] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineStatusCode] [sql_variant] NULL,
[RequestedShipping] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillMiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCompany] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillAddressValidationSuggestionCount] [int] NOT NULL,
[BillAddressValidationStatus] [int] NOT NULL,
[BillAddressValidationError] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillResidentialStatus] [int] NOT NULL,
[BillPOBox] [int] NOT NULL,
[BillUSTerritory] [int] NOT NULL,
[BillMilitaryAddress] [int] NOT NULL,
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
[ShipFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipAddressValidationSuggestionCount] [int] NOT NULL,
[ShipAddressValidationStatus] [int] NOT NULL,
[ShipAddressValidationError] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipResidentialStatus] [int] NOT NULL,
[ShipPOBox] [int] NOT NULL,
[ShipUSTerritory] [int] NOT NULL,
[ShipMilitaryAddress] [int] NOT NULL,
[RollupItemCount] [int] NOT NULL,
[RollupItemName] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemSKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemLocation] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemQuantity] [float] NULL,
[RollupItemTotalWeight] [decimal] (29, 9) NOT NULL,
[RollupNoteCount] [int] NOT NULL,
[BillNameParseStatus] [int] NOT NULL,
[BillUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipNameParseStatus] [int] NOT NULL,
[ShipUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipSenseHashKey] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
[ShipSenseRecognitionStatus] int NOT NULL,
[ShipAddressType] [int] NOT NULL,
[CombineSplitStatus] [INT] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Order] on [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_Order_OnlineLastModified_StoreID_IsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order] ([OnlineLastModified] DESC, [StoreID], [IsManual])
GO
PRINT N'Creating index [IX_SWDefault_CustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_CustomerID] ON [dbo].[Order] ([CustomerID])
GO
PRINT N'Creating index [IX_SWDefault_OrderNumber] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderNumber] ON [dbo].[Order] ([OrderNumber])
GO
PRINT N'Creating index [IX_SWDefault_OrderNumberComplete] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderNumberComplete] ON [dbo].[Order] ([OrderNumberComplete])
GO
PRINT N'Creating index [IX_SWDefault_OrderDate] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderDate] ON [dbo].[Order] ([OrderDate]) INCLUDE ([IsManual])
GO
PRINT N'Creating index [IX_SWDefault_OrderTotal] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderTotal] ON [dbo].[Order] ([OrderTotal])
GO
PRINT N'Creating index [IX_SWDefault_LocalStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_LocalStatus] ON [dbo].[Order] ([LocalStatus])
GO
PRINT N'Creating index [IX_SWDefault_Order_OnlineCustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_OnlineCustomerID] ON [dbo].[Order] ([OnlineCustomerID])
GO
PRINT N'Creating index [IX_SWDefault_OnlineStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OnlineStatus] ON [dbo].[Order] ([OnlineStatus])
GO
PRINT N'Creating index [IX_SWDefault_Order_StoreIdOnlineStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_StoreIdOnlineStatus] ON [dbo].[Order] ([StoreId], [OnlineStatus])
GO
PRINT N'Creating index [IX_SWDefault_RequestedShipping] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RequestedShipping] ON [dbo].[Order] ([RequestedShipping])
GO
PRINT N'Creating index [IX_SWDefault_BillFirstName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillFirstName] ON [dbo].[Order] ([BillFirstName])
GO
PRINT N'Creating index [IX_SWDefault_BillLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillLastName] ON [dbo].[Order] ([BillLastName])
GO
PRINT N'Creating index [IX_SWDefault_BillCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillCompany] ON [dbo].[Order] ([BillCompany])
GO
PRINT N'Creating index [IX_SWDefault_BillStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillStateProvCode] ON [dbo].[Order] ([BillStateProvCode])
GO
PRINT N'Creating index [IX_SWDefault_BillPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillPostalCode] ON [dbo].[Order] ([BillPostalCode])
GO
PRINT N'Creating index [IX_SWDefault_BillCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillCountryCode] ON [dbo].[Order] ([BillCountryCode])
GO
PRINT N'Creating index [IX_SWDefault_BillEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillEmail] ON [dbo].[Order] ([BillEmail])
GO
PRINT N'Creating index [IX_SWDefault_ShipLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipLastName] ON [dbo].[Order] ([ShipLastName])
GO
PRINT N'Creating index [IX_SWDefault_ShipCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipCompany] ON [dbo].[Order] ([ShipCompany])
GO
PRINT N'Creating index [IX_SWDefault_ShipStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipStateProvCode] ON [dbo].[Order] ([ShipStateProvCode])
GO
PRINT N'Creating index [IX_SWDefault_ShipPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode])
GO
PRINT N'Creating index [IX_SWDefault_ShipCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipCountryCode] ON [dbo].[Order] ([ShipCountryCode])
GO
PRINT N'Creating index [IX_SWDefault_ShipEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipEmail] ON [dbo].[Order] ([ShipEmail])
GO
PRINT N'Creating index [IX_SWDefault_RollupItemCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupItemCount] ON [dbo].[Order] ([RollupItemCount])
GO
PRINT N'Creating index [IX_SWDefault_RollupItemName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupItemName] ON [dbo].[Order] ([RollupItemName], [OrderID])
GO
PRINT N'Creating index [IX_SWDefault_RollupItemCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupItemCode] ON [dbo].[Order] ([RollupItemCode])
GO
PRINT N'Creating index [IX_SWDefault_RollupItemSKU] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU])
GO
PRINT N'Creating index [IX_SWDefault_RollupNoteCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupNoteCount] ON [dbo].[Order] ([RollupNoteCount])
GO
PRINT N'Adding [Order].[IX_SWDefault_ShipSenseRecognitionStatus] Index'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipSenseRecognitionStatus] ON [dbo].[Order] ([ShipSenseRecognitionStatus])
GO
PRINT N'Adding [Order].[IX_SWDefault_ShipSenseHashKey] Index'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipSenseHashKey] ON [dbo].[Order] ([ShipSenseHashKey])
GO
PRINT N'Creating index [IX_SWDefault_Order_BillAddressValidationStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_BillAddressValidationStatus] ON [dbo].[Order] ([BillAddressValidationStatus] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_BillMilitaryAddress] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_BillMilitaryAddress] ON [dbo].[Order] ([BillMilitaryAddress] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_BillPOBox] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_BillPOBox] ON [dbo].[Order] ([BillPOBox] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_BillResidentialStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_BillResidentialStatus] ON [dbo].[Order] ([BillResidentialStatus] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_BillUSTerritory] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_BillUSTerritory] ON [dbo].[Order] ([BillUSTerritory] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_ShipAddressValidationStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_ShipAddressValidationStatus] ON [dbo].[Order] ([ShipAddressValidationStatus] DESC) INCLUDE ([OrderDate])
GO
PRINT N'Creating index [IX_SWDefault_Order_StoreIDIsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_StoreIDIsManual] ON [dbo].[Order] ([StoreID] ASC, [IsManual] ASC) INCLUDE ([OrderDate], [OrderNumber])
GO
PRINT N'Creating index [IX_SWDefault_ShipFirstName] on [dbo].[Order]'
GO
-- *********************
-- Purposely using If Not Exists here because this index may already exist in some db's and we dont' want
-- to fail on upgrade.
-- *********************
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = N'IX_SWDefault_ShipFirstName')
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipFirstName] ON [dbo].[Order]
(
    [ShipFirstName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
PRINT N'Creating index [IX_SWDefault_Order_ShipMilitaryAddress] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_ShipMilitaryAddress] ON [dbo].[Order] ([ShipMilitaryAddress] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_ShipPOBox] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_ShipPOBox] ON [dbo].[Order] ([ShipPOBox] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_ShipResidentialStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_ShipResidentialStatus] ON [dbo].[Order] ([ShipResidentialStatus] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_ShipUSTerritory] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_ShipUSTerritory] ON [dbo].[Order] ([ShipUSTerritory] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Order_StoreIDOrderDateLocalStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_StoreIDOrderDateLocalStatus] ON [dbo].[Order] ([StoreID], [OrderDate], [LocalStatus])
GO
PRINT N'Creating index [IX_SWDefault_Order_CombineSplitStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_CombineSplitStatus] ON [dbo].[Order] ([CombineSplitStatus])
GO
ALTER TABLE [dbo].[Order] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Order]'
GO
PRINT N'Creating [dbo].[AmazonOrder]'
GO
CREATE TABLE [dbo].[AmazonOrder]
(
[OrderID] [bigint] NOT NULL,
[AmazonOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonCommission] [money] NOT NULL,
[FulfillmentChannel] [int] NOT NULL,
[IsPrime] [int] NOT NULL,
[EarliestExpectedDeliveryDate] [datetime] NULL,
[LatestExpectedDeliveryDate] [datetime] NULL,
[PurchaseOrderNumber] [nvarchar](50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonOrder] on [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD CONSTRAINT [PK_AmazonOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_AmazonOrderID] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AmazonOrderID] ON [dbo].[AmazonOrder] ([AmazonOrderID])
GO
PRINT N'Creating index [IX_SWDefault_FulfillmentChannel] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FulfillmentChannel] ON [dbo].[AmazonOrder] ([FulFillmentChannel])
GO
PRINT N'Creating index [IX_SWDefault_IsPrime] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_IsPrime] ON [dbo].[AmazonOrder] ([IsPrime])
GO
PRINT N'Creating index [IX_SWDefault_EarliestExpectedDeliveryDate] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EarliestExpectedDeliveryDate] ON [dbo].[AmazonOrder] ([EarliestExpectedDeliveryDate])
GO
PRINT N'Creating index [IX_SWDefault_LatestExpectedDeliveryDate] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_LatestExpectedDeliveryDate] ON [dbo].[AmazonOrder] ([LatestExpectedDeliveryDate])
GO
PRINT N'Creating [dbo].[OrderItem]'
GO
CREATE TABLE [dbo].[OrderItem]
(
[OrderItemID] [bigint] NOT NULL IDENTITY(1013, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ISBN] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UPC] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Location] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Image] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Thumbnail] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UnitPrice] [money] NOT NULL,
[UnitCost] [money] NOT NULL,
[Weight] [decimal] (29, 9) NOT NULL,
[Quantity] [decimal] (29, 9) NOT NULL,
[LocalStatus] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL,
[TotalWeight] AS ([Weight]*[Quantity]),
[HarmonizedCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [BIGINT] NOT NULL,
[Length] [decimal] (10, 2) NOT NULL,
[Width] [decimal] (10, 2) NOT NULL,
[Height] [decimal] (10, 2) NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderItem] on [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating index [IX_SWDefault_OrderItem_OrderID] on [dbo].[OrderItem]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_OrderItem_OrderID] ON [dbo].[OrderItem] ([OrderID] ASC, [OrderItemID] ASC)
GO
ALTER TABLE [dbo].[OrderItem] ENABLE CHANGE_TRACKING
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderItem_OriginalOrderID] ON [dbo].[OrderItem] ([OriginalOrderID] ASC)
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderItem_Code_OrderId] ON [dbo].[OrderItem] ([Code], [OrderID])
GO
PRINT N'Creating [dbo].[AmazonOrderItem]'
GO
CREATE TABLE [dbo].[AmazonOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[AmazonOrderItemCode] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ASIN] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ConditionNote] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonOrderItem] on [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] ADD CONSTRAINT [PK_AmazonOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO

PRINT N'Creating [dbo].[QuickFilterNodeContentDirty]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeContentDirty]
(
[FilterNodeContentDirtyID] [bigint] NOT NULL IDENTITY(1, 1),
[RowVersion] [timestamp] NOT NULL,
[ObjectID] [bigint] NOT NULL,
[ParentID] [bigint] NULL,
[ObjectType] [int] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeContentDirty] on [dbo].[QuickFilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeContentDirty' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeContentDirty]'))
ALTER TABLE [dbo].[QuickFilterNodeContentDirty] ADD CONSTRAINT [PK_QuickFilterNodeContentDirty] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated], [ComputerID]) WITH (IGNORE_DUP_KEY=ON)
GO

PRINT N'Creating [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCheckpoint]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateCheckpoint]
(
[CheckpointID] [bigint] NOT NULL IDENTITY(1080, 1000),
[MaxDirtyID] [bigint] NOT NULL,
[DirtyCount] [int] NOT NULL,
[State] [int] NOT NULL,
[Duration] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateCheckpoint] on [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateCheckpoint' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCheckpoint]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateCheckpoint] ADD CONSTRAINT [PK_QuickFilterNodeUpdateCheckpoint] PRIMARY KEY CLUSTERED  ([CheckpointID])
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateCustomer]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCustomer]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateCustomer]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateCustomer] on [dbo].[QuickFilterNodeUpdateCustomer]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateCustomer' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCustomer]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateCustomer] ADD CONSTRAINT [PK_QuickFilterNodeUpdateCustomer] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateItem]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateItem]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateItem]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateItem] on [dbo].[QuickFilterNodeUpdateItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateItem' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateItem]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateItem] ADD CONSTRAINT [PK_QuickFilterNodeUpdateItem] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateOrder]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateOrder]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateOrder]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateOrder] on [dbo].[QuickFilterNodeUpdateOrder]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateOrder' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateOrder]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateOrder] ADD CONSTRAINT [PK_QuickFilterNodeUpdateOrder] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdatePending]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdatePending]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdatePending]
(
[FilterNodeContentID] [bigint] NOT NULL,
[FilterTarget] [int] NOT NULL,
[ColumnMask] [varbinary] (100) NOT NULL,
[JoinMask] [int] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating [dbo].[QuickFilterNodeUpdateShipment]'
GO
IF OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateShipment]', 'U') IS NULL
CREATE TABLE [dbo].[QuickFilterNodeUpdateShipment]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateShipment] on [dbo].[QuickFilterNodeUpdateShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateShipment' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateShipment]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateShipment] ADD CONSTRAINT [PK_QuickFilterNodeUpdateShipment] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[Store]'
GO
CREATE TABLE [dbo].[Store]
(
[StoreID] [bigint] NOT NULL IDENTITY(1005, 1000),
[RowVersion] [timestamp] NOT NULL,
[License] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Edition] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TypeCode] [int] NOT NULL,
[Enabled] [bit] NOT NULL,
[SetupComplete] [bit] NOT NULL,
[StoreName] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Fax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AutoDownload] [bit] NOT NULL,
[AutoDownloadMinutes] [int] NOT NULL,
[AutoDownloadOnlyAway] [bit] NOT NULL,
[DomesticAddressValidationSetting] [int] NOT NULL,
[InternationalAddressValidationSetting] [int] NOT NULL,
[ComputerDownloadPolicy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultEmailAccountID] [bigint] NOT NULL,
[ManualOrderPrefix] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ManualOrderPostfix] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InitialDownloadDays] [int] NULL,
[InitialDownloadOrder] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_Store] on [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] ADD CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating index [IX_SWDefault_Store_StoreName] on [dbo].[Store]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_Store_StoreName] ON [dbo].[Store] ([StoreName])
GO
ALTER TABLE [dbo].[Store] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Store]'
GO
PRINT N'Creating [dbo].[AmeriCommerceStore]'
GO
CREATE TABLE [dbo].[AmeriCommerceStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StoreUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StoreCode] [int] NOT NULL,
[StatusCodes] [xml] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmeriCommerceStore] on [dbo].[AmeriCommerceStore]'
GO
ALTER TABLE [dbo].[AmeriCommerceStore] ADD CONSTRAINT [PK_AmeriCommerceStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[Audit]'
GO
CREATE TABLE [dbo].[Audit]
(
[AuditID] [bigint] NOT NULL IDENTITY(1048, 1000),
[RowVersion] [timestamp] NOT NULL,
[TransactionID] [bigint] NOT NULL,
[UserID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[Reason] [int] NOT NULL,
[ReasonDetail] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Date] [datetime] NOT NULL,
[Action] [int] NOT NULL,
[ObjectID] [bigint] NULL,
[HasEvents] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Audit] on [dbo].[Audit]'
GO
ALTER TABLE [dbo].[Audit] ADD CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED  ([AuditID])
GO
PRINT N'Creating index [IX_SWDefault_Audit_TransactionID] on [dbo].[Audit]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_Audit_TransactionID] ON [dbo].[Audit] ([TransactionID]) INCLUDE ([Action])
GO
PRINT N'Creating index [IX_SWDefault_Audit_Date] on [dbo].[Audit]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Audit_Date] ON [dbo].[Audit] ([Date])
GO
PRINT N'Creating index [IX_SWDefault_Audit_ObjectIDDate] on [dbo].[Audit]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Audit_ObjectIDDate] ON [dbo].[Audit] ([ObjectID]) INCLUDE ([Date])
GO
PRINT N'Creating index [IX_SWDefault_Audit_Action] on [dbo].[Audit]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Audit_Action] ON [dbo].[Audit] ([Action])
GO
ALTER TABLE [dbo].[Audit] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Audit]'
GO
PRINT N'Creating [dbo].[User]'
GO
CREATE TABLE [dbo].[User]
(
[UserID] [bigint] NOT NULL IDENTITY(1002, 1000),
[RowVersion] [timestamp] NOT NULL,
[Username] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsAdmin] [bit] NOT NULL,
[IsDeleted] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_User] on [dbo].[User]'
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED  ([UserID])
GO
PRINT N'Creating index [IX_SWDefault_User_Username] on [dbo].[User]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_User_Username] ON [dbo].[User] ([Username])
GO
ALTER TABLE [dbo].[User] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[User]'
GO
PRINT N'Creating [dbo].[AuditChange]'
GO
CREATE TABLE [dbo].[AuditChange]
(
[AuditChangeID] [bigint] NOT NULL IDENTITY(1003, 1000),
[AuditID] [bigint] NOT NULL,
[ChangeType] [int] NOT NULL,
[ObjectID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AuditChange] on [dbo].[AuditChange]'
GO
ALTER TABLE [dbo].[AuditChange] ADD CONSTRAINT [PK_AuditChange] PRIMARY KEY CLUSTERED  ([AuditChangeID])
GO
PRINT N'Creating index [IX_SWDefault_AuditChange_AuditID] on [dbo].[AuditChange]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AuditChange_AuditID] ON [dbo].[AuditChange] ([AuditID])
GO
PRINT N'Creating [dbo].[AuditChangeDetail]'
GO
CREATE TABLE [dbo].[AuditChangeDetail]
(
[AuditChangeDetailID] [bigint] NOT NULL IDENTITY(1047, 1000),
[AuditChangeID] [bigint] NOT NULL,
[AuditID] [bigint] NOT NULL,
[DisplayName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DisplayFormat] [tinyint] NOT NULL,
[DataType] [tinyint] NOT NULL,
[TextOld] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TextNew] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VariantOld] [sql_variant] NULL,
[VariantNew] [sql_variant] NULL
)
GO
PRINT N'Creating primary key [PK_AuditChangeDetail] on [dbo].[AuditChangeDetail]'
GO
ALTER TABLE [dbo].[AuditChangeDetail] ADD CONSTRAINT [PK_AuditChangeDetail] PRIMARY KEY CLUSTERED  ([AuditChangeDetailID])
GO
PRINT N'Creating index [IX_SWDefault_AuditChangeDetail_AuditChangeID] on [dbo].[AuditChangeDetail]'
GO
CREATE UNIQUE INDEX [IX_SWDefault_AuditChangeDetail_AuditChangeID] ON [dbo].[AuditChangeDetail] ([AuditChangeID], [AuditChangeDetailID] )
	INCLUDE ( [AuditID])
GO
PRINT N'Creating index [IX_SWDefault_AuditChangeDetail_AuditID] on [dbo].[AuditChangeDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AuditChangeDetail_AuditID] ON [dbo].[AuditChangeDetail] ([AuditID])
GO
PRINT N'Creating index [IX_SWDefault_AuditChangeDetail_VariantNew] on [dbo].[AuditChangeDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AuditChangeDetail_VariantNew] ON [dbo].[AuditChangeDetail] ([VariantNew]) INCLUDE ([AuditID])
GO
PRINT N'Creating [dbo].[ShippingProfile]'
GO
CREATE TABLE [dbo].[ShippingProfile]
(
[ShippingProfileID] [bigint] NOT NULL IDENTITY(1053, 1000),
[RowVersion] [timestamp] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentType] [int] NULL,
[ShipmentTypePrimary] [bit] NOT NULL,
[OriginID] [bigint] NULL,
[Insurance] [bit] NULL,
[InsuranceInitialValueSource] [int] NULL,
[InsuranceInitialValueAmount] [money] NULL,
[ReturnShipment] [bit] NULL,
[RequestedLabelFormat] [int] NULL
)
GO
PRINT N'Creating primary key [PK_ShippingProfile] on [dbo].[ShippingProfile]'
GO
ALTER TABLE [dbo].[ShippingProfile] ADD CONSTRAINT [PK_ShippingProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Altering [dbo].[ShippingProfile]'
GO
ALTER TABLE [dbo].[ShippingProfile] ENABLE CHANGE_TRACKING
GO
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
[DimsAddWeight] [bit] NULL
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
PRINT N'Creating [dbo].[BestRateProfile]'
GO
CREATE TABLE [dbo].[BestRateProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[ServiceLevel] [int] NULL
)
GO
PRINT N'Creating primary key [PK_BestRateProfile] on [dbo].[BestRateProfile]'
GO
ALTER TABLE [dbo].[BestRateProfile] ADD CONSTRAINT [PK_BestRateProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO

PRINT N'Creating [dbo].[Shipment]'
GO
CREATE TABLE [dbo].[Shipment]
(
[ShipmentID] [bigint] NOT NULL IDENTITY(1031, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[ShipmentType] [int] NOT NULL,
[ContentWeight] [float] NOT NULL,
[TotalWeight] [decimal] (29, 9) NOT NULL,
[Processed] [bit] NOT NULL,
[ProcessedDate] [datetime] NULL,
[ProcessedUserID] [bigint] NULL,
[ProcessedComputerID] [bigint] NULL,
[ShipDate] [datetime] NOT NULL,
[ShipmentCost] [money] NOT NULL,
[Voided] [bit] NOT NULL,
[VoidedDate] [datetime] NULL,
[VoidedUserID] [bigint] NULL,
[VoidedComputerID] [bigint] NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsGenerated] [bit] NOT NULL,
[CustomsValue] [money] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL,
[ActualLabelFormat] [int] NULL,
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
[ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipAddressValidationSuggestionCount] [int] NOT NULL,
[ShipAddressValidationStatus] [int] NOT NULL,
[ShipAddressValidationError] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipResidentialStatus] [int] NOT NULL,
[ShipPOBox] [int] NOT NULL,
[ShipUSTerritory] [int] NOT NULL,
[ShipMilitaryAddress] [int] NOT NULL,
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
[OriginEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnShipment] [bit] NOT NULL,
[Insurance] [bit] NOT NULL,
[InsuranceProvider] [int] NOT NULL,
[ShipNameParseStatus] [int] NOT NULL,
[ShipUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginNameParseStatus] [int] NOT NULL,
[OriginUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BestRateEvents] [tinyint] NOT NULL,
[ShipSenseStatus] [int] NOT NULL,
[ShipSenseChangeSets] [xml] NOT NULL,
[ShipSenseEntry] [varbinary] (max) NOT NULL,
[OnlineShipmentID] [varchar] (128) NOT NULL,
[BilledType] [int] NOT NULL,
[BilledWeight] [float] NOT NULL,
[ProcessedWithUiMode] [int] NULL
)
GO
PRINT N'Creating primary key [PK_Shipment] on [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_OrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_OrderID] ON [dbo].[Shipment] ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ProcessedOrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC, [ProcessedDate]) INCLUDE ([OrderID], [Voided])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ReturnShipment] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ReturnShipment] ON [dbo].[Shipment] ([ReturnShipment]) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_OrderID_ShipSenseStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_OrderID_ShipSenseStatus] ON [dbo].[Shipment]
(
    [OrderID] ASC,
    [Processed] ASC,
    [ShipSenseStatus] ASC
)
GO
PRINT N'Creating index [IX_SWDefault_Shipment_RequestedLabelFormat] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_RequestedLabelFormat] ON [dbo].[Shipment] ([RequestedLabelFormat])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ActualLabelFormat] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ActualLabelFormat] ON [dbo].[Shipment] ([ActualLabelFormat])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ShipAddressValidationStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ShipAddressValidationStatus] ON [dbo].[Shipment] ([ShipAddressValidationStatus] DESC) INCLUDE ([OrderID], [Processed], [Voided])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ShipMilitaryAddress] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ShipMilitaryAddress] ON [dbo].[Shipment] ([ShipMilitaryAddress] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ShipPOBox] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ShipPOBox] ON [dbo].[Shipment] ([ShipPOBox] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ShipResidentialStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ShipResidentialStatus] ON [dbo].[Shipment] ([ShipResidentialStatus] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ShipUSTerritory] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ShipUSTerritory] ON [dbo].[Shipment] ([ShipUSTerritory] DESC)
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ShipDate] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ShipDate] ON [dbo].[Shipment] ([ShipDate]) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_Shipment_ShipmentType] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_ShipmentType] ON [dbo].[Shipment] ([ShipmentType]) INCLUDE ([OrderID])
GO
ALTER TABLE [dbo].[Shipment] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Shipment]'
GO
PRINT N'Creating [dbo].[BestRateShipment]'
GO
CREATE TABLE [dbo].[BestRateShipment]
(
[ShipmentID] [bigint] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[ServiceLevel] [int] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL,
[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_BestRateShipment] on [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [PK_BestRateShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[BigCommerceStore]'
GO
CREATE TABLE [dbo].[BigCommerceStore]
(
[StoreID] [bigint] NOT NULL,
[ApiUrl] [nvarchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserName] [nvarchar] (65) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StatusCodes] [xml] NULL,
[WeightUnitOfMeasure] [int] NOT NULL,
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL,
[BigCommerceAuthentication] [int] NOT NULL,
[OauthClientId] [nvarchar](100) NOT NULL,
[OauthToken] [nvarchar](100) NOT NULL,
[Identifier] [nvarchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_BigCommerceStore] on [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD CONSTRAINT [PK_BigCommerceStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD  CONSTRAINT [DF_BigCommerceStore_BigCommerceAuthentication]  DEFAULT ((1)) FOR [BigCommerceAuthentication]
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD  CONSTRAINT [DF_BigCommerceStore_OauthClientId]  DEFAULT ('') FOR [OauthClientId]
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD  CONSTRAINT [DF_BigCommerceStore_OauthToken]  DEFAULT ('') FOR [OauthToken]
GO
PRINT N'Creating [dbo].[BuyDotComStore]'
GO
CREATE TABLE [dbo].[BuyDotComStore]
(
[StoreID] [bigint] NOT NULL,
[FtpUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_BuyComStore] on [dbo].[BuyDotComStore]'
GO
ALTER TABLE [dbo].[BuyDotComStore] ADD CONSTRAINT [PK_BuyComStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[BuyDotComOrderItem]'
GO
CREATE TABLE [dbo].[BuyDotComOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ReceiptItemID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ListingID] [int] NOT NULL,
[Shipping] [money] NOT NULL,
[Tax] [money] NOT NULL,
[Commission] [money] NOT NULL,
[ItemFee] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_BuyDotComOrderItem] on [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] ADD CONSTRAINT [PK_BuyDotComOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[ChannelAdvisorOrder]'
GO
CREATE TABLE [dbo].[ChannelAdvisorOrder]
(
[OrderID] [bigint] NOT NULL,
[CustomOrderIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ResellerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineShippingStatus] [int] NOT NULL,
[OnlineCheckoutStatus] [int] NOT NULL,
[OnlinePaymentStatus] [int] NOT NULL,
[FlagStyle] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FlagDescription] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FlagType] [int] NOT NULL,
[MarketplaceNames] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsPrime] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrder] on [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [PK_ChannelAdvisorOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_ChannelAdvisorOrder_OnlineStatus] on [dbo].[ChannelAdvisorOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrder_OnlineShippingStatus] ON [dbo].[ChannelAdvisorOrder] ([OnlineShippingStatus])
GO
PRINT N'Creating index [IX_SWDefault_ChannelAdvisorOrder_IsPrime] on [dbo].[ChannelAdvisorOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrder_IsPrime] ON [dbo].[ChannelAdvisorOrder]([IsPrime])
GO
PRINT N'Creating index [IX_SWDefault_ChannelAdvisorOrder_CustomerOrderIdentifier_OrderID] on [dbo].[ChannelAdvisorOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrder_CustomerOrderIdentifier_OrderID]
	ON [dbo].[ChannelAdvisorOrder] ([CustomOrderIdentifier], [OrderID])
GO
PRINT N'Creating [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE TABLE [dbo].[ChannelAdvisorOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[MarketplaceName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceStoreName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceBuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceSalesID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Classification] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DistributionCenter] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsFBA] [bit] NOT NULL,
[MPN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DistributionCenterID] [bigint] NOT NULL,
[DistributionCenterName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderItem] on [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderItem_MarketplaceBuyerID] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceBuyerID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderItem_MarketPlaceName] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceName])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderItem_MarketplaceSalesID] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceSalesID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderItem_MarketplaceStoreName] ON [dbo].[ChannelAdvisorOrderItem] ([MarketplaceStoreName]) INCLUDE ([MarketplaceBuyerID], [MarketplaceSalesID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderItem_Classification] ON [dbo].[ChannelAdvisorOrderItem] ([Classification])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderItem_DistributionCenterID] ON [dbo].[ChannelAdvisorOrderItem] ([DistributionCenterID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderItem_DistributionCenter] ON [dbo].[ChannelAdvisorOrderItem] ([DistributionCenter])
GO
PRINT N'Creating [dbo].[ChannelAdvisorStore]'
GO
CREATE TABLE [dbo].[ChannelAdvisorStore]
(
[StoreID] [bigint] NOT NULL,
[AccountKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProfileID] [int] NOT NULL,
[AttributesToDownload] [xml] NOT NULL,
[ConsolidatorAsUsps] [bit] NOT NULL,
[AmazonMerchantID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonAuthToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonApiRegion] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RefreshToken] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ChannelAdvisorStore] on [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD CONSTRAINT [PK_ChannelAdvisorStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[ClickCartProOrder]'
GO
CREATE TABLE [dbo].[ClickCartProOrder]
(
[OrderID] [bigint] NOT NULL,
[ClickCartProOrderID] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ClickCartProOrder] on [dbo].[ClickCartProOrder]'
GO
ALTER TABLE [dbo].[ClickCartProOrder] ADD CONSTRAINT [PK_ClickCartProOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[CommerceInterfaceOrder]'
GO
CREATE TABLE [dbo].[CommerceInterfaceOrder]
(
[OrderID] [bigint] NOT NULL,
[CommerceInterfaceOrderNumber] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_CommerceInterfaceOrder] on [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] ADD CONSTRAINT [PK_CommerceInterfaceOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[Download]'
GO
CREATE TABLE [dbo].[Download]
(
[DownloadID] [bigint] NOT NULL IDENTITY(1018, 1000),
[RowVersion] [timestamp] NOT NULL,
[StoreID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[UserID] [bigint] NOT NULL,
[InitiatedBy] [int] NOT NULL,
[Started] [datetime] NOT NULL,
[Ended] [datetime] NULL,
[Duration] AS (datediff(second,[Started],[Ended])) PERSISTED,
[QuantityTotal] [int] NULL,
[QuantityNew] [int] NULL,
[Result] [int] NOT NULL,
[ErrorMessage] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_Download] on [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] ADD CONSTRAINT [PK_Download] PRIMARY KEY CLUSTERED  ([DownloadID])
GO
PRINT N'Creating index [IX_SWDefault_DownloadLog_StoreID_Ended] on [dbo].[Download]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_DownloadLog_StoreID_Ended] ON [dbo].[Download] ([StoreID], [Ended])
GO
ALTER TABLE [dbo].[Download] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Download]'
GO
PRINT N'Creating [dbo].[DownloadDetail]'
GO
CREATE TABLE [dbo].[DownloadDetail]
(
[DownloadedDetailID] [bigint] NOT NULL IDENTITY(1019, 1000),
[DownloadID] [bigint] NOT NULL,
[OrderID] [bigint] NOT NULL,
[InitialDownload] [bit] NOT NULL,
[OrderNumber] [bigint] NULL,
[ExtraBigIntData1] [bigint] NULL,
[ExtraBigIntData2] [bigint] NULL,
[ExtraBigIntData3] [bigint] NULL,
[ExtraStringData1] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_DownloadDetail] on [dbo].[DownloadDetail]'
GO
ALTER TABLE [dbo].[DownloadDetail] ADD CONSTRAINT [PK_DownloadDetail] PRIMARY KEY CLUSTERED  ([DownloadedDetailID])
GO
PRINT N'Creating index [IX_SWDefault_DownloadDetail_DownloadID] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_DownloadDetail_DownloadID] ON [dbo].[DownloadDetail] ([DownloadID])
GO
PRINT N'Creating index [IX_SWDefault_DownloadDetail_OrderNumber] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_DownloadDetail_OrderNumber] ON [dbo].[DownloadDetail] ([OrderNumber], [ExtraStringData1]) INCLUDE ([DownloadID])
GO
PRINT N'Creating index [IX_SWDefault_DownloadDetail_BigIntIndex] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_DownloadDetail_BigIntIndex] ON [dbo].[DownloadDetail] ([ExtraBigIntData1], [ExtraBigIntData2], [ExtraBigIntData3]) INCLUDE ([DownloadID])
GO
PRINT N'Creating index [IX_SWDefault_DownloadDetail_String] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_DownloadDetail_String] ON [dbo].[DownloadDetail] ([ExtraStringData1]) INCLUDE ([DownloadID])
GO
PRINT N'Creating index [IX_SWDefault_DownloadDetail_OrderID] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_DownloadDetail_OrderID] ON [dbo].[DownloadDetail] ([OrderID]) INCLUDE ([DownloadID], [InitialDownload])
GO
PRINT N'Creating [dbo].[EbayCombinedOrderRelation]'
GO
CREATE TABLE [dbo].[EbayCombinedOrderRelation]
(
[EbayCombinedOrderRelationID] [bigint] NOT NULL IDENTITY(1099, 1000),
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[StoreID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EbayCombinedOrderRelation] on [dbo].[EbayCombinedOrderRelation]'
GO
ALTER TABLE [dbo].[EbayCombinedOrderRelation] ADD CONSTRAINT [PK_EbayCombinedOrderRelation] PRIMARY KEY CLUSTERED  ([EbayCombinedOrderRelationID])
GO
PRINT N'Creating index [IX_SWDefault_EbayCombinedOrderRelation] on [dbo].[EbayCombinedOrderRelation]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_EbayCombinedOrderRelation] ON [dbo].[EbayCombinedOrderRelation] ([EbayOrderID])
GO
PRINT N'Creating [dbo].[EbayOrderItem]'
GO
CREATE TABLE [dbo].[EbayOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[OrderID] [bigint] NOT NULL,
[EbayItemID] [bigint] NOT NULL,
[EbayTransactionID] [bigint] NOT NULL,
[SellingManagerRecord] [int] NOT NULL,
[EffectiveCheckoutStatus] [int] NOT NULL,
[EffectivePaymentMethod] [int] NOT NULL,
[PaymentStatus] [int] NOT NULL,
[PaymentMethod] [int] NOT NULL,
[CompleteStatus] [int] NOT NULL,
[FeedbackLeftType] [int] NOT NULL,
[FeedbackLeftComments] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FeedbackReceivedType] [int] NOT NULL,
[FeedbackReceivedComments] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MyEbayPaid] [bit] NOT NULL,
[MyEbayShipped] [bit] NOT NULL,
[PayPalTransactionID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalAddressStatus] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EbayOrderItem] on [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] ADD CONSTRAINT [PK_EbayOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating index [IX_SWDefault_EbayOrderItem_OrderID] on [dbo].[EbayOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrderItem_OrderID] ON [dbo].[EbayOrderItem] ([OrderID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrderItem_EbayItemID_EbayTransactionID] ON [dbo].[EbayOrderItem] ([EbayItemID],[EbayTransactionID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrderItem_EffectiveCheckoutStatus_EbayOrderItemID] ON [dbo].[EbayOrderItem] ([EffectiveCheckoutStatus], [OrderItemID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrderItem_SellingManagerRecord_OrderID] ON [dbo].[EbayOrderItem]
	([SellingManagerRecord] ASC, [OrderID] ASC)
GO
PRINT N'Creating [dbo].[EbayStore]'
GO
CREATE TABLE [dbo].[EbayStore]
(
[StoreID] [bigint] NOT NULL,
[eBayUserID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[eBayToken] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[eBayTokenExpire] [datetime] NOT NULL,
[AcceptedPaymentList] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DownloadItemDetails] [bit] NOT NULL,
[DownloadOlderOrders] [bit] NOT NULL,
[DownloadPayPalDetails] [bit] NOT NULL,
[PayPalApiCredentialType] [smallint] NOT NULL,
[PayPalApiUserName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiPassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiSignature] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiCertificate] [varbinary] (4096) NULL,
[DomesticShippingService] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternationalShippingService] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FeedbackUpdatedThrough] [datetime] NULL
)
GO
PRINT N'Creating primary key [PK_EbayStore] on [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD CONSTRAINT [PK_EbayStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[EmailOutbound]'
GO
CREATE TABLE [dbo].[EmailOutbound]
(
[EmailOutboundID] [bigint] NOT NULL IDENTITY(1035, 1000),
[RowVersion] [timestamp] NOT NULL,
[ContextID] [bigint] NULL,
[ContextType] [int] NULL,
[TemplateID] [bigint] NULL,
[AccountID] [bigint] NOT NULL,
[Visibility] [int] NOT NULL,
[FromAddress] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToList] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CcList] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BccList] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Subject] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HtmlPartResourceID] [bigint] NULL,
[PlainPartResourceID] [bigint] NOT NULL,
[Encoding] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ComposedDate] [datetime] NOT NULL,
[SentDate] [datetime] NOT NULL,
[DontSendBefore] [datetime] NULL,
[SendStatus] [int] NOT NULL,
[SendAttemptCount] [int] NOT NULL,
[SendAttemptLastError] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_EmailOutbound] on [dbo].[EmailOutbound]'
GO
ALTER TABLE [dbo].[EmailOutbound] ADD CONSTRAINT [PK_EmailOutbound] PRIMARY KEY CLUSTERED  ([EmailOutboundID])
GO
PRINT N'Creating index [IX_SWDefault_EmailOutbound] on [dbo].[EmailOutbound]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EmailOutbound] ON [dbo].[EmailOutbound] ([SendStatus], [AccountID], [DontSendBefore], [SentDate], [ComposedDate]) INCLUDE ([Visibility])
GO
ALTER TABLE [dbo].[EmailOutbound] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[EmailOutbound]'
GO
PRINT N'Creating [dbo].[EmailOutboundRelation]'
GO
CREATE TABLE [dbo].[EmailOutboundRelation]
(
[EmailOutboundRelationID] [bigint] NOT NULL IDENTITY(1046, 1000),
[EmailOutboundID] [bigint] NOT NULL,
[ObjectID] [bigint] NOT NULL,
[RelationType] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EmailOutboundObject] on [dbo].[EmailOutboundRelation]'
GO
ALTER TABLE [dbo].[EmailOutboundRelation] ADD CONSTRAINT [PK_EmailOutboundObject] PRIMARY KEY CLUSTERED  ([EmailOutboundRelationID])
GO
PRINT N'Creating index [IX_SWDefault_EmailOutboundRelation_EmailOutboundIDRelationTypeObjectID] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EmailOutboundRelation_EmailOutboundIDRelationTypeObjectID] ON [dbo].[EmailOutboundRelation] ([EmailOutboundID], [RelationType]) INCLUDE ([ObjectID])
GO
PRINT N'Creating index [IX_SWDefault_EmailOutboundRelation_ObjectIDRelationTypeEmailOutboundID] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EmailOutboundRelation_ObjectIDRelationTypeEmailOutboundID] ON [dbo].[EmailOutboundRelation] ([ObjectID], [RelationType]) INCLUDE ([EmailOutboundID])
GO
PRINT N'Creating index [IX_SWDefault_EmailOutboundRelation_RelationTypeObject] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EmailOutboundRelation_RelationTypeObject] ON [dbo].[EmailOutboundRelation] ([RelationType], [ObjectID]) INCLUDE ([EmailOutboundID])
GO
PRINT N'Creating [dbo].[PostalProfile]'
GO
CREATE TABLE [dbo].[PostalProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[Service] [int] NULL,
[Confirmation] [int] NULL,
[PackagingType] [int] NULL,
[NonRectangular] [bit] NULL,
[NonMachinable] [bit] NULL,
[CustomsContentType] [int] NULL,
[CustomsContentDescription] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExpressSignatureWaiver] [bit] NULL,
[SortType] [int] NULL,
[EntryFacility] [int] NULL,
[Memo1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Memo2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Memo3] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NoPostage] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_PostalProfile] on [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] ADD CONSTRAINT [PK_PostalProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[EndiciaProfile]'
GO
CREATE TABLE [dbo].[EndiciaProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[EndiciaAccountID] [bigint] NULL,
[StealthPostage] [bit] NULL,
[ReferenceID] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ScanBasedReturn] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_EndiciaProfile] on [dbo].[EndiciaProfile]'
GO
ALTER TABLE [dbo].[EndiciaProfile] ADD CONSTRAINT [PK_EndiciaProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[ScanFormBatch]'
GO
CREATE TABLE [dbo].[ScanFormBatch]
(
[ScanFormBatchID] [bigint] NOT NULL IDENTITY(1095, 1000),
[ShipmentType] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ShipmentCount] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ScanFormBatch] on [dbo].[ScanFormBatch]'
GO
ALTER TABLE [dbo].[ScanFormBatch] ADD CONSTRAINT [PK_ScanFormBatch] PRIMARY KEY CLUSTERED  ([ScanFormBatchID])
GO
PRINT N'Creating [dbo].[EndiciaScanForm]'
GO
CREATE TABLE [dbo].[EndiciaScanForm]
(
[EndiciaScanFormID] [bigint] NOT NULL IDENTITY(1067, 1000),
[EndiciaAccountID] [bigint] NOT NULL,
[EndiciaAccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SubmissionID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ScanFormBatchID] [bigint] NOT NULL,
[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_EndiciaScanForm] on [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ADD CONSTRAINT [PK_EndiciaScanForm] PRIMARY KEY CLUSTERED  ([EndiciaScanFormID])
GO
PRINT N'Creating [dbo].[PostalShipment]'
GO
CREATE TABLE [dbo].[PostalShipment]
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
[ExpressSignatureWaiver] [bit] NOT NULL,
[SortType] [int] NOT NULL,
[EntryFacility] [int] NOT NULL,
[Memo1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Memo2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Memo3] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NoPostage] [bit] NOT NULL CONSTRAINT [DF_PostalShipment_NoPostage] DEFAULT ((0)),
[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PostalShipment] on [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [PK_PostalShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_SWDefault_PostalShipment_Service] on [dbo].[PostalShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_PostalShipment_Service] ON [dbo].[PostalShipment] ([Service])
GO
PRINT N'Creating index [IX_SWDefault_PostalShipment_PackagingType] on [dbo].[PostalShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_PostalShipment_PackagingType] ON [dbo].[PostalShipment] ([PackagingType])
GO
PRINT N'Creating index [IX_SWDefault_PostalShipment_Confirmation] on [dbo].[PostalShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_PostalShipment_Confirmation] ON [dbo].[PostalShipment] ([Confirmation])
GO
PRINT N'Creating [dbo].[EndiciaShipment]'
GO
CREATE TABLE [dbo].[EndiciaShipment]
(
[ShipmentID] [bigint] NOT NULL,
[EndiciaAccountID] [bigint] NOT NULL,
[OriginalEndiciaAccountID] [bigint] NULL,
[StealthPostage] [bit] NOT NULL,
[ReferenceID] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TransactionID] [int] NULL,
[RefundFormID] [int] NULL,
[ScanFormBatchID] [bigint] NULL,
[ScanBasedReturn] [bit] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL,
[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EndiciaShipment] on [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [PK_EndiciaShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[EtsyOrder]'
GO
CREATE TABLE [dbo].[EtsyOrder]
(
[OrderID] [bigint] NOT NULL,
[WasPaid] [bit] NOT NULL,
[WasShipped] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EtsyOrder] on [dbo].[EtsyOrder]'
GO
ALTER TABLE [dbo].[EtsyOrder] ADD CONSTRAINT [PK_EtsyOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[EtsyStore]'
GO
CREATE TABLE [dbo].[EtsyStore]
(
[StoreID] [bigint] NOT NULL,
[EtsyShopID] [bigint] NOT NULL,
[EtsyLogin] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EtsyStoreName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OAuthToken] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OAuthTokenSecret] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_EtsyStore] on [dbo].[EtsyStore]'
GO
ALTER TABLE [dbo].[EtsyStore] ADD CONSTRAINT [PK_EtsyStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[AmazonShipment]'
GO
CREATE TABLE [dbo].[AmazonShipment]
(
[ShipmentID] [bigint] NOT NULL,
[CarrierName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_CarrierName] DEFAULT (''),
[ShippingServiceName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ShippingServiceName] DEFAULT (''),
[ShippingServiceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ShippingServiceId] DEFAULT (''),
[InsuranceValue] [money] NOT NULL CONSTRAINT [DF_AmazonShipment_InsuranceValue] DEFAULT ((0)),
[DimsProfileID] [bigint] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsProfileID] DEFAULT ((0)),
[DimsLength] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsLength] DEFAULT ((0)),
[DimsWidth] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsWidth] DEFAULT ((0)),
[DimsHeight] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsHeight] DEFAULT ((0)),
[DimsWeight] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsWeight] DEFAULT ((0)),
[DimsAddWeight] [bit] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsAddWeight] DEFAULT ((0)),
[DeliveryExperience] [int] NOT NULL CONSTRAINT [DF_AmazonShipment_DeliveryExperience] DEFAULT ((2)),
[DeclaredValue] [money] NULL,
[AmazonUniqueShipmentID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Insurance] [bit] NOT NULL,
[RequestedLabelFormat] [INT] NOT NULL CONSTRAINT [DF_AmazonShipment_RequestedLabelFormat] DEFAULT (-1),
[Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ReferenceNumber] DEFAULT ('')
)
GO
PRINT N'Creating primary key [PK_AmazonShipment] on [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ADD CONSTRAINT [PK_AmazonShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[AmazonProfile]'
GO
CREATE TABLE [dbo].[AmazonProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[DeliveryExperience] [int] NULL,
[ShippingServiceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_AmazonProfile] on [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [PK_AmazonProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[FedExShipment]'
GO
CREATE TABLE [dbo].[FedExShipment]
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
[ReferenceShipmentIntegrity] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[CustomsRecipientTIN] [varchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [bit] NOT NULL,
[CustomsDocumentsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsExportFilingOption] [int] NOT NULL,
[CustomsAESEEI] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsRecipientIdentificationType] [int] NOT NULL,
[CustomsRecipientIdentificationValue] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsOptionsType] [int] NOT NULL,
[CustomsOptionsDesription] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoice] [bit] NOT NULL,
[CommercialInvoiceFileElectronically] [bit] NOT NULL,
[CommercialInvoiceTermsOfSale] [int] NOT NULL,
[CommercialInvoicePurpose] [int] NOT NULL,
[CommercialInvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoiceFreight] [money] NOT NULL,
[CommercialInvoiceInsurance] [money] NOT NULL,
[CommercialInvoiceOther] [money] NOT NULL,
[CommercialInvoiceReference] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterOfRecord] [bit] NOT NULL,
[ImporterAccount] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterTIN] [nvarchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[SmartPostUspsApplicationId] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[IntlExportDetailType] [int] NOT NULL,
[IntlExportDetailForeignTradeZoneCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IntlExportDetailEntryNumber] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IntlExportDetailLicenseOrPermitNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IntlExportDetailLicenseOrPermitExpirationDate] [datetime] NULL,
[WeightUnitType] [int] NOT NULL,
[LinearUnitType] [int] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL,
[FimsAirWaybill] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnsClearance] [bit] NOT NULL CONSTRAINT [DF_FedExShipment_ReturnsClearance] DEFAULT ((0)),
[MaskedData] [int] NULL,
[ReferenceFIMS] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ThirdPartyConsignee] [bit] NOT NULL,
[Currency] [int] NULL,
[InternationalTrafficInArmsService] [bit] NULL,
[FreightRole] [int] NOT NULL,
[FreightCollectTerms] [int] NOT NULL,
[FreightTotalHandlinUnits] [int] NOT NULL,
[FreightClass] [int] NOT NULL,
[FreightSpecialServices] [int] NOT NULL,
[FreightGuaranteeType] [int] NOT NULL,
[FreightGuaranteeDate] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FedExShipment] on [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [PK_FedExShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_SWDefault_FedExShipment_Service] on [dbo].[FedExShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FedExShipment_Service] ON [dbo].[FedExShipment] ([Service])
GO
PRINT N'Creating index [IX_SWDefault_FedExShipment_PackagingType] on [dbo].[FedExShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FedExShipment_PackagingType] ON [dbo].[FedExShipment] ([PackagingType])
GO
PRINT N'Creating [dbo].[FedExProfile]'
GO
CREATE TABLE [dbo].[FedExProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[FedExAccountID] [bigint] NULL,
[Service] [int] NULL,
[Signature] [int] NULL,
[PackagingType] [int] NULL,
[NonStandardContainer] [bit] NULL,
[ReferenceCustomer] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReferenceInvoice] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReferencePO] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReferenceShipmentIntegrity] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorTransportType] [int] NULL,
[PayorTransportAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorDutiesType] [int] NULL,
[PayorDutiesAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SaturdayDelivery] [bit] NULL,
[EmailNotifySender] [int] NULL,
[EmailNotifyRecipient] [int] NULL,
[EmailNotifyOther] [int] NULL,
[EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifyMessage] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResidentialDetermination] [int] NULL,
[SmartPostIndicia] [int] NULL,
[SmartPostEndorsement] [int] NULL,
[SmartPostConfirmation] [bit] NULL,
[SmartPostCustomerManifest] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SmartPostHubID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifyBroker] [int] NULL,
[DropoffType] [int] NULL,
[OriginResidentialDetermination] [int] NULL,
[PayorTransportName] [nchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnType] [int] NULL,
[RmaNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RmaReason] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnSaturdayPickup] [bit] NULL,
[ReturnsClearance] [bit] NULL,
[ReferenceFIMS] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ThirdPartyConsignee] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_FedExProfile] on [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD CONSTRAINT [PK_FedExProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[FedExProfilePackage]'
GO
CREATE TABLE [dbo].[FedExProfilePackage]
(
	[PackageProfileID] [bigint] NOT NULL,
	[PriorityAlert] [bit] NULL,
	[PriorityAlertEnhancementType] [int] NULL,
	[PriorityAlertDetailContent] [nvarchar](1024) NULL,
	[DryIceWeight] [float] NULL,
	[ContainsAlcohol] [bit] NULL,
	[DangerousGoodsEnabled] [bit] NULL,
	[DangerousGoodsType] [int] NULL,
	[DangerousGoodsAccessibilityType] [int] NULL,
	[DangerousGoodsCargoAircraftOnly] [bit] NULL,
	[DangerousGoodsEmergencyContactPhone] [nvarchar](16) NULL,
	[DangerousGoodsOfferor] [nvarchar](128) NULL,
	[DangerousGoodsPackagingCount] [int] NULL,
	[HazardousMaterialNumber] [nvarchar](16) NULL,
	[HazardousMaterialClass] [nvarchar](8) NULL,
	[HazardousMaterialProperName] [nvarchar](64) NULL,
	[HazardousMaterialPackingGroup] [int] NULL,
	[HazardousMaterialQuantityValue] [float] NULL,
	[HazardousMaterialQuanityUnits] [int] NULL,
	[SignatoryContactName] [nvarchar](100) NULL,
	[SignatoryTitle] [nvarchar](100) NULL,
	[SignatoryPlace] [nvarchar](100) NULL,
	[ContainerType] [nvarchar](100) NULL,
	[NumberOfContainers] [int] NULL,
	[PackingDetailsCargoAircraftOnly] [bit] NULL,
	[PackingDetailsPackingInstructions] [nvarchar](50) NULL,
	[BatteryMaterial] [int] NULL,
	[BatteryPacking] [int] NULL,
	[BatteryRegulatorySubtype] [int] NULL
)
GO
PRINT N'Creating primary key [PK_FedExProfilePackage] on [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD CONSTRAINT [PK_FedExProfilePackage_PackageProfileID] PRIMARY KEY CLUSTERED ([PackageProfileID])
GO
PRINT N'Adding foreign key to [FedExProfilePackage]'
ALTER TABLE [dbo].[FedExProfilePackage] WITH CHECK ADD CONSTRAINT [FK_FedExProfilePackage_PackageProfile] FOREIGN KEY([PackageProfileID])
REFERENCES [dbo].[PackageProfile] ([PackageProfileID])
ON DELETE CASCADE
GO
PRINT N'Creating [dbo].[FilterNode]'
GO
CREATE TABLE [dbo].[FilterNode]
(
[FilterNodeID] [bigint] NOT NULL IDENTITY(1007, 1000),
[RowVersion] [timestamp] NOT NULL,
[ParentFilterNodeID] [bigint] NULL,
[FilterSequenceID] [bigint] NOT NULL,
[FilterNodeContentID] [bigint] NOT NULL,
[Created] [datetime] NOT NULL,
[Purpose] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNode] on [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [PK_FilterNode] PRIMARY KEY CLUSTERED  ([FilterNodeID])
GO
PRINT N'Creating index [IX_SWDefault_FilterNode_ParentFilterNodeID] on [dbo].[FilterNode]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FilterNode_ParentFilterNodeID] ON [dbo].[FilterNode] ([ParentFilterNodeID])
GO
PRINT N'Creating index [IX_SWDefault_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose] on [dbo].[FilterNode]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose] ON [dbo].[FilterNode] ([FilterSequenceID], [FilterNodeContentID], [Purpose]) INCLUDE ([FilterNodeID])
GO

PRINT N'Creating [dbo].[FilterLayout]'
GO
CREATE TABLE [dbo].[FilterLayout]
(
[FilterLayoutID] [bigint] NOT NULL IDENTITY(1011, 1000),
[RowVersion] [timestamp] NOT NULL,
[UserID] [bigint] NULL,
[FilterTarget] [int] NOT NULL,
[FilterNodeID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterLayout] on [dbo].[FilterLayout]'
GO
ALTER TABLE [dbo].[FilterLayout] ADD CONSTRAINT [PK_FilterLayout] PRIMARY KEY CLUSTERED  ([FilterLayoutID])
GO
PRINT N'Creating index [IX_SWDefault_FilterLayout] on [dbo].[FilterLayout]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_FilterLayout] ON [dbo].[FilterLayout] ([UserID], [FilterTarget])
GO
PRINT N'Creating [dbo].[FilterNodeContent]'
GO
CREATE TABLE [dbo].[FilterNodeContent]
(
[FilterNodeContentID] [bigint] NOT NULL IDENTITY(1014, 1000),
[RowVersion] [timestamp] NOT NULL,
[CountVersion] [bigint] NOT NULL,
[Status] [smallint] NOT NULL,
[InitialCalculation] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UpdateCalculation] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ColumnMask] [varbinary] (100) NOT NULL,
[JoinMask] [int] NOT NULL,
[Cost] [int] NOT NULL,
[Count] [int] NOT NULL,
[EntityExistsQuery] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeContent] on [dbo].[FilterNodeContent]'
GO
ALTER TABLE [dbo].[FilterNodeContent] ADD CONSTRAINT [PK_FilterNodeContent] PRIMARY KEY CLUSTERED  ([FilterNodeContentID])
GO
PRINT N'Creating index [IX_SWDefault_FilterNodeContent_Status] on [dbo].[FilterNodeContent]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FilterNodeContent_Status] ON [dbo].[FilterNodeContent] ([Status]) INCLUDE ([ColumnMask], [Cost], [FilterNodeContentID], [JoinMask], [UpdateCalculation])
GO
PRINT N'Creating [dbo].[FilterSequence]'
GO
CREATE TABLE [dbo].[FilterSequence]
(
[FilterSequenceID] [bigint] NOT NULL IDENTITY(1009, 1000),
[RowVersion] [timestamp] NOT NULL,
[ParentFilterID] [bigint] NULL,
[FilterID] [bigint] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterFolderContent] on [dbo].[FilterSequence]'
GO
ALTER TABLE [dbo].[FilterSequence] ADD CONSTRAINT [PK_FilterFolderContent] PRIMARY KEY CLUSTERED  ([FilterSequenceID])
GO
PRINT N'Creating index [IX_SWDefault_FilterSequence_FilterChild_ParentFilterID] on [dbo].[FilterSequence]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FilterSequence_FilterChild_ParentFilterID] ON [dbo].[FilterSequence] ([ParentFilterID])
GO
PRINT N'Creating index [IX_SWDefault_FilterSequence_FilterID] on [dbo].[FilterSequence]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FilterSequence_FilterID] ON [dbo].[FilterSequence] ([FilterID]) INCLUDE ([FilterSequenceID])
GO


PRINT N'Creating [dbo].[FilterNodeColumnSettings]'
GO
CREATE TABLE [dbo].[FilterNodeColumnSettings]
(
[FilterNodeColumnSettingsID] [bigint] NOT NULL IDENTITY(1032, 1000),
[UserID] [bigint] NULL,
[FilterNodeID] [bigint] NOT NULL,
[Inherit] [bit] NOT NULL,
[GridColumnLayoutID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeColumnSettings] on [dbo].[FilterNodeColumnSettings]'
GO
ALTER TABLE [dbo].[FilterNodeColumnSettings] ADD CONSTRAINT [PK_FilterNodeColumnSettings] PRIMARY KEY CLUSTERED  ([FilterNodeColumnSettingsID])
GO
PRINT N'Creating index [IX_SWDefault_FilterNodeColumnSettings] on [dbo].[FilterNodeColumnSettings]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_FilterNodeColumnSettings] ON [dbo].[FilterNodeColumnSettings] ([UserID], [FilterNodeID])
GO
PRINT N'Creating [dbo].[GridColumnLayout]'
GO
CREATE TABLE [dbo].[GridColumnLayout]
(
[GridColumnLayoutID] [bigint] NOT NULL IDENTITY(1016, 1000),
[DefinitionSet] [int] NOT NULL,
[DefaultSortColumnGuid] [uniqueidentifier] NOT NULL,
[DefaultSortOrder] [int] NOT NULL,
[LastSortColumnGuid] [uniqueidentifier] NOT NULL,
[LastSortOrder] [int] NOT NULL,
[DetailViewSettings] [xml] NULL
)
GO
PRINT N'Creating primary key PK_GridColumnLayout on [dbo].[GridColumnLayout]'
GO
ALTER TABLE [dbo].[GridColumnLayout] ADD CONSTRAINT [PK_GridColumnLayout] PRIMARY KEY CLUSTERED  ([GridColumnLayoutID])
GO
PRINT N'Creating [dbo].[FilterNodeContentDetail]'
GO
CREATE TABLE [dbo].[FilterNodeContentDetail]
(
[FilterNodeContentID] [bigint] NOT NULL,
[ObjectID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeContentDetail] on [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] ADD CONSTRAINT [PK_FilterNodeContentDetail] PRIMARY KEY CLUSTERED  ([FilterNodeContentID], [ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[Filter]'
GO
CREATE TABLE [dbo].[Filter]
(
[FilterID] [bigint] NOT NULL IDENTITY(1010, 1000),
[RowVersion] [timestamp] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FilterTarget] [int] NOT NULL,
[IsFolder] [bit] NOT NULL,
[Definition] [xml] NULL,
[State] [tinyint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Filter] on [dbo].[Filter]'
GO
ALTER TABLE [dbo].[Filter] ADD CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED  ([FilterID])
GO
PRINT N'Creating index [IX_SWDefault_Filter_State] on [dbo].[Filter]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Filter_State] ON [dbo].[Filter] ([State])
GO
PRINT N'Creating index [IX_SWDefault_Filter_IsFolder] on [dbo].[Filter]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_Filter_IsFolder] ON [dbo].[Filter] ([IsFolder]) INCLUDE ([FilterID])
GO

PRINT N'Creating [dbo].[GenericModuleStore]'
GO
CREATE TABLE [dbo].[GenericModuleStore]
(
[StoreID] [bigint] NOT NULL,
[ModuleUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleVersion] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePlatform] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleDeveloper] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleOnlineStoreCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleStatusCodes] [xml] NOT NULL,
[ModuleDownloadPageSize] [int] NOT NULL,
[ModuleRequestTimeout] [int] NOT NULL,
[ModuleDownloadStrategy] [int] NOT NULL,
[ModuleOnlineStatusSupport] [int] NOT NULL,
[ModuleOnlineStatusDataType] [int] NOT NULL,
[ModuleOnlineCustomerSupport] [bit] NOT NULL,
[ModuleOnlineCustomerDataType] [int] NOT NULL,
[ModuleOnlineShipmentDetails] [bit] NOT NULL,
[ModuleHttpExpect100Continue] [bit] NOT NULL,
[ModuleResponseEncoding] [int] NOT NULL,
[SchemaVersion] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonMerchantID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonAuthToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonApiRegion] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncludeMilliseconds] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_GenericModuleStore] on [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD CONSTRAINT [PK_GenericModuleStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[GridColumnFormat]'
GO
CREATE TABLE [dbo].[GridColumnFormat]
(
[GridColumnFormatID] [bigint] NOT NULL IDENTITY(1015, 1000),
[UserID] [bigint] NOT NULL,
[ColumnGuid] [uniqueidentifier] NOT NULL,
[Settings] [xml] NOT NULL
)
GO
PRINT N'Creating primary key [PK_GridColumnFormat] on [dbo].[GridColumnFormat]'
GO
ALTER TABLE [dbo].[GridColumnFormat] ADD CONSTRAINT [PK_GridColumnFormat] PRIMARY KEY CLUSTERED  ([GridColumnFormatID])
GO
PRINT N'Creating index [IX_SWDefault_GridColumnDisplay] on [dbo].[GridColumnFormat]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_GridColumnDisplay] ON [dbo].[GridColumnFormat] ([UserID], [ColumnGuid])
GO
PRINT N'Creating [dbo].[GridColumnPosition]'
GO
CREATE TABLE [dbo].[GridColumnPosition]
(
[GridColumnPositionID] [bigint] NOT NULL IDENTITY(1017, 1000),
[GridColumnLayoutID] [bigint] NOT NULL,
[ColumnGuid] [uniqueidentifier] NOT NULL,
[Visible] [bit] NOT NULL,
[Width] [int] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating primary key PK_GridColumnPosition on [dbo].[GridColumnPosition]'
GO
ALTER TABLE [dbo].[GridColumnPosition] ADD CONSTRAINT PK_GridColumnPosition PRIMARY KEY CLUSTERED  ([GridColumnPositionID])
GO
PRINT N'Creating index [IX_SWDefault_GridLayoutColumn] on [dbo].[GridColumnPosition]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_GridColumnPosition_GridColumnLayoutIdColumn] ON [dbo].[GridColumnPosition] ([GridColumnLayoutID], [ColumnGuid])
GO
PRINT N'Creating [dbo].[InfopiaOrderItem]'
GO
CREATE TABLE [dbo].[InfopiaOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[Marketplace] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MarketplaceItemID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BuyerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_InfopiaOrderItem] on [dbo].[InfopiaOrderItem]'
GO
ALTER TABLE [dbo].[InfopiaOrderItem] ADD CONSTRAINT [PK_InfopiaOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[InfopiaStore]'
GO
CREATE TABLE [dbo].[InfopiaStore]
(
[StoreID] [bigint] NOT NULL,
[ApiToken] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_InfopiaStore] on [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] ADD CONSTRAINT [PK_InfopiaStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[InsurancePolicy]'
GO
CREATE TABLE [dbo].[InsurancePolicy]
(
[ShipmentID] [bigint] NOT NULL,
[InsureShipStoreName] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedWithApi] [bit] NOT NULL,
[ItemName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClaimType] [int] NULL,
[DamageValue] [money] NULL,
[SubmissionDate] [datetime] NULL,
[ClaimID] [bigint] NULL,
[EmailAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_InsurancePolicy] on [dbo].[InsurancePolicy]'
GO
ALTER TABLE [dbo].[InsurancePolicy] ADD CONSTRAINT [PK_InsurancePolicy] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[iParcelShipment]'
GO
CREATE TABLE [dbo].[iParcelShipment]
(
[ShipmentID] [bigint] NOT NULL,
[iParcelAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[Reference] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TrackByEmail] [bit] NOT NULL,
[TrackBySMS] [bit] NOT NULL,
[IsDeliveryDutyPaid] [bit] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_iParcelShipment] on [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] ADD CONSTRAINT [PK_iParcelShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_SWDefault_IParcelShipment_Service] on [dbo].[IParcelShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_IParcelShipment_Service] ON [dbo].[IParcelShipment] ([Service])
GO
PRINT N'Creating [dbo].[iParcelPackage]'
GO
CREATE TABLE [dbo].[iParcelPackage]
(
[iParcelPackageID] [bigint] NOT NULL IDENTITY(1092, 1000),
[ShipmentID] [bigint] NOT NULL,
[Weight] [float] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[DimsWeight] [float] NOT NULL,
[Insurance] [bit] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[InsurancePennyOne] [bit] NOT NULL,
[DeclaredValue] [money] NOT NULL,
[TrackingNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ParcelNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SkuAndQuantities] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_iParcelPackage] on [dbo].[iParcelPackage]'
GO
ALTER TABLE [dbo].[iParcelPackage] ADD CONSTRAINT [PK_iParcelPackage] PRIMARY KEY CLUSTERED  ([iParcelPackageID])
GO
PRINT N'Creating [dbo].[iParcelProfile]'
GO
CREATE TABLE [dbo].[iParcelProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[iParcelAccountID] [bigint] NULL,
[Service] [int] NULL,
[Reference] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TrackByEmail] [bit] NULL,
[TrackBySMS] [bit] NULL,
[IsDeliveryDutyPaid] [bit] NULL,
[SkuAndQuantities] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_iParcelProfile] on [dbo].[iParcelProfile]'
GO
ALTER TABLE [dbo].[iParcelProfile] ADD CONSTRAINT [PK_iParcelProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating index [IX_SWDefault_iParcelPackage_ShipmentID] on [dbo].[iParcelPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_iParcelPackage_ShipmentID] ON [dbo].[iParcelPackage] ([ShipmentID])
GO
PRINT N'Creating [dbo].[MagentoOrder]'
GO
CREATE TABLE [dbo].[MagentoOrder]
(
[OrderID] [bigint] NOT NULL,
[MagentoOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MagentoOrder] on [dbo].[MagentoOrder]'
GO
ALTER TABLE [dbo].[MagentoOrder] ADD CONSTRAINT [PK_MagentoOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[MagentoStore]'
GO
CREATE TABLE [dbo].[MagentoStore]
(
[StoreID] [bigint] NOT NULL,
[MagentoTrackingEmails] [bit] NOT NULL,
[MagentoVersion] [int] NOT NULL,
[UpdateSplitOrderOnlineStatus] BIT NOT NULL
)
GO
PRINT N'Creating primary key [PK_MagentoStore] on [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] ADD CONSTRAINT [PK_MagentoStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[MarketplaceAdvisorOrder]'
GO
CREATE TABLE [dbo].[MarketplaceAdvisorOrder]
(
[OrderID] [bigint] NOT NULL,
[BuyerNumber] [bigint] NOT NULL,
[SellerOrderNumber] [bigint] NOT NULL,
[InvoiceNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ParcelID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MarketworksOrder] on [dbo].[MarketplaceAdvisorOrder]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrder] ADD CONSTRAINT [PK_MarketworksOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[MarketplaceAdvisorStore]'
GO
CREATE TABLE [dbo].[MarketplaceAdvisorStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccountType] [int] NOT NULL,
[DownloadFlags] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MarketworksStore] on [dbo].[MarketplaceAdvisorStore]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorStore] ADD CONSTRAINT [PK_MarketworksStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[OrderItemAttribute]'
GO
CREATE TABLE [dbo].[OrderItemAttribute]
(
[OrderItemAttributeID] [bigint] NOT NULL IDENTITY(1020, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderItemID] [bigint] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UnitPrice] [money] NOT NULL,
[IsManual] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderItemAttribute] on [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] ADD CONSTRAINT [PK_OrderItemAttribute] PRIMARY KEY CLUSTERED  ([OrderItemAttributeID])
GO
PRINT N'Creating index [IX_SWDefault_OrderItemAttribute_OrderItemID] on [dbo].[OrderItemAttribute]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_OrderItemAttribute_OrderItemID] ON [dbo].[OrderItemAttribute] ([OrderItemID] ASC,[OrderItemAttributeID] ASC)
GO
ALTER TABLE [dbo].[OrderItemAttribute] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[OrderItemAttribute]'
GO
PRINT N'Creating [dbo].[MivaOrderItemAttribute]'
GO
CREATE TABLE [dbo].[MivaOrderItemAttribute]
(
[OrderItemAttributeID] [bigint] NOT NULL,
[MivaOptionCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MivaAttributeID] [int] NOT NULL,
[MivaAttributeCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_MivaOrderItemAttributes] on [dbo].[MivaOrderItemAttribute]'
GO
ALTER TABLE [dbo].[MivaOrderItemAttribute] ADD CONSTRAINT [PK_MivaOrderItemAttributes] PRIMARY KEY CLUSTERED  ([OrderItemAttributeID])
GO
PRINT N'Creating [dbo].[MivaStore]'
GO
CREATE TABLE [dbo].[MivaStore]
(
[StoreID] [bigint] NOT NULL,
[EncryptionPassphrase] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LiveManualOrderNumbers] [bit] NOT NULL,
[SebenzaCheckoutDataEnabled] [bit] NOT NULL,
[OnlineUpdateStrategy] [int] NOT NULL,
[OnlineUpdateStatusChangeEmail] [bit] NOT NULL,
[AddendumCheckoutDataEnabled] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MivaStore] on [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] ADD CONSTRAINT [PK_MivaStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[NetworkSolutionsOrder]'
GO
CREATE TABLE [dbo].[NetworkSolutionsOrder]
(
[OrderID] [bigint] NOT NULL,
[NetworkSolutionsOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_NetworkSolutionsOrder] on [dbo].[NetworkSolutionsOrder]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrder] ADD CONSTRAINT [PK_NetworkSolutionsOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[NetworkSolutionsStore]'
GO
CREATE TABLE [dbo].[NetworkSolutionsStore]
(
[StoreID] [bigint] NOT NULL,
[UserToken] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DownloadOrderStatuses] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StatusCodes] [xml] NOT NULL,
[StoreUrl] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_NetworkSolutionsStore] on [dbo].[NetworkSolutionsStore]'
GO
ALTER TABLE [dbo].[NetworkSolutionsStore] ADD CONSTRAINT [PK_NetworkSolutionsStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[NeweggOrder]'
GO
CREATE TABLE [dbo].[NeweggOrder]
(
[OrderID] [bigint] NOT NULL,
[InvoiceNumber] [bigint] NULL,
[RefundAmount] [money] NULL,
[IsAutoVoid] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_NeweggOrder] on [dbo].[NeweggOrder]'
GO
ALTER TABLE [dbo].[NeweggOrder] ADD CONSTRAINT [PK_NeweggOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[NeweggOrderItem]'
GO
CREATE TABLE [dbo].[NeweggOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[SellerPartNumber] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NeweggItemNumber] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ManufacturerPartNumber] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShippingStatusID] [int] NULL,
[ShippingStatusDescription] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QuantityShipped] [int] NULL
)
GO
PRINT N'Creating primary key [PK_NeweggOrderItem] on [dbo].[NeweggOrderItem]'
GO
ALTER TABLE [dbo].[NeweggOrderItem] ADD CONSTRAINT [PK_NeweggOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[NeweggStore]'
GO
CREATE TABLE [dbo].[NeweggStore]
(
[StoreID] [bigint] NOT NULL,
[SellerID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SecretKey] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ExcludeFulfilledByNewegg] [bit] NOT NULL,
[Channel] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_NeweggStore] on [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] ADD CONSTRAINT [PK_NeweggStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[Note]'
GO
CREATE TABLE [dbo].[Note]
(
[NoteID] [bigint] NOT NULL IDENTITY(1044, 1000),
[RowVersion] [timestamp] NOT NULL,
[ObjectID] [bigint] NOT NULL,
[UserID] [bigint] NULL,
[Edited] [datetime] NOT NULL,
[Text] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Source] [int] NOT NULL,
[Visibility] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Note] on [dbo].[Note]'
GO
ALTER TABLE [dbo].[Note] ADD CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED  ([NoteID])
GO
PRINT N'Creating index [IX_SWDefault_OrderNote_ObjectID] on [dbo].[Note]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_OrderNote_ObjectID] ON [dbo].[Note] ([ObjectID] ASC, [NoteID] ASC) INCLUDE ([Edited])
GO
ALTER TABLE [dbo].[Note] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Note]'
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
[DeclaredValue] [money] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL,
[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OnTracShipment] on [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD CONSTRAINT [PK_OnTracShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_SWDefault_OnTracShipment_Service] on [dbo].[OnTracShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OnTracShipment_Service] ON [dbo].[OnTracShipment] ([Service])
GO
PRINT N'Creating index [IX_SWDefault_OnTracShipment_PackagingType] on [dbo].[OnTracShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OnTracShipment_PackagingType] ON [dbo].[OnTracShipment] ([PackagingType])
GO
PRINT N'Creating [dbo].[DhlExpressShipment]'
GO
CREATE TABLE [dbo].[DhlExpressShipment](
	[ShipmentID] [bigint] NOT NULL,
	[DhlExpressAccountID] [bigint] NOT NULL,
	[Service] [int] NOT NULL,
	[DeliveredDutyPaid] [bit] NOT NULL,
	[NonMachinable] [bit] NOT NULL,
	[SaturdayDelivery] [bit] NOT NULL,
	[RequestedLabelFormat] [int] NOT NULL,
	[Contents][int] NOT NULL,
	[NonDelivery] [int] NOT NULL,
	[ShipEngineLabelID] [nvarchar] (12) NOT NULL
)
GO
PRINT N'Creating primary key [PK_DhlExpressShipment] on [dbo].[DhlExpressShipment]'
GO
ALTER TABLE [dbo].[DhlExpressShipment] ADD CONSTRAINT [PK_DhlExpressShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[DhlExpressShipment]'
GO
ALTER TABLE [dbo].[DhlExpressShipment] ADD CONSTRAINT [FK_DhlExpressShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating index [IX_SWDefault_OnTracShipment_Service] on [dbo].[OnTracShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_DhlExpressShipment_Service] ON [dbo].[DhlExpressShipment] ([Service])
GO

PRINT N'Creating [dbo].[DhlExpressPackage]'
GO
CREATE TABLE [dbo].[DhlExpressPackage](
	[DhlExpressPackageID] [bigint] IDENTITY(1093,1000) NOT NULL,
	[ShipmentID] [bigint] NOT NULL,
	[Weight] [float] NOT NULL,
	[DimsProfileID] [bigint] NOT NULL,
	[DimsLength] [float] NOT NULL,
	[DimsWidth] [float] NOT NULL,
	[DimsHeight] [float] NOT NULL,
	[DimsAddWeight] [bit] NOT NULL,
	[DimsWeight] [float] NOT NULL,
	[Insurance] [bit] NOT NULL,
	[InsuranceValue] [money] NOT NULL,
	[TrackingNumber] [varchar](50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_DhlExpressPackage] on [dbo].[DhlExpressPackage]'
GO
ALTER TABLE [dbo].[DhlExpressPackage] ADD CONSTRAINT [PK_DhlExpressPackage] PRIMARY KEY CLUSTERED  ([DhlExpressPackageID])
GO
PRINT N'Adding foreign keys to [dbo].[DhlExpressPackage]'
GO
ALTER TABLE [dbo].[DhlExpressPackage] ADD CONSTRAINT [FK_DhlExpressPackage_DhlExpressShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[DhlExpressShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating [dbo].[DhlExpressProfile]'
GO
CREATE TABLE [dbo].[DhlExpressProfile](
	[ShippingProfileID] [bigint] NOT NULL,
	[DhlExpressAccountID] [bigint] NULL,
	[Service] [int] NULL,
	[DeliveryDutyPaid] [bit] NULL,
	[NonMachinable] [bit] NULL,
	[SaturdayDelivery] [bit] NULL,
	[Contents][int] Null,
	[NonDelivery] [int] Null
)
GO
PRINT N'Creating primary key [PK_DhlExpressProfile] on [dbo].[DhlExpressProfile]'
GO
ALTER TABLE [dbo].[DhlExpressProfile] ADD CONSTRAINT [PK_DhlExpressProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[DhlExpressProfile]'
GO
ALTER TABLE [dbo].[DhlExpressProfile] ADD CONSTRAINT [FK_DhlExpressProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Creating [dbo].[AsendiaProfile]'
GO
CREATE TABLE [dbo].[AsendiaProfile](
	[ShippingProfileID] [bigint] NOT NULL,
	[AsendiaAccountID] [bigint] NULL,
	[Service] [int] NULL,
	[NonMachinable] [bit] NULL,
	[Contents] [int] NULL,
	[NonDelivery] [int] NULL
)
GO
PRINT N'Creating primary key [PK_AsendiaProfile] on [dbo].[AsendiaProfile]'
GO
ALTER TABLE [dbo].[AsendiaProfile] ADD CONSTRAINT [PK_AsendiaProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[AsendiaProfile]'
GO
ALTER TABLE [dbo].[AsendiaProfile] ADD CONSTRAINT [FK_AsendiaProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Creating [dbo].[AsendiaShipment]'
GO
CREATE TABLE [dbo].[AsendiaShipment](
	[ShipmentID] [bigint] NOT NULL,
	[AsendiaAccountID] [bigint] NOT NULL,
	[Service] [int] NOT NULL,
	[NonMachinable] [bit] NOT NULL,
	[RequestedLabelFormat] [int] NOT NULL,
	[Contents][int] NOT NULL,
	[NonDelivery] [int] NOT NULL,
	[ShipEngineLabelID] [nvarchar] (12) NOT NULL,
	[DimsProfileID] [bigint] NOT NULL,
	[DimsLength] [float] NOT NULL,
	[DimsWidth] [float] NOT NULL,
	[DimsHeight] [float] NOT NULL,
	[DimsAddWeight] [bit] NOT NULL,
	[DimsWeight] [float] NOT NULL,
	[InsuranceValue] [money] NOT NULL,
	[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AsendiaShipment] on [dbo].[AsendiaShipment]'
GO
ALTER TABLE [dbo].[AsendiaShipment] ADD CONSTRAINT [PK_AsendiaShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[AsendiaShipment]'
GO
ALTER TABLE [dbo].[AsendiaShipment] ADD CONSTRAINT [FK_AsendiaShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating index [IX_SWDefault_AsendiaShipment_Service] on [dbo].[AsendiaShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AsendiaShipment_Service] ON [dbo].[AsendiaShipment] ([Service])
GO
PRINT N'Creating [dbo].[Customer]'
GO
CREATE TABLE [dbo].[Customer]
(
[CustomerID] [bigint] NOT NULL IDENTITY(1012, 1000),
[RowVersion] [timestamp] NOT NULL,
[BillFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillMiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCompany] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[ShipFax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RollupOrderCount] [int] NOT NULL,
[RollupOrderTotal] [money] NOT NULL,
[RollupNoteCount] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Customer] on [dbo].[Customer]'
GO
ALTER TABLE [dbo].[Customer] ADD CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED  ([CustomerID])
GO
PRINT N'Creating index [IX_SWDefault_BillFirstName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillFirstName] ON [dbo].[Customer] ([BillFirstName])
GO
PRINT N'Creating index [IX_SWDefault_BillLastName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillLastName] ON [dbo].[Customer] ([BillLastName])
GO
PRINT N'Creating index [IX_SWDefault_BillCompany] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillCompany] ON [dbo].[Customer] ([BillCompany])
GO
PRINT N'Creating index [IX_SWDefault_BillStateProvCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillStateProvCode] ON [dbo].[Customer] ([BillStateProvCode])
GO
PRINT N'Creating index [IX_SWDefault_BillPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillPostalCode] ON [dbo].[Customer] ([BillPostalCode])
GO
PRINT N'Creating index [IX_SWDefault_BillCountryCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillCountryCode] ON [dbo].[Customer] ([BillCountryCode])
GO
PRINT N'Creating index [IX_SWDefault_BillEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_BillEmail] ON [dbo].[Customer] ([BillEmail])
GO
PRINT N'Creating index [IX_SWDefault_ShipLastName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipLastName] ON [dbo].[Customer] ([ShipLastName])
GO
PRINT N'Creating index [IX_SWDefault_ShipCompany] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipCompany] ON [dbo].[Customer] ([ShipCompany])
GO
PRINT N'Creating index [IX_SWDefault_ShipStateProvCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipStateProvCode] ON [dbo].[Customer] ([ShipStateProvCode])
GO
PRINT N'Creating index [IX_SWDefault_ShipPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipPostalCode] ON [dbo].[Customer] ([ShipPostalCode])
GO
PRINT N'Creating index [IX_SWDefault_ShipCountryCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipCountryCode] ON [dbo].[Customer] ([ShipCountryCode])
GO
PRINT N'Creating index [IX_SWDefault_ShipEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipEmail] ON [dbo].[Customer] ([ShipEmail])
GO
PRINT N'Creating index [IX_SWDefault_RollupOrderCount] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupOrderCount] ON [dbo].[Customer] ([RollupOrderCount])
GO
PRINT N'Creating index [IX_SWDefault_RollupOrderTotal] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupOrderTotal] ON [dbo].[Customer] ([RollupOrderTotal])
GO
PRINT N'Creating index [IX_SWDefault_RollupNoteCount] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RollupNoteCount] ON [dbo].[Customer] ([RollupNoteCount])
GO
ALTER TABLE [dbo].[Customer] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Customer]'
GO
PRINT N'Creating [dbo].[OrderCharge]'
GO
CREATE TABLE [dbo].[OrderCharge]
(
[OrderChargeID] [bigint] NOT NULL IDENTITY(1021, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[Type] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Amount] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderCharge] on [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] ADD CONSTRAINT [PK_OrderCharge] PRIMARY KEY CLUSTERED  ([OrderChargeID])
GO
PRINT N'Creating index [IX_SWDefault_OrderCharge_OrderID] on [dbo].[OrderCharge]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_OrderCharge_OrderID] ON [dbo].[OrderCharge] ([OrderID] ASC, [OrderChargeID] ASC)
GO
ALTER TABLE [dbo].[OrderCharge] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[OrderCharge]'
GO
PRINT N'Creating [dbo].[OrderMotionOrder]'
GO
CREATE TABLE [dbo].[OrderMotionOrder]
(
[OrderID] [bigint] NOT NULL,
[OrderMotionShipmentID] [int] NOT NULL,
[OrderMotionPromotion] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrderMotionInvoiceNumber] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderMotionOrder] on [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] ADD CONSTRAINT [PK_OrderMotionOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[OrderMotionStore]'
GO
CREATE TABLE [dbo].[OrderMotionStore]
(
[StoreID] [bigint] NOT NULL,
[OrderMotionEmailAccountID] [bigint] NOT NULL,
[OrderMotionBizID] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderMotionStore] on [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] ADD CONSTRAINT [PK_OrderMotionStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[OrderPaymentDetail]'
GO
CREATE TABLE [dbo].[OrderPaymentDetail]
(
[OrderPaymentDetailID] [bigint] NOT NULL IDENTITY(1023, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[Label] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Value] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderPaymentDetail] on [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] ADD CONSTRAINT [PK_OrderPaymentDetail] PRIMARY KEY CLUSTERED  ([OrderPaymentDetailID])
GO
PRINT N'Creating index [IX_SWDefault_OrderPaymentDetail_OrderID] on [dbo].[OrderPaymentDetail]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_OrderPaymentDetail_OrderID] ON [dbo].[OrderPaymentDetail] ([OrderID] ASC, [OrderPaymentDetailID] ASC)
GO
ALTER TABLE [dbo].[OrderPaymentDetail] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[OrderPaymentDetail]'
GO
PRINT N'Creating [dbo].[OtherProfile]'
GO
CREATE TABLE [dbo].[OtherProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[Carrier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Service] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_OtherProfile] on [dbo].[OtherProfile]'
GO
ALTER TABLE [dbo].[OtherProfile] ADD CONSTRAINT [PK_OtherProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[OtherShipment]'
GO
CREATE TABLE [dbo].[OtherShipment]
(
[ShipmentID] [bigint] NOT NULL,
[Carrier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Service] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceValue] [money] NOT NULL,
[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OtherShipment] on [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] ADD CONSTRAINT [PK_OtherShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[PayPalOrder]'
GO
CREATE TABLE [dbo].[PayPalOrder]
(
[OrderID] [bigint] NOT NULL,
[TransactionID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AddressStatus] [int] NOT NULL,
[PayPalFee] [money] NOT NULL,
[PaymentStatus] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PayPalOrder] on [dbo].[PayPalOrder]'
GO
ALTER TABLE [dbo].[PayPalOrder] ADD CONSTRAINT [PK_PayPalOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_PayPalOrder_PaymentStatus] on [dbo].[PayPalOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_PayPalOrder_PaymentStatus] ON [dbo].[PayPalOrder] ([PaymentStatus])
GO
PRINT N'Creating [dbo].[PayPalStore]'
GO
CREATE TABLE [dbo].[PayPalStore]
(
[StoreID] [bigint] NOT NULL,
[ApiCredentialType] [smallint] NOT NULL,
[ApiUserName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiPassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiSignature] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiCertificate] [varbinary] (4096) NULL,
[LastTransactionDate] [datetime] NOT NULL,
[LastValidTransactionDate] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PayPalStore] on [dbo].[PayPalStore]'
GO
ALTER TABLE [dbo].[PayPalStore] ADD CONSTRAINT [PK_PayPalStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[Permission]'
GO
CREATE TABLE [dbo].[Permission]
(
[PermissionID] [bigint] NOT NULL IDENTITY(1004, 1000),
[UserID] [bigint] NOT NULL,
[PermissionType] [int] NOT NULL,
[ObjectID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_Permission] on [dbo].[Permission]'
GO
ALTER TABLE [dbo].[Permission] ADD CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED  ([PermissionID])
GO
PRINT N'Creating index [IX_SWDefault_Permission] on [dbo].[Permission]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_Permission] ON [dbo].[Permission] ([UserID], [PermissionType], [ObjectID])
GO
PRINT N'Creating [dbo].[PrintResult]'
GO
CREATE TABLE [dbo].[PrintResult]
(
[PrintResultID] [bigint] NOT NULL IDENTITY(1045, 1000),
[RowVersion] [timestamp] NOT NULL,
[JobIdentifier] [uniqueidentifier] NOT NULL,
[RelatedObjectID] [bigint] NOT NULL,
[ContextObjectID] [bigint] NOT NULL,
[TemplateID] [bigint] NULL,
[TemplateType] [int] NULL,
[OutputFormat] [int] NULL,
[LabelSheetID] [bigint] NULL,
[ComputerID] [bigint] NOT NULL,
[ContentResourceID] [bigint] NOT NULL,
[PrintDate] [datetime] NOT NULL,
[PrinterName] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PaperSource] [int] NOT NULL,
[PaperSourceName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Copies] [int] NOT NULL,
[Collated] [bit] NOT NULL,
[PageMarginLeft] [float] NOT NULL,
[PageMarginRight] [float] NOT NULL,
[PageMarginBottom] [float] NOT NULL,
[PageMarginTop] [float] NOT NULL,
[PageWidth] [float] NOT NULL,
[PageHeight] [float] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PrintResult] on [dbo].[PrintResult]'
GO
ALTER TABLE [dbo].[PrintResult] ADD CONSTRAINT [PK_PrintResult] PRIMARY KEY CLUSTERED  ([PrintResultID])
GO
PRINT N'Creating index [IX_SWDefault_PrintResult_RelatedObjectID] on [dbo].[PrintResult]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_PrintResult_RelatedObjectID] ON [dbo].[PrintResult] ([RelatedObjectID])
GO
PRINT N'Creating index [IX_SWDefault_PrintResult_PrintDateRelatedObjectID] on [dbo].[PrintResult]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_PrintResult_PrintDateRelatedObjectID] ON [dbo].[PrintResult] ([PrintDate], [RelatedObjectID]) INCLUDE ([TemplateType], [ContentResourceID])
GO
ALTER TABLE [dbo].[PrintResult] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[PrintResult]'
GO
PRINT N'Creating [dbo].[ProStoresOrder]'
GO
CREATE TABLE [dbo].[ProStoresOrder]
(
[OrderID] [bigint] NOT NULL,
[ConfirmationNumber] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuthorizedDate] [datetime] NULL,
[AuthorizedBy] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProStoresOrder] on [dbo].[ProStoresOrder]'
GO
ALTER TABLE [dbo].[ProStoresOrder] ADD CONSTRAINT [PK_ProStoresOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProStoresOrder_ConfirmationNumber]
	ON [dbo].[ProStoresOrder] ( [ConfirmationNumber] ASC )
GO
PRINT N'Creating [dbo].[ProStoresStore]'
GO
CREATE TABLE [dbo].[ProStoresStore]
(
[StoreID] [bigint] NOT NULL,
[ShortName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Username] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LoginMethod] [int] NOT NULL,
[ApiEntryPoint] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiToken] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiStorefrontUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiTokenLogonUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiXteUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiRestSecureUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiRestNonSecureUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiRestScriptSuffix] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LegacyAdminUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LegacyXtePath] [varchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LegacyPrefix] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LegacyPassword] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LegacyCanUpgrade] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProStoresStore] on [dbo].[ProStoresStore]'
GO
ALTER TABLE [dbo].[ProStoresStore] ADD CONSTRAINT [PK_ProStoresStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[Scheduling_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DESCRIPTION] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NEXT_FIRE_TIME] [bigint] NULL,
[PREV_FIRE_TIME] [bigint] NULL,
[PRIORITY] [int] NULL,
[TRIGGER_STATE] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_TYPE] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[START_TIME] [bigint] NOT NULL,
[END_TIME] [bigint] NULL,
[CALENDAR_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MISFIRE_INSTR] [int] NULL,
[JOB_DATA] [image] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_TRIGGERS] on [dbo].[Scheduling_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_C] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_C] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [CALENDAR_NAME])
GO
PRINT N'Creating index [IDX_Scheduling_T_JG] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_JG] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_J] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_J] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_MISFIRE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_MISFIRE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [MISFIRE_INSTR], [NEXT_FIRE_TIME])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_ST_MISFIRE_GRP] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_ST_MISFIRE_GRP] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [MISFIRE_INSTR], [NEXT_FIRE_TIME], [TRIGGER_GROUP], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_ST_MISFIRE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_ST_MISFIRE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [MISFIRE_INSTR], [NEXT_FIRE_TIME], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_NEXT_FIRE_TIME] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NEXT_FIRE_TIME] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [NEXT_FIRE_TIME])
GO
PRINT N'Creating index [IDX_Scheduling_T_G] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_G] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_N_G_STATE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_N_G_STATE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_GROUP], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_N_STATE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_N_STATE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_STATE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_STATE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_ST] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_ST] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_STATE], [NEXT_FIRE_TIME])
GO
PRINT N'Creating [dbo].[Scheduling_CRON_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_CRON_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CRON_EXPRESSION] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TIME_ZONE_ID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_CRON_TRIGGERS] on [dbo].[Scheduling_CRON_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_CRON_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_CRON_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_SIMPLE_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_SIMPLE_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[REPEAT_COUNT] [int] NOT NULL,
[REPEAT_INTERVAL] [bigint] NOT NULL,
[TIMES_TRIGGERED] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_SIMPLE_TRIGGERS] on [dbo].[Scheduling_SIMPLE_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPLE_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_SIMPLE_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_SIMPROP_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_SIMPROP_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[STR_PROP_1] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[STR_PROP_2] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[STR_PROP_3] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[INT_PROP_1] [int] NULL,
[INT_PROP_2] [int] NULL,
[LONG_PROP_1] [bigint] NULL,
[LONG_PROP_2] [bigint] NULL,
[DEC_PROP_1] [numeric] (13, 4) NULL,
[DEC_PROP_2] [numeric] (13, 4) NULL,
[BOOL_PROP_1] [bit] NULL,
[BOOL_PROP_2] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_SIMPROP_TRIGGERS] on [dbo].[Scheduling_SIMPROP_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPROP_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_SIMPROP_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_JOB_DETAILS]'
GO
CREATE TABLE [dbo].[Scheduling_JOB_DETAILS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DESCRIPTION] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[JOB_CLASS_NAME] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IS_DURABLE] [bit] NOT NULL,
[IS_NONCONCURRENT] [bit] NOT NULL,
[IS_UPDATE_DATA] [bit] NOT NULL,
[REQUESTS_RECOVERY] [bit] NOT NULL,
[JOB_DATA] [image] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_JOB_DETAILS] on [dbo].[Scheduling_JOB_DETAILS]'
GO
ALTER TABLE [dbo].[Scheduling_JOB_DETAILS] ADD CONSTRAINT [PK_Scheduling_JOB_DETAILS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Creating [dbo].[SearsOrder]'
GO
CREATE TABLE [dbo].[SearsOrder]
(
[OrderID] [bigint] NOT NULL,
[PoNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PoNumberWithDate] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationID] [int] NOT NULL,
[Commission] [money] NOT NULL,
[CustomerPickup] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsOrder] on [dbo].[SearsOrder]'
GO
ALTER TABLE [dbo].[SearsOrder] ADD CONSTRAINT [PK_SearsOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_SearsOrder_PoNumber_OrderID] on [dbo].[SearsOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_SearsOrder_PoNumber_OrderID] ON [dbo].[SearsOrder] ([PoNumber], [OrderID])
GO
PRINT N'Creating [dbo].[SearsOrderItem]'
GO
CREATE TABLE [dbo].[SearsOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[LineNumber] [int] NOT NULL,
[ItemID] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Commission] [money] NOT NULL,
[Shipping] [money] NOT NULL,
[OnlineStatus] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsOrderItem] on [dbo].[SearsOrderItem]'
GO
ALTER TABLE [dbo].[SearsOrderItem] ADD CONSTRAINT [PK_SearsOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[SearsStore]'
GO
CREATE TABLE [dbo].[SearsStore]
(
[StoreID] [bigint] NOT NULL,
[SearsEmail] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SecretKey] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellerID] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsStore] on [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] ADD CONSTRAINT [PK_SearsStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[ServerMessageSignoff]'
GO
CREATE TABLE [dbo].[ServerMessageSignoff]
(
[ServerMessageSignoffID] [bigint] NOT NULL IDENTITY(1037, 1000),
[RowVersion] [timestamp] NOT NULL,
[ServerMessageID] [bigint] NOT NULL,
[UserID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[Dismissed] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ServerMessageSignoff] on [dbo].[ServerMessageSignoff]'
GO
ALTER TABLE [dbo].[ServerMessageSignoff] ADD CONSTRAINT [PK_ServerMessageSignoff] PRIMARY KEY CLUSTERED  ([ServerMessageSignoffID])
GO
PRINT N'Creating index [IX_SWDefault_ServerMessageSignoff] on [dbo].[ServerMessageSignoff]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ServerMessageSignoff] ON [dbo].[ServerMessageSignoff] ([UserID], [ComputerID], [ServerMessageID])
GO
PRINT N'Creating [dbo].[ServerMessage]'
GO
CREATE TABLE [dbo].[ServerMessage]
(
[ServerMessageID] [bigint] NOT NULL IDENTITY(1036, 1000),
[RowVersion] [timestamp] NOT NULL,
[Number] [int] NOT NULL,
[Published] [datetime] NOT NULL,
[Active] [bit] NOT NULL,
[Dismissable] [bit] NOT NULL,
[Expires] [datetime] NULL,
[ResponseTo] [int] NULL,
[ResponseAction] [int] NULL,
[EditTo] [int] NULL,
[Image] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PrimaryText] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SecondaryText] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Actions] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Stores] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Shippers] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ServerMessage] on [dbo].[ServerMessage]'
GO
ALTER TABLE [dbo].[ServerMessage] ADD CONSTRAINT [PK_ServerMessage] PRIMARY KEY CLUSTERED  ([ServerMessageID])
GO
PRINT N'Creating index [IX_SWDefault_ServerMessage_RowVersion] on [dbo].[ServerMessage]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ServerMessage_RowVersion] ON [dbo].[ServerMessage] ([RowVersion])
GO
PRINT N'Creating index [IX_SWDefault_ServerMessage_Number] on [dbo].[ServerMessage]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ServerMessage_Number] ON [dbo].[ServerMessage] ([Number])
GO
PRINT N'Creating index [IX_SWDefault_ServerMessage_Expires] on [dbo].[ServerMessage]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ServerMessage_Expires] ON [dbo].[ServerMessage] ([Expires])
GO
PRINT N'Creating [dbo].[ServiceStatus]'
GO
CREATE TABLE [dbo].[ServiceStatus]
(
[ServiceStatusID] [bigint] NOT NULL IDENTITY(1096, 1000),
[RowVersion] [timestamp] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ServiceType] [int] NOT NULL,
[LastStartDateTime] [datetime] NULL,
[LastStopDateTime] [datetime] NULL,
[LastCheckInDateTime] [datetime] NULL,
[ServiceFullName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServiceDisplayName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ServiceStatus] on [dbo].[ServiceStatus]'
GO
ALTER TABLE [dbo].[ServiceStatus] ADD CONSTRAINT [PK_ServiceStatus] PRIMARY KEY CLUSTERED  ([ServiceStatusID])
GO
ALTER TABLE [dbo].[ServiceStatus] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[ServiceStatus]'
GO
PRINT N'Creating [dbo].[ShipmentCustomsItem]'
GO
CREATE TABLE [dbo].[ShipmentCustomsItem]
(
[ShipmentCustomsItemID] [bigint] NOT NULL IDENTITY(1051, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentID] [bigint] NOT NULL,
[Description] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Quantity] [float] NOT NULL,
[Weight] [float] NOT NULL,
[UnitValue] [money] NOT NULL,
[CountryOfOrigin] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HarmonizedCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NumberOfPieces] [int] NOT NULL,
[UnitPriceAmount] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShipmentCustomsItem] on [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD CONSTRAINT [PK_ShipmentCustomsItem] PRIMARY KEY CLUSTERED  ([ShipmentCustomsItemID])
GO
PRINT N'Creating index [IX_SWDefault_ShipmentCustomsItem_ShipmentID] on [dbo].[ShipmentCustomsItem]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ShipmentCustomsItem_ShipmentID] ON [dbo].[ShipmentCustomsItem] ([ShipmentID])
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[ShipmentCustomsItem]'
GO
PRINT N'Creating [dbo].[ShippingPrintOutput]'
GO
CREATE TABLE [dbo].[ShippingPrintOutput]
(
[ShippingPrintOutputID] [bigint] NOT NULL IDENTITY(1058, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentType] [int] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShippingPrintOutput] on [dbo].[ShippingPrintOutput]'
GO
ALTER TABLE [dbo].[ShippingPrintOutput] ADD CONSTRAINT [PK_ShippingPrintOutput] PRIMARY KEY CLUSTERED  ([ShippingPrintOutputID])
GO
ALTER TABLE [dbo].[ShippingPrintOutput] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[ShippingPrintOutput]'
GO
PRINT N'Creating [dbo].[ShippingPrintOutputRule]'
GO
CREATE TABLE [dbo].[ShippingPrintOutputRule]
(
[ShippingPrintOutputRuleID] [bigint] NOT NULL IDENTITY(1059, 1000),
[ShippingPrintOutputID] [bigint] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[TemplateID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShippingPrintOutputRule] on [dbo].[ShippingPrintOutputRule]'
GO
ALTER TABLE [dbo].[ShippingPrintOutputRule] ADD CONSTRAINT [PK_ShippingPrintOutputRule] PRIMARY KEY CLUSTERED  ([ShippingPrintOutputRuleID])
GO
PRINT N'Creating [dbo].[ShopifyOrder]'
GO
CREATE TABLE [dbo].[ShopifyOrder]
(
[OrderID] [bigint] NOT NULL,
[ShopifyOrderID] [bigint] NOT NULL,
[FulfillmentStatusCode] [int] NOT NULL,
[PaymentStatusCode] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyOrder] on [dbo].[ShopifyOrder]'
GO
ALTER TABLE [dbo].[ShopifyOrder] ADD CONSTRAINT [PK_ShopifyOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[ShopifyOrderItem]'
GO
CREATE TABLE [dbo].[ShopifyOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ShopifyOrderItemID] [bigint] NOT NULL,
[ShopifyProductID] [bigint] NOT NULL,
[InventoryItemID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyOrderItem] on [dbo].[ShopifyOrderItem]'
GO
ALTER TABLE [dbo].[ShopifyOrderItem] ADD CONSTRAINT [PK_ShopifyOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[ShopifyStore]'
GO
CREATE TABLE [dbo].[ShopifyStore]
(
[StoreID] [bigint] NOT NULL,
[ShopifyShopUrlName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyShopDisplayName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyAccessToken] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShopifyRequestedShippingOption] [int] NOT NULL,
[ApiKey] [nvarchar](100) NOT NULL,
[Password] [nvarchar](100) NOT NULL,
[ShopifyNotifyCustomer] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyStore] on [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD CONSTRAINT [PK_ShopifyStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[UspsProfile]'
GO
CREATE TABLE [dbo].[UspsProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[UspsAccountID] [bigint] NULL,
[HidePostage] [bit] NULL,
[RequireFullAddressValidation] [bit] NULL,
[RateShop] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_UspsProfile] on [dbo].[UspsProfile]'
GO
ALTER TABLE [dbo].[UspsProfile] ADD CONSTRAINT [PK_UspsProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[UspsScanForm]'
GO
CREATE TABLE [dbo].[UspsScanForm]
(
[UspsScanFormID] [bigint] NOT NULL IDENTITY(1072, 1000),
[UspsAccountID] [bigint] NOT NULL,
[ScanFormTransactionID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ScanFormUrl] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ScanFormBatchID] [bigint] NOT NULL,
[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_UspsScanForm] on [dbo].[UspsScanForm]'
GO
ALTER TABLE [dbo].[UspsScanForm] ADD CONSTRAINT [PK_UspsScanForm] PRIMARY KEY CLUSTERED  ([UspsScanFormID])
GO
PRINT N'Creating [dbo].[UspsShipment]'
GO
CREATE TABLE [dbo].[UspsShipment]
(
[ShipmentID] [bigint] NOT NULL,
[UspsAccountID] [bigint] NOT NULL,
[HidePostage] [bit] NOT NULL,
[RequireFullAddressValidation] [bit] NOT NULL,
[IntegratorTransactionID] [uniqueidentifier] NOT NULL,
[UspsTransactionID] [uniqueidentifier] NOT NULL,
[OriginalUspsAccountID] [bigint] NULL,
[ScanFormBatchID] [bigint] NULL,
[RequestedLabelFormat] [int] NOT NULL,
[RateShop] [bit] NOT NULL,
[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_UspsShipment] on [dbo].[UspsShipment]'
GO
ALTER TABLE [dbo].[UspsShipment] ADD CONSTRAINT [PK_UspsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[StatusPreset]'
GO
CREATE TABLE [dbo].[StatusPreset]
(
[StatusPresetID] [bigint] NOT NULL IDENTITY(1022, 1000),
[RowVersion] [timestamp] NOT NULL,
[StoreID] [bigint] NULL,
[StatusTarget] [int] NOT NULL,
[StatusText] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsDefault] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_StatusPreset] on [dbo].[StatusPreset]'
GO
ALTER TABLE [dbo].[StatusPreset] ADD CONSTRAINT [PK_StatusPreset] PRIMARY KEY CLUSTERED  ([StatusPresetID])
GO
ALTER TABLE [dbo].[StatusPreset] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[StatusPreset]'
GO
PRINT N'Creating [dbo].[ShopSiteStore]'
GO
CREATE TABLE [dbo].[ShopSiteStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RequireSSL] [bit] NOT NULL,
[DownloadPageSize] [int] NOT NULL,
[RequestTimeout] [int] NOT NULL,
[ShopSiteAuthentication] [int] NOT NULL,
[OauthClientID] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OauthSecretKey] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OauthAuthorizationCode] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Identifier] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_StoreShopSite] on [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] ADD CONSTRAINT [PK_StoreShopSite] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[TemplateFolder]'
GO
CREATE TABLE [dbo].[TemplateFolder]
(
[TemplateFolderID] [bigint] NOT NULL IDENTITY(1024, 1000),
[RowVersion] [timestamp] NOT NULL,
[ParentFolderID] [bigint] NULL,
[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_TemplateFolder] on [dbo].[TemplateFolder]'
GO
ALTER TABLE [dbo].[TemplateFolder] ADD CONSTRAINT [PK_TemplateFolder] PRIMARY KEY CLUSTERED  ([TemplateFolderID])
GO
ALTER TABLE [dbo].[TemplateFolder] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[TemplateFolder]'
GO
PRINT N'Creating [dbo].[Template]'
GO
CREATE TABLE [dbo].[Template]
(
[TemplateID] [bigint] NOT NULL IDENTITY(1025, 1000),
[RowVersion] [timestamp] NOT NULL,
[ParentFolderID] [bigint] NOT NULL,
[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Xsl] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Type] [int] NOT NULL,
[Context] [int] NOT NULL,
[OutputFormat] [int] NOT NULL,
[OutputEncoding] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PageMarginLeft] [float] NOT NULL,
[PageMarginRight] [float] NOT NULL,
[PageMarginBottom] [float] NOT NULL,
[PageMarginTop] [float] NOT NULL,
[PageWidth] [float] NOT NULL,
[PageHeight] [float] NOT NULL,
[LabelSheetID] [bigint] NOT NULL,
[PrintCopies] [int] NOT NULL,
[PrintCollate] [bit] NOT NULL,
[SaveFileName] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SaveFileFolder] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SaveFilePrompt] [int] NOT NULL,
[SaveFileBOM] [bit] NOT NULL,
[SaveFileOnlineResources] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Template] on [dbo].[Template]'
GO
ALTER TABLE [dbo].[Template] ADD CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED  ([TemplateID])
GO
ALTER TABLE [dbo].[Template] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Template]'
GO
PRINT N'Creating [dbo].[TemplateComputerSettings]'
GO
CREATE TABLE [dbo].[TemplateComputerSettings]
(
[TemplateComputerSettingsID] [bigint] NOT NULL IDENTITY(1029, 1000),
[TemplateID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[PrinterName] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PaperSource] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_TemplateComputerSettings] on [dbo].[TemplateComputerSettings]'
GO
ALTER TABLE [dbo].[TemplateComputerSettings] ADD CONSTRAINT [PK_TemplateComputerSettings] PRIMARY KEY CLUSTERED  ([TemplateComputerSettingsID])
GO
PRINT N'Creating [dbo].[TemplateStoreSettings]'
GO
CREATE TABLE [dbo].[TemplateStoreSettings]
(
[TemplateStoreSettingsID] [bigint] NOT NULL IDENTITY(1033, 1000),
[TemplateID] [bigint] NOT NULL,
[StoreID] [bigint] NULL,
[EmailUseDefault] [bit] NOT NULL,
[EmailAccountID] [bigint] NOT NULL,
[EmailTo] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailCc] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailBcc] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailSubject] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_TemplateStoreSettings] on [dbo].[TemplateStoreSettings]'
GO
ALTER TABLE [dbo].[TemplateStoreSettings] ADD CONSTRAINT [PK_TemplateStoreSettings] PRIMARY KEY CLUSTERED  ([TemplateStoreSettingsID])
GO
PRINT N'Creating index [IX_SWDefault_TemplateStoreSettings] on [dbo].[TemplateStoreSettings]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_TemplateStoreSettings] ON [dbo].[TemplateStoreSettings] ([TemplateID], [StoreID])
GO
PRINT N'Creating [dbo].[TemplateUserSettings]'
GO
CREATE TABLE [dbo].[TemplateUserSettings]
(
[TemplateUserSettingsID] [bigint] NOT NULL IDENTITY(1028, 1000),
[TemplateID] [bigint] NOT NULL,
[UserID] [bigint] NOT NULL,
[PreviewSource] [int] NOT NULL,
[PreviewCount] [int] NOT NULL,
[PreviewFilterNodeID] [bigint] NULL,
[PreviewZoom] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_TemplateUserSettings] on [dbo].[TemplateUserSettings]'
GO
ALTER TABLE [dbo].[TemplateUserSettings] ADD CONSTRAINT [PK_TemplateUserSettings] PRIMARY KEY CLUSTERED  ([TemplateUserSettingsID])
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[ThreeDCartOrder]'
GO
CREATE TABLE [dbo].[ThreeDCartOrder]
(
[OrderID] [bigint] NOT NULL,
[ThreeDCartOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrder] on [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [PK_ThreeDCartOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [FK_ThreeDCartOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Creating [dbo].[ThreeDCartOrderItem]'
GO
CREATE TABLE [dbo].[ThreeDCartOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ThreeDCartShipmentID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrderItem] on [dbo].[ThreeDCartOrderItem]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderItem] ADD CONSTRAINT [PK_ThreeDCartOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[ThreeDCartStore]'
GO
CREATE TABLE [dbo].[ThreeDCartStore]
(
[StoreID] [bigint] NOT NULL,
[StoreUrl] [nvarchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserKey] [nvarchar] (65) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TimeZoneID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StatusCodes] [xml] NULL,
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL,
[RestUser] [bit] NOT NULL,
[OrderIDUpgradeFixDate] [datetime] NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartStore] on [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [PK_ThreeDCartStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[UpsShipment]'
GO
CREATE TABLE [dbo].[UpsShipment]
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
[EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifyFrom] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifySubject] [int] NOT NULL,
[EmailNotifyMessage] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [bit] NOT NULL,
[CustomsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialPaperlessInvoice] [bit] NOT NULL,
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
[ReturnUndeliverableEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnContents] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Endorsement] [int] NOT NULL,
[Subclassification] [int] NOT NULL,
[PaperlessAdditionalDocumentation] [bit] NOT NULL,
[ShipperRelease] [bit] NOT NULL,
[CarbonNeutral] [bit] NOT NULL,
[CostCenter] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IrregularIndicator] [int] NOT NULL,
[Cn22Number] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentChargeType] [int] NOT NULL,
[ShipmentChargeAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentChargePostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentChargeCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsPackageID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_UpsShipment] on [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [PK_UpsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_SWDefault_UpsShipment_Service] on [dbo].[UpsShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_UpsShipment_Service] ON [dbo].[UpsShipment] ([Service])
GO
PRINT N'Creating index [IX_SWDefault_UpsShipment_DeliveryConfirmation] on [dbo].[UpsShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_UpsShipment_DeliveryConfirmation] ON [dbo].[UpsShipment] ([DeliveryConfirmation])
GO
PRINT N'Creating [dbo].[UpsPackage]'
GO
CREATE TABLE [dbo].[UpsPackage]
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
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AdditionalHandlingEnabled] [bit] NOT NULL,
[VerbalConfirmationEnabled] [bit] NOT NULL,
[VerbalConfirmationName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[VerbalConfirmationPhone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[VerbalConfirmationPhoneExtension] [nvarchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DryIceEnabled] [bit] NOT NULL,
[DryIceRegulationSet] [int] NOT NULL,
[DryIceWeight] [float] NOT NULL,
[DryIceIsForMedicalUse] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_UpsPackage] on [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [PK_UpsPackage] PRIMARY KEY CLUSTERED  ([UpsPackageID])
GO
PRINT N'Creating index [IX_SWDefault_UpsPackage_ShipmentID] on [dbo].[UpsPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_UpsPackage_ShipmentID] ON [dbo].[UpsPackage] ([ShipmentID])
GO
PRINT N'Creating index [IX_SWDefault_UpsPackage_PackagingType] on [dbo].[UpsPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_UpsPackage_PackagingType] ON [dbo].[UpsPackage] ([PackagingType])
GO
PRINT N'Creating [dbo].[UpsProfile]'
GO
CREATE TABLE [dbo].[UpsProfile]
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
[EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifyFrom] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifySubject] [int] NULL,
[EmailNotifyMessage] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnService] [int] NULL,
[ReturnUndeliverableEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReturnContents] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Endorsement] [int] NULL,
[Subclassification] [int] NULL,
[PaperlessAdditionalDocumentation] [bit] NULL,
[ShipperRelease] [bit] NULL,
[CarbonNeutral] [bit] NULL,
[CommercialPaperlessInvoice] [bit] NULL,
[CostCenter] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IrregularIndicator] [int] NULL,
[Cn22Number] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentChargeType] [int] NULL,
[ShipmentChargeAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentChargePostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentChargeCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UspsPackageID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CustomsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_UpsProfile] on [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD CONSTRAINT [PK_UpsProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[UpsProfilePackage]'
GO
CREATE TABLE [dbo].[UpsProfilePackage]
(
	[PackageProfileID] [bigint] NOT NULL,
	[PackagingType] [int] NULL,
	[AdditionalHandlingEnabled] [bit] NULL,
	[VerbalConfirmationEnabled] [bit] NULL,
	[VerbalConfirmationName] [nvarchar](35) NULL,
	[VerbalConfirmationPhone] [nvarchar](15) NULL,
	[VerbalConfirmationPhoneExtension] [nvarchar](4) NULL,
	[DryIceEnabled] [bit] NULL,
	[DryIceRegulationSet] [int] NULL,
	[DryIceWeight] [float] NULL,
	[DryIceIsForMedicalUse] [bit] NULL,
)
GO
PRINT N'Creating primary key [PK_UpsProfilePackage] on [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage]ADD CONSTRAINT [PK_UpsProfilePackage_PackageProfileID] PRIMARY KEY CLUSTERED ([PackageProfileID])
GO
PRINT N'Adding foreign key to [UpsProfilePackage]'
ALTER TABLE [dbo].[UpsProfilePackage]  WITH CHECK ADD  CONSTRAINT [FK_UpsProfilePackage_PackageProfile] FOREIGN KEY([PackageProfileID])
REFERENCES [dbo].[PackageProfile] ([PackageProfileID])
ON DELETE CASCADE
GO
PRINT N'Creating [dbo].[UserColumnSettings]'
GO
CREATE TABLE [dbo].[UserColumnSettings]
(
[UserColumnSettingsID] [bigint] NOT NULL IDENTITY(1039, 1000),
[SettingsKey] [uniqueidentifier] NOT NULL,
[UserID] [bigint] NOT NULL,
[InitialSortType] [int] NOT NULL,
[GridColumnLayoutID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_UserColumnSettings] on [dbo].[UserColumnSettings]'
GO
ALTER TABLE [dbo].[UserColumnSettings] ADD CONSTRAINT [PK_UserColumnSettings] PRIMARY KEY CLUSTERED  ([UserColumnSettingsID])
GO
PRINT N'Creating index [IX_SWDefault_UserColumnSettings] on [dbo].[UserColumnSettings]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_UserColumnSettings] ON [dbo].[UserColumnSettings] ([UserID], [SettingsKey])
GO
PRINT N'Creating [dbo].[UserSettings]'
GO
CREATE TABLE [dbo].[UserSettings]
(
[UserID] [bigint] NOT NULL,
[DisplayColorScheme] [int] NOT NULL,
[DisplaySystemTray] [bit] NOT NULL,
[WindowLayout] [varbinary] (max) NOT NULL,
[GridMenuLayout] [xml] NULL,
[FilterInitialUseLastActive] [bit] NOT NULL,
[FilterInitialSpecified] [bigint] NOT NULL,
[FilterInitialSortType] [int] NOT NULL,
[OrderFilterLastActive] [bigint] NOT NULL,
[OrderFilterExpandedFolders] [xml] NULL,
[ShippingWeightFormat] [int] NOT NULL,
[TemplateExpandedFolders] [xml] NULL,
[TemplateLastSelected] [bigint] NOT NULL,
[CustomerFilterLastActive] [bigint] NOT NULL,
[CustomerFilterExpandedFolders] [xml] NULL,
[SingleScanSettings] [int] NOT NULL,
[AutoWeigh] [bit] NOT NULL,
[DialogSettings] [xml] NULL,
[UIMode] [int] NOT NULL,
[OrderLookupLayout] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS
)
GO
PRINT N'Creating primary key [PK_UserSetting_1] on [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] ADD CONSTRAINT [PK_UserSetting_1] PRIMARY KEY CLUSTERED  ([UserID])
GO
PRINT N'Creating [dbo].[VersionSignoff]'
GO
CREATE TABLE [dbo].[VersionSignoff]
(
[VersionSignoffID] [bigint] NOT NULL IDENTITY(1038, 1000),
[Version] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UserID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_VersionVerification] on [dbo].[VersionSignoff]'
GO
ALTER TABLE [dbo].[VersionSignoff] ADD CONSTRAINT [PK_VersionVerification] PRIMARY KEY CLUSTERED  ([VersionSignoffID])
GO
PRINT N'Creating index [IX_SWDefault_VersionSignoff] on [dbo].[VersionSignoff]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_VersionSignoff] ON [dbo].[VersionSignoff] ([ComputerID], [UserID])
GO
PRINT N'Creating [dbo].[VolusionStore]'
GO
CREATE TABLE [dbo].[VolusionStore]
(
[StoreID] [bigint] NOT NULL,
[StoreUrl] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WebUserName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WebPassword] [varchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiPassword] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PaymentMethods] [xml] NOT NULL,
[ShipmentMethods] [xml] NOT NULL,
[DownloadOrderStatuses] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServerTimeZone] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServerTimeZoneDST] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_VolusionStore] on [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] ADD CONSTRAINT [PK_VolusionStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[WorldShipShipment]'
GO
CREATE TABLE [dbo].[WorldShipShipment]
(
[ShipmentID] [bigint] NOT NULL,
[OrderNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromCompanyOrName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromAttention] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromAddress1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromAddress2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromAddress3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromCountryCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromPostalCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromStateProvCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromTelephone] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FromAccountNumber] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToCustomerID] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToCompanyOrName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToAttention] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToAddress1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToAddress2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToAddress3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToCountryCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToPostalCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToStateProvCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToTelephone] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToAccountNumber] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToResidential] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServiceType] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillTransportationTo] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SaturdayDelivery] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[QvnOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[QvnFrom] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnSubjectLine] [nvarchar] (18) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnMemo] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn1ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1DeliveryNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1ExceptionNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2DeliveryNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2ExceptionNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3DeliveryNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3ExceptionNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDescriptionOfGoods] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CustomsDocumentsOnly] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipperNumber] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PackageCount] [int] NOT NULL,
[DeliveryConfirmation] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DeliveryConfirmationAdult] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceTermsOfSale] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceReasonForExport] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceCurrencyCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceChargesFreight] [money] NULL,
[InvoiceChargesInsurance] [money] NULL,
[InvoiceChargesOther] [money] NULL,
[ShipmentProcessedOnComputerID] [bigint] NULL,
[UspsEndorsement] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CarbonNeutral] [char] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_WorldShipShipment] on [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ADD CONSTRAINT [PK_WorldShipShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[WorldShipGoods]'
GO
CREATE TABLE [dbo].[WorldShipGoods]
(
[WorldShipGoodsID] [bigint] NOT NULL IDENTITY(1098, 1000),
[ShipmentID] [bigint] NOT NULL,
[ShipmentCustomsItemID] [bigint] NOT NULL,
[Description] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TariffCode] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryOfOrigin] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Units] [int] NOT NULL,
[UnitOfMeasure] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UnitPrice] [money] NOT NULL,
[Weight] [float] NOT NULL,
[InvoiceCurrencyCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_WorldShipGoods] on [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD CONSTRAINT [PK_WorldShipGoods] PRIMARY KEY CLUSTERED  ([WorldShipGoodsID])
GO
PRINT N'Creating [dbo].[YahooOrder]'
GO
CREATE TABLE [dbo].[YahooOrder]
(
[OrderID] [bigint] NOT NULL,
[YahooOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_YahooOrder] on [dbo].[YahooOrder]'
GO
ALTER TABLE [dbo].[YahooOrder] ADD CONSTRAINT [PK_YahooOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[YahooOrderItem]'
GO
CREATE TABLE [dbo].[YahooOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[YahooProductID] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Url] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL Default ''
)
GO
PRINT N'Creating primary key [PK_YahooOrderItem] on [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD CONSTRAINT [PK_YahooOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[YahooStore]'
GO
CREATE TABLE [dbo].[YahooStore]
(
[StoreID] [bigint] NOT NULL,
[YahooEmailAccountID] [bigint] NOT NULL,
[TrackingUpdatePassword] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[YahooStoreID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
[AccessToken] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT '',
[BackupOrderNumber] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_YahooStore] on [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD CONSTRAINT [PK_YahooStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[YahooProduct]'
GO
CREATE TABLE [dbo].[YahooProduct]
(
[StoreID] [bigint] NOT NULL,
[YahooProductID] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Weight] [float] NOT NULL
)
GO
PRINT N'Creating primary key [PK_YahooProduct_1] on [dbo].[YahooProduct]'
GO
ALTER TABLE [dbo].[YahooProduct] ADD CONSTRAINT [PK_YahooProduct_1] PRIMARY KEY CLUSTERED  ([StoreID], [YahooProductID])
GO
PRINT N'Creating [dbo].[ActionQueue]'
GO
CREATE TABLE [dbo].[ActionQueue]
(
[ActionQueueID] [bigint] NOT NULL IDENTITY(1041, 1000),
[RowVersion] [timestamp] NOT NULL,
[ActionID] [bigint] NOT NULL,
[ActionName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ActionQueueType] [int] NOT NULL CONSTRAINT [DF_ActionQueue_ActionQueueType] DEFAULT ((0)),
[ActionVersion] [binary] (8) NOT NULL CONSTRAINT [DF_ActionQueue_ActionVersion] DEFAULT ((0)),
[QueueVersion] [binary] (8) NOT NULL CONSTRAINT [DF_ActionQueue_QueueVersion] DEFAULT (@@dbts),
[TriggerDate] [datetime] NOT NULL CONSTRAINT [DF_ActionQueue_QueuedDate] DEFAULT (getutcdate()),
[TriggerComputerID] [bigint] NOT NULL,
[ComputerLimitedList] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ObjectID] [bigint] NULL,
[Status] [int] NOT NULL,
[NextStep] [int] NOT NULL,
[ContextLock] [nvarchar] (36) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExtraData] [xml] NULL
)
GO
PRINT N'Creating primary key [PK_ActionQueue] on [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] ADD CONSTRAINT [PK_ActionQueue] PRIMARY KEY CLUSTERED  ([ActionQueueID])
GO
PRINT N'Creating index [IX_SWDefault_ActionQueue_Search] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ActionQueue_Search] ON [dbo].[ActionQueue] ([Status], [ActionQueueType], [ActionQueueID])
GO
PRINT N'Creating index [IX_SWDefault_ActionQueue_ActionQueueType] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ActionQueue_ActionQueueType] ON [dbo].[ActionQueue] ([ActionQueueType] DESC) INCLUDE ([ComputerLimitedList], [Status])
GO
PRINT N'Creating index [IX_SWDefault_ActionQueue_ContextLock] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ActionQueue_ContextLock] ON [dbo].[ActionQueue] ([ContextLock]) INCLUDE ([Status])
GO
ALTER TABLE [dbo].[ActionQueue] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[ActionQueue]'
GO
PRINT N'Creating [dbo].[Configuration]'
GO
CREATE TABLE [dbo].[Configuration]
(
[ConfigurationID] [bit] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[LogOnMethod] [int] NOT NULL,
[AddressCasing] [bit] NOT NULL,
[CustomerCompareEmail] [bit] NOT NULL,
[CustomerCompareAddress] [bit] NOT NULL,
[CustomerUpdateBilling] [bit] NOT NULL,
[CustomerUpdateShipping] [bit] NOT NULL,
[CustomerUpdateModifiedBilling] [int] NOT NULL,
[CustomerUpdateModifiedShipping] [int] NOT NULL,
[AuditNewOrders] [bit] NOT NULL,
[AuditDeletedOrders] [bit] NOT NULL,
[CustomerKey] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UseParallelActionQueue] [bit] NOT NULL CONSTRAINT [DF_Configuration_UseParallelActionQueue] DEFAULT ((1)),
[AllowEbayCombineLocally] [bit] NOT NULL CONSTRAINT [DF_Configuration_AllowEbayCombineLocally] DEFAULT ((0)),
[ArchivalSettingsXml] [xml] NOT NULL CONSTRAINT [DF_Configuration_ArchivalSettingsXml] DEFAULT ('<ArchivalSettings/>'),
[AuditEnabled] [bit] NOT NULL CONSTRAINT [DF_Configuration_AuditEnabled] DEFAULT ((1)),
[AllowUIModeToggle] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Configuration] on [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] ADD CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED  ([ConfigurationID])
GO
PRINT N'Creating [dbo].[DimensionsProfile]'
GO
CREATE TABLE [dbo].[DimensionsProfile]
(
[DimensionsProfileID] [bigint] NOT NULL IDENTITY(1049, 1000),
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Length] [float] NOT NULL,
[Width] [float] NOT NULL,
[Height] [float] NOT NULL,
[Weight] [float] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PackagingProfile] on [dbo].[DimensionsProfile]'
GO
ALTER TABLE [dbo].[DimensionsProfile] ADD CONSTRAINT [PK_PackagingProfile] PRIMARY KEY CLUSTERED  ([DimensionsProfileID])
GO
PRINT N'Creating index [IX_SWDefault_DimensionsProfile_Name] on [dbo].[DimensionsProfile]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_DimensionsProfile_Name] ON [dbo].[DimensionsProfile] ([Name])
GO
ALTER TABLE [dbo].[DimensionsProfile] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[Dirty]'
GO
CREATE TABLE [dbo].[Dirty]
(
[RowVersion] [timestamp] NOT NULL,
[Count] [bigint] NULL
)
GO
PRINT N'Creating [dbo].[EmailAccount]'
GO
CREATE TABLE [dbo].[EmailAccount]
(
[EmailAccountID] [bigint] NOT NULL IDENTITY(1034, 1000),
[RowVersion] [timestamp] NOT NULL,
[AccountName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DisplayName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingServer] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingServerType] [int] NOT NULL,
[IncomingPort] [int] NOT NULL,
[IncomingSecurityType] [int] NOT NULL,
[IncomingUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingPassword] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingServer] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingPort] [int] NOT NULL,
[OutgoingSecurityType] [int] NOT NULL,
[OutgoingCredentialSource] [int] NOT NULL,
[OutgoingUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingPassword] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AutoSend] [bit] NOT NULL,
[AutoSendMinutes] [int] NOT NULL,
[AutoSendLastTime] [datetime] NOT NULL,
[LimitMessagesPerConnection] [bit] NOT NULL,
[LimitMessagesPerConnectionQuantity] [int] NOT NULL,
[LimitMessagesPerHour] [bit] NOT NULL,
[LimitMessagesPerHourQuantity] [int] NOT NULL,
[LimitMessageInterval] [bit] NOT NULL,
[LimitMessageIntervalSeconds] [int] NOT NULL,
[InternalOwnerID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_EmailAccount] on [dbo].[EmailAccount]'
GO
ALTER TABLE [dbo].[EmailAccount] ADD CONSTRAINT [PK_EmailAccount] PRIMARY KEY CLUSTERED  ([EmailAccountID])
GO
ALTER TABLE [dbo].[EmailAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[EndiciaAccount]'
GO
CREATE TABLE [dbo].[EndiciaAccount]
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
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ScanFormAddressSource] [int] NOT NULL,
[AcceptedFCMILetterWarning] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EndiciaAccount] on [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] ADD CONSTRAINT [PK_EndiciaAccount] PRIMARY KEY CLUSTERED  ([EndiciaAccountID])
GO
ALTER TABLE [dbo].[EndiciaAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[FedExAccount]'
GO
CREATE TABLE [dbo].[FedExAccount]
(
[FedExAccountID] [bigint] NOT NULL IDENTITY(1055, 1000),
[RowVersion] [timestamp] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccountNumber] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SignatureRelease] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MeterNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostHubList] [xml] NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_FedExAccount] on [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] ADD CONSTRAINT [PK_FedExAccount] PRIMARY KEY CLUSTERED  ([FedExAccountID])
GO
ALTER TABLE [dbo].[FedExAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[FedExEndOfDayClose]'
GO
CREATE TABLE [dbo].[FedExEndOfDayClose]
(
[FedExEndOfDayCloseID] [bigint] NOT NULL IDENTITY(1065, 1000),
[FedExAccountID] [bigint] NOT NULL,
[AccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CloseDate] [datetime] NOT NULL,
[IsSmartPost] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FedExEndOfDayClose] on [dbo].[FedExEndOfDayClose]'
GO
ALTER TABLE [dbo].[FedExEndOfDayClose] ADD CONSTRAINT [PK_FedExEndOfDayClose] PRIMARY KEY CLUSTERED  ([FedExEndOfDayCloseID])
GO
PRINT N'Creating index [IX_SWDefault_FedExEndOfDayClose_CloseDate] on [dbo].[FedExEndOfDayClose]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FedExEndOfDayClose_CloseDate] ON [dbo].[FedExEndOfDayClose] ([CloseDate]) INCLUDE ([FedExAccountID])
GO
PRINT N'Creating [dbo].[FilterNodeContentDirty]'
GO
CREATE TABLE [dbo].[FilterNodeContentDirty]
(
[FilterNodeContentDirtyID] [bigint] NOT NULL IDENTITY(1, 1),
[RowVersion] [timestamp] NOT NULL,
[ObjectID] [bigint] NOT NULL,
[ParentID] [bigint] NULL,
[ObjectType] [int] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeContentDirty] on [dbo].[FilterNodeContentDirty]'
GO
ALTER TABLE [dbo].[FilterNodeContentDirty] ADD CONSTRAINT [PK_FilterNodeContentDirty] PRIMARY KEY CLUSTERED  ([FilterNodeContentDirtyID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_FilterNodeContentDirty_FilterNodeContentDirtyID] ON [dbo].[FilterNodeContentDirty] ([ObjectID], [ComputerID], [ColumnsUpdated]) INCLUDE ([ObjectType], [ParentID])
GO
PRINT N'Creating [dbo].[FilterNodeRootDirty]'
GO
CREATE TABLE [dbo].[FilterNodeRootDirty]
(
[FilterNodeContentID] [bigint] NOT NULL,
[Change] [int] NOT NULL
)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateCheckpoint]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateCheckpoint]
(
[CheckpointID] [bigint] NOT NULL IDENTITY(1070, 1000),
[MaxDirtyID] [bigint] NOT NULL,
[DirtyCount] [int] NOT NULL,
[State] [int] NOT NULL,
[Duration] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateCheckpoint] on [dbo].[FilterNodeUpdateCheckpoint]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCheckpoint] ADD CONSTRAINT [PK_FilterNodeUpdateCheckpoint] PRIMARY KEY CLUSTERED  ([CheckpointID])
GO
PRINT N'Creating [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateCustomer]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateCustomer] on [dbo].[FilterNodeUpdateCustomer]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateCustomer] ADD CONSTRAINT [PK_FilterNodeUpdateCustomer] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateItem]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateItem]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateItem] on [dbo].[FilterNodeUpdateItem]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateItem] ADD CONSTRAINT [PK_FilterNodeUpdateItem] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateOrder]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateOrder]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateOrder] on [dbo].[FilterNodeUpdateOrder]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateOrder] ADD CONSTRAINT [PK_FilterNodeUpdateOrder] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdatePending]'
GO
CREATE TABLE [dbo].[FilterNodeUpdatePending]
(
[FilterNodeContentID] [bigint] NOT NULL,
[FilterTarget] [int] NOT NULL,
[ColumnMask] [varbinary] (100) NOT NULL,
[JoinMask] [int] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateShipment]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateShipment]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[ColumnsUpdated] [varbinary] (100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeUpdateShipment] on [dbo].[FilterNodeUpdateShipment]'
GO
ALTER TABLE [dbo].[FilterNodeUpdateShipment] ADD CONSTRAINT [PK_FilterNodeUpdateShipment] PRIMARY KEY CLUSTERED  ([ObjectID], [ColumnsUpdated]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FtpAccount]'
GO
CREATE TABLE [dbo].[FtpAccount]
(
[FtpAccountID] [bigint] NOT NULL IDENTITY(1071, 1000),
[Host] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Port] [int] NOT NULL,
[SecurityType] [int] NOT NULL,
[Passive] [bit] NOT NULL,
[InternalOwnerID] [bigint] NULL,
[ReuseControlConnectionSession] [bit] NOT NULL DEFAULT ((0))
)
GO
PRINT N'Creating primary key [PK_FtpAccount] on [dbo].[FtpAccount]'
GO
ALTER TABLE [dbo].[FtpAccount] ADD CONSTRAINT [PK_FtpAccount] PRIMARY KEY CLUSTERED  ([FtpAccountID])
GO
ALTER TABLE [dbo].[FtpAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[iParcelAccount]'
GO
CREATE TABLE [dbo].[iParcelAccount]
(
[iParcelAccountID] [bigint] NOT NULL IDENTITY(1091, 1000),
[RowVersion] [timestamp] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_iParcelAccount] on [dbo].[iParcelAccount]'
GO
ALTER TABLE [dbo].[iParcelAccount] ADD CONSTRAINT [PK_iParcelAccount] PRIMARY KEY CLUSTERED  ([iParcelAccountID])
GO
ALTER TABLE [dbo].[iParcelAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[LabelSheet]'
GO
CREATE TABLE [dbo].[LabelSheet]
(
[LabelSheetID] [bigint] NOT NULL IDENTITY(1027, 1000),
[RowVersion] [timestamp] NOT NULL,
[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PaperSizeHeight] [float] NOT NULL,
[PaperSizeWidth] [float] NOT NULL,
[MarginTop] [float] NOT NULL,
[MarginLeft] [float] NOT NULL,
[LabelHeight] [float] NOT NULL,
[LabelWidth] [float] NOT NULL,
[VerticalSpacing] [float] NOT NULL,
[HorizontalSpacing] [float] NOT NULL,
[Rows] [int] NOT NULL,
[Columns] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_LabelSheet] on [dbo].[LabelSheet]'
GO
ALTER TABLE [dbo].[LabelSheet] ADD CONSTRAINT [PK_LabelSheet] PRIMARY KEY CLUSTERED  ([LabelSheetID])
GO
PRINT N'Creating index [IX_SWDefault_LabelSheet_Name] on [dbo].[LabelSheet]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_LabelSheet_Name] ON [dbo].[LabelSheet] ([Name])
GO
ALTER TABLE [dbo].[LabelSheet] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[ObjectLabel]'
GO
CREATE TABLE [dbo].[ObjectLabel]
(
[ObjectID] [bigint] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[ObjectType] [int] NOT NULL,
[ParentID] [bigint] NULL,
[Label] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsDeleted] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ObjectLabel] on [dbo].[ObjectLabel]'
GO
ALTER TABLE [dbo].[ObjectLabel] ADD CONSTRAINT [PK_ObjectLabel] PRIMARY KEY CLUSTERED  ([ObjectID])
GO
PRINT N'Creating index [IX_SWDefault_ObjectLabel_ObjectTypeIsDeleted] on [dbo].[ObjectLabel]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ObjectLabel_ObjectTypeIsDeleted] ON [dbo].[ObjectLabel] ([ObjectType], [IsDeleted])
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
PRINT N'Creating [dbo].[Resource]'
GO
CREATE TABLE [dbo].[Resource]
(
[ResourceID] [bigint] NOT NULL IDENTITY(1026, 1000),
[Data] [varbinary] (max) NOT NULL,
[Checksum] [binary] (32) NOT NULL,
[Compressed] [bit] NOT NULL,
[Filename] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_Resource] on [dbo].[Resource]'
GO
ALTER TABLE [dbo].[Resource] ADD CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED  ([ResourceID])
GO
PRINT N'Creating index [IX_SWDefault_Resource_Checksum] on [dbo].[Resource]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_Resource_Checksum] ON [dbo].[Resource] ([Checksum])
GO
PRINT N'Creating index [IX_SWDefault_Resource_Filename] on [dbo].[Resource]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_Resource_Filename] ON [dbo].[Resource] ([Filename])
GO
PRINT N'Creating [dbo].[Scheduling_BLOB_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_BLOB_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BLOB_DATA] [image] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_BLOB_TRIGGERS] on [dbo].[Scheduling_BLOB_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_BLOB_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_BLOB_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_CALENDARS]'
GO
CREATE TABLE [dbo].[Scheduling_CALENDARS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CALENDAR_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CALENDAR] [image] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_CALENDARS] on [dbo].[Scheduling_CALENDARS]'
GO
ALTER TABLE [dbo].[Scheduling_CALENDARS] ADD CONSTRAINT [PK_Scheduling_CALENDARS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [CALENDAR_NAME])
GO
PRINT N'Creating [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_FIRED_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ENTRY_ID] [nvarchar] (95) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[INSTANCE_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FIRED_TIME] [bigint] NOT NULL,
[SCHED_TIME] [bigint] NOT NULL,
[PRIORITY] [int] NOT NULL,
[STATE] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[JOB_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IS_NONCONCURRENT] [bit] NULL,
[REQUESTS_RECOVERY] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_FIRED_TRIGGERS] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_FIRED_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_FIRED_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [ENTRY_ID])
GO
PRINT N'Creating index [IDX_Scheduling_FT_TRIG_INST_NAME] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_TRIG_INST_NAME] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [INSTANCE_NAME])
GO
PRINT N'Creating index [IDX_Scheduling_FT_INST_JOB_REQ_RCVRY] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_INST_JOB_REQ_RCVRY] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [INSTANCE_NAME], [REQUESTS_RECOVERY])
GO
PRINT N'Creating index [IDX_Scheduling_FT_JG] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_JG] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_FT_J_G] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_J_G] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_FT_TG] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_TG] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_FT_T_G] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_T_G] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_LOCKS]'
GO
CREATE TABLE [dbo].[Scheduling_LOCKS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LOCK_NAME] [nvarchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_LOCKS] on [dbo].[Scheduling_LOCKS]'
GO
ALTER TABLE [dbo].[Scheduling_LOCKS] ADD CONSTRAINT [PK_Scheduling_LOCKS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [LOCK_NAME])
GO
PRINT N'Creating [dbo].[Scheduling_PAUSED_TRIGGER_GRPS]'
GO
CREATE TABLE [dbo].[Scheduling_PAUSED_TRIGGER_GRPS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_PAUSED_TRIGGER_GRPS] on [dbo].[Scheduling_PAUSED_TRIGGER_GRPS]'
GO
ALTER TABLE [dbo].[Scheduling_PAUSED_TRIGGER_GRPS] ADD CONSTRAINT [PK_Scheduling_PAUSED_TRIGGER_GRPS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_SCHEDULER_STATE]'
GO
CREATE TABLE [dbo].[Scheduling_SCHEDULER_STATE]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[INSTANCE_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LAST_CHECKIN_TIME] [bigint] NOT NULL,
[CHECKIN_INTERVAL] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_SCHEDULER_STATE] on [dbo].[Scheduling_SCHEDULER_STATE]'
GO
ALTER TABLE [dbo].[Scheduling_SCHEDULER_STATE] ADD CONSTRAINT [PK_Scheduling_SCHEDULER_STATE] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [INSTANCE_NAME])
GO
PRINT N'Creating [dbo].[Search]'
GO
CREATE TABLE [dbo].[Search]
(
[SearchID] [bigint] NOT NULL IDENTITY(1069, 1000),
[Started] [datetime] NOT NULL,
[Pinged] [datetime] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[UserID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Search] on [dbo].[Search]'
GO
ALTER TABLE [dbo].[Search] ADD CONSTRAINT [PK_Search] PRIMARY KEY CLUSTERED  ([SearchID])
GO
PRINT N'Creating [dbo].[ShippingDefaultsRule]'
GO
CREATE TABLE [dbo].[ShippingDefaultsRule]
(
[ShippingDefaultsRuleID] [bigint] NOT NULL IDENTITY(1057, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentType] [int] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[ShippingProfileID] [bigint] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShippingDefaultsRule] on [dbo].[ShippingDefaultsRule]'
GO
ALTER TABLE [dbo].[ShippingDefaultsRule] ADD CONSTRAINT [PK_ShippingDefaultsRule] PRIMARY KEY CLUSTERED  ([ShippingDefaultsRuleID])
GO
ALTER TABLE [dbo].[ShippingDefaultsRule] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[ShippingOrigin]'
GO
CREATE TABLE [dbo].[ShippingOrigin]
(
[ShippingOriginID] [bigint] NOT NULL IDENTITY(1050, 1000),
[RowVersion] [timestamp] NOT NULL,
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
[Fax] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShippingOrigin] on [dbo].[ShippingOrigin]'
GO
ALTER TABLE [dbo].[ShippingOrigin] ADD CONSTRAINT [PK_ShippingOrigin] PRIMARY KEY CLUSTERED  ([ShippingOriginID])
GO
PRINT N'Creating index [IX_SWDefault_ShippingOrigin_Description] on [dbo].[ShippingOrigin]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ShippingOrigin_Description] ON [dbo].[ShippingOrigin] ([Description])
GO
ALTER TABLE [dbo].[ShippingOrigin] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[ShippingProviderRule]'
GO
CREATE TABLE [dbo].[ShippingProviderRule]
(
[ShippingProviderRuleID] [bigint] NOT NULL IDENTITY(1060, 1000),
[RowVersion] [timestamp] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[ShipmentType] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShippingProviderRule] on [dbo].[ShippingProviderRule]'
GO
ALTER TABLE [dbo].[ShippingProviderRule] ADD CONSTRAINT [PK_ShippingProviderRule] PRIMARY KEY CLUSTERED  ([ShippingProviderRuleID])
GO
ALTER TABLE [dbo].[ShippingProviderRule] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[ShippingSettings]'
GO

PRINT N'Creating [dbo].[ShippingSettings]'
GO
CREATE TABLE [dbo].[ShippingSettings]
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
[OrderLookupFieldLayout] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_OrderLookupFieldLayout] DEFAULT ('')
)
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Creating [dbo].[ShipSenseKnowledgeBase]'
GO
CREATE TABLE [dbo].[ShipSenseKnowledgeBase]
(
[Hash] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
[Entry] [varbinary] (max) NOT NULL
)
GO
PRINT N'Creating [dbo].[ExcludedServiceType]'
GO
CREATE TABLE [dbo].[ExcludedServiceType](
    [ShipmentType] [int] NOT NULL,
    [ServiceType] [int] NOT NULL,
 CONSTRAINT [PK_ExcludedServiceType] PRIMARY KEY CLUSTERED
(
    [ShipmentType] ASC,
    [ServiceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT N'Creating [dbo].[ExcludedPackageType]'
GO
CREATE TABLE [dbo].[ExcludedPackageType](
    [ShipmentType] [int] NOT NULL,
    [PackageType] [int] NOT NULL,
 CONSTRAINT [PK_ExcludedPackageType] PRIMARY KEY CLUSTERED
(
    [ShipmentType] ASC,
    [PackageType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT N'Creating primary key [PK_ShipSenseKnowledgeBase] on [dbo].[ShipSenseKnowledgeBase]'
GO
ALTER TABLE [dbo].[ShipSenseKnowledgeBase] ADD CONSTRAINT [PK_ShipSenseKnowledgeBase] PRIMARY KEY CLUSTERED  ([Hash])
GO
PRINT N'Creating [dbo].[UspsAccount]'
GO
CREATE TABLE [dbo].[UspsAccount]
(
[UspsAccountID] [bigint] NOT NULL IDENTITY(1052, 1000),
[RowVersion] [timestamp] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsReseller] [int] NOT NULL,
[ContractType] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[PendingInitialAccount] [int] NOT NULL,
[GlobalPostAvailability] [int] NOT NULL,
[AcceptedFCMILetterWarning] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PostalUspsAccount] on [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] ADD CONSTRAINT [PK_PostalUspsAccount] PRIMARY KEY CLUSTERED  ([UspsAccountID])
GO
ALTER TABLE [dbo].[UspsAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[DhlExpressAccount]'
GO
CREATE TABLE [dbo].[DhlExpressAccount]
(
[DhlExpressAccountID] [bigint] NOT NULL IDENTITY(1102, 1000),
[RowVersion] [timestamp] NOT NULL,
[AccountNumber] [bigint] NOT NULL,
[ShipEngineCarrierId] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
PRINT N'Creating primary key [PK_DhlExpressAccount] on [dbo].[DhlExpressAccount]'
GO
ALTER TABLE [dbo].[DhlExpressAccount] ADD CONSTRAINT [PK_DhlExpressAccount] PRIMARY KEY CLUSTERED  ([DhlExpressAccountID])
GO
ALTER TABLE [dbo].[DhlExpressAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[AsendiaAccount]'
GO
CREATE TABLE [dbo].[AsendiaAccount]
(
[AsendiaAccountID] [bigint] NOT NULL IDENTITY(1103, 1000),
[RowVersion] [timestamp] NOT NULL,
[AccountNumber] [bigint] NOT NULL,
[ShipEngineCarrierId] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
PRINT N'Creating primary key [PK_AsendiaAccount] on [dbo].[AsendiaAccount]'
GO
ALTER TABLE [dbo].[AsendiaAccount] ADD CONSTRAINT [PK_AsendiaAccount] PRIMARY KEY CLUSTERED  ([AsendiaAccountID])
GO
ALTER TABLE [dbo].[AsendiaAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[SystemData]'
GO
CREATE TABLE [dbo].[SystemData]
(
[SystemDataID] [bit] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[DatabaseID] [uniqueidentifier] NOT NULL,
[DateFiltersLastUpdate] [datetime] NOT NULL,
[TemplateVersion] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_SystemData] on [dbo].[SystemData]'
GO
ALTER TABLE [dbo].[SystemData] ADD CONSTRAINT [PK_SystemData] PRIMARY KEY CLUSTERED  ([SystemDataID])
GO
PRINT N'Creating [dbo].[UpsAccount]'
GO
CREATE TABLE [dbo].[UpsAccount]
(
[UpsAccountID] [bigint] NOT NULL IDENTITY(1056, 1000),
[RowVersion] [timestamp] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccountNumber] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UserID] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RateType] [int] NOT NULL,
[InvoiceAuth] [bit] NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PromoStatus] [tinyint] NOT NULL,
[LocalRatingEnabled] [bit] NOT NULL,
[UpsRateTableID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_UpsAccount] on [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] ADD CONSTRAINT [PK_UpsAccount] PRIMARY KEY CLUSTERED  ([UpsAccountID])
GO
ALTER TABLE [dbo].[UpsAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[WorldShipProcessed]'
GO
CREATE TABLE [dbo].[WorldShipProcessed]
(
[WorldShipProcessedID] [bigint] NOT NULL IDENTITY(1068, 1000),
[ShipmentID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RowVersion] [timestamp] NOT NULL,
[PublishedCharges] [float] NOT NULL,
[NegotiatedCharges] [float] NOT NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ServiceType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PackageType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsPackageID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DeclaredValueAmount] [float] NULL,
[DeclaredValueOption] [nchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorldShipShipmentID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VoidIndicator] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NumberOfPackages] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LeadTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShipmentIdCalculated] AS (case when isnumeric([ShipmentID]+'.e0')=(1) then CONVERT([bigint],[ShipmentID],(0))  end) PERSISTED
)
GO
PRINT N'Creating primary key [PK_WorldShipProcessed] on [dbo].[WorldShipProcessed]'
GO
ALTER TABLE [dbo].[WorldShipProcessed] ADD CONSTRAINT [PK_WorldShipProcessed] PRIMARY KEY CLUSTERED  ([WorldShipProcessedID])
GO

GO
PRINT N'Creating [dbo].[ValidatedAddress]'
GO
CREATE TABLE [dbo].[ValidatedAddress](
    [ValidatedAddressID] [bigint] IDENTITY(1100,1000) NOT NULL,
    [ConsumerID] [bigint] NOT NULL,
    [AddressPrefix] [nvarchar](10) NOT NULL,
    [IsOriginal] [bit] NOT NULL,
    [Street1] [nvarchar](60) NOT NULL,
    [Street2] [nvarchar](60) NOT NULL,
    [Street3] [nvarchar](60) NOT NULL,
    [City] [nvarchar](50) NOT NULL,
    [StateProvCode] [nvarchar](50) NOT NULL,
    [PostalCode] [nvarchar](20) NOT NULL,
    [CountryCode] [nvarchar](50) NOT NULL,
    [ResidentialStatus] [int] NOT NULL,
    [POBox] [int] NOT NULL,
    [USTerritory] [int] NOT NULL,
    [MilitaryAddress] [int] NOT NULL,
 CONSTRAINT [PK_ValidatedAddress] PRIMARY KEY CLUSTERED
(
    [ValidatedAddressID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Print N'Creating [IX_SWDefault_ValidatedAddress_ConsumerIDAddressPrefix]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ValidatedAddress_ConsumerIDAddressPrefix]
    ON [dbo].[ValidatedAddress]([ConsumerID] ASC, [AddressPrefix] ASC);
GO

PRINT N'Creating [dbo].[ShipmentReturnItem]'
GO
CREATE TABLE [dbo].[ShipmentReturnItem]
(
[ShipmentReturnItemID] [bigint] NOT NULL IDENTITY(1101, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentID] [bigint] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Quantity] [float] NOT NULL,
[Weight] [float] NOT NULL,
[Notes] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShipmentReturnItem] on [dbo].[ShipmentReturnItem]'
GO
ALTER TABLE [dbo].[ShipmentReturnItem] ADD CONSTRAINT [PK_ShipmentReturnItem] PRIMARY KEY CLUSTERED  ([ShipmentReturnItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ShipmentReturnItem]'
GO
ALTER TABLE [dbo].[ShipmentReturnItem] ADD CONSTRAINT [FK_ShipmentReturnItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO

PRINT N'Altering [dbo].[DimensionsProfile]'
GO
PRINT N'Altering [dbo].[EmailAccount]'
GO
PRINT N'Altering [dbo].[EndiciaAccount]'
GO
PRINT N'Altering [dbo].[FedExAccount]'
GO
PRINT N'Altering [dbo].[FtpAccount]'
GO
PRINT N'Altering [dbo].[iParcelAccount]'
GO
PRINT N'Altering [dbo].[LabelSheet]'
GO
PRINT N'Altering [dbo].[OnTracAccount]'
GO
PRINT N'Altering [dbo].[ShippingDefaultsRule]'
GO
PRINT N'Altering [dbo].[ShippingOrigin]'
GO
PRINT N'Altering [dbo].[ShippingProviderRule]'
GO
PRINT N'Altering [dbo].[UspsAccount]'
GO
PRINT N'Altering [dbo].[UpsAccount]'
GO
PRINT N'Adding constraints to [dbo].[Computer]'
GO
ALTER TABLE [dbo].[Computer] ADD CONSTRAINT [IX_SWDefault_Computer_Identifier] UNIQUE NONCLUSTERED  ([Identifier])
GO
PRINT N'Adding constraints to [dbo].[ServiceStatus]'
GO
ALTER TABLE [dbo].[ServiceStatus] ADD CONSTRAINT [IX_SWDefault_ServiceStatus] UNIQUE NONCLUSTERED  ([ComputerID], [ServiceType])
GO
PRINT N'Adding foreign keys to [dbo].[ActionFilterTrigger]'
GO
ALTER TABLE [dbo].[ActionFilterTrigger] ADD CONSTRAINT [FK_ActionFilterTrigger_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] ADD CONSTRAINT [FK_ActionQueue_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID]) ON DELETE CASCADE
ALTER TABLE [dbo].[ActionQueue] ADD CONSTRAINT [FK_ActionQueue_Computer] FOREIGN KEY ([TriggerComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionTask]'
GO
ALTER TABLE [dbo].[ActionTask] ADD CONSTRAINT [FK_ActionTask_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueueSelection]'
GO
ALTER TABLE [dbo].[ActionQueueSelection] ADD CONSTRAINT [FK_ActionQueueSelection_ActionQueue] FOREIGN KEY ([ActionQueueID]) REFERENCES [dbo].[ActionQueue] ([ActionQueueID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueueStep]'
GO
ALTER TABLE [dbo].[ActionQueueStep] ADD CONSTRAINT [FK_ActionQueueStep_ActionQueue] FOREIGN KEY ([ActionQueueID]) REFERENCES [dbo].[ActionQueue] ([ActionQueueID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonASIN]'
GO
ALTER TABLE [dbo].[AmazonASIN] ADD CONSTRAINT [FK_AmazonASIN_AmazonStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[AmazonStore] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD CONSTRAINT [FK_AmazonOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] ADD CONSTRAINT [FK_AmazonOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD CONSTRAINT [FK_AmazonStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[AmeriCommerceStore]'
GO
ALTER TABLE [dbo].[AmeriCommerceStore] ADD CONSTRAINT [FK_AmeriCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[AuditChange]'
GO
ALTER TABLE [dbo].[AuditChange] ADD CONSTRAINT [FK_AuditChange_Audit] FOREIGN KEY ([AuditID]) REFERENCES [dbo].[Audit] ([AuditID])
GO
PRINT N'Adding foreign keys to [dbo].[Audit]'
GO
ALTER TABLE [dbo].[Audit] ADD CONSTRAINT [FK_Audit_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Audit] ADD CONSTRAINT [FK_Audit_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[AuditChangeDetail]'
GO
ALTER TABLE [dbo].[AuditChangeDetail] ADD CONSTRAINT [FK_AuditChangeDetail_AuditChange] FOREIGN KEY ([AuditChangeID]) REFERENCES [dbo].[AuditChange] ([AuditChangeID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditChangeDetail] ADD CONSTRAINT [FK_AuditChangeDetail_Audit] FOREIGN KEY ([AuditID]) REFERENCES [dbo].[Audit] ([AuditID])
GO
PRINT N'Adding foreign keys to [dbo].[BestRateProfile]'
GO
ALTER TABLE [dbo].[BestRateProfile] ADD CONSTRAINT [FK_BestRateProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [FK_BestRateShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[BigCommerceOrderItem]'
GO
ALTER TABLE [dbo].[BigCommerceOrderItem] ADD CONSTRAINT [FK_BigCommerceOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD CONSTRAINT [FK_BigCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] ADD CONSTRAINT [FK_BuyDotComOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[BuyDotComStore]'
GO
ALTER TABLE [dbo].[BuyDotComStore] ADD CONSTRAINT [FK_BuyComStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD CONSTRAINT [FK_ChannelAdvisorStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ClickCartProOrder]'
GO
ALTER TABLE [dbo].[ClickCartProOrder] ADD CONSTRAINT [FK_ClickCartProOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] ADD CONSTRAINT [FK_CommerceInterfaceOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] ADD CONSTRAINT [FK_Download_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
ALTER TABLE [dbo].[Download] ADD CONSTRAINT [FK_Download_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
ALTER TABLE [dbo].[Download] ADD CONSTRAINT [FK_Download_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayCombinedOrderRelation]'
GO
ALTER TABLE [dbo].[EbayCombinedOrderRelation] ADD CONSTRAINT [FK_EbayCombinedOrderRelation_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID]) ON DELETE CASCADE
ALTER TABLE [dbo].[EbayCombinedOrderRelation] ADD CONSTRAINT [FK_EbayCombinedOrderRelation_EbayStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[EbayStore] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PrintResult]'
GO
ALTER TABLE [dbo].[PrintResult] ADD CONSTRAINT [FK_PrintResult_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[ServerMessageSignoff]'
GO
ALTER TABLE [dbo].[ServerMessageSignoff] ADD CONSTRAINT [FK_ServerMessageSignoff_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE
ALTER TABLE [dbo].[ServerMessageSignoff] ADD CONSTRAINT [FK_ServerMessageSignoff_DashboardMessage] FOREIGN KEY ([ServerMessageID]) REFERENCES [dbo].[ServerMessage] ([ServerMessageID])
GO
PRINT N'Adding foreign keys to [dbo].[ServiceStatus]'
GO
ALTER TABLE [dbo].[ServiceStatus] ADD CONSTRAINT [FK_ServiceStatus_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_ProcessedComputer] FOREIGN KEY ([ProcessedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_VoidedComputer] FOREIGN KEY ([VoidedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_ProcessedUser] FOREIGN KEY ([ProcessedUserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_VoidedUser] FOREIGN KEY ([VoidedUserID]) REFERENCES [dbo].[User] ([UserID])

GO
PRINT N'Adding foreign keys to [dbo].[TemplateComputerSettings]'
GO
ALTER TABLE [dbo].[TemplateComputerSettings] ADD CONSTRAINT [FK_TemplateComputerSettings_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE
ALTER TABLE [dbo].[TemplateComputerSettings] ADD CONSTRAINT [FK_TemplateComputerSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[VersionSignoff]'
GO
ALTER TABLE [dbo].[VersionSignoff] ADD CONSTRAINT [FK_VersionVerification_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[DownloadDetail]'
GO
ALTER TABLE [dbo].[DownloadDetail] ADD CONSTRAINT [FK_DownloadDetail_Download] FOREIGN KEY ([DownloadID]) REFERENCES [dbo].[Download] ([DownloadID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD CONSTRAINT [FK_EbayOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] ADD CONSTRAINT [FK_EbayOrderItem_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID])
ALTER TABLE [dbo].[EbayOrderItem] ADD CONSTRAINT [FK_EbayOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD CONSTRAINT [FK_EbayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[EmailOutboundRelation]'
GO
ALTER TABLE [dbo].[EmailOutboundRelation] ADD CONSTRAINT [FK_EmailOutboundObject_EmailOutbound] FOREIGN KEY ([EmailOutboundID]) REFERENCES [dbo].[EmailOutbound] ([EmailOutboundID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaProfile]'
GO
ALTER TABLE [dbo].[EndiciaProfile] ADD CONSTRAINT [FK_EndiciaProfile_PostalProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[PostalProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ADD CONSTRAINT [FK_EndiciaScanForm_EndiciaScanForm] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [FK_EndiciaShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [FK_EndiciaShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[EtsyOrder]'
GO
ALTER TABLE [dbo].[EtsyOrder] ADD CONSTRAINT [FK_EtsyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EtsyStore]'
GO
ALTER TABLE [dbo].[EtsyStore] ADD CONSTRAINT [FK_EtsyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD CONSTRAINT [FK_FedExProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ADD CONSTRAINT [FK_AmazonShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [FK_AmazonProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FilterSequence]'
GO
ALTER TABLE [dbo].[FilterSequence] ADD CONSTRAINT [FK_FilterSequence_Filter] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filter] ([FilterID])
ALTER TABLE [dbo].[FilterSequence] ADD CONSTRAINT [FK_FilterSequence_Folder] FOREIGN KEY ([ParentFilterID]) REFERENCES [dbo].[Filter] ([FilterID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterLayout]'
GO
ALTER TABLE [dbo].[FilterLayout] ADD CONSTRAINT [FK_FilterLayout_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[FilterLayout] ADD CONSTRAINT [FK_FilterLayout_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [FK_FilterNode_Parent] FOREIGN KEY ([ParentFilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID])
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [FK_FilterNode_FilterSequence] FOREIGN KEY ([FilterSequenceID]) REFERENCES [dbo].[FilterSequence] ([FilterSequenceID])
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [FK_FilterNode_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNodeColumnSettings]'
GO
ALTER TABLE [dbo].[FilterNodeColumnSettings] ADD CONSTRAINT [FK_FilterNodeColumnSettings_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID]) ON DELETE CASCADE
ALTER TABLE [dbo].[FilterNodeColumnSettings] ADD CONSTRAINT [FK_FilterNodeColumnSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[FilterNodeColumnSettings] ADD CONSTRAINT [FK_FilterNodeColumnSettings_GridColumnLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] ADD CONSTRAINT [FK_FilterNodeContentDetail_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD CONSTRAINT [FK_GenericFileStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD CONSTRAINT [FK_GenericModuleStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] ADD CONSTRAINT [FK_MagentoStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] ADD CONSTRAINT [FK_MivaStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GridColumnFormat]'
GO
ALTER TABLE [dbo].[GridColumnFormat] ADD CONSTRAINT [FK_GridColumnFormat_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[GridColumnPosition]'
GO
ALTER TABLE [dbo].[GridColumnPosition] ADD CONSTRAINT [FK_GridLayoutColumn_GridLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UserColumnSettings]'
GO
ALTER TABLE [dbo].[UserColumnSettings] ADD CONSTRAINT [FK_UserColumnSettings_GridColumnLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID])
ALTER TABLE [dbo].[UserColumnSettings] ADD CONSTRAINT [FK_UserColumnSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaOrderItem]'
GO
ALTER TABLE [dbo].[InfopiaOrderItem] ADD CONSTRAINT [FK_InfopiaOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[InsurancePolicy]'
GO
ALTER TABLE [dbo].[InsurancePolicy] ADD CONSTRAINT [FK_InsurancePolicy_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] ADD CONSTRAINT [FK_InfopiaStore_InfopiaStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[iParcelPackage]'
GO
ALTER TABLE [dbo].[iParcelPackage] ADD CONSTRAINT [FK_iParcelPackage_iParcelShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[iParcelShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[iParcelProfile]'
GO
ALTER TABLE [dbo].[iParcelProfile] ADD CONSTRAINT [FK_iParcelProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] ADD CONSTRAINT [FK_iParcelShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorOrder]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrder] ADD CONSTRAINT [FK_MarketworksOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorStore]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorStore] ADD CONSTRAINT [FK_MarketworksStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MivaOrderItemAttribute]'
GO
ALTER TABLE [dbo].[MivaOrderItemAttribute] ADD CONSTRAINT [FK_MivaOrderItemAttribute_OrderItemAttribute] FOREIGN KEY ([OrderItemAttributeID]) REFERENCES [dbo].[OrderItemAttribute] ([OrderItemAttributeID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsOrder]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrder] ADD CONSTRAINT [FK_NetworkSolutionsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsStore]'
GO
ALTER TABLE [dbo].[NetworkSolutionsStore] ADD CONSTRAINT [FK_NetworkSolutionsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggOrder]'
GO
ALTER TABLE [dbo].[NeweggOrder] ADD CONSTRAINT [FK_NeweggOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggOrderItem]'
GO
ALTER TABLE [dbo].[NeweggOrderItem] ADD CONSTRAINT [FK_NeweggOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] ADD CONSTRAINT [FK_NeweggStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Note]'
GO
ALTER TABLE [dbo].[Note] ADD CONSTRAINT [FK_Note_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[OnTracProfile]'
GO
ALTER TABLE [dbo].[OnTracProfile] ADD CONSTRAINT [FK_OnTracProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD CONSTRAINT [FK_OnTracShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] ADD CONSTRAINT [FK_OrderCharge_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] ADD CONSTRAINT [FK_OrderMotionOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] ADD CONSTRAINT [FK_OrderPaymentDetail_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[PayPalOrder]'
GO
ALTER TABLE [dbo].[PayPalOrder] ADD CONSTRAINT [FK_PayPalOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresOrder]'
GO
ALTER TABLE [dbo].[ProStoresOrder] ADD CONSTRAINT [FK_ProStoresOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[SearsOrder]'
GO
ALTER TABLE [dbo].[SearsOrder] ADD CONSTRAINT [FK_SearsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyOrder]'
GO
ALTER TABLE [dbo].[ShopifyOrder] ADD CONSTRAINT [FK_ShopifyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrder]'
GO
ALTER TABLE [dbo].[YahooOrder] ADD CONSTRAINT [FK_YahooOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] ADD CONSTRAINT [FK_OrderItemAttribute_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[SearsOrderItem]'
GO
ALTER TABLE [dbo].[SearsOrderItem] ADD CONSTRAINT [FK_SearsOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyOrderItem]'
GO
ALTER TABLE [dbo].[ShopifyOrderItem] ADD CONSTRAINT [FK_ShopifyOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrderItem]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderItem] ADD CONSTRAINT [FK_ThreeDCartOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD CONSTRAINT [FK_YahooOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] ADD CONSTRAINT [FK_OrderMotionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OtherProfile]'
GO
ALTER TABLE [dbo].[OtherProfile] ADD CONSTRAINT [FK_OtherProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] ADD CONSTRAINT [FK_OtherShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PayPalStore]'
GO
ALTER TABLE [dbo].[PayPalStore] ADD CONSTRAINT [FK_PayPalStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Permission]'
GO
ALTER TABLE [dbo].[Permission] ADD CONSTRAINT [FK_Permission_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] ADD CONSTRAINT [FK_PostalProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UspsProfile]'
GO
ALTER TABLE [dbo].[UspsProfile] ADD CONSTRAINT [FK_UspsProfile_PostalProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[PostalProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [FK_PostalShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UspsShipment]'
GO
ALTER TABLE [dbo].[UspsShipment] ADD CONSTRAINT [FK_UspsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
ALTER TABLE [dbo].[UspsShipment] ADD CONSTRAINT [FK_UspsShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresStore]'
GO
ALTER TABLE [dbo].[ProStoresStore] ADD CONSTRAINT [FK_ProStoresStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[UspsScanForm]'
GO
ALTER TABLE [dbo].[UspsScanForm] ADD CONSTRAINT [FK_UspsScanForm_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_CRON_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_CRON_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_CRON_TRIGGERS_Scheduling_TRIGGERS] FOREIGN KEY ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) REFERENCES [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_TRIGGERS_Scheduling_JOB_DETAILS] FOREIGN KEY ([SCHED_NAME], [JOB_NAME], [JOB_GROUP]) REFERENCES [dbo].[Scheduling_JOB_DETAILS] ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_SIMPLE_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPLE_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_SIMPLE_TRIGGERS_Scheduling_TRIGGERS] FOREIGN KEY ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) REFERENCES [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_SIMPROP_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPROP_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_SIMPROP_TRIGGERS_Scheduling_TRIGGERS] FOREIGN KEY ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) REFERENCES [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] ADD CONSTRAINT [FK_SearsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD CONSTRAINT [FK_ShipmentCustomsItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [FK_UpsShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShippingPrintOutputRule]'
GO
ALTER TABLE [dbo].[ShippingPrintOutputRule] ADD CONSTRAINT [FK_ShippingPrintOutputRule_ShippingPrintOutput] FOREIGN KEY ([ShippingPrintOutputID]) REFERENCES [dbo].[ShippingPrintOutput] ([ShippingPrintOutputID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD CONSTRAINT [FK_UpsProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyStore]'
GO
ALTER TABLE [dbo].[ShopifyStore] ADD CONSTRAINT [FK_ShopifyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] ADD CONSTRAINT [FK_StoreShopSite_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[StatusPreset]'
GO
ALTER TABLE [dbo].[StatusPreset] ADD CONSTRAINT [FK_StatusPreset_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [FK_ThreeDCartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] ADD CONSTRAINT [FK_VolusionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD CONSTRAINT [FK_YahooStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[TemplateStoreSettings]'
GO
ALTER TABLE [dbo].[TemplateStoreSettings] ADD CONSTRAINT [FK_TemplateStoreSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[TemplateUserSettings]'
GO
ALTER TABLE [dbo].[TemplateUserSettings] ADD CONSTRAINT [FK_TemplateUserSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE
ALTER TABLE [dbo].[TemplateUserSettings] ADD CONSTRAINT [FK_TemplateUserSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Template]'
GO
ALTER TABLE [dbo].[Template] ADD CONSTRAINT [FK_Template_TemplateFolder] FOREIGN KEY ([ParentFolderID]) REFERENCES [dbo].[TemplateFolder] ([TemplateFolderID])
GO
PRINT N'Adding foreign keys to [dbo].[TemplateFolder]'
GO
ALTER TABLE [dbo].[TemplateFolder] ADD CONSTRAINT [FK_TemplateFolder_TemplateFolder] FOREIGN KEY ([ParentFolderID]) REFERENCES [dbo].[TemplateFolder] ([TemplateFolderID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [FK_UpsPackage_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ADD CONSTRAINT [FK_WorldShipShipment_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] ADD CONSTRAINT [FK_UserSetting_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD CONSTRAINT [FK_WorldShipGoods_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD CONSTRAINT [FK_WorldShipPackage_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[YahooProduct]'
GO
ALTER TABLE [dbo].[YahooProduct] ADD CONSTRAINT [FK_YahooProduct_YahooStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[YahooStore] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Creating [dbo].[GrouponOrder]'
GO
CREATE TABLE [dbo].[GrouponOrder]
(
[OrderID] [bigint] NOT NULL,
[GrouponOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ParentOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_GrouponOrder] on [dbo].[GrouponOrder]'
GO
ALTER TABLE [dbo].[GrouponOrder] ADD CONSTRAINT [PK_GrouponOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_GrouponOrderID] on [dbo].[GrouponOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_GrouponOrderID] ON [dbo].[GrouponOrder] ([GrouponOrderID])
GO
PRINT N'Creating index [IX_SWDefault_GrouponOrder_ParentOrderID] on [dbo].[GrouponOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_GrouponOrder_ParentOrderID] ON [dbo].[GrouponOrder] ([ParentOrderID])
GO
PRINT N'Creating [dbo].[GrouponOrderItem]'
GO
CREATE TABLE [dbo].[GrouponOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[Permalink] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ChannelSKUProvided] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FulfillmentLineItemID] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BomSKU] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GrouponLineItemID] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PONumber] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_GrouponOrderItem] on [dbo].[GrouponOrderItem]'
GO
ALTER TABLE [dbo].[GrouponOrderItem] ADD CONSTRAINT [PK_GrouponOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[GrouponStore]'
GO
CREATE TABLE [dbo].[GrouponStore]
(
[StoreID] [bigint] NOT NULL,
[SupplierID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Token] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_GrouponStore] on [dbo].[GrouponStore]'
GO
ALTER TABLE [dbo].[GrouponStore] ADD CONSTRAINT [PK_GrouponStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GrouponOrder]'
GO
ALTER TABLE [dbo].[GrouponOrder] ADD CONSTRAINT [FK_GrouponOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[GrouponOrderItem]'
GO
ALTER TABLE [dbo].[GrouponOrderItem] ADD CONSTRAINT [FK_GrouponOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[GrouponStore]'
GO
ALTER TABLE [dbo].[GrouponStore] ADD CONSTRAINT [FK_GrouponStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO

PRINT N'Creating table to [dbo].[EtsyOrderItem]'
GO
CREATE TABLE [dbo].[EtsyOrderItem](
    [OrderItemID] [bigint] NOT NULL,
	[TransactionID] [nvarchar] (300) NOT NULL,
	[ListingID] [nvarchar] (100) NOT NULL
 CONSTRAINT [PK_EtsyOrderItem] PRIMARY KEY CLUSTERED
(
    [OrderItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EtsyOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_EtsyOrderItem_OrderItem] FOREIGN KEY([OrderItemID])
REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO

ALTER TABLE [dbo].[EtsyOrderItem] CHECK CONSTRAINT [FK_EtsyOrderItem_OrderItem]
GO

PRINT N'Creating [dbo].[LemonStandStore]'
GO
CREATE TABLE [dbo].[LemonStandStore]
(
    [StoreID] [bigint] NOT NULL,
    [Token] [varchar](100) NOT NULL,
    [StoreURL] [varchar](255) NOT NULL,
    [StatusCodes] [xml] NULL,
)
GO
PRINT N'Creating primary key [PK_LemonStandStore] on [dbo].[LemonStandStore]'
GO
ALTER TABLE [dbo].[LemonStandStore] ADD CONSTRAINT [PK_LemonStandStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[LemonStandStore]'
GO
ALTER TABLE [dbo].[LemonStandStore] ADD CONSTRAINT [FK_LemonStandStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO

PRINT N'Creating [dbo].[LemonStandOrder]'
GO
CREATE TABLE [dbo].[LemonStandOrder]
(
[OrderID] [bigint] NOT NULL,
[LemonStandOrderID] [nvarchar](20) NOT NULL
)
GO
PRINT N'Creating primary key [PK_LemonStandOrder] on [dbo].[LemonStandOrder]'
GO
ALTER TABLE [dbo].[LemonStandOrder] ADD CONSTRAINT [PK_LemonStandOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO

ALTER TABLE [dbo].[LemonStandOrder]  WITH CHECK ADD  CONSTRAINT [FK_LemonStandOrder_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO

ALTER TABLE [dbo].[LemonStandOrder] CHECK CONSTRAINT [FK_LemonStandOrder_Order]
GO

PRINT N'Creating [dbo].[LemonStandOrderItem]'
GO
CREATE TABLE [dbo].[LemonStandOrderItem]
(
    [OrderItemID] [bigint] NOT NULL,
    [UrlName] [nvarchar](100) NOT NULL,
    [ShortDescription] [nvarchar](255) NOT NULL,
    [Category] [nvarchar](100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_LemonStandOrderItem] on [dbo].[LemonStandOrderItem]'
GO
ALTER TABLE [dbo].[LemonStandOrderItem] ADD CONSTRAINT [PK_LemonStandOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID])
GO

ALTER TABLE [dbo].[LemonStandOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_LemonStandOrderItem_OrderItem] FOREIGN KEY([OrderItemID])
REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO

ALTER TABLE [dbo].[LemonStandOrderItem] CHECK CONSTRAINT [FK_LemonStandOrderItem_OrderItem]
GO

CREATE NONCLUSTERED INDEX [IX_SWDefault_LemonStandOrderID] ON [dbo].[LemonStandOrder]
(
    [LemonStandOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

PRINT N'Creating [dbo].[SparkPayStore]'
GO
CREATE TABLE [dbo].[SparkPayStore]
(
[StoreID] [bigint] NOT NULL,
[Token] [nvarchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StoreUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StatusCodes] [xml] NULL
)
GO
PRINT N'Creating primary key [PK_SparkPayStore] on [dbo].[SparkPayStore]'
GO
ALTER TABLE [dbo].[SparkPayStore] ADD CONSTRAINT [PK_SparkPayStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO

PRINT N'Adding foreign keys to [dbo].[SparkPayStore]'
GO
ALTER TABLE [dbo].[SparkPayStore] ADD CONSTRAINT [FK_SparkPayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating [dbo].[OdbcStore]'
GO
CREATE TABLE [dbo].[OdbcStore]
(
    [StoreID] [bigint] NOT NULL,
    [ImportConnectionString] [nvarchar](2048) NOT NULL,
    [ImportMap] [nvarchar](max) NOT NULL,
    [ImportStrategy] [int] NOT NULL,
    [ImportColumnSourceType] [int] NOT NULL,
    [ImportColumnSource] [nvarchar](2048) NOT NULL,
    [ImportOrderItemStrategy] [int] NOT NULL,
    [UploadStrategy] [int] NOT NULL,
    [UploadMap] [nvarchar](max) NOT NULL,
    [UploadColumnSourceType] [int] NOT NULL,
    [UploadColumnSource] [nvarchar](2048) NOT NULL,
    [UploadConnectionString] [nvarchar](2048) NOT NULL,
)
GO
PRINT N'Creating primary key [PK_OdbcStore] on [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] ADD CONSTRAINT [PK_OdbcStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] ADD CONSTRAINT [FK_OdbcStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO

PRINT N'Creating [dbo].[WalmartOrder]'
GO
CREATE TABLE [dbo].[WalmartOrder]
(
[OrderID] [bigint] NOT NULL,
[PurchaseOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomerOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EstimatedDeliveryDate] [datetime] NOT NULL,
[EstimatedShipDate] [datetime] NOT NULL,
[RequestedShippingMethodCode] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartOrder] on [dbo].[WalmartOrder]'
GO
ALTER TABLE [dbo].[WalmartOrder] ADD CONSTRAINT [PK_WalmartOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating table GenericModuleOrder'
GO
CREATE TABLE [dbo].[GenericModuleOrder](
	[OrderID] [bigint] NOT NULL,
	[AmazonOrderID] [varchar](32) NOT NULL,
	[IsFBA] [bit] NOT NULL,
	[IsPrime] [int] NOT NULL,
	[IsSameDay] bit NOT NULL
 CONSTRAINT [PK_GenericModuleOrder] PRIMARY KEY CLUSTERED
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GenericModuleOrder]  WITH CHECK ADD  CONSTRAINT [FK_GenericModuleOrder_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO

ALTER TABLE [dbo].[GenericModuleOrder] CHECK CONSTRAINT [FK_GenericModuleOrder_Order]
GO

PRINT N'Creating MagentoOrder foreign key'
GO
ALTER TABLE [dbo].[MagentoOrder]  WITH CHECK ADD CONSTRAINT [FK_MagentoOrder_GenericModuleOrder] FOREIGN KEY([OrderID])
REFERENCES [dbo].[GenericModuleOrder] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MagentoOrder] CHECK CONSTRAINT [FK_MagentoOrder_GenericModuleOrder]
GO

PRINT N'Creating table GenericModuleOrderItem'
GO
CREATE TABLE [dbo].[GenericModuleOrderItem](
	[OrderItemID] [bigint] NOT NULL,
	[AmazonOrderItemCode] [nvarchar](64) NOT NULL
 CONSTRAINT [PK_GenericModuleOrderItem] PRIMARY KEY CLUSTERED
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[GenericModuleOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_GenericModuleOrderItem_OrderItem] FOREIGN KEY([OrderItemID])
REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO

ALTER TABLE [dbo].[GenericModuleOrderItem] CHECK CONSTRAINT [FK_GenericModuleOrderItem_OrderItem]
GO

PRINT N'Creating index [IX_SWDefault_PurchaseOrderId] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_PurchaseOrderId] ON [dbo].[WalmartOrder] ([PurchaseOrderID])
GO
PRINT N'Creating index [IX_SWDefault_CustomerOrderId] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_CustomerOrderId] ON [dbo].[WalmartOrder] ([CustomerOrderID])
GO
PRINT N'Creating index [IX_SWDefault_EstimatedDeliveryDate] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EstimatedDeliveryDate] ON [dbo].[WalmartOrder] ([EstimatedDeliveryDate])
GO
PRINT N'Creating index [IX_SWDefault_EstimatedShipDate] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EstimatedShipDate] ON [dbo].[WalmartOrder] ([EstimatedShipDate])
GO
PRINT N'Creating index [IX_SWDefault_RequestedShippingMethodCode] on [dbo].[WalmartOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_RequestedShippingMethodCode] ON [dbo].[WalmartOrder] ([RequestedShippingMethodCode])
GO
PRINT N'Creating [dbo].[WalmartOrderItem]'
GO
CREATE TABLE [dbo].[WalmartOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[LineNumber] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineStatus] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartOrderItem] on [dbo].[WalmartOrderItem]'
GO
ALTER TABLE [dbo].[WalmartOrderItem] ADD CONSTRAINT [PK_WalmartOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[WalmartStore]'
GO
CREATE TABLE [dbo].[WalmartStore]
(
[StoreID] [bigint] NOT NULL,
[ConsumerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PrivateKey] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartStore] on [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].[WalmartStore] ADD CONSTRAINT [PK_WalmartStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[WalmartOrderItem]'
GO
ALTER TABLE [dbo].[WalmartOrderItem] ADD CONSTRAINT [FK_WalmartOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[WalmartOrder]'
GO
ALTER TABLE [dbo].[WalmartOrder] ADD CONSTRAINT [FK_WalmartOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].[WalmartStore] ADD CONSTRAINT [FK_WalmartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating [dbo].[JetStore]'
GO
CREATE TABLE [dbo].[JetStore]
(
[StoreID] [bigint] NOT NULL,
[ApiUser] [nvarchar](100) NOT NULL,
[Secret] [nvarchar](100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetStore] on [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] ADD CONSTRAINT [PK_JetStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] ADD CONSTRAINT [FK_JetStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating [dbo].[UpsRateTable]'
GO
PRINT N'Creating [dbo].[JetOrder]'
GO
CREATE TABLE [dbo].[JetOrder]
(
[OrderID] [bigint] NOT NULL,
[MerchantOrderId] [nvarchar](50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetOrder] on [dbo].[JetOrder]'
GO
ALTER TABLE [dbo].[JetOrder] ADD CONSTRAINT [PK_JetOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[JetOrderItem]'
GO
CREATE TABLE [dbo].[JetOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[MerchantSku] [nvarchar](50) NOT NULL,
[JetOrderItemID] [nvarchar](50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetOrderItem] on [dbo].[JetOrderItem]'
GO
ALTER TABLE [dbo].[JetOrderItem] ADD CONSTRAINT [PK_JetOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[JetOrderItem]'
GO
ALTER TABLE [dbo].[JetOrderItem] ADD CONSTRAINT [FK_JetOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[JetOrder]'
GO
ALTER TABLE [dbo].[JetOrder] ADD CONSTRAINT [FK_JetOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
CREATE TABLE [dbo].[UpsRateTable](
    [UpsRateTableID] [bigint] NOT NULL IDENTITY(1, 1),
    [UploadDate][DateTime2] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsRateTable] on [dbo].[UpsRateTable]'
GO
ALTER TABLE [UpsRateTable] ADD CONSTRAINT [PK_UpsRateTable] PRIMARY KEY CLUSTERED ([UpsRateTableID])
GO
PRINT N'Creating [dbo].[UpsPackageRate]'
GO
CREATE TABLE [dbo].[UpsPackageRate](
    [UpsPackageRateID] [bigint] NOT NULL IDENTITY(1, 1),
    [UpsRateTableID][bigint] NOT NULL,
    [Zone][varchar](3) NOT NULL,
    [WeightInPounds][int] NOT NULL,
    [Service][int] NOT NULL,
    [Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsPackageRate] on [dbo].[UpsPackageRate]'
GO
ALTER TABLE [dbo].[UpsPackageRate] ADD CONSTRAINT [PK_UpsPackageRate] PRIMARY KEY CLUSTERED ([UpsPackageRateID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsPackageRate]'
ALTER TABLE [dbo].[UpsPackageRate] ADD CONSTRAINT [FK_UpsPackageRate_UpsPackageRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO

PRINT N'Creating index [IX_SWDefault_UpsPackageRate_WeightInPounds_Zone] on [dbo].[UpsPackageRate]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_UpsPackageRate_WeightInPounds_Zone] ON [dbo].[UpsPackageRate] ([WeightInPounds],[Zone]) INCLUDE ([UpsPackageRateID], [UpsRateTableID], [Service], [Rate])
GO

PRINT N'Creating [dbo].[UpsLetterRate]'
GO
CREATE TABLE [dbo].[UpsLetterRate](
    [UpsLetterRateID] [bigint] NOT NULL IDENTITY(1, 1),
    [UpsRateTableID][bigint] NOT NULL,
    [Zone][varchar](3) NOT NULL,
    [Service][int] NOT NULL,
    [Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLetterRate] on [dbo].[UpsLetterRate]'
GO
ALTER TABLE [dbo].[UpsLetterRate] ADD CONSTRAINT [PK_UpsLetterRate] PRIMARY KEY CLUSTERED ([UpsLetterRateID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLetterRate]'
ALTER TABLE [dbo].[UpsLetterRate] ADD CONSTRAINT [FK_UpsLetterRate_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[UpsPricePerPound]'
GO
CREATE TABLE [dbo].[UpsPricePerPound](
    [UpsPricePerPoundID] [bigint] NOT NULL IDENTITY(1, 1),
    [UpsRateTableID][bigint] NOT NULL,
    [Zone][varchar](3) NOT NULL,
    [Service][int] NOT NULL,
    [Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsPricePerPound] on [dbo].[UpsPricePerPound]'
GO
ALTER TABLE [dbo].[UpsPricePerPound] ADD CONSTRAINT [PK_UpsPricePerPound] PRIMARY KEY CLUSTERED ([UpsPricePerPoundID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsRate]'
ALTER TABLE [dbo].[UpsPricePerPound] ADD CONSTRAINT [FK_UpsPricePerPound_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO

GO
PRINT N'Creating [dbo].[UpsRateSurcharge]'
GO
CREATE TABLE [dbo].[UpsRateSurcharge](
    [UpsRateSurchargeID] [bigint] NOT NULL IDENTITY(1, 1),
    [UpsRateTableID][bigint] NOT NULL,
    [SurchargeType][int] NOT NULL,
    [Amount][float] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsRateSurcharge] on [dbo].[UpsRateSurcharge]'
GO
ALTER TABLE [dbo].[UpsRateSurcharge] ADD CONSTRAINT [PK_UpsRateSurcharge] PRIMARY KEY CLUSTERED ([UpsRateSurchargeID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsRateSurcharge]'
GO
ALTER TABLE [dbo].[UpsRateSurcharge] ADD CONSTRAINT [FK_UpsRateSurcharge_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] ADD CONSTRAINT [FK_UpsAccount_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
GO

PRINT N'Creating [dbo].[UpsLocalRatingZoneFile]'
GO
CREATE TABLE [dbo].[UpsLocalRatingZoneFile](
    [ZoneFileID] [bigint] NOT NULL IDENTITY(1, 1),
    [UploadDate] [DateTime2] NOT NULL,
    [FileContent] [varbinary](max) NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRatingZoneFile] on [dbo].[UpsLocalRatingZoneFile]'
GO
ALTER TABLE [dbo].[UpsLocalRatingZoneFile] ADD CONSTRAINT [PK_UpsLocalRatingZoneFile] PRIMARY KEY CLUSTERED ([ZoneFileID])
GO

PRINT N'Creating [dbo].[UpsLocalRatingZone]'
GO
CREATE TABLE [dbo].[UpsLocalRatingZone](
    [ZoneID] [bigint] NOT NULL IDENTITY(1, 1),
    [ZoneFileID] [bigint] NOT NULL,
    [OriginZipFloor] [int] NOT NULL,
    [OriginZipCeiling] [int] NOT NULL,
    [DestinationZipFloor] [int] NOT NULL,
    [DestinationZipCeiling] [int] NOT NULL,
    [Service] [int] NOT NULL,
    [Zone] [varchar](3) NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRatingZone] on [dbo].[UpsLocalRatingZone]'
GO
ALTER TABLE [dbo].[UpsLocalRatingZone] ADD CONSTRAINT [PK_UpsLocalRatingZone] PRIMARY KEY CLUSTERED ([ZoneID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLocalRatingZone]'
GO
ALTER TABLE [dbo].[UpsLocalRatingZone] ADD CONSTRAINT [FK_UpsLocalRatingZone_UpsLocalRatingZoneFile] FOREIGN KEY([ZoneFileID]) REFERENCES [dbo].[UpsLocalRatingZoneFile] ([ZoneFileID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
CREATE TABLE [dbo].UpsLocalRatingDeliveryAreaSurcharge(
    [DeliveryAreaSurchargeID] [bigint] NOT NULL IDENTITY(1, 1),
    [ZoneFileID] [bigint] NOT NULL,
    [DestinationZip] [int] NOT NULL,
    [DeliveryAreaType] [int] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRatingDeliveryAreaSurcharge] on [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
ALTER TABLE [dbo].[UpsLocalRatingDeliveryAreaSurcharge] ADD CONSTRAINT [PK_UpsLocalRatingDeliveryAreaSurcharge] PRIMARY KEY CLUSTERED ([DeliveryAreaSurchargeID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
ALTER TABLE [dbo].[UpsLocalRatingDeliveryAreaSurcharge] ADD CONSTRAINT [FK_UpsLocalRatingDeliveryAreaSurcharge_UpsLocalRatingZoneFile] FOREIGN KEY([ZoneFileID]) REFERENCES [dbo].[UpsLocalRatingZoneFile] ([ZoneFileID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[Shortcut]'
GO
CREATE TABLE [dbo].[Shortcut]
(
[ShortcutID] [bigint] NOT NULL IDENTITY(1105, 1000),
[RowVersion] [timestamp] NOT NULL,
[ModifierKeys] [int] NULL,
[VirtualKey] [int] NULL,
[Barcode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Action] [int] NOT NULL,
[RelatedObjectID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_Shortcut] on [dbo].[Shortcut]'
GO
ALTER TABLE [dbo].[Shortcut] ADD CONSTRAINT [PK_Shortcut] PRIMARY KEY CLUSTERED  ([ShortcutID])
GO
PRINT N'Creating index [IX_SWDefault_Shortcut_Keys] on [dbo].[Shortcut]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_Shortcut_Keys] ON [dbo].[Shortcut] ([ModifierKeys], [VirtualKey])
WHERE [ModifierKeys] IS NOT NULL
AND [VirtualKey] IS NOT NULL
GO
PRINT N'Creating index [IX_SWDefault_Shortcut_Barcode] on [dbo].[Shortcut]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_Shortcut_Barcode] ON [dbo].[Shortcut] ([Barcode]) WHERE ([Barcode]<>'')
GO
PRINT N'Altering [dbo].[Shortcut]'
GO
ALTER TABLE [dbo].[Shortcut] ENABLE CHANGE_TRACKING
GO

PRINT N'Creating index [IX_SWDefault_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip] on [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip] ON [dbo].[UpsLocalRatingDeliveryAreaSurcharge] ([DestinationZip]) INCLUDE ([DeliveryAreaSurchargeID],	[ZoneFileID], [DeliveryAreaType])
GO
PRINT N'Creating [dbo].[OrderSearch]'
GO
CREATE TABLE [dbo].[OrderSearch]
(
[OrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[StoreID] [bigint] NOT NULL,
[OrderNumber] [bigint] NOT NULL,
[OrderNumberComplete] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderSearch] on [dbo].[OrderSearch]'
GO
ALTER TABLE [dbo].[OrderSearch] ADD CONSTRAINT [PK_OrderSearch] PRIMARY KEY CLUSTERED  ([OrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_OrderSearch_OrderNumber] on [dbo].[OrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderSearch_OrderNumber] ON [dbo].[OrderSearch] ([OrderNumber]) INCLUDE ([OrderID], [StoreID])
GO
PRINT N'Creating index [IX_SWDefault_OrderSearch_OrderNumberComplete] on [dbo].[OrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderSearch_OrderNumberComplete] ON [dbo].[OrderSearch] ([OrderNumberComplete]) INCLUDE ([OrderID], [StoreID])
GO
PRINT N'Creating index [IX_SWDefault_OrderSearch_StoreIDIsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OrderSearch_StoreIDIsManual] ON [dbo].[OrderSearch] ([StoreID] ASC, [IsManual] ASC) INCLUDE ([OrderNumber])
GO
PRINT N'Adding foreign keys to [dbo].[OrderSearch]'
GO
ALTER TABLE [dbo].[OrderSearch] ADD CONSTRAINT [FK_OrderSearch_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderSearch] ADD CONSTRAINT [FK_OrderSearch_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating [dbo].[AmazonOrderSearch]'
GO
CREATE TABLE [dbo].[AmazonOrderSearch]
(
[AmazonOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[AmazonOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonOrderSearch] on [dbo].[AmazonOrderSearch]'
GO
ALTER TABLE [dbo].[AmazonOrderSearch] ADD CONSTRAINT [PK_AmazonOrderSearch] PRIMARY KEY CLUSTERED  ([AmazonOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_AmazonOrderSearch_AmazonOrderID_OrderID] on [dbo].[AmazonOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AmazonOrderSearch_AmazonOrderID_OrderID] ON [dbo].[AmazonOrderSearch] ([AmazonOrderID], [OrderID])
GO
PRINT N'Creating [dbo].[ChannelAdvisorOrderSearch]'
GO
CREATE TABLE [dbo].[ChannelAdvisorOrderSearch]
(
[ChannelAdvisorOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[CustomOrderIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderSearch] on [dbo].[ChannelAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderSearch] ADD CONSTRAINT [PK_ChannelAdvisorOrderSearch] PRIMARY KEY CLUSTERED  ([ChannelAdvisorOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID] on [dbo].[ChannelAdvisorOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID]
	ON [dbo].[ChannelAdvisorOrderSearch] ([CustomOrderIdentifier], [OrderID])
GO
PRINT N'Creating [dbo].[ClickCartProOrderSearch]'
GO
CREATE TABLE [dbo].[ClickCartProOrderSearch]
(
[ClickCartProOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ClickCartProOrderID] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ClickCartProOrderSearch] on [dbo].[ClickCartProOrderSearch]'
GO
ALTER TABLE [dbo].[ClickCartProOrderSearch] ADD CONSTRAINT [PK_ClickCartProOrderSearch] PRIMARY KEY CLUSTERED  ([ClickCartProOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_ClickCartProOrderSearch_ClickCartProOrderID] on [dbo].[ClickCartProOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ClickCartProOrderSearch_ClickCartProOrderID] ON [dbo].[ClickCartProOrderSearch] ([ClickCartProOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[CommerceInterfaceOrderSearch]'
GO
CREATE TABLE [dbo].[CommerceInterfaceOrderSearch]
(
[CommerceInterfaceOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[CommerceInterfaceOrderNumber] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_CommerceInterfaceOrderSearch] on [dbo].[CommerceInterfaceOrderSearch]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrderSearch] ADD CONSTRAINT [PK_CommerceInterfaceOrderSearch] PRIMARY KEY CLUSTERED  ([CommerceInterfaceOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_CommerceInterfaceOrderSearch_CommerceInterfaceOrderNumber] on [dbo].[CommerceInterfaceOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_CommerceInterfaceOrderSearch_CommerceInterfaceOrderNumber] ON [dbo].[CommerceInterfaceOrderSearch] ([CommerceInterfaceOrderNumber]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[EbayOrderSearch]'
GO
CREATE TABLE [dbo].[EbayOrderSearch]
(
[EbayOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[EbayBuyerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellingManagerRecord] [int] NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EbayOrderSearch] on [dbo].[EbayOrderSearch]'
GO
ALTER TABLE [dbo].[EbayOrderSearch] ADD CONSTRAINT [PK_EbayOrderSearch] PRIMARY KEY CLUSTERED  ([EbayOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_EbayOrderSearch_EbayBuyerID_OrderID] on [dbo].[EbayOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_EbayOrderSearch_EbayBuyerID_OrderID] ON [dbo].[EbayOrderSearch] ([EbayBuyerID], [OrderID])
GO
PRINT N'Creating [dbo].[GrouponOrderSearch]'
GO
CREATE TABLE [dbo].[GrouponOrderSearch]
(
[GrouponOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[GrouponOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 [ParentOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_GrouponOrderSearch] on [dbo].[GrouponOrderSearch]'
GO
ALTER TABLE [dbo].[GrouponOrderSearch] ADD CONSTRAINT [PK_GrouponOrderSearch] PRIMARY KEY CLUSTERED  ([GrouponOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_GrouponOrderSearch_GrouponOrderID_OrderID] on [dbo].[GrouponOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_GrouponOrderSearch_GrouponOrderID_OrderID] ON [dbo].[GrouponOrderSearch] ([GrouponOrderID], [OrderID])
GO
PRINT N'Creating index [IX_SWDefault_GrouponOrderSearch_ParentOrderID_OrderID] on [dbo].[GrouponOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_GrouponOrderSearch_ParentOrderID_OrderID] ON [dbo].[GrouponOrderSearch] ([ParentOrderID], [OrderID])
GO
PRINT N'Creating [dbo].[JetOrderSearch]'
GO
CREATE TABLE [dbo].[JetOrderSearch]
(
[JetOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[MerchantOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetOrderSearch] on [dbo].[JetOrderSearch]'
GO
ALTER TABLE [dbo].[JetOrderSearch] ADD CONSTRAINT [PK_JetOrderSearch] PRIMARY KEY CLUSTERED  ([JetOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_JetOrderSearch_JetOrderID] on [dbo].[JetOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_JetOrderSearch_JetOrderID] ON [dbo].[JetOrderSearch] ([MerchantOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[LemonStandOrderSearch]'
GO
CREATE TABLE [dbo].[LemonStandOrderSearch]
(
[LemonStandOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[LemonStandOrderID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_LemonStandOrderSearch] on [dbo].[LemonStandOrderSearch]'
GO
ALTER TABLE [dbo].[LemonStandOrderSearch] ADD CONSTRAINT [PK_LemonStandOrderSearch] PRIMARY KEY CLUSTERED  ([LemonStandOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_LemonStandOrderSearch_LemonStandOrderID_OrderID] on [dbo].[LemonStandOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_LemonStandOrderSearch_LemonStandOrderID_OrderID] ON [dbo].[LemonStandOrderSearch] ([LemonStandOrderID], [OrderID])
GO
PRINT N'Creating [dbo].[MagentoOrderSearch]'
GO
CREATE TABLE [dbo].[MagentoOrderSearch]
(
[MagentoOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[MagentoOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MagentoOrderSearch] on [dbo].[MagentoOrderSearch]'
GO
ALTER TABLE [dbo].[MagentoOrderSearch] ADD CONSTRAINT [PK_MagentoOrderSearch] PRIMARY KEY CLUSTERED  ([MagentoOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_MagentoOrderSearch_MagentoOrderID] on [dbo].[MagentoOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_MagentoOrderSearch_MagentoOrderID] ON [dbo].[MagentoOrderSearch] ([MagentoOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[MarketplaceAdvisorOrderSearch]'
GO
CREATE TABLE [dbo].[MarketplaceAdvisorOrderSearch]
(
[MarketplaceAdvisorOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[InvoiceNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellerOrderNumber] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_MarketplaceAdvisorOrderSearch] on [dbo].[MarketplaceAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrderSearch] ADD CONSTRAINT [PK_MarketplaceAdvisorOrderSearch] PRIMARY KEY CLUSTERED  ([MarketplaceAdvisorOrderSearchID])
GO
PRINT N'Creating [dbo].[NetworkSolutionsOrderSearch]'
GO
CREATE TABLE [dbo].[NetworkSolutionsOrderSearch]
(
[NetworkSolutionsOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[NetworkSolutionsOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_NetworkSolutionsOrderSearch] on [dbo].[NetworkSolutionsOrderSearch]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrderSearch] ADD CONSTRAINT [PK_NetworkSolutionsOrderSearch] PRIMARY KEY CLUSTERED  ([NetworkSolutionsOrderSearchID])
GO
PRINT N'Creating [dbo].[OrderMotionOrderSearch]'
GO
CREATE TABLE [dbo].[OrderMotionOrderSearch]
(
[OrderMotionOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[OrderMotionShipmentID] [int] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderMotionOrderSearch] on [dbo].[OrderMotionOrderSearch]'
GO
ALTER TABLE [dbo].[OrderMotionOrderSearch] ADD CONSTRAINT [PK_OrderMotionOrderSearch] PRIMARY KEY CLUSTERED  ([OrderMotionOrderSearchID])
GO
PRINT N'Creating [dbo].[PayPalOrderSearch]'
GO
CREATE TABLE [dbo].[PayPalOrderSearch]
(
[PayPalOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[TransactionID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PayPalOrderSearch] on [dbo].[PayPalOrderSearch]'
GO
ALTER TABLE [dbo].[PayPalOrderSearch] ADD CONSTRAINT [PK_PayPalOrderSearch] PRIMARY KEY CLUSTERED  ([PayPalOrderSearchID])
GO
PRINT N'Creating [dbo].[ProStoresOrderSearch]'
GO
CREATE TABLE [dbo].[ProStoresOrderSearch]
(
[ProStoresOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ConfirmationNumber] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProStoresOrderSearch] on [dbo].[ProStoresOrderSearch]'
GO
ALTER TABLE [dbo].[ProStoresOrderSearch] ADD CONSTRAINT [PK_ProStoresOrderSearch] PRIMARY KEY CLUSTERED  ([ProStoresOrderSearchID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProStoresOrderSearch_ConfirmationNumber]
	ON [dbo].[ProStoresOrderSearch] ( [ConfirmationNumber] ASC )
GO
PRINT N'Creating [dbo].[SearsOrderSearch]'
GO
CREATE TABLE [dbo].[SearsOrderSearch]
(
[SearsOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[PoNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_SearsOrderSearch] on [dbo].[SearsOrderSearch]'
GO
ALTER TABLE [dbo].[SearsOrderSearch] ADD CONSTRAINT [PK_SearsOrderSearch] PRIMARY KEY CLUSTERED  ([SearsOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_SearsOrderSearch_PoNumber_OrderID] on [dbo].[SearsOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_SearsOrderSearch_PoNumber_OrderID] ON [dbo].[SearsOrderSearch] ([PoNumber], [OrderID])
GO
PRINT N'Creating [dbo].[ShopifyOrderSearch]'
GO
CREATE TABLE [dbo].[ShopifyOrderSearch]
(
[ShopifyOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ShopifyOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShopifyOrderSearch] on [dbo].[ShopifyOrderSearch]'
GO
ALTER TABLE [dbo].[ShopifyOrderSearch] ADD CONSTRAINT [PK_ShopifyOrderSearch] PRIMARY KEY CLUSTERED  ([ShopifyOrderSearchID])
GO
PRINT N'Creating [dbo].[ThreeDCartOrderSearch]'
GO
CREATE TABLE [dbo].[ThreeDCartOrderSearch]
(
[ThreeDCartOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[ThreeDCartOrderID] [bigint] NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrderSearch] on [dbo].[ThreeDCartOrderSearch]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderSearch] ADD CONSTRAINT [PK_ThreeDCartOrderSearch] PRIMARY KEY CLUSTERED  ([ThreeDCartOrderSearchID])
GO
PRINT N'Creating [dbo].[WalmartOrderSearch]'
GO
CREATE TABLE [dbo].[WalmartOrderSearch]
(
[WalmartOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[PurchaseOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL,
[CustomerOrderID] [varchar] (50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_WalmartOrderSearch] on [dbo].[WalmartOrderSearch]'
GO
ALTER TABLE [dbo].[WalmartOrderSearch] ADD CONSTRAINT [PK_WalmartOrderSearch] PRIMARY KEY CLUSTERED  ([WalmartOrderSearchID])
GO
PRINT N'Creating index [IX_SWDefault_WalmartOrderSearch_PurchaseOrderID] on [dbo].[WalmartOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_WalmartOrderSearch_PurchaseOrderID] ON [dbo].[WalmartOrderSearch] ([PurchaseOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating index [IX_SWDefault_WalmartOrderSearch_CustomerOrderID] on [dbo].[WalmartOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_WalmartOrderSearch_CustomerOrderID] ON [dbo].[WalmartOrderSearch] ([CustomerOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Creating [dbo].[YahooOrderSearch]'
GO
CREATE TABLE [dbo].[YahooOrderSearch]
(
[YahooOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[YahooOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_YahooOrderSearch] on [dbo].[YahooOrderSearch]'
GO
ALTER TABLE [dbo].[YahooOrderSearch] ADD CONSTRAINT [PK_YahooOrderSearch] PRIMARY KEY CLUSTERED  ([YahooOrderSearchID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrderSearch]'
GO
ALTER TABLE [dbo].[AmazonOrderSearch] ADD CONSTRAINT [FK_AmazonOrderSearch_AmazonOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[AmazonOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderSearch] ADD CONSTRAINT [FK_ChannelAdvisorOrderSearch_ChannelAdvisorOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ChannelAdvisorOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ClickCartProOrderSearch]'
GO
ALTER TABLE [dbo].[ClickCartProOrderSearch] ADD CONSTRAINT [FK_ClickCartProOrderSearch_ClickCartProOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ClickCartProOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[CommerceInterfaceOrderSearch]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrderSearch] ADD CONSTRAINT [FK_CommerceInterfaceOrderSearch_CommerceInterfaceOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[CommerceInterfaceOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderSearch]'
GO
ALTER TABLE [dbo].[EbayOrderSearch] ADD CONSTRAINT [FK_EbayOrderSearch_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[GrouponOrderSearch]'
GO
ALTER TABLE [dbo].[GrouponOrderSearch] ADD CONSTRAINT [FK_GrouponOrderSearch_GrouponOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[GrouponOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[JetOrderSearch]'
GO
ALTER TABLE [dbo].[JetOrderSearch] ADD CONSTRAINT [FK_JetOrderSearch_JetOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[JetOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[LemonStandOrderSearch]'
GO
ALTER TABLE [dbo].[LemonStandOrderSearch] ADD CONSTRAINT [FK_LemonStandOrderSearch_LemonStandOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[LemonStandOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[MagentoOrderSearch]'
GO
ALTER TABLE [dbo].[MagentoOrderSearch] ADD CONSTRAINT [FK_MagentoOrderSearch_MagentoOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[MagentoOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorOrderSearch]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrderSearch] ADD CONSTRAINT [FK_MarketplaceAdvisorOrderSearch_MarketplaceAdvisorOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[MarketplaceAdvisorOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsOrderSearch]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrderSearch] ADD CONSTRAINT [FK_NetworkSolutionsOrderSearch_NetworkSolutionsOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[NetworkSolutionsOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionOrderSearch]'
GO
ALTER TABLE [dbo].[OrderMotionOrderSearch] ADD CONSTRAINT [FK_OrderMotionOrderSearch_OrderMotionOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[OrderMotionOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PayPalOrderSearch]'
GO
ALTER TABLE [dbo].[PayPalOrderSearch] ADD CONSTRAINT [FK_PayPalOrderSearch_PayPalOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[PayPalOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresOrderSearch]'
GO
ALTER TABLE [dbo].[ProStoresOrderSearch] ADD CONSTRAINT [FK_ProStoresOrderSearch_ProStoresOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ProStoresOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[SearsOrderSearch]'
GO
ALTER TABLE [dbo].[SearsOrderSearch] ADD CONSTRAINT [FK_SearsOrderSearch_SearsOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[SearsOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShopifyOrderSearch]'
GO
ALTER TABLE [dbo].[ShopifyOrderSearch] ADD CONSTRAINT [FK_ShopifyOrderSearch_ShopifyOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ShopifyOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrderSearch]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderSearch] ADD CONSTRAINT [FK_ThreeDCartOrderSearch_ThreeDCartOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[ThreeDCartOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WalmartOrderSearch]'
GO
ALTER TABLE [dbo].[WalmartOrderSearch] ADD CONSTRAINT [FK_WalmartOrderSearch_WalmartOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[WalmartOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrderSearch]'
GO
ALTER TABLE [dbo].[YahooOrderSearch] ADD CONSTRAINT [FK_YahooOrderSearch_YahooOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[YahooOrder] ([OrderID]) ON DELETE CASCADE
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[OverstockOrder]'
GO
CREATE TABLE [dbo].[OverstockOrder]
(
[OrderID] [bigint] NOT NULL,
[SalesChannelName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WarehouseCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SofsCreatedDate] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OverstockOrder] on [dbo].[OverstockOrder]'
GO
ALTER TABLE [dbo].[OverstockOrder] ADD CONSTRAINT [PK_OverstockOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OverstockOrder_SalesChannelName] ON [dbo].[OverstockOrder]
(
	[SalesChannelName] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OverstockOrder_WarehouseCode] ON [dbo].[OverstockOrder]
(
	[WarehouseCode] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_OverstockOrder_SofsCreatedDate] ON [dbo].[OverstockOrder]
(
	[SofsCreatedDate] ASC
) ON [PRIMARY]
GO

PRINT N'Creating [dbo].[OverstockOrderItem]'
GO
CREATE TABLE [dbo].[OverstockOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[SalesChannelLineNumber] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OverstockOrderItem] on [dbo].[OverstockOrderItem]'
GO
ALTER TABLE [dbo].[OverstockOrderItem] ADD CONSTRAINT [PK_OverstockOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[OverstockOrderSearch]'
GO
CREATE TABLE [dbo].[OverstockOrderSearch]
(
[OverstockOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[SalesChannelName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WarehouseCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OverstockOrderSearch] on [dbo].[OverstockOrderSearch]'
GO
ALTER TABLE [dbo].[OverstockOrderSearch] ADD CONSTRAINT [PK_OverstockOrderSearch] PRIMARY KEY CLUSTERED  ([OverstockOrderSearchID])
GO
PRINT N'Creating [dbo].[OverstockStore]'
GO
CREATE TABLE [dbo].[OverstockStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_OverstockStore] on [dbo].[OverstockStore]'
GO
ALTER TABLE [dbo].[OverstockStore] ADD CONSTRAINT [PK_OverstockStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OverstockOrderItem]'
GO
ALTER TABLE [dbo].[OverstockOrderItem] ADD CONSTRAINT [FK_OverstockOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[OverstockOrderSearch]'
GO
ALTER TABLE [dbo].[OverstockOrderSearch] ADD CONSTRAINT [FK_OverstockOrderSearch_OverstockOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[OverstockOrder] ([OrderID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OverstockOrder]'
GO
ALTER TABLE [dbo].[OverstockOrder] ADD CONSTRAINT [FK_OverstockOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OverstockStore]'
GO
ALTER TABLE [dbo].[OverstockStore] ADD CONSTRAINT [FK_OverstockStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating custom types'
GO
CREATE TYPE dbo.LongList AS TABLE ( item BIGINT );
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiToken'
GO
EXEC sp_addextendedproperty N'AuditName', N'Api Token', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiToken'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUrl'
GO
EXEC sp_addextendedproperty N'AuditName', N'Api Url', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUrl'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUserName'
GO
EXEC sp_addextendedproperty N'AuditName', N'Api User Name', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUserName'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BigCommerceStore', @level2type=N'COLUMN',@level2name=N'BigCommerceAuthentication'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditName', @value=N'BigCommerce Authentication Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BigCommerceStore', @level2type=N'COLUMN',@level2name=N'BigCommerceAuthentication'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BigCommerceStore', @level2type=N'COLUMN',@level2name=N'OauthClientId'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditName', @value=N'OAuth Client ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BigCommerceStore', @level2type=N'COLUMN',@level2name=N'OauthClientId'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BigCommerceStore', @level2type=N'COLUMN',@level2name=N'OauthToken'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditName', @value=N'OAuth Token' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BigCommerceStore', @level2type=N'COLUMN',@level2name=N'OauthToken'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'BillCountry', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'BillState', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipCountry', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'ShipStateProvCode'
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
EXEC sp_addextendedproperty N'AuditFormat', N'128', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
EXEC sp_addextendedproperty N'AuditName', N'Shipping Method', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SellingManagerRecord'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'EndiciaAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'RefundFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'TransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'121', 'SCHEMA', N'dbo', 'TABLE', N'EtsyOrder', 'COLUMN', N'WasPaid'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'120', 'SCHEMA', N'dbo', 'TABLE', N'EtsyOrder', 'COLUMN', N'WasShipped'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCompany'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerEmail'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerFirstName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerLastName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhoneExtension'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAccountNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAddFreight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD Amount', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodChargeBasis'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCompany'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCountryCode'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTIN'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceReference'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceTermsOfSale'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAdmissibilityPackaging'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAESEEI'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsDescription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsOnly'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientTIN'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'123', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Dropoff Type', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyBroker'
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
EXEC sp_addextendedproperty N'AuditName', N'Hold At Location Selected', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FedExHoldAtLocationEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightBookingNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsideDelivery'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsidePickup'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightLoadAndCount'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'MasterFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'111', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditName', N'Origin Residential \ Commercial', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'109', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PackagingType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Account', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesAccount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Country', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Name', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'110', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees To', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transportation Account', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportAccount'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transportation Name', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'110', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transporation To', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'124', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ReturnType'
GO
EXEC sp_addextendedproperty N'AuditName', N'RMA Number', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaNumber'
GO
EXEC sp_addextendedproperty N'AuditName', N'RMA Reason', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaReason'
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
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Note', 'COLUMN', N'ObjectID'
GO
EXEC sp_addextendedproperty N'AuditName', N'RelatedTo', 'SCHEMA', N'dbo', 'TABLE', N'Note', 'COLUMN', N'ObjectID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'101', 'SCHEMA', N'dbo', 'TABLE', N'Note', 'COLUMN', N'Source'
GO
EXEC sp_addextendedproperty N'AuditName', N'Note', 'SCHEMA', N'dbo', 'TABLE', N'Note', 'COLUMN', N'Text'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'102', 'SCHEMA', N'dbo', 'TABLE', N'Note', 'COLUMN', N'Visibility'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'BillCountry', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'BillState', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'BillStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'CustomerID'
GO
EXEC sp_addextendedproperty N'AuditName', N'Customer', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'CustomerID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OnlineCustomerID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OnlineStatusCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderNumber'
GO
EXEC sp_addextendedproperty N'AuditName', N'Order Number', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderNumberComplete'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'OrderTotal'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipCountry', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
EXEC sp_addextendedproperty N'AuditName', N'Store', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'StoreID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderCharge', 'COLUMN', N'Amount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'UnitCost'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'UnitPrice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'Weight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderItemAttribute', 'COLUMN', N'UnitPrice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OtherShipment', 'COLUMN', N'InsuranceValue'
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
EXEC sp_addextendedproperty N'AuditFormat', N'122', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'FulfillmentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Fulfillment Status', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'FulfillmentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'121', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'PaymentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Fulfillment Status', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyOrder', 'COLUMN', N'PaymentStatusCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyAccessToken'
GO
EXEC sp_addextendedproperty N'AuditName', N'Access Token', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyAccessToken'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyShopUrlName'
GO
EXEC sp_addextendedproperty N'AuditName', N'Shop Name', 'SCHEMA', N'dbo', 'TABLE', N'ShopifyStore', 'COLUMN', N'ShopifyShopUrlName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UspsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'UspsShipment', 'COLUMN', N'UspsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UspsShipment', 'COLUMN', N'UspsTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
EXEC sp_addextendedproperty N'AuditName', N'User Key', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO
EXEC sp_addextendedproperty N'AuditName', N'Store URL', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD Amount', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodPaymentType'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialPaperlessInvoice'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'CarrierName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'ShippingServiceName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'InsuranceValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsProfileID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsAddWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'129', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DeliveryExperience'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DeclaredValue'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AsendiaShipment', @level2type=N'COLUMN',@level2name=N'AsendiaAccountID'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'130' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AsendiaShipment', @level2type=N'COLUMN',@level2name=N'Service'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DhlExpressShipment', @level2type=N'COLUMN',@level2name=N'DhlExpressAccountID'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'130' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DhlExpressShipment', @level2type=N'COLUMN',@level2name=N'Service'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DhlExpressShipment', @level2type=N'COLUMN',@level2name=N'DeliveredDutyPaid'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DhlExpressShipment', @level2type=N'COLUMN',@level2name=N'NonMachinable'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DhlExpressShipment', @level2type=N'COLUMN',@level2name=N'SaturdayDelivery'
GO

PRINT N'Adding FilterNodeSetSwFilterNodeID trigger'
GO
CREATE TRIGGER FilterNodeSetSwFilterNodeID
   ON  FilterNode
   WITH ENCRYPTION
   AFTER INSERT,UPDATE
AS
BEGIN
	SET NOCOUNT ON;

    declare @FilterNodeID bigint
    declare @FilterNodeContentID bigint
	select @FilterNodeID = FilterNodeID, @FilterNodeContentID = FilterNodeContentID from inserted

	update FilterNodeContent
	set EntityExistsQuery = REPLACE(EntityExistsQuery, '<SwFilterNodeID />', convert(nvarchar(40), @FilterNodeID))
	where FilterNodeContentID = @FilterNodeContentID
END
GO

PRINT 'Creating FilterInfo view.'
GO
IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[FilterInfo]'))
	DROP VIEW [dbo].[FilterInfo]
GO
CREATE VIEW FilterInfo WITH ENCRYPTION AS
	SELECT f.Name, f.FilterID, f.IsFolder, f.State, f.[Definition], n.FilterNodeID, c.*
		FROM FilterNode n INNER JOIN FilterSequence s ON n.FilterSequenceID = s.FilterSequenceID
						  INNER JOIN Filter f ON s.FilterID = f.FilterID
						  INNER JOIN FilterNodeContent c ON n.FilterNodeContentID = c.FilterNodeContentID
GO

PRINT 'Creating AreFilterCountsUpToDate.'
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AreFilterCountsUpToDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[AreFilterCountsUpToDate]
END
GO
CREATE FUNCTION [dbo].[AreFilterCountsUpToDate](@currentDbts binary(8))
RETURNS BIT
WITH ENCRYPTION
AS
BEGIN
	DECLARE @upToDate bit = 0
	DECLARE @rowVersion rowversion
	DECLARE @filterNodeContentDirtyMinRowVersion timestamp

	IF (@currentDbts IS NULL OR @currentDbts = CONVERT(VARBINARY, 0x0))
	BEGIN
		SELECT @rowVersion = @@DBTS
	END
	ELSE
	BEGIN
		SELECT @rowVersion = CONVERT(rowversion, @currentDbts)
	END

	SELECT @filterNodeContentDirtyMinRowVersion = MIN(RowVersion) FROM FilterNodeContentDirty WITH (NOLOCK)

	IF @filterNodeContentDirtyMinRowVersion is null or len(@filterNodeContentDirtyMinRowVersion) = 0
	BEGIN
		SET @upToDate = 1
	END
	ELSE
	BEGIN
		IF @filterNodeContentDirtyMinRowVersion > @rowVersion
		BEGIN
			SET @upToDate = 1
		END
	END

	RETURN(@upToDate);
END
GO

PRINT 'Creating AreQuickFilterCountsUpToDate.'
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AreQuickFilterCountsUpToDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[AreQuickFilterCountsUpToDate]
END
GO

CREATE FUNCTION dbo.AreQuickFilterCountsUpToDate(@currentDbts binary(8))
RETURNS BIT
WITH ENCRYPTION
AS
BEGIN
	DECLARE @upToDate bit = 0
	DECLARE @rowVersion rowversion
	DECLARE @quickFilterNodeContentDirtyMinRowVersion timestamp

	IF (@currentDbts IS NULL OR @currentDbts = CONVERT(VARBINARY, 0x0))
	BEGIN
		SELECT @rowVersion = @@DBTS
	END
	ELSE
	BEGIN
		SELECT @rowVersion = CONVERT(rowversion, @currentDbts)
	END

	SELECT @quickFilterNodeContentDirtyMinRowVersion = MIN(RowVersion) FROM QuickFilterNodeContentDirty WITH (NOLOCK)

	IF @quickFilterNodeContentDirtyMinRowVersion is null or len(@quickFilterNodeContentDirtyMinRowVersion) = 0
	BEGIN
		SET @upToDate = 1
	END
	ELSE
	BEGIN
		IF @quickFilterNodeContentDirtyMinRowVersion > @rowVersion
		BEGIN
			SET @upToDate = 1
		END
	END

	RETURN(@upToDate);
END
GO

PRINT N'Adding DoesFilterNodeApplyToEntity stored procedure'
GO
CREATE PROCEDURE DoesFilterNodeApplyToEntity
(
	@entityID BIGINT,
	@filterNodeID BIGINT
)
WITH ENCRYPTION
AS
BEGIN

	DECLARE @upToDate bit
	SELECT @upToDate = dbo.AreFilterCountsUpToDate(NULL) & dbo.AreFilterCountsUpToDate(NULL)

	IF @upToDate = 1
	BEGIN
		SELECT fi.FilterNodeID
		FROM FilterNodeContentDetail fncd, FilterInfo fi
		WHERE fncd.FilterNodeContentID = fi.FilterNodeContentID
		  AND fncd.ObjectID = @entityID
		  AND fi.FilterNodeID = @filterNodeID
	END
	ELSE
	BEGIN
		DECLARE @sql nvarchar(max)
		select @sql = fn.EntityExistsQuery
		from FilterInfo fn
		WHERE fn.FilterNodeID = @filterNodeID

		EXECUTE sp_executesql @sql, N'@ExistsQueryObjectID bigint, @filterNodeID bigint', @ExistsQueryObjectID = @entityID, @filterNodeID = @filterNodeID
	END
END
GO

PRINT N'Creating [dbo].[Product]'
GO
CREATE TABLE [dbo].[Product]
(
[ProductID] [bigint] NOT NULL IDENTITY(1, 1),
[CreatedDate] [datetime] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsActive] [bit] NOT NULL,
[IsBundle] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Product] on [dbo].[Product]'
GO
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED  ([ProductID])
GO
PRINT N'Creating [dbo].[ProductBundle]'
GO
CREATE TABLE [dbo].[ProductBundle]
(
[ProductID] [bigint] NOT NULL,
[ChildProductVariantID] [bigint] NOT NULL,
[Quantity] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProductBundle] on [dbo].[ProductBundle]'
GO
ALTER TABLE [dbo].[ProductBundle] ADD CONSTRAINT [PK_ProductBundle] PRIMARY KEY CLUSTERED  ([ProductID], [ChildProductVariantID])
GO
PRINT N'Creating [dbo].[ProductVariant]'
GO
CREATE TABLE [dbo].[ProductVariant]
(
[ProductVariantID] [bigint] NOT NULL IDENTITY(1, 1),
[ProductID] [bigint] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsActive] [bit] NOT NULL,
[UPC] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ASIN] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ISBN] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Weight] [decimal] (29, 9) NULL,
[Length] [decimal] (10, 2) NULL,
[Width] [decimal] (10, 2) NULL,
[Height] [decimal] (10, 2) NULL,
[ImageUrl] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BinLocation] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HarmonizedCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DeclaredValue] [money] NULL,
[CountryOfOrigin] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_ProductVariant] on [dbo].[ProductVariant]'
GO
ALTER TABLE [dbo].[ProductVariant] ADD CONSTRAINT [PK_ProductVariant] PRIMARY KEY CLUSTERED  ([ProductVariantID])
GO
PRINT N'Creating [dbo].[ProductVariantAlias]'
GO
CREATE TABLE [dbo].[ProductVariantAlias]
(
[ProductVariantAliasID] [bigint] NOT NULL IDENTITY(1, 1),
[ProductVariantID] [bigint] NOT NULL,
[AliasName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Sku] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsDefault] [bit] NOT NULL CONSTRAINT [DF_ProductVariantAlias_IsDefault] DEFAULT ((0))
)
GO
PRINT N'Creating primary key [PK_ProductVariantAlias] on [dbo].[ProductVariantAlias]'
GO
ALTER TABLE [dbo].[ProductVariantAlias] ADD CONSTRAINT [PK_ProductVariantAlias] PRIMARY KEY CLUSTERED  ([ProductVariantAliasID])
GO
PRINT N'Creating index [IX_SWDefault_ProductVariantAlias_Sku] on [dbo].[ProductVariantAlias]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAlias_Sku] ON [dbo].[ProductVariantAlias] ([Sku]) INCLUDE ([ProductVariantID])
GO
PRINT N'Creating index [IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku] on [dbo].[ProductVariantAlias]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku] ON [dbo].[ProductVariantAlias]
(
	[ProductVariantID] ASC,
	[IsDefault] ASC
)
INCLUDE ([Sku])  ON [PRIMARY]
GO
PRINT N'Creating [dbo].[ProductVariantAttribute]'
GO
CREATE TABLE [dbo].[ProductVariantAttribute]
(
[ProductVariantAttributeID] [bigint] NOT NULL IDENTITY(1, 1),
[ProductVariantID] [bigint] NOT NULL,
[AttributeName] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AttributeValue] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProductVariantAttribute] on [dbo].[ProductVariantAttribute]'
GO
ALTER TABLE [dbo].[ProductVariantAttribute] ADD CONSTRAINT [PK_ProductVariantAttribute] PRIMARY KEY CLUSTERED  ([ProductVariantAttributeID])
GO
PRINT N'Creating index [IX_SWDefault_ProductVariantAttribute_ProductVariantID] on [dbo].[ProductVariantAttribute]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAttribute_ProductVariantID] ON [dbo].[ProductVariantAttribute] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductBundle]'
GO
ALTER TABLE [dbo].[ProductBundle] ADD CONSTRAINT [FK_ProductBundle_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ProductBundle] ADD CONSTRAINT [FK_ProductBundle_ProductVariant] FOREIGN KEY ([ChildProductVariantID]) REFERENCES [dbo].[ProductVariant] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductVariantAlias]'
GO
ALTER TABLE [dbo].[ProductVariantAlias] ADD CONSTRAINT [FK_ProductVariantAlias_ProductVariant] FOREIGN KEY ([ProductVariantID]) REFERENCES [dbo].[ProductVariant] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductVariantAttribute]'
GO
ALTER TABLE [dbo].[ProductVariantAttribute] ADD CONSTRAINT [FK_ProductVariantAttribute_ProductVariant] FOREIGN KEY ([ProductVariantID]) REFERENCES [dbo].[ProductVariant] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductVariant]'
GO
ALTER TABLE [dbo].[ProductVariant] ADD CONSTRAINT [FK_ProductVariant_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID])
GO
