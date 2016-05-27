EXECUTE sp_rename N'dbo.ShipmentContent', N'ShipmentCustomsItem', 'OBJECT' 

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP
CONSTRAINT [FK_FedExShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] DROP
CONSTRAINT [FK_OtherShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP
CONSTRAINT [FK_PostalShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP
CONSTRAINT [FK_ShipmentContent_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP
CONSTRAINT [FK_UpsShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP
CONSTRAINT [FK_Shipment_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] DROP
CONSTRAINT [FK_WorldShipGoods_WorldShipShipment]
GO
PRINT N'Dropping constraints from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [PK_Shipment]
GO
PRINT N'Dropping constraints from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [IX_Shipment_Other]
GO
PRINT N'Dropping constraints from [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP CONSTRAINT [PK_ShipmentContent]
GO
PRINT N'Dropping constraints from [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] DROP CONSTRAINT [PK_WorldShipGoods]
GO
PRINT N'Dropping index [IX_Auto_BillPostalCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillPostalCode] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipPostalCode] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipPostalCode] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Shipment_OrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment]
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [BillPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[Order] ALTER COLUMN [ShipPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order] ([BillPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID]) ON [PRIMARY]
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID]) ON [PRIMARY]
GO
PRINT N'Altering [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] ALTER COLUMN [PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [CodPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [BrokerPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Rebuilding [dbo].[Shipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Shipment]
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
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Shipment]([ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [TrackingNumber], [CustomsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite]) SELECT [ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [TrackingNumber], [CustomsContentsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite] FROM [dbo].[Shipment]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] OFF
GO
DROP TABLE [dbo].[Shipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Shipment]', N'Shipment'
GO
PRINT N'Creating primary key [PK_Shipment] on [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED  ([ShipmentID]) ON [PRIMARY]
GO
PRINT N'Creating index [IX_Shipment_OrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment] ([OrderID]) ON [PRIMARY]
GO
PRINT N'Altering [dbo].[Customer]'
GO
ALTER TABLE [dbo].[Customer] ALTER COLUMN [BillPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[Customer] ALTER COLUMN [ShipPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Creating index [IX_Auto_BillPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Customer] ([BillPostalCode]) INCLUDE ([RowVersion]) ON [PRIMARY]
GO
PRINT N'Creating index [IX_Auto_ShipPostalCode] on [dbo].[Customer]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Customer] ([ShipPostalCode]) INCLUDE ([RowVersion]) ON [PRIMARY]
GO
PRINT N'Rebuilding [dbo].[ShipmentCustomsItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShipmentCustomsItem]
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
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ShipmentCustomsItem] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_ShipmentCustomsItem]([ShipmentCustomsItemID], [ShipmentID], [Description], [Quantity], [Weight], [UnitValue], [CountryOfOrigin], [HarmonizedCode]) SELECT [ShipmentContentID], [ShipmentID], [Description], [Quantity], [Weight], [UnitValue], [CountryOfOrigin], [HarmonizedCode] FROM [dbo].[ShipmentCustomsItem]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ShipmentCustomsItem] OFF
GO
DROP TABLE [dbo].[ShipmentCustomsItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShipmentCustomsItem]', N'ShipmentCustomsItem'
GO
PRINT N'Creating primary key [PK_ShipmentCustomsItem] on [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD CONSTRAINT [PK_ShipmentCustomsItem] PRIMARY KEY CLUSTERED  ([ShipmentCustomsItemID]) ON [PRIMARY]
GO
PRINT N'Altering [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ALTER COLUMN [PayorPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ALTER COLUMN [PayorPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [FromPostalCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [ToPostalCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Rebuilding [dbo].[WorldShipGoods]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_WorldShipGoods]
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
) ON [PRIMARY]
GO
INSERT INTO [dbo].[tmp_rg_xx_WorldShipGoods]([ShipmentCustomsItemID], [ShipmentID], [Description], [TariffCode], [CountryOfOrigin], [Units], [UnitOfMeasure], [UnitPrice], [Weight]) SELECT [ShipmentContentID], [ShipmentID], [Description], [TariffCode], [CountryOfOrigin], [Units], [UnitOfMeasure], [UnitPrice], [Weight] FROM [dbo].[WorldShipGoods]
GO
DROP TABLE [dbo].[WorldShipGoods]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_WorldShipGoods]', N'WorldShipGoods'
GO
PRINT N'Creating primary key [PK_WorldShipGoods] on [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD CONSTRAINT [PK_WorldShipGoods] PRIMARY KEY CLUSTERED  ([ShipmentCustomsItemID]) ON [PRIMARY]
GO
PRINT N'Altering [dbo].[EndiciaAccount]'
GO
ALTER TABLE [dbo].[EndiciaAccount] ALTER COLUMN [PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] ALTER COLUMN [PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[ShippingOrigin]'
GO
ALTER TABLE [dbo].[ShippingOrigin] ALTER COLUMN [PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] ALTER COLUMN [PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Rebuilding [dbo].[Search]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Search]
(
[SearchID] [bigint] NOT NULL IDENTITY(1069, 1000),
[Started] [datetime] NOT NULL,
[Pinged] [datetime] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[UserID] [bigint] NOT NULL,
[ComputerID] [bigint] NOT NULL
) ON [PRIMARY]
GO
INSERT INTO [dbo].[tmp_rg_xx_Search]([FilterNodeID]) SELECT [FilterNodeID] FROM [dbo].[Search]
GO
DROP TABLE [dbo].[Search]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Search]', N'Search'
GO
PRINT N'Creating primary key [PK_Search] on [dbo].[Search]'
GO
ALTER TABLE [dbo].[Search] ADD CONSTRAINT [PK_Search] PRIMARY KEY CLUSTERED  ([SearchID]) ON [PRIMARY]
GO
PRINT N'Adding constraints to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [IX_Shipment_Other] UNIQUE NONCLUSTERED  ([ShipmentID]) ON [PRIMARY]
GO
PRINT N'Adding foreign keys to [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] ADD
CONSTRAINT [FK_OtherShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD
CONSTRAINT [FK_PostalShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
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
PRINT N'Adding foreign keys to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD
CONSTRAINT [FK_WorldShipGoods_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
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
