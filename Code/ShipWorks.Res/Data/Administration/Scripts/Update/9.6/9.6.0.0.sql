SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Shipment]'
GO
IF COL_LENGTH(N'[dbo].[Shipment]', N'TrackingHubTimestamp') IS NULL
ALTER TABLE [dbo].[Shipment] ADD[TrackingHubTimestamp] [datetime2] (3) NULL
IF COL_LENGTH(N'[dbo].[Shipment]', N'TrackingStatus') IS NULL
ALTER TABLE [dbo].[Shipment] ADD[TrackingStatus] [tinyint] NOT NULL CONSTRAINT [DF_Shipment_TrackingStatus] DEFAULT ((0))
IF COL_LENGTH(N'[dbo].[Shipment]', N'EstimatedDeliveryDate') IS NULL
ALTER TABLE [dbo].[Shipment] ADD[EstimatedDeliveryDate] [datetime2] (3) NULL
IF COL_LENGTH(N'[dbo].[Shipment]', N'ActualDeliveryDate') IS NULL
ALTER TABLE [dbo].[Shipment] ADD[ActualDeliveryDate] [datetime2] (3) NULL
IF COL_LENGTH(N'[dbo].[Shipment]', N'CarrierStatusDescription') IS NULL
ALTER TABLE [dbo].[Shipment] ADD[CarrierStatusDescription] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Shipment_CarrierStatusDescription] DEFAULT ('')
GO
PRINT N'Creating index [IX_SWDefault_Shipment_TrackingHubTimestamp] on [dbo].[Shipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_Shipment_TrackingHubTimestamp' AND object_id = OBJECT_ID(N'[dbo].[Shipment]'))
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_TrackingHubTimestamp] ON [dbo].[Shipment] ([TrackingHubTimestamp]) WHERE ([TrackingHubTimestamp] IS NOT NULL)
GO
PRINT N'Creating index [IX_SWDefault_Shipment_TrackingNumber] on [dbo].[Shipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_Shipment_TrackingNumber' AND object_id = OBJECT_ID(N'[dbo].[Shipment]'))
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_TrackingNumber] ON [dbo].[Shipment] ([TrackingNumber]) WHERE ([TrackingNumber]<>'')
GO
