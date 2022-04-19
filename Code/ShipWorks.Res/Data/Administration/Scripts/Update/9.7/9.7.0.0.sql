SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Shipment]'
GO

PRINT N'Creating [dbo].[DhlEcommerceProfile]'
GO
IF OBJECT_ID(N'[dbo].[DhlEcommerceProfile]', 'U') IS NULL
	CREATE TABLE [dbo].[DhlEcommerceProfile]
	(
	[ShippingProfileID] [bigint] NOT NULL,
	[DhlEcommerceAccountID] [bigint] NULL,
	[Service] [int] NULL,
	[DeliveryDutyPaid] [bit] NULL,
	[NonMachinable] [bit] NULL,
	[SaturdayDelivery] [bit] NULL,
	[Contents] [int] NULL,
	[NonDelivery] [int] NULL,
	[ResidentialDelivery] [bit] NULL,
	[CustomsRecipientTin] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CustomsTaxIdType] [int] NULL,
	[CustomsTinIssuingAuthority] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PackagingType] [int] NULL,
	[Reference1] [nvarchar](300) NULL
	)
GO
PRINT N'Creating primary key [PK_DhlEcommerceProfile] on [dbo].[DhlEcommerceProfile]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_DhlEcommerceProfile' AND object_id = OBJECT_ID(N'[dbo].[DhlEcommerceProfile]'))
	ALTER TABLE [dbo].[DhlEcommerceProfile] ADD CONSTRAINT [PK_DhlEcommerceProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Creating [dbo].[DhlEcommerceScanForm]'
GO
IF OBJECT_ID(N'[dbo].[DhlEcommerceScanForm]', 'U') IS NULL
	CREATE TABLE [dbo].[DhlEcommerceScanForm]
	(
	[DhlEcommerceScanFormID] [bigint] NOT NULL IDENTITY(1107, 1000),
	[DhlEcommerceAccountID] [bigint] NOT NULL,
	[ScanFormTransactionID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ScanFormUrl] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ScanFormBatchID] [bigint] NOT NULL,
	[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	)
GO
PRINT N'Creating primary key [PK_DhlEcommerceScanForm] on [dbo].[DhlEcommerceScanForm]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_DhlEcommerceScanForm' AND object_id = OBJECT_ID(N'[dbo].[DhlEcommerceScanForm]'))
	ALTER TABLE [dbo].[DhlEcommerceScanForm] ADD CONSTRAINT [PK_DhlEcommerceScanForm] PRIMARY KEY CLUSTERED  ([DhlEcommerceScanFormID])
GO

PRINT N'Creating [dbo].[DhlEcommerceShipment]'
GO
IF OBJECT_ID(N'[dbo].[DhlEcommerceShipment]', 'U') IS NULL
CREATE TABLE [dbo].[DhlEcommerceShipment]
	(
	[ShipmentID] [bigint] NOT NULL,
	[DhlEcommerceAccountID] [bigint] NOT NULL,
	[Service] [int] NOT NULL,
	[DeliveredDutyPaid] [bit] NOT NULL,
	[NonMachinable] [bit] NOT NULL,
	[SaturdayDelivery] [bit] NOT NULL,
	[RequestedLabelFormat] [int] NOT NULL,
	[Contents] [int] NOT NULL,
	[NonDelivery] [int] NOT NULL,
	[ShipEngineLabelID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IntegratorTransactionID] [uniqueidentifier] NULL,
	[StampsTransactionID] [uniqueidentifier] NULL,
	[ResidentialDelivery] [bit] NOT NULL,
	[PackagingType] [int] NOT NULL,
	[DimsProfileID] [bigint] NOT NULL,
	[DimsLength] [float] NOT NULL,
	[DimsWidth] [float] NOT NULL,
	[DimsHeight] [float] NOT NULL,
	[DimsWeight] [float] NOT NULL,
	[DimsAddWeight] [bit] NOT NULL,
	[Reference1] [nvarchar](300) NOT NULL,
	[CustomsRecipientTin] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CustomsTaxIdType] [int] NULL,
	[CustomsTinIssuingAuthority] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ScanFormBatchID] [bigint] NULL
	)
GO
PRINT N'Creating primary key [PK_DhlEcommerceShipment] on [dbo].[DhlEcommerceShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_DhlEcommerceShipment' AND object_id = OBJECT_ID(N'[dbo].[DhlEcommerceShipment]'))
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD CONSTRAINT [PK_DhlEcommerceShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO

PRINT N'Creating [dbo].[DhlEcommerceAccount]'
GO
IF OBJECT_ID(N'[dbo].[DhlEcommerceAccount]', 'U') IS NULL
	CREATE TABLE [dbo].[DhlEcommerceAccount]
	(
	[DhlEcommerceAccountID] [bigint] NOT NULL IDENTITY(1106, 1000),
	[RowVersion] [timestamp] NOT NULL,
	[ShipEngineCarrierId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ClientId] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ApiSecret] [nvarchar] (400) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PickupNumber] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DistributionCenter] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SoldTo] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
	[Phone] [nvarchar] (26) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedDate] [datetime] NOT NULL
	)
GO

PRINT N'Creating primary key [PK_PostalDhlEcommerceAccount] on [dbo].[DhlEcommerceAccount]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_PostalDhlEcommerceAccount' AND object_id = OBJECT_ID(N'[dbo].[DhlEcommerceAccount]'))
	ALTER TABLE [dbo].[DhlEcommerceAccount] ADD CONSTRAINT [PK_PostalDhlEcommerceAccount] PRIMARY KEY CLUSTERED  ([DhlEcommerceAccountID])
GO

ALTER TABLE [dbo].[DhlEcommerceAccount] ENABLE CHANGE_TRACKING
GO

PRINT N'Adding foreign keys to [dbo].[DhlEcommerceProfile]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DhlEcommerceProfile_ShippingProfile]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[DhlEcommerceProfile]', 'U'))
	ALTER TABLE [dbo].[DhlEcommerceProfile] ADD CONSTRAINT [FK_DhlEcommerceProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[DhlEcommerceScanForm]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DhlEcommerceScanForm_ScanFormBatch]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[DhlEcommerceScanForm]', 'U'))
	ALTER TABLE [dbo].[DhlEcommerceScanForm] ADD CONSTRAINT [FK_DhlEcommerceScanForm_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[DhlEcommerceShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DhlEcommerceShipment_Shipment]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[DhlEcommerceShipment]', 'U'))
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD CONSTRAINT [FK_DhlEcommerceShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DhlEcommerceShipment_ScanFormBatch]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[DhlEcommerceShipment]', 'U'))
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD CONSTRAINT [FK_DhlEcommerceShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DeliveredDutyPaid'))
	EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DeliveredDutyPaid'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DhlEcommerceAccountID'))
	EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DhlEcommerceAccountID'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'NonMachinable'))
	EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'NonMachinable'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'SaturdayDelivery'))
	EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'SaturdayDelivery'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'Service'))
	EXEC sp_addextendedproperty N'AuditFormat', N'130', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'Service'
GO
