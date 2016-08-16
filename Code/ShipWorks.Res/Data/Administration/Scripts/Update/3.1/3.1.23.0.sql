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
PRINT N'Dropping constraints from [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP CONSTRAINT [PK_PostalShipment]
GO
PRINT N'Altering [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] ADD
[SortType] [int] NULL,
[EntryFacility] [int] NULL
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
[ExpressSignatureWaiver] [bit] NOT NULL,
[SortType] [int] NOT NULL,
[EntryFacility] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_PostalShipment]([ShipmentID], [Service], [Confirmation], [PackagingType], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [NonRectangular], [NonMachinable], [CustomsContentType], [CustomsContentDescription], [InsuranceValue], [ExpressSignatureWaiver], [SortType], [EntryFacility]) SELECT [ShipmentID], [Service], [Confirmation], [PackagingType], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [NonRectangular], [NonMachinable], [CustomsContentType], [CustomsContentDescription], [InsuranceValue], [ExpressSignatureWaiver], 0, 0 FROM [dbo].[PostalShipment]
GO
DROP TABLE [dbo].[PostalShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_PostalShipment]', N'PostalShipment'
GO
PRINT N'Creating primary key [PK_PostalShipment] on [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [PK_PostalShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
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

UPDATE [dbo].PostalProfile
	SET SortType = 0,
	    EntryFacility = 0
	WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (2,3,4,9) AND ShipmentTypePrimary = 1)