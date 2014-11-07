SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[Shipment] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ContentWeight'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsGenerated'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsValue'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginNameParseStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginOriginID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialDetermination'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ResidentialResult'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipDate'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentCost'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipmentType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'TotalWeight'
GO
PRINT N'Dropping foreign keys from [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] DROP CONSTRAINT [FK_BestRateShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] DROP CONSTRAINT [FK_EquashipShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [FK_FedExShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[InsurancePolicy]'
GO
ALTER TABLE [dbo].[InsurancePolicy] DROP CONSTRAINT [FK_InsurancePolicy_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] DROP CONSTRAINT [FK_iParcelShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] DROP CONSTRAINT [FK_OnTracShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] DROP CONSTRAINT [FK_OtherShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] DROP CONSTRAINT [FK_PostalShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP CONSTRAINT [FK_ShipmentCustomsItem_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT [FK_UpsShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_Order]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_ProcessedUser]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_ProcessedComputer]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_VoidedUser]
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [FK_Shipment_VoidedComputer]
GO
PRINT N'Dropping constraints from [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] DROP CONSTRAINT [PK_Shipment]
GO
PRINT N'Dropping index [IX_Shipment_OrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Shipment_OrderID_ShipSenseStatus] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_OrderID_ShipSenseStatus] ON [dbo].[Shipment]
GO
PRINT N'Dropping index [IX_Shipment_ProcessedOrderID] from [dbo].[Shipment]'
GO
DROP INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment]
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
[ProcessedUserID] [bigint] NULL,
[ProcessedComputerID] [bigint] NULL,
[ShipDate] [datetime] NOT NULL,
[ShipmentCost] [money] NOT NULL,
[Voided] [bit] NOT NULL,
[VoidedDate] [datetime] NULL,
[VoidedUserID] [bigint] NULL,
[VoidedComputerID] [bigint] NULL,
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
[ShipEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[OriginEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginWebsite] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnShipment] [bit] NOT NULL,
[Insurance] [bit] NOT NULL,
[InsuranceProvider] [int] NOT NULL,
[ShipNameParseStatus] [int] NOT NULL,
[ShipUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginNameParseStatus] [int] NOT NULL,
[OriginUnparsedName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BestRateEvents] [tinyint] NOT NULL,
[ShipSenseStatus] [int] NOT NULL,
[ShipSenseChangeSets] [xml] NOT NULL,
[ShipSenseEntry] [varbinary] (max) NOT NULL,
[OnlineShipmentID] [varchar] (128) NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Shipment]([ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ProcessedUserID], [ProcessedComputerID], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [VoidedUserID], [VoidedComputerID], [TrackingNumber], [CustomsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], [Insurance], [InsuranceProvider], [ShipNameParseStatus], [ShipUnparsedName], [OriginNameParseStatus], [OriginUnparsedName], [BestRateEvents], [ShipSenseStatus], [ShipSenseChangeSets], [ShipSenseEntry],
	[OnlineShipmentID]) 
	SELECT [ShipmentID], [OrderID], [ShipmentType], [ContentWeight], [TotalWeight], [Processed], [ProcessedDate], [ProcessedUserID], [ProcessedComputerID], [ShipDate], [ShipmentCost], [Voided], [VoidedDate], [VoidedUserID], [VoidedComputerID], [TrackingNumber], [CustomsGenerated], [CustomsValue], [ThermalType], [ShipFirstName], [ShipMiddleName], [ShipLastName], [ShipCompany], [ShipStreet1], [ShipStreet2], [ShipStreet3], [ShipCity], [ShipStateProvCode], [ShipPostalCode], [ShipCountryCode], [ShipPhone], [ShipEmail], [ResidentialDetermination], [ResidentialResult], [OriginOriginID], [OriginFirstName], [OriginMiddleName], [OriginLastName], [OriginCompany], [OriginStreet1], [OriginStreet2], [OriginStreet3], [OriginCity], [OriginStateProvCode], [OriginPostalCode], [OriginCountryCode], [OriginPhone], [OriginFax], [OriginEmail], [OriginWebsite], [ReturnShipment], [Insurance], [InsuranceProvider], [ShipNameParseStatus], [ShipUnparsedName], [OriginNameParseStatus], [OriginUnparsedName], [BestRateEvents], [ShipSenseStatus], [ShipSenseChangeSets], [ShipSenseEntry], '' 
	FROM [dbo].[Shipment]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Shipment] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Shipment]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Shipment]', RESEED, @idVal)
GO
DROP TABLE [dbo].[Shipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Shipment]', N'Shipment'
GO
PRINT N'Creating primary key [PK_Shipment] on [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating index [IX_Shipment_OrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment] ([OrderID])
GO
PRINT N'Creating index [IX_Shipment_OrderID_ShipSenseStatus] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID_ShipSenseStatus] ON [dbo].[Shipment] ([OrderID], [Processed], [ShipSenseStatus])
GO
PRINT N'Creating index [IX_Shipment_ProcessedOrderID] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC) INCLUDE ([OrderID])
GO
ALTER TABLE [dbo].[Shipment] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[Shipment]'
GO
PRINT N'Adding foreign keys to [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [FK_BestRateShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] ADD CONSTRAINT [FK_EquashipShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[InsurancePolicy]'
GO
ALTER TABLE [dbo].[InsurancePolicy] ADD CONSTRAINT [FK_InsurancePolicy_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] ADD CONSTRAINT [FK_iParcelShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD CONSTRAINT [FK_OnTracShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OtherShipment]'
GO
ALTER TABLE [dbo].[OtherShipment] ADD CONSTRAINT [FK_OtherShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PostalShipment]'
GO
ALTER TABLE [dbo].[PostalShipment] ADD CONSTRAINT [FK_PostalShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ShipmentCustomsItem]'
GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD CONSTRAINT [FK_ShipmentCustomsItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [FK_UpsShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_ProcessedUser] FOREIGN KEY ([ProcessedUserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_ProcessedComputer] FOREIGN KEY ([ProcessedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_VoidedUser] FOREIGN KEY ([VoidedUserID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_Shipment_VoidedComputer] FOREIGN KEY ([VoidedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ContentWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsGenerated'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'CustomsValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'112', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_addextendedproperty N'AuditName', N'Insurance', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'InsuranceProvider'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'OriginCountry', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'OriginNameParseStatus'
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
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipNameParseStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'5', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'ShipState', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'ShipStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'Shipment', 'COLUMN', N'TotalWeight'
GO