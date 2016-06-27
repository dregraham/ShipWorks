IF OBJECT_ID(N'v2m_EbayTemp') IS NOT NULL
	DROP TABLE dbo.v2m_EbayTemp
GO

CREATE TABLE [dbo].[v2m_EbayTemp](
	[OrderID] [bigint] NOT NULL,
	[PaymentStatus] [int] NOT NULL,
	[PaymentMethod] [int] NOT NULL,
	[CheckoutStatus] [int] NOT NULL,
	[CompleteStatus] [int] NOT NULL,
	[SellerPaidStatus] [int] NOT NULL,
	[FeedbackLeftType] [int] NOT NULL,
	[FeedbackLeftComments] [nvarchar](80) NOT NULL,
	[FeedbackReceivedType] [int] NOT NULL,
	[FeedbackReceivedComments] [nvarchar](80) NOT NULL,
	[MyEbayPaid] [bit] NOT NULL,
	[MyEbayShipped] [bit] NOT NULL,
	[PayPalTransactionID] [varchar](50) NOT NULL,
	[PayPalAddressStatus] [int] NOT NULL,
 CONSTRAINT [PK_OldEbayTemp] PRIMARY KEY CLUSTERED ( [OrderID] ASC ) )
GO

IF OBJECT_ID(N'v2m_SkippedShipments') IS NOT NULL
	DROP TABLE dbo.v2m_SkippedShipments
GO

-- table for tracking why shipments were skipped during the migration
-- Reason included solely for debugging purposes
CREATE TABLE [dbo].[v2m_SkippedShipments](
	ShipmentID int NOT NULL,
	Reason varchar(50))
GO

-- Table for holding status code blobs for conversion later
IF OBJECT_ID(N'v2m_StoreStatusTemp') IS NOT NULL
	DROP TABLE dbo.v2m_StoreStatusTemp
GO

-- For holding online status code blobs
CREATE TABLE [dbo].[v2m_StoreStatusTemp](
	[StatusTempID] [bigint] IDENTITY(1,1) NOT NULL,
	[StoreID] [bigint] NOT NULL,
	[TypeCode] [int] NOT NULL,
	[OnlineStatusCodes] [ntext] NOT NULL,
 CONSTRAINT [PK_v2m_StoreStatusTemp] PRIMARY KEY CLUSTERED ( [StatusTempID] ASC ) )
GO

-- Table for holding Amazon certificates for later conversion
IF OBJECT_ID(N'v2m_AmazonCertificateTemp') IS NOT NULL
	DROP TABLE dbo.v2m_AmazonCertificateTemp
GO

CREATE TABLE [dbo].[v2m_AmazonCertificateTemp](
	[CertificateID] [bigint] IDENTITY(1,1) NOT NULL,
	[StoreID] [bigint] NOT NULL,
	[CertificateName] varchar(32) NOT NULL,
	[PublicKey] nvarchar(max) NOT NULL,
	[PrivateKey] nvarchar(max) NOT NULL,
 CONSTRAINT [PK_v2m_AmazonCertificateTemp] PRIMARY KEY CLUSTERED ( [CertificateID] ASC ) )
GO

-- Table for holding the FilterLayout until the filter converter is ready for it
IF OBJECT_ID(N'[v2m_StoreFilterLayoutXml]') IS NOT NULL
	DROP TABLE dbo.[v2m_StoreFilterLayoutXml]
GO

CREATE TABLE [dbo].[v2m_StoreFilterLayoutXml]
(
	[FilterLayout] nvarchar(max)  NOT NULL
)
GO


-- Table for holding Amazon certificates for later conversion
IF OBJECT_ID(N'v2m_StoreStatusString') IS NOT NULL
	DROP TABLE dbo.v2m_StoreStatusString
GO

CREATE TABLE [dbo].[v2m_StoreStatusString](
	[StoreID] [bigint] NOT NULL,
	[OrderStatusStrings] [nvarchar](500) NOT NULL,
	[ItemStatusStrings] [nvarchar](500) NOT NULL)
GO

IF OBJECT_ID(N'v2m_UpsShipmentNotify') IS NOT NULL
	DROP TABLE v2m_UpsShipmentNotify
GO

CREATE TABLE [dbo].v2m_UpsShipmentNotify(
	ShipmentID bigint NOT NULL,
	NotificationEmailRecipients nvarchar(max) NOT NULL,
	CONSTRAINT [PK_v2m_UpsShipmentNotify] PRIMARY KEY CLUSTERED ( [ShipmentID] ASC )
	)
GO

IF OBJECT_ID(N'v2m_ChannelAdvisorOrder') IS NOT NULL
	DROP TABLE v2m_ChannelAdvisorOrder
GO

CREATE TABLE [dbo].v2m_ChannelAdvisorOrder
(
	OrderID bigint NOT NULL,
	DistributionCenter nvarchar(80) NOT NULL,
	CONSTRAINT [PK_v2m_ChannelAdvisorOrder] PRIMARY KEY CLUSTERED ( [OrderID] ASC )
	)
GO

IF OBJECT_ID(N'v2m_UpsAccessKey') IS NOT NULL
	DROP TABLE v2m_UpsAccessKey
GO

CREATE TABLE [dbo].v2m_UpsAccessKey
(
	UpsAccessKey nvarchar(50) NOT NULL
)
GO

IF OBJECT_ID(N'v2m_Express1Account') IS NOT NULL
	DROP TABLE v2m_Express1Account
GO

CREATE TABLE [dbo].v2m_Express1Account
(
	EndiciaAccountID bigint NOT NULL
)
GO


IF OBJECT_ID(N'v2m_MivaItemAttribute') IS NOT NULL
	DROP TABLE	v2m_MivaItemAttribute
GO

CREATE TABLE [dbo].v2m_MivaItemAttribute
(
	OrderItemAttributeID bigint NOT NULL,
	MivaAttributeID int NOT NULL,
	MivaOptionCode nvarchar(300) NOT NULL,
	MivaAttributeCode nvarchar(300) NOT NULL
)

