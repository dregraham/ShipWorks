SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Creating [dbo].[ObjectReference]'
GO
CREATE TABLE [dbo].[ObjectReference]
(
[ObjectReferenceID] [bigint] NOT NULL IDENTITY(1030, 1000),
[ConsumerID] [bigint] NOT NULL,
[ReferenceKey] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ObjectReference_ReferenceKey] DEFAULT (''),
[ObjectID] [bigint] NOT NULL,
[Reason] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_ObjectReference] on [dbo].[ObjectReference]'
GO
ALTER TABLE [dbo].[ObjectReference] ADD CONSTRAINT [PK_ObjectReference] PRIMARY KEY CLUSTERED  ([ObjectReferenceID])
GO
PRINT N'Creating index [IX_ObjectReference] on [dbo].[ObjectReference]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectReference] ON [dbo].[ObjectReference] ([ConsumerID], [ReferenceKey])
GO
PRINT N'Creating [dbo].[FilterNodeContentRemoved]'
GO
CREATE TABLE [dbo].[FilterNodeContentRemoved]
(
[ObjectType] [int] NOT NULL,
[ObjectID] [bigint] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[DeletionDate] [datetime] NOT NULL CONSTRAINT [DF_ObjectDeletion_DeletionDate] DEFAULT (getutcdate())
)
GO
PRINT N'Creating index [IX_ObjectDeletion_RowVersion] on [dbo].[FilterNodeContentRemoved]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectDeletion_RowVersion] ON [dbo].[FilterNodeContentRemoved] ([ObjectType], [RowVersion])
GO
PRINT N'Creating index [IX_ObjectDeletion_Date] on [dbo].[FilterNodeContentRemoved]'
GO
CREATE NONCLUSTERED INDEX [IX_ObjectDeletion_Date] ON [dbo].[FilterNodeContentRemoved] ([DeletionDate])
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
PRINT N'Creating primary key [PK_WorldShipPackage] on [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD CONSTRAINT [PK_WorldShipPackage] PRIMARY KEY CLUSTERED  ([UpsPackageID])
GO
PRINT N'Creating [dbo].[Action]'
GO
CREATE TABLE [dbo].[Action]
(
[ActionID] [bigint] NOT NULL IDENTITY(1040, 1000),
[RowVersion] [timestamp] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Enabled] [bit] NOT NULL,
[ComputerLimited] [bit] NOT NULL,
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
PRINT N'Creating [dbo].[ActionFilterTrigger]'
GO
CREATE TABLE [dbo].[ActionFilterTrigger]
(
[ActionID] [bigint] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[Direction] [int] NOT NULL,
[ComputerLimited] [bit] NOT NULL
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
PRINT N'Creating [dbo].[AmazonStore]'
GO
CREATE TABLE [dbo].[AmazonStore]
(
[StoreID] [bigint] NOT NULL,
[SellerCentralUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellerCentralPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantName] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MerchantToken] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccessKeyID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Cookie] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CookieExpires] [datetime] NOT NULL,
[CookieWaitUntil] [datetime] NOT NULL,
[Certificate] [varbinary] (2048) NULL,
[WeightDownloads] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
[OnlineLastModified] [datetime] NOT NULL,
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
[BillEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[ShipEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RollupItemCount] [int] NOT NULL,
[RollupItemName] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemSKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemLocation] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupItemQuantity] [float] NULL,
[RollupItemTotalWeight] [float] NOT NULL,
[RollupNoteCount] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Order] on [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating index [IX_Auto_BillCity] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCity] ON [dbo].[Order] ([BillCity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Order] ([BillCompany]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order] ([BillCountryCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Order] ([BillEmail]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillFax] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillFax] ON [dbo].[Order] ([BillFax]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillFirstName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillFirstName] ON [dbo].[Order] ([BillFirstName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Order] ([BillLastName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillMiddleName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillMiddleName] ON [dbo].[Order] ([BillMiddleName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillPhone] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPhone] ON [dbo].[Order] ([BillPhone]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order] ([BillPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order] ([BillStateProvCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStreet1] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet1] ON [dbo].[Order] ([BillStreet1]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStreet2] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet2] ON [dbo].[Order] ([BillStreet2]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillStreet3] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet3] ON [dbo].[Order] ([BillStreet3]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_BillWebsite] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillWebsite] ON [dbo].[Order] ([BillWebsite]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_CustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_CustomerID] ON [dbo].[Order] ([CustomerID]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_IsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_IsManual] ON [dbo].[Order] ([IsManual]) INCLUDE ([RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_LocalStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_LocalStatus] ON [dbo].[Order] ([LocalStatus]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OnlineLastModified] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineLastModified] ON [dbo].[Order] ([OnlineLastModified]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OnlineStatus] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order] ([OnlineStatus]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderDate] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate] ON [dbo].[Order] ([OrderDate]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderNumber] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumber] ON [dbo].[Order] ([OrderNumber]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderNumberComplete] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order] ([OrderNumberComplete]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_OrderTotal] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderTotal] ON [dbo].[Order] ([OrderTotal]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RequestedShipping] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order] ([RequestedShipping]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order] ([RollupItemCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order] ([RollupItemCount]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemLocation] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemLocation] ON [dbo].[Order] ([RollupItemLocation]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemQuantity] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemQuantity] ON [dbo].[Order] ([RollupItemQuantity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemSKU] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupItemTotalWeight] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemTotalWeight] ON [dbo].[Order] ([RollupItemTotalWeight]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_RollupNoteCount] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order] ([RollupNoteCount]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCity] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCity] ON [dbo].[Order] ([ShipCity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCompany] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Order] ([ShipCompany]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipCountryCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order] ([ShipCountryCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Order] ([ShipEmail]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipFax] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipFax] ON [dbo].[Order] ([ShipFax]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipFirstName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipFirstName] ON [dbo].[Order] ([ShipFirstName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipLastName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Order] ([ShipLastName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipMiddleName] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Order] ([ShipMiddleName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipPhone] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPhone] ON [dbo].[Order] ([ShipPhone]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStateProvCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order] ([ShipStateProvCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStreet1] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet1] ON [dbo].[Order] ([ShipStreet1]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStreet2] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet2] ON [dbo].[Order] ([ShipStreet2]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipStreet3] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet3] ON [dbo].[Order] ([ShipStreet3]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_ShipWebsite] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipWebsite] ON [dbo].[Order] ([ShipWebsite]) INCLUDE ([IsManual], [RowVersion], [StoreID])
GO
PRINT N'Creating index [IX_Auto_StoreID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_StoreID] ON [dbo].[Order] ([StoreID]) INCLUDE ([IsManual], [RowVersion])
GO
PRINT N'Creating index [IX_OnlineLastModified_StoreID_IsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order] ([OnlineLastModified] DESC, [StoreID], [IsManual])
GO
PRINT N'Creating index [IX_OnlineCustomerID] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_OnlineCustomerID] ON [dbo].[Order] ([OnlineCustomerID])
GO
PRINT N'Creating [dbo].[AmazonOrder]'
GO
CREATE TABLE [dbo].[AmazonOrder]
(
[OrderID] [bigint] NOT NULL,
[AmazonOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonCommission] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonOrder] on [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD CONSTRAINT [PK_AmazonOrder] PRIMARY KEY CLUSTERED  ([OrderID])
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
[Weight] [float] NOT NULL,
[TotalWeight] AS ([Weight]*[Quantity]),
[Quantity] [float] NOT NULL,
[LocalStatus] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderItem] on [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating index [IX_OrderItem_OrderID] on [dbo].[OrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem] ([OrderID])
GO
PRINT N'Creating [dbo].[AmazonOrderItem]'
GO
CREATE TABLE [dbo].[AmazonOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[AmazonOrderItemCode] [bigint] NOT NULL,
[ASIN] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ConditionNote] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonOrderItem] on [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] ADD CONSTRAINT [PK_AmazonOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[Store]'
GO
CREATE TABLE [dbo].[Store]
(
[StoreID] [bigint] NOT NULL IDENTITY(1005, 1000),
[RowVersion] [timestamp] NOT NULL,
[License] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TypeCode] [int] NOT NULL,
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
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsSetupComplete] [bit] NOT NULL,
[AutoDownload] [bit] NOT NULL,
[AutoDownloadMinutes] [int] NOT NULL,
[AutoDownloadOnlyAway] [bit] NOT NULL,
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
PRINT N'Creating index [IX_Store_StoreName] on [dbo].[Store]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Store_StoreName] ON [dbo].[Store] ([StoreName])
GO
ALTER TABLE [dbo].[Store] ENABLE CHANGE_TRACKING
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
PRINT N'Creating index [IX_Audit_TransactionID] on [dbo].[Audit]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Audit_TransactionID] ON [dbo].[Audit] ([TransactionID])
GO
PRINT N'Creating index [IX_Audit_Action] on [dbo].[Audit]'
GO
CREATE NONCLUSTERED INDEX [IX_Audit_Action] ON [dbo].[Audit] ([Action])
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
PRINT N'Creating index [IX_User_Username] on [dbo].[User]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Username] ON [dbo].[User] ([Username])
GO
ALTER TABLE [dbo].[User] ENABLE CHANGE_TRACKING
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
PRINT N'Creating index [IX_AuditChange] on [dbo].[AuditChange]'
GO
CREATE NONCLUSTERED INDEX [IX_AuditChange] ON [dbo].[AuditChange] ([AuditID])
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
PRINT N'Creating index [IX_AuditChangeDetail] on [dbo].[AuditChangeDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail] ON [dbo].[AuditChangeDetail] ([AuditChangeID])
GO
PRINT N'Creating index [IX_AuditChangeDetail_VariantNew] on [dbo].[AuditChangeDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail_VariantNew] ON [dbo].[AuditChangeDetail] ([VariantNew]) INCLUDE ([AuditID])
GO
PRINT N'Creating [dbo].[ChannelAdvisorOrder]'
GO
CREATE TABLE [dbo].[ChannelAdvisorOrder]
(
[OrderID] [bigint] NOT NULL,
[CustomOrderIdentifier] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ResellerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrder] on [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD CONSTRAINT [PK_ChannelAdvisorOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE TABLE [dbo].[ChannelAdvisorOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[SiteName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SalesSourceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Classification] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DistributionCenter] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderItem] on [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[ChannelAdvisorStore]'
GO
CREATE TABLE [dbo].[ChannelAdvisorStore]
(
[StoreID] [bigint] NOT NULL,
[AccountKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DownloadCriteria] [smallint] NOT NULL,
[ProfileID] [int] NOT NULL,
[ProfileUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProfilePassword] [nvarchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
[CommerceInterfaceOrderNumber] [nchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
PRINT N'Creating index [IX_DownloadLog_StoreID_Ended] on [dbo].[Download]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadLog_StoreID_Ended] ON [dbo].[Download] ([StoreID], [Ended])
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
[AmazonOrderID] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EbayOrderID] [bigint] NULL,
[EbayItemID] [bigint] NULL,
[EbayTransactionID] [bigint] NULL,
[PayPalTransactionID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[YahooOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NetworkSolutionsOrderID] [bigint] NULL,
[OrderMotionShipmentID] [int] NULL,
[ClickCartProOrderID] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_DownloadDetail] on [dbo].[DownloadDetail]'
GO
ALTER TABLE [dbo].[DownloadDetail] ADD CONSTRAINT [PK_DownloadDetail] PRIMARY KEY CLUSTERED  ([DownloadedDetailID])
GO
PRINT N'Creating index [IX_DownloadDetail_OrderNumber] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_OrderNumber] ON [dbo].[DownloadDetail] ([OrderNumber])
GO
PRINT N'Creating index [IX_DownloadDetail_AmazonOrderID] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_AmazonOrderID] ON [dbo].[DownloadDetail] ([AmazonOrderID])
GO
PRINT N'Creating index [IX_DownloadDetail_Ebay] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_Ebay] ON [dbo].[DownloadDetail] ([EbayItemID], [EbayOrderID], [EbayTransactionID])
GO
PRINT N'Creating [dbo].[EbayOrder]'
GO
CREATE TABLE [dbo].[EbayOrder]
(
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[EbayBuyerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BuyerFeedbackScore] [int] NOT NULL,
[BuyerFeedbackPrivate] [bit] NOT NULL,
[CombinedLocally] [bit] NOT NULL,
[RollupEbayItemCount] [int] NOT NULL,
[RollupEffectiveCheckoutStatus] [int] NULL,
[RollupEffectivePaymentMethod] [int] NULL,
[RollupFeedbackLeftType] [int] NULL,
[RollupFeedbackLeftComments] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupFeedbackReceivedType] [int] NULL,
[RollupFeedbackReceivedComments] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupPayPalAddressStatus] [int] NULL,
[RollupSellingManagerRecord] [int] NULL
)
GO
PRINT N'Creating primary key [PK_EbayOrder] on [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD CONSTRAINT [PK_EbayOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Creating [dbo].[EbayOrderItem]'
GO
CREATE TABLE [dbo].[EbayOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[OrderID] [bigint] NOT NULL,
[EbayItemID] [bigint] NOT NULL,
[EbayTransactionID] [bigint] NOT NULL,
[SellingManagerProductName] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellingManagerProductPart] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SellingManagerRecord] [int] NOT NULL,
[EffectiveCheckoutStatus] [int] NOT NULL,
[EffectivePaymentMethod] [int] NOT NULL,
[PaymentStatus] [int] NOT NULL,
[PaymentMethod] [int] NOT NULL,
[CheckoutStatus] [int] NOT NULL,
[CompleteStatus] [int] NOT NULL,
[SellerPaidStatus] [int] NOT NULL,
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
[DownloadPayPalDetails] [bit] NOT NULL,
[PayPalApiCredentialType] [smallint] NOT NULL,
[PayPalApiUserName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiPassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiSignature] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiCertificate] [varbinary] (2048) NULL,
[DomesticShippingService] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternationalShippingService] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
PRINT N'Creating index [IX_EmailOutbound] on [dbo].[EmailOutbound]'
GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound] ON [dbo].[EmailOutbound] ([SendStatus], [AccountID], [DontSendBefore], [SentDate], [ComposedDate])
GO
ALTER TABLE [dbo].[EmailOutbound] ENABLE CHANGE_TRACKING
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
PRINT N'Creating index [IX_EmailOutbound_Email] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound_Email] ON [dbo].[EmailOutboundRelation] ([EmailOutboundID], [RelationType]) INCLUDE ([ObjectID])
GO
PRINT N'Creating index [IX_EmailOutbound_Object] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound_Object] ON [dbo].[EmailOutboundRelation] ([ObjectID], [RelationType]) INCLUDE ([EmailOutboundID])
GO
PRINT N'Creating [dbo].[PostalProfile]'
GO
CREATE TABLE [dbo].[PostalProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[Service] [int] NULL,
[Confirmation] [int] NULL,
[Weight] [float] NULL,
[PackagingType] [int] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[NonRectangular] [bit] NULL,
[NonMachinable] [bit] NULL,
[CustomsContentType] [int] NULL,
[CustomsContentDescription] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InsuranceType] [int] NULL
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
[NoPostage] [bit] NULL,
[ReferenceID] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RubberStamp1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RubberStamp2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RubberStamp3] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_EndiciaProfile] on [dbo].[EndiciaProfile]'
GO
ALTER TABLE [dbo].[EndiciaProfile] ADD CONSTRAINT [PK_EndiciaProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
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
[InsuranceType] [int] NOT NULL,
[InsuranceValue] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_PostalShipment] on [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [PK_PostalShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[EndiciaShipment]'
GO
CREATE TABLE [dbo].[EndiciaShipment]
(
[ShipmentID] [bigint] NOT NULL,
[EndiciaAccountID] [bigint] NOT NULL,
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
PRINT N'Creating primary key [PK_EndiciaShipment] on [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [PK_EndiciaShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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
[PayorTransportType] [int] NOT NULL,
[PayorTransportAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorDutiesType] [int] NOT NULL,
[PayorDutiesAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[HomeDeliveryType] [int] NOT NULL,
[HomeDeliveryInstructions] [varchar] (74) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HomeDeliveryDate] [datetime] NOT NULL,
[HomeDeliveryPhone] [varchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FreightInsidePickup] [bit] NOT NULL,
[FreightInsideDelivery] [bit] NOT NULL,
[FreightBookingNumber] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FreightLoadAndCount] [int] NOT NULL,
[EmailNotifySender] [int] NOT NULL,
[EmailNotifyRecipient] [int] NOT NULL,
[EmailNotifyOther] [int] NOT NULL,
[EmailNotifyOtherAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[CodPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTrackingNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTrackingFormID] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[CustomsAdmissibilityPackaging] [int] NOT NULL,
[CustomsRecipientTIN] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [bit] NOT NULL,
[CustomsDocumentsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoice] [bit] NOT NULL,
[CommercialInvoiceTermsOfSale] [int] NOT NULL,
[CommercialInvoicePurpose] [int] NOT NULL,
[CommercialInvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoiceFreight] [money] NOT NULL,
[CommercialInvoiceInsurance] [money] NOT NULL,
[CommercialInvoiceOther] [money] NOT NULL,
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
[InsuranceType] [int] NOT NULL,
[SmartPostIndicia] [int] NOT NULL,
[SmartPostEndorsement] [int] NOT NULL,
[SmartPostConfirmation] [bit] NOT NULL,
[SmartPostCustomerManifest] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostHubID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_FedExShipment] on [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [PK_FedExShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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
[DeclaredValue] [money] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[TrackingNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_FedExPackage] on [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD CONSTRAINT [PK_FedExPackage] PRIMARY KEY CLUSTERED  ([FedExPackageID])
GO
PRINT N'Creating [dbo].[ShippingProfile]'
GO
CREATE TABLE [dbo].[ShippingProfile]
(
[ShippingProfileID] [bigint] NOT NULL IDENTITY(1053, 1000),
[RowVersion] [timestamp] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentType] [int] NOT NULL,
[ShipmentTypePrimary] [bit] NOT NULL,
[OriginID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_ShippingProfile] on [dbo].[ShippingProfile]'
GO
ALTER TABLE [dbo].[ShippingProfile] ADD CONSTRAINT [PK_ShippingProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
ALTER TABLE [dbo].[ShippingProfile] ENABLE CHANGE_TRACKING
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
[PayorTransportType] [int] NULL,
[PayorTransportAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorDutiesType] [int] NULL,
[PayorDutiesAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SaturdayDelivery] [bit] NULL,
[EmailNotifySender] [int] NULL,
[EmailNotifyRecipient] [int] NULL,
[EmailNotifyOther] [int] NULL,
[EmailNotifyOtherAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNotifyMessage] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResidentialDetermination] [int] NULL,
[InsuranceType] [int] NULL,
[SmartPostIndicia] [int] NULL,
[SmartPostEndorsement] [int] NULL,
[SmartPostConfirmation] [bit] NULL,
[SmartPostCustomerManifest] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SmartPostHubID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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
[FedExProfilePackageID] [bigint] NOT NULL IDENTITY(1062, 1000),
[ShippingProfileID] [bigint] NOT NULL,
[DeclaredValue] [money] NULL,
[Weight] [float] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_FedExProfilePackage] on [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD CONSTRAINT [PK_FedExProfilePackage] PRIMARY KEY CLUSTERED  ([FedExProfilePackageID])
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
[OriginWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_Shipment] on [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_Shipment_OrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment] ([OrderID])
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
PRINT N'Creating index [IX_FilterNode_ParentFilterNodeID] on [dbo].[FilterNode]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNode_ParentFilterNodeID] ON [dbo].[FilterNode] ([ParentFilterNodeID])
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
PRINT N'Creating index [IX_FilterLayout] on [dbo].[FilterLayout]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterLayout] ON [dbo].[FilterLayout] ([UserID], [FilterTarget])
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
[Cost] [int] NOT NULL,
[Count] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FilterNodeContent] on [dbo].[FilterNodeContent]'
GO
ALTER TABLE [dbo].[FilterNodeContent] ADD CONSTRAINT [PK_FilterNodeContent] PRIMARY KEY CLUSTERED  ([FilterNodeContentID])
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
PRINT N'Creating index [IX_FilterChild_ParentFilterID] on [dbo].[FilterSequence]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterChild_ParentFilterID] ON [dbo].[FilterSequence] ([ParentFilterID])
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
PRINT N'Creating index [IX_FilterNodeColumnSettings] on [dbo].[FilterNodeColumnSettings]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeColumnSettings] ON [dbo].[FilterNodeColumnSettings] ([UserID], [FilterNodeID])
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
PRINT N'Creating primary key [PK_GridLayout] on [dbo].[GridColumnLayout]'
GO
ALTER TABLE [dbo].[GridColumnLayout] ADD CONSTRAINT [PK_GridLayout] PRIMARY KEY CLUSTERED  ([GridColumnLayoutID])
GO
PRINT N'Creating [dbo].[FilterNodeContentDetail]'
GO
CREATE TABLE [dbo].[FilterNodeContentDetail]
(
[FilterNodeContentID] [bigint] NOT NULL,
[ObjectID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeCountDetail] on [dbo].[FilterNodeContentDetail]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeCountDetail] ON [dbo].[FilterNodeContentDetail] ([FilterNodeContentID], [ObjectID]) WITH (IGNORE_DUP_KEY=ON)
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
[Definition] [xml] NULL
)
GO
PRINT N'Creating primary key [PK_Filter] on [dbo].[Filter]'
GO
ALTER TABLE [dbo].[Filter] ADD CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED  ([FilterID])
GO
PRINT N'Creating [dbo].[GenericStore]'
GO
CREATE TABLE [dbo].[GenericStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleVersion] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePlatform] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleDeveloper] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OnlineStoreCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StatusCodes] [xml] NOT NULL,
[DownloadPageSize] [int] NOT NULL,
[RequestTimeout] [int] NOT NULL,
[DownloadStrategy] [int] NOT NULL,
[OnlineStatusSupport] [int] NOT NULL,
[OnlineStatusDataType] [int] NOT NULL,
[OnlineCustomerSupport] [bit] NOT NULL,
[OnlineCustomerDataType] [int] NOT NULL,
[OnlineShipmentDetails] [bit] NOT NULL,
[HttpExpect100Continue] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_GenericStore] on [dbo].[GenericStore]'
GO
ALTER TABLE [dbo].[GenericStore] ADD CONSTRAINT [PK_GenericStore] PRIMARY KEY CLUSTERED  ([StoreID])
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
PRINT N'Creating index [IX_GridColumnDisplay] on [dbo].[GridColumnFormat]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GridColumnDisplay] ON [dbo].[GridColumnFormat] ([UserID], [ColumnGuid])
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
PRINT N'Creating primary key [PK_GridColumnLayout] on [dbo].[GridColumnPosition]'
GO
ALTER TABLE [dbo].[GridColumnPosition] ADD CONSTRAINT [PK_GridColumnLayout] PRIMARY KEY CLUSTERED  ([GridColumnPositionID])
GO
PRINT N'Creating index [IX_GridLayoutColumn] on [dbo].[GridColumnPosition]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GridLayoutColumn] ON [dbo].[GridColumnPosition] ([GridColumnLayoutID], [ColumnGuid])
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
[MagentoTrackingEmails] [bit] NOT NULL
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
PRINT N'Creating [dbo].[MivaStore]'
GO
CREATE TABLE [dbo].[MivaStore]
(
[StoreID] [bigint] NOT NULL,
[EncryptionPassphrase] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LiveManualOrderNumbers] [bit] NOT NULL,
[SebenzaCheckoutDataEnabled] [bit] NOT NULL,
[OnlineUpdateStrategy] [int] NOT NULL,
[OnlineUpdateStatusChangeEmail] [bit] NOT NULL
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
PRINT N'Creating index [IX_OrderNote_ObjectID] on [dbo].[Note]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderNote_ObjectID] ON [dbo].[Note] ([ObjectID])
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
[BillEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[ShipEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
PRINT N'Creating index [IX_Auto_BillCity] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCity] ON [dbo].[Customer] ([BillCity]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillCompany] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Customer] ([BillCompany]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillCountryCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Customer] ([BillCountryCode]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Customer] ([BillEmail]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillFax] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillFax] ON [dbo].[Customer] ([BillFax]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillFirstName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillFirstName] ON [dbo].[Customer] ([BillFirstName]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillLastName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Customer] ([BillLastName]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillMiddleName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillMiddleName] ON [dbo].[Customer] ([BillMiddleName]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillPhone] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPhone] ON [dbo].[Customer] ([BillPhone]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Customer] ([BillPostalCode]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillStateProvCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Customer] ([BillStateProvCode]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillStreet1] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet1] ON [dbo].[Customer] ([BillStreet1]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillStreet2] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet2] ON [dbo].[Customer] ([BillStreet2]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillStreet3] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet3] ON [dbo].[Customer] ([BillStreet3]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_BillWebsite] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillWebsite] ON [dbo].[Customer] ([BillWebsite]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_RollupNoteCount] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Customer] ([RollupNoteCount]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_RollupOrderCount] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderCount] ON [dbo].[Customer] ([RollupOrderCount]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_RollupOrderTotal] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderTotal] ON [dbo].[Customer] ([RollupOrderTotal]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipCity] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCity] ON [dbo].[Customer] ([ShipCity]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipCompany] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Customer] ([ShipCompany]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipCountryCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Customer] ([ShipCountryCode]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Customer] ([ShipEmail]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipFax] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipFax] ON [dbo].[Customer] ([ShipFax]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipFirstName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipFirstName] ON [dbo].[Customer] ([ShipFirstName]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipLastName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Customer] ([ShipLastName]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipMiddleName] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Customer] ([ShipMiddleName]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipPhone] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPhone] ON [dbo].[Customer] ([ShipPhone]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Customer] ([ShipPostalCode]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipStateProvCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Customer] ([ShipStateProvCode]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipStreet1] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet1] ON [dbo].[Customer] ([ShipStreet1]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipStreet2] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet2] ON [dbo].[Customer] ([ShipStreet2]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipStreet3] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet3] ON [dbo].[Customer] ([ShipStreet3]) INCLUDE ([RowVersion])
GO
PRINT N'Creating index [IX_Auto_ShipWebsite] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipWebsite] ON [dbo].[Customer] ([ShipWebsite]) INCLUDE ([RowVersion])
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
PRINT N'Creating index [IX_OrderCharge_OrderID] on [dbo].[OrderCharge]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderCharge_OrderID] ON [dbo].[OrderCharge] ([OrderID])
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
[UnitPrice] [money] NOT NULL
)
GO
PRINT N'Creating primary key [PK_OrderItemAttribute] on [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] ADD CONSTRAINT [PK_OrderItemAttribute] PRIMARY KEY CLUSTERED  ([OrderItemAttributeID])
GO
PRINT N'Creating index [IX_OrderItemAttribute_OrderItemID] on [dbo].[OrderItemAttribute]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderItemAttribute_OrderItemID] ON [dbo].[OrderItemAttribute] ([OrderItemID])
GO
PRINT N'Creating [dbo].[OrderMotionOrder]'
GO
CREATE TABLE [dbo].[OrderMotionOrder]
(
[OrderID] [bigint] NOT NULL,
[OrderMotionShipmentID] [int] NOT NULL,
[OrderMotionPromotion] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
[OrderMotionBizID] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrderMotionCluster] [int] NOT NULL
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
PRINT N'Creating index [IX_OrderPaymentDetail_OrderID] on [dbo].[OrderPaymentDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderPaymentDetail_OrderID] ON [dbo].[OrderPaymentDetail] ([OrderID])
GO
PRINT N'Creating [dbo].[OtherProfile]'
GO
CREATE TABLE [dbo].[OtherProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[Carrier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Service] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InsuranceType] [int] NULL
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
[InsuranceType] [int] NOT NULL,
[InsuranceValue] [money] NOT NULL
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
PRINT N'Creating [dbo].[PayPalStore]'
GO
CREATE TABLE [dbo].[PayPalStore]
(
[StoreID] [bigint] NOT NULL,
[ApiCredentialType] [smallint] NOT NULL,
[ApiUserName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiPassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiSignature] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiCertificate] [varbinary] (2048) NULL,
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
PRINT N'Creating index [IX_Permission] on [dbo].[Permission]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Permission] ON [dbo].[Permission] ([UserID], [PermissionType], [ObjectID])
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
PRINT N'Creating index [IX_PrintResult_RelatedObjectID] on [dbo].[PrintResult]'
GO
CREATE NONCLUSTERED INDEX [IX_PrintResult_RelatedObjectID] ON [dbo].[PrintResult] ([RelatedObjectID])
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
PRINT N'Creating index [IX_ServerMessageSignoff] on [dbo].[ServerMessageSignoff]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessageSignoff] ON [dbo].[ServerMessageSignoff] ([UserID], [ComputerID], [ServerMessageID])
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
PRINT N'Creating index [IX_ServerMessage_RowVersion] on [dbo].[ServerMessage]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessage_RowVersion] ON [dbo].[ServerMessage] ([RowVersion])
GO
PRINT N'Creating index [IX_ServerMessage_Number] on [dbo].[ServerMessage]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessage_Number] ON [dbo].[ServerMessage] ([Number])
GO
PRINT N'Creating index [IX_ServerMessage_Expires] on [dbo].[ServerMessage]'
GO
CREATE NONCLUSTERED INDEX [IX_ServerMessage_Expires] ON [dbo].[ServerMessage] ([Expires])
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
[HarmonizedCode] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShipmentCustomsItem] on [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD CONSTRAINT [PK_ShipmentCustomsItem] PRIMARY KEY CLUSTERED  ([ShipmentCustomsItemID])
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
PRINT N'Creating [dbo].[StampsProfile]'
GO
CREATE TABLE [dbo].[StampsProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[StampsAccountID] [bigint] NULL,
[HidePostage] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_StampsProfile] on [dbo].[StampsProfile]'
GO
ALTER TABLE [dbo].[StampsProfile] ADD CONSTRAINT [PK_StampsProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[StampsShipment]'
GO
CREATE TABLE [dbo].[StampsShipment]
(
[ShipmentID] [bigint] NOT NULL,
[StampsAccountID] [bigint] NOT NULL,
[HidePostage] [bit] NOT NULL,
[IntegratorTransactionID] [uniqueidentifier] NOT NULL,
[StampsTransactionID] [uniqueidentifier] NOT NULL
)
GO
PRINT N'Creating primary key [PK_StampsShipment] on [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [PK_StampsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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
PRINT N'Creating [dbo].[ShopSiteStore]'
GO
CREATE TABLE [dbo].[ShopSiteStore]
(
[StoreID] [bigint] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CgiUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RequireSSL] [bit] NOT NULL,
[DownloadPageSize] [int] NOT NULL,
[RequestTimeout] [int] NOT NULL
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
PRINT N'Creating index [IX_TemplateStoreSettings] on [dbo].[TemplateStoreSettings]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TemplateStoreSettings] ON [dbo].[TemplateStoreSettings] ([TemplateID], [StoreID])
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
[InsuranceType] [int] NOT NULL,
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
[NegotiatedRate] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_UpsShipment] on [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [PK_UpsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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
[InsuranceValue] [money] NOT NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_UpsPackage] on [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [PK_UpsPackage] PRIMARY KEY CLUSTERED  ([UpsPackageID])
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
[InsuranceType] [int] NULL,
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
[EmailNotifyMessage] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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
[UpsProfilePackageID] [bigint] NOT NULL IDENTITY(1064, 1000),
[ShippingProfileID] [bigint] NOT NULL,
[PackagingType] [int] NULL,
[Weight] [float] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_UpsProfilePackage] on [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] ADD CONSTRAINT [PK_UpsProfilePackage] PRIMARY KEY CLUSTERED  ([UpsProfilePackageID])
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
PRINT N'Creating index [IX_UserColumnSettings] on [dbo].[UserColumnSettings]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserColumnSettings] ON [dbo].[UserColumnSettings] ([UserID], [SettingsKey])
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
[FilterLastActive] [bigint] NOT NULL,
[FilterExpandedFolders] [xml] NULL,
[ShippingWeightFormat] [int] NOT NULL,
[TemplateExpandedFolders] [xml] NULL,
[TemplateLastSelected] [bigint] NOT NULL
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
PRINT N'Creating index [IX_VersionSignoff] on [dbo].[VersionSignoff]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_VersionSignoff] ON [dbo].[VersionSignoff] ([ComputerID], [UserID])
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
[FromEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[ToEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToAccountNumber] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ToResidential] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServiceType] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillTransportationTo] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SaturdayDelivery] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[QvnOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[QvnFrom] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[QvnSubjectLine] [nvarchar] (18) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[QvnMemo] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1DeliveryNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1ExceptionNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn1Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2DeliveryNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2ExceptionNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn2Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3DeliveryNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3ExceptionNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Qvn3Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDescriptionOfGoods] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipperNumber] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PackageCount] [int] NOT NULL,
[DeliveryConfirmation] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DeliveryConfirmationAdult] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceTermsOfSale] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceReasonForExport] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceCurrencyCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceChargesFreight] [money] NOT NULL,
[InvoiceChargesInsurance] [money] NOT NULL,
[InvoiceChargesOther] [money] NOT NULL
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
[ShipmentCustomsItemID] [bigint] NOT NULL,
[ShipmentID] [bigint] NOT NULL,
[Description] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TariffCode] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryOfOrigin] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Units] [int] NOT NULL,
[UnitOfMeasure] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UnitPrice] [money] NOT NULL,
[Weight] [float] NOT NULL
)
GO
PRINT N'Creating primary key [PK_WorldShipGoods] on [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD CONSTRAINT [PK_WorldShipGoods] PRIMARY KEY CLUSTERED  ([ShipmentCustomsItemID])
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
[YahooProductID] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
[TrackingUpdatePassword] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
PRINT N'Creating [dbo].[ShippingSettings]'
GO
CREATE TABLE [dbo].[ShippingSettings]
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
[WorldShipLaunch] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Creating [dbo].[ActionQueue]'
GO
CREATE TABLE [dbo].[ActionQueue]
(
[ActionQueueID] [bigint] NOT NULL IDENTITY(1041, 1000),
[RowVersion] [timestamp] NOT NULL,
[ActionID] [bigint] NOT NULL,
[ActionName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ActionVersion] [binary] (8) NOT NULL CONSTRAINT [DF_ActionQueue_ActionVersion] DEFAULT ((0)),
[TriggerDate] [datetime] NOT NULL CONSTRAINT [DF_ActionQueue_QueuedDate] DEFAULT (getutcdate()),
[TriggerComputerID] [bigint] NOT NULL,
[RunComputerID] [bigint] NULL,
[ObjectID] [bigint] NULL,
[Status] [int] NOT NULL,
[NextStep] [int] NOT NULL,
[ContextLock] [nvarchar] (36) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_ActionQueue] on [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] ADD CONSTRAINT [PK_ActionQueue] PRIMARY KEY CLUSTERED  ([ActionQueueID])
GO
PRINT N'Creating index [IX_ActionQueue_Search] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_Search] ON [dbo].[ActionQueue] ([ActionQueueID], [RunComputerID], [Status])
GO
PRINT N'Creating index [IX_ActionQueue_ContextLock] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_ContextLock] ON [dbo].[ActionQueue] ([ContextLock])
GO
ALTER TABLE [dbo].[ActionQueue] ENABLE CHANGE_TRACKING
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
[CustomerUpdateShipping] [bit] NOT NULL
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
PRINT N'Creating index [IX_DimensionsProfile_Name] on [dbo].[DimensionsProfile]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DimensionsProfile_Name] ON [dbo].[DimensionsProfile] ([Name])
GO
ALTER TABLE [dbo].[DimensionsProfile] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[Dirty]'
GO
CREATE TABLE [dbo].[Dirty]
(
[RowVersion] [timestamp] NOT NULL
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
[EmailAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingServer] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingPort] [int] NOT NULL,
[IncomingSecurityType] [int] NOT NULL,
[IncomingUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingServer] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingPort] [int] NOT NULL,
[OutgoingSecurityType] [int] NOT NULL,
[OutgoingCredentialSource] [int] NOT NULL,
[OutgoingUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_EndiciaAccount] on [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] ADD CONSTRAINT [PK_EndiciaAccount] PRIMARY KEY CLUSTERED  ([EndiciaAccountID])
GO
ALTER TABLE [dbo].[EndiciaAccount] ENABLE CHANGE_TRACKING
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
[ShipmentCount] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EndiciaScanForm] on [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ADD CONSTRAINT [PK_EndiciaScanForm] PRIMARY KEY CLUSTERED  ([EndiciaScanFormID])
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
[CloseDate] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_FedExEndOfDayClose] on [dbo].[FedExEndOfDayClose]'
GO
ALTER TABLE [dbo].[FedExEndOfDayClose] ADD CONSTRAINT [PK_FedExEndOfDayClose] PRIMARY KEY CLUSTERED  ([FedExEndOfDayCloseID])
GO
PRINT N'Creating index [IX_FedExEndOfDayClose_CloseDate] on [dbo].[FedExEndOfDayClose]'
GO
CREATE NONCLUSTERED INDEX [IX_FedExEndOfDayClose_CloseDate] ON [dbo].[FedExEndOfDayClose] ([CloseDate]) INCLUDE ([FedExAccountID])
GO
PRINT N'Creating [dbo].[FilterNodeContentDirty]'
GO
CREATE TABLE [dbo].[FilterNodeContentDirty]
(
[ObjectID] [bigint] NOT NULL,
[ObjectType] [int] NOT NULL,
[ComputerID] [bigint] NOT NULL,
[RowVersion] [timestamp] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeCountDirty] on [dbo].[FilterNodeContentDirty]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeCountDirty] ON [dbo].[FilterNodeContentDirty] ([RowVersion]) INCLUDE ([ComputerID], [ObjectID], [ObjectType])
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
[RowVersion] [binary] (8) NOT NULL,
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
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateCustomer] on [dbo].[FilterNodeUpdateCustomer]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateItem]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateItem]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateItem] on [dbo].[FilterNodeUpdateItem]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateOrder]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateOrder]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateOrder] on [dbo].[FilterNodeUpdateOrder]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
GO
PRINT N'Creating [dbo].[FilterNodeUpdatePending]'
GO
CREATE TABLE [dbo].[FilterNodeUpdatePending]
(
[FilterNodeContentID] [bigint] NOT NULL,
[FilterTarget] [int] NOT NULL,
[Position] [int] NOT NULL
)
GO
PRINT N'Creating [dbo].[FilterNodeUpdateShipment]'
GO
CREATE TABLE [dbo].[FilterNodeUpdateShipment]
(
[ObjectID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
)
GO
PRINT N'Creating index [IX_FilterNodeUpdateShipment] on [dbo].[FilterNodeUpdateShipment]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
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
PRINT N'Creating index [IX_LabelSheet_Name] on [dbo].[LabelSheet]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LabelSheet_Name] ON [dbo].[LabelSheet] ([Name])
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
PRINT N'Creating index [IX_ObjectLabel] on [dbo].[ObjectLabel]'
GO
CREATE NONCLUSTERED INDEX [IX_ObjectLabel] ON [dbo].[ObjectLabel] ([ObjectType], [IsDeleted])
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
PRINT N'Creating index [IX_Resource_Checksum] on [dbo].[Resource]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resource_Checksum] ON [dbo].[Resource] ([Checksum])
GO
PRINT N'Creating index [IX_Resource_Filename] on [dbo].[Resource]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resource_Filename] ON [dbo].[Resource] ([Filename])
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
[ShippingProfileID] [bigint] NOT NULL
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
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShippingOrigin] on [dbo].[ShippingOrigin]'
GO
ALTER TABLE [dbo].[ShippingOrigin] ADD CONSTRAINT [PK_ShippingOrigin] PRIMARY KEY CLUSTERED  ([ShippingOriginID])
GO
PRINT N'Creating index [IX_ShippingOrigin_Description] on [dbo].[ShippingOrigin]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ShippingOrigin_Description] ON [dbo].[ShippingOrigin] ([Description])
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
PRINT N'Creating [dbo].[StampsAccount]'
GO
CREATE TABLE [dbo].[StampsAccount]
(
[StampsAccountID] [bigint] NOT NULL IDENTITY(1052, 1000),
[RowVersion] [timestamp] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_PostalStampsAccount] on [dbo].[StampsAccount]'
GO
ALTER TABLE [dbo].[StampsAccount] ADD CONSTRAINT [PK_PostalStampsAccount] PRIMARY KEY CLUSTERED  ([StampsAccountID])
GO
ALTER TABLE [dbo].[StampsAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[SystemData]'
GO
CREATE TABLE [dbo].[SystemData]
(
[SystemDataID] [bit] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[DatabaseID] [uniqueidentifier] NOT NULL,
[DateFiltersLastUpdate] [datetime] NOT NULL,
[TemplateVersion] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FilterSqlVersion] [int] NOT NULL
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
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
[ShipmentID] [bigint] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[PublishedCharges] [float] NOT NULL,
[NegotiatedCharges] [float] NOT NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_WorldShipProcessed] on [dbo].[WorldShipProcessed]'
GO
ALTER TABLE [dbo].[WorldShipProcessed] ADD CONSTRAINT [PK_WorldShipProcessed] PRIMARY KEY CLUSTERED  ([WorldShipProcessedID])
GO
PRINT N'Adding constraints to [dbo].[Computer]'
GO
ALTER TABLE [dbo].[Computer] ADD CONSTRAINT [UK_Computer_Identifier] UNIQUE NONCLUSTERED  ([Identifier])
GO
PRINT N'Adding constraints to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [IX_Shipment_Other] UNIQUE NONCLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionFilterTrigger]'
GO
ALTER TABLE [dbo].[ActionFilterTrigger] ADD
CONSTRAINT [FK_ActionFilterTrigger_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] ADD
CONSTRAINT [FK_ActionQueue_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID]) ON DELETE CASCADE,
CONSTRAINT [FK_ActionQueue_Computer] FOREIGN KEY ([TriggerComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionTask]'
GO
ALTER TABLE [dbo].[ActionTask] ADD
CONSTRAINT [FK_ActionTask_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueueStep]'
GO
ALTER TABLE [dbo].[ActionQueueStep] ADD
CONSTRAINT [FK_ActionQueueStep_ActionQueue] FOREIGN KEY ([ActionQueueID]) REFERENCES [dbo].[ActionQueue] ([ActionQueueID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonASIN]'
GO
ALTER TABLE [dbo].[AmazonASIN] ADD
CONSTRAINT [FK_AmazonASIN_AmazonStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[AmazonStore] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD
CONSTRAINT [FK_AmazonOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] ADD
CONSTRAINT [FK_AmazonOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD
CONSTRAINT [FK_AmazonStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[AmeriCommerceStore]'
GO
ALTER TABLE [dbo].[AmeriCommerceStore] ADD
CONSTRAINT [FK_AmeriCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[AuditChange]'
GO
ALTER TABLE [dbo].[AuditChange] ADD
CONSTRAINT [FK_AuditChange_Audit] FOREIGN KEY ([AuditID]) REFERENCES [dbo].[Audit] ([AuditID])
GO
PRINT N'Adding foreign keys to [dbo].[Audit]'
GO
ALTER TABLE [dbo].[Audit] ADD
CONSTRAINT [FK_Audit_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]),
CONSTRAINT [FK_Audit_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[AuditChangeDetail]'
GO
ALTER TABLE [dbo].[AuditChangeDetail] ADD
CONSTRAINT [FK_AuditChangeDetail_AuditChange] FOREIGN KEY ([AuditChangeID]) REFERENCES [dbo].[AuditChange] ([AuditChangeID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD
CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorStore]'
GO
ALTER TABLE [dbo].[ChannelAdvisorStore] ADD
CONSTRAINT [FK_ChannelAdvisorStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ClickCartProOrder]'
GO
ALTER TABLE [dbo].[ClickCartProOrder] ADD
CONSTRAINT [FK_ClickCartProOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[CommerceInterfaceOrder]'
GO
ALTER TABLE [dbo].[CommerceInterfaceOrder] ADD
CONSTRAINT [FK_CommerceInterfaceOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[Download]'
GO
ALTER TABLE [dbo].[Download] ADD
CONSTRAINT [FK_Download_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]),
CONSTRAINT [FK_Download_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE,
CONSTRAINT [FK_Download_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[PrintResult]'
GO
ALTER TABLE [dbo].[PrintResult] ADD
CONSTRAINT [FK_PrintResult_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Adding foreign keys to [dbo].[ServerMessageSignoff]'
GO
ALTER TABLE [dbo].[ServerMessageSignoff] ADD
CONSTRAINT [FK_ServerMessageSignoff_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE,
CONSTRAINT [FK_ServerMessageSignoff_DashboardMessage] FOREIGN KEY ([ServerMessageID]) REFERENCES [dbo].[ServerMessage] ([ServerMessageID])
GO
PRINT N'Adding foreign keys to [dbo].[TemplateComputerSettings]'
GO
ALTER TABLE [dbo].[TemplateComputerSettings] ADD
CONSTRAINT [FK_TemplateComputerSettings_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE,
CONSTRAINT [FK_TemplateComputerSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[VersionSignoff]'
GO
ALTER TABLE [dbo].[VersionSignoff] ADD
CONSTRAINT [FK_VersionVerification_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD
CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID]),
CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[DownloadDetail]'
GO
ALTER TABLE [dbo].[DownloadDetail] ADD
CONSTRAINT [FK_DownloadDetail_Download] FOREIGN KEY ([DownloadID]) REFERENCES [dbo].[Download] ([DownloadID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD
CONSTRAINT [FK_EbayOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] ADD
CONSTRAINT [FK_EbayOrderItem_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID]),
CONSTRAINT [FK_EbayOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD
CONSTRAINT [FK_EbayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[EmailOutboundRelation]'
GO
ALTER TABLE [dbo].[EmailOutboundRelation] ADD
CONSTRAINT [FK_EmailOutboundObject_EmailOutbound] FOREIGN KEY ([EmailOutboundID]) REFERENCES [dbo].[EmailOutbound] ([EmailOutboundID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaProfile]'
GO
ALTER TABLE [dbo].[EndiciaProfile] ADD
CONSTRAINT [FK_EndiciaProfile_PostalProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[PostalProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD
CONSTRAINT [FK_EndiciaShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD
CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD
CONSTRAINT [FK_FedExProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD
CONSTRAINT [FK_FedExProfilePackage_FedExProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[FedExProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FilterSequence]'
GO
ALTER TABLE [dbo].[FilterSequence] ADD
CONSTRAINT [FK_FilterSequence_Filter] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filter] ([FilterID]),
CONSTRAINT [FK_FilterSequence_Folder] FOREIGN KEY ([ParentFilterID]) REFERENCES [dbo].[Filter] ([FilterID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterLayout]'
GO
ALTER TABLE [dbo].[FilterLayout] ADD
CONSTRAINT [FK_FilterLayout_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]),
CONSTRAINT [FK_FilterLayout_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] ADD
CONSTRAINT [FK_FilterNode_Parent] FOREIGN KEY ([ParentFilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID]),
CONSTRAINT [FK_FilterNode_FilterSequence] FOREIGN KEY ([FilterSequenceID]) REFERENCES [dbo].[FilterSequence] ([FilterSequenceID]),
CONSTRAINT [FK_FilterNode_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNodeColumnSettings]'
GO
ALTER TABLE [dbo].[FilterNodeColumnSettings] ADD
CONSTRAINT [FK_FilterNodeColumnSettings_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID]) ON DELETE CASCADE,
CONSTRAINT [FK_FilterNodeColumnSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]),
CONSTRAINT [FK_FilterNodeColumnSettings_GridColumnLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNodeContentDetail]'
GO
ALTER TABLE [dbo].[FilterNodeContentDetail] ADD
CONSTRAINT [FK_FilterNodeContentDetail_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[GenericStore]'
GO
ALTER TABLE [dbo].[GenericStore] ADD
CONSTRAINT [FK_GenericStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] ADD
CONSTRAINT [FK_MagentoStore_GenericStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericStore] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] ADD
CONSTRAINT [FK_MivaStore_GenericStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericStore] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GridColumnFormat]'
GO
ALTER TABLE [dbo].[GridColumnFormat] ADD
CONSTRAINT [FK_GridColumnFormat_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[GridColumnPosition]'
GO
ALTER TABLE [dbo].[GridColumnPosition] ADD
CONSTRAINT [FK_GridLayoutColumn_GridLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UserColumnSettings]'
GO
ALTER TABLE [dbo].[UserColumnSettings] ADD
CONSTRAINT [FK_UserColumnSettings_GridColumnLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID]),
CONSTRAINT [FK_UserColumnSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaOrderItem]'
GO
ALTER TABLE [dbo].[InfopiaOrderItem] ADD
CONSTRAINT [FK_InfopiaOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaStore]'
GO
ALTER TABLE [dbo].[InfopiaStore] ADD
CONSTRAINT [FK_InfopiaStore_InfopiaStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoOrder]'
GO
ALTER TABLE [dbo].[MagentoOrder] ADD
CONSTRAINT [FK_MagentoOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorOrder]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorOrder] ADD
CONSTRAINT [FK_MarketworksOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[MarketplaceAdvisorStore]'
GO
ALTER TABLE [dbo].[MarketplaceAdvisorStore] ADD
CONSTRAINT [FK_MarketworksStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsOrder]'
GO
ALTER TABLE [dbo].[NetworkSolutionsOrder] ADD
CONSTRAINT [FK_NetworkSolutionsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[NetworkSolutionsStore]'
GO
ALTER TABLE [dbo].[NetworkSolutionsStore] ADD
CONSTRAINT [FK_NetworkSolutionsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Note]'
GO
ALTER TABLE [dbo].[Note] ADD
CONSTRAINT [FK_Note_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderCharge]'
GO
ALTER TABLE [dbo].[OrderCharge] ADD
CONSTRAINT [FK_OrderCharge_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD
CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] ADD
CONSTRAINT [FK_OrderMotionOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderPaymentDetail]'
GO
ALTER TABLE [dbo].[OrderPaymentDetail] ADD
CONSTRAINT [FK_OrderPaymentDetail_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[PayPalOrder]'
GO
ALTER TABLE [dbo].[PayPalOrder] ADD
CONSTRAINT [FK_PayPalOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ProStoresOrder]'
GO
ALTER TABLE [dbo].[ProStoresOrder] ADD
CONSTRAINT [FK_ProStoresOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrder]'
GO
ALTER TABLE [dbo].[YahooOrder] ADD
CONSTRAINT [FK_YahooOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] ADD
CONSTRAINT [FK_OrderItemAttribute_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD
CONSTRAINT [FK_YahooOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionStore]'
GO
ALTER TABLE [dbo].[OrderMotionStore] ADD
CONSTRAINT [FK_OrderMotionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OtherProfile]'
GO
ALTER TABLE [dbo].[OtherProfile] ADD
CONSTRAINT [FK_OtherProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] ADD
CONSTRAINT [FK_OtherShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PayPalStore]'
GO
ALTER TABLE [dbo].[PayPalStore] ADD
CONSTRAINT [FK_PayPalStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[Permission]'
GO
ALTER TABLE [dbo].[Permission] ADD
CONSTRAINT [FK_Permission_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] ADD
CONSTRAINT [FK_PostalProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[StampsProfile]'
GO
ALTER TABLE [dbo].[StampsProfile] ADD
CONSTRAINT [FK_StampsProfile_PostalProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[PostalProfile] ([ShippingProfileID]) ON DELETE CASCADE
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
PRINT N'Adding foreign keys to [dbo].[ProStoresStore]'
GO
ALTER TABLE [dbo].[ProStoresStore] ADD
CONSTRAINT [FK_ProStoresStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
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
PRINT N'Adding foreign keys to [dbo].[ShippingPrintOutputRule]'
GO
ALTER TABLE [dbo].[ShippingPrintOutputRule] ADD
CONSTRAINT [FK_ShippingPrintOutputRule_ShippingPrintOutput] FOREIGN KEY ([ShippingPrintOutputID]) REFERENCES [dbo].[ShippingPrintOutput] ([ShippingPrintOutputID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD
CONSTRAINT [FK_UpsProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShopSiteStore]'
GO
ALTER TABLE [dbo].[ShopSiteStore] ADD
CONSTRAINT [FK_StoreShopSite_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[StatusPreset]'
GO
ALTER TABLE [dbo].[StatusPreset] ADD
CONSTRAINT [FK_StatusPreset_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] ADD
CONSTRAINT [FK_VolusionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooStore]'
GO
ALTER TABLE [dbo].[YahooStore] ADD
CONSTRAINT [FK_YahooStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[TemplateStoreSettings]'
GO
ALTER TABLE [dbo].[TemplateStoreSettings] ADD
CONSTRAINT [FK_TemplateStoreSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[TemplateUserSettings]'
GO
ALTER TABLE [dbo].[TemplateUserSettings] ADD
CONSTRAINT [FK_TemplateUserSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE,
CONSTRAINT [FK_TemplateUserSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Template]'
GO
ALTER TABLE [dbo].[Template] ADD
CONSTRAINT [FK_Template_TemplateFolder] FOREIGN KEY ([ParentFolderID]) REFERENCES [dbo].[TemplateFolder] ([TemplateFolderID])
GO
PRINT N'Adding foreign keys to [dbo].[TemplateFolder]'
GO
ALTER TABLE [dbo].[TemplateFolder] ADD
CONSTRAINT [FK_TemplateFolder_TemplateFolder] FOREIGN KEY ([ParentFolderID]) REFERENCES [dbo].[TemplateFolder] ([TemplateFolderID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD
CONSTRAINT [FK_UpsPackage_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
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
PRINT N'Adding foreign keys to [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] ADD
CONSTRAINT [FK_UserSetting_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD
CONSTRAINT [FK_WorldShipGoods_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD
CONSTRAINT [FK_WorldShipPackage_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[YahooProduct]'
GO
ALTER TABLE [dbo].[YahooProduct] ADD
CONSTRAINT [FK_YahooProduct_YahooStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[YahooStore] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
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
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'EndiciaAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'RefundFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'ScanFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'TransactionID'
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
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'InsuranceType'
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
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'OtherShipment', 'COLUMN', N'InsuranceType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Insurance', 'SCHEMA', N'dbo', 'TABLE', N'OtherShipment', 'COLUMN', N'InsuranceType'
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
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'InsuranceType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Insurance', 'SCHEMA', N'dbo', 'TABLE', N'PostalShipment', 'COLUMN', N'InsuranceType'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
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
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'InsuranceType'
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
