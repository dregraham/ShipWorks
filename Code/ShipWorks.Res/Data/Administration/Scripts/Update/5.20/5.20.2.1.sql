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
	[Insurance] [bit] NOT NULL,
	[InsuranceValue] [money] NOT NULL,
	[TrackingNumber] [varchar](50) NOT NULL
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

PRINT N'Creating index [IX_AsendiaShipment_Service] on [dbo].[AsendiaShipment]'
GO

CREATE NONCLUSTERED INDEX [IX_AsendiaShipment_Service] ON [dbo].[AsendiaShipment] ([Service])
GO

ALTER TABLE [dbo].[AsendiaShipment] ENABLE CHANGE_TRACKING
GO


EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AsendiaShipment', @level2type=N'COLUMN',@level2name=N'AsendiaAccountID'
GO

EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'130' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AsendiaShipment', @level2type=N'COLUMN',@level2name=N'Service'
GO
