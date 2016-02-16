SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP
CONSTRAINT [FK_StampsShipment_PostalShipment]
GO
PRINT N'Dropping constraints from [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] DROP CONSTRAINT [PK_Configuration]
GO
PRINT N'Dropping constraints from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [PK_StampsShipment]
GO
PRINT N'Altering [dbo].[StampsProfile]'
GO
ALTER TABLE [dbo].[StampsProfile] ADD
[RequireFullAddressValidation] [bit] NULL
GO
PRINT N'Rebuilding [dbo].[StampsShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_StampsShipment]
(
[ShipmentID] [bigint] NOT NULL,
[StampsAccountID] [bigint] NOT NULL,
[HidePostage] [bit] NOT NULL,
[RequireFullAddressValidation] [bit] NOT NULL,
[IntegratorTransactionID] [uniqueidentifier] NOT NULL,
[StampsTransactionID] [uniqueidentifier] NOT NULL
)
GO

-- Added RequiresFullAddressValidation by hand
INSERT INTO [dbo].[tmp_rg_xx_StampsShipment]([ShipmentID], [StampsAccountID], [HidePostage], [RequireFullAddressValidation], [IntegratorTransactionID], [StampsTransactionID]) 
                                      SELECT [ShipmentID], [StampsAccountID], [HidePostage], 1,                              [IntegratorTransactionID], [StampsTransactionID] FROM [dbo].[StampsShipment]
GO

DROP TABLE [dbo].[StampsShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_StampsShipment]', N'StampsShipment'
GO
PRINT N'Creating primary key [PK_StampsShipment] on [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [PK_StampsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[Configuration]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Configuration]
(
[ConfigurationID] [bit] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[LogOnMethod] [int] NOT NULL,
[AddressCasing] [bit] NOT NULL,
[CustomerCompareEmail] [bit] NOT NULL,
[CustomerCompareAddress] [bit] NOT NULL,
[CustomerUpdateBilling] [bit] NOT NULL,
[CustomerUpdateShipping] [bit] NOT NULL,
[AuditNewOrders] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_Configuration]([ConfigurationID], [LogOnMethod], [AddressCasing], [CustomerCompareEmail], [CustomerCompareAddress], [CustomerUpdateBilling], [CustomerUpdateShipping], [AuditNewOrders]) SELECT [ConfigurationID], [LogOnMethod], [AddressCasing], [CustomerCompareEmail], [CustomerCompareAddress], [CustomerUpdateBilling], [CustomerUpdateShipping], 0 FROM [dbo].[Configuration]
GO
DROP TABLE [dbo].[Configuration]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Configuration]', N'Configuration'
GO
PRINT N'Creating primary key [PK_Configuration] on [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] ADD CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED  ([ConfigurationID])
GO
PRINT N'Adding foreign keys to [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD
CONSTRAINT [FK_StampsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
GO

-- Added by hand
UPDATE StampsProfile
  SET RequireFullAddressValidation = 1
  WHERE ShippingProfileID = 
     (SELECT ShippingProfileID 
        FROM ShippingProfile 
       WHERE ShipmentType = 3 AND ShipmentTypePrimary = 1)
GO

PRINT N'Dropping constraints from [dbo].[ObjectReference]'
GO
ALTER TABLE [dbo].[ObjectReference] DROP CONSTRAINT [DF_ObjectReference_ReferenceKey]
GO
PRINT N'Dropping index [IX_ObjectReference] from [dbo].[ObjectReference]'
GO
DROP INDEX [IX_ObjectReference] ON [dbo].[ObjectReference]
GO
PRINT N'Altering [dbo].[ObjectReference]'
GO
ALTER TABLE [dbo].[ObjectReference] ALTER COLUMN [ReferenceKey] [varchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[ObjectReference] ALTER COLUMN [Reason] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Creating index [IX_ObjectReference] on [dbo].[ObjectReference]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectReference] ON [dbo].[ObjectReference] ([ConsumerID], [ReferenceKey]) ON [PRIMARY]
GO
PRINT N'Adding constraints to [dbo].[ObjectReference]'
GO
ALTER TABLE [dbo].[ObjectReference] ADD CONSTRAINT [DF_ObjectReference_ReferenceKey] DEFAULT ('') FOR [ReferenceKey]
GO