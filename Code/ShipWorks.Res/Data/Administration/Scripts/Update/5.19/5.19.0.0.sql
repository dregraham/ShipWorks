SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[ShipEngineApiKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_ShipEngineApiKey] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_ShipEngineApiKey]
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
PRINT N'Altering [dbo].[DhlExpressAccount]'
GO
ALTER TABLE [dbo].[DhlExpressAccount] ENABLE CHANGE_TRACKING
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
	[RequestedLabelFormat] [int] NOT NULL
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
PRINT N'Creating index [IX_OnTracShipment_Service] on [dbo].[OnTracShipment]'
GO

CREATE NONCLUSTERED INDEX [IX_DhlExpressShipment_Service] ON [dbo].[DhlExpressShipment] ([Service])
GO

ALTER TABLE [dbo].[DhlExpressShipment] ENABLE CHANGE_TRACKING
GO

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
	[DeclaredValue] [money] NOT NULL,
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