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
[ShippingServiceId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ShippingServiceId] DEFAULT (''),
[ShippingServiceOfferId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ShippingServiceOfferId] DEFAULT (''),
[InsuranceValue] [money] NOT NULL CONSTRAINT [DF_AmazonShipment_InsuranceValue] DEFAULT ((0)),
[DimsProfileID] [bigint] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsProfileID] DEFAULT ((0)),
[DimsLength] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsLength] DEFAULT ((0)),
[DimsWidth] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsWidth] DEFAULT ((0)),
[DimsHeight] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsHeight] DEFAULT ((0)),
[DimsWeight] [float] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsWeight] DEFAULT ((0)),
[DimsAddWeight] [bit] NOT NULL CONSTRAINT [DF_AmazonShipment_DimsAddWeight] DEFAULT ((0)),
[DateMustArriveBy] [datetime] NOT NULL,
[DeliveryExperience] [int] NOT NULL CONSTRAINT [DF_AmazonShipment_DeliveryExperience] DEFAULT ((0)),
[CarrierWillPickUp] [bit] NOT NULL,
[DeclaredValue] [money] NULL,
[AmazonUniqueShipmentID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_AmazonShipment] on [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ADD CONSTRAINT [PK_AmazonShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ADD CONSTRAINT [FK_AmazonShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'CarrierName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'7', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DateMustArriveBy'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsProfileID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'InsuranceValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'ShippingServiceName'
GO

