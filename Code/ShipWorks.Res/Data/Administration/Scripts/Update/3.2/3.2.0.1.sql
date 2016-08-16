SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping index [IX_Auto_BillEmail] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillEmail] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipEmail] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipEmail] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillEmail] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillEmail] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipEmail] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipEmail] ON [dbo].[Order]
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [BillEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[Order] ALTER COLUMN [ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Order] ([BillEmail])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Order] ([ShipEmail])
GO
PRINT N'Altering [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] ALTER COLUMN [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[Shipment] ALTER COLUMN [OriginEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ALTER COLUMN [EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[Customer]'
GO
ALTER TABLE [dbo].[Customer] ALTER COLUMN [BillEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[Customer] ALTER COLUMN [ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Creating index [IX_Auto_BillEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Customer] ([BillEmail])
GO
PRINT N'Creating index [IX_Auto_ShipEmail] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Customer] ([ShipEmail])
GO
PRINT N'Altering [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ALTER COLUMN [EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[UpsShipment] ALTER COLUMN [EmailNotifyFrom] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[UpsShipment] ALTER COLUMN [EmailNotifyMessage] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[UpsShipment] ALTER COLUMN [ReturnUndeliverableEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ALTER COLUMN [EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[UpsProfile] ALTER COLUMN [EmailNotifyFrom] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[UpsProfile] ALTER COLUMN [EmailNotifyMessage] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[UpsProfile] ALTER COLUMN [ReturnUndeliverableEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [FromEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [ToEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [Qvn1Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [Qvn2Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [Qvn3Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[EmailAccount]'
GO
ALTER TABLE [dbo].[EmailAccount] ALTER COLUMN [EmailAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] ALTER COLUMN [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[EquaShipAccount]'
GO
ALTER TABLE [dbo].[EquaShipAccount] ALTER COLUMN [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] ALTER COLUMN [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[ShippingOrigin]'
GO
ALTER TABLE [dbo].[ShippingOrigin] ALTER COLUMN [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] ALTER COLUMN [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
