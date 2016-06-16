SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
GO
PRINT N'Dropping foreign keys from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [FK_StampsShipment_PostalShipment]
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [FK_StampsShipment_ScanFormBatch]
GO
PRINT N'Dropping constraints from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [PK_StampsShipment]
GO
PRINT N'Altering [dbo].[StampsProfile]'
GO
ALTER TABLE [dbo].[StampsProfile] ADD
[RateShop] [bit] NULL
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
[StampsTransactionID] [uniqueidentifier] NOT NULL,
[Memo] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalStampsAccountID] [bigint] NULL,
[ScanFormBatchID] [bigint] NULL,
[RequestedLabelFormat] [int] NOT NULL,
[RateShop] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_StampsShipment]([ShipmentID], [StampsAccountID], [HidePostage], [RequireFullAddressValidation], [IntegratorTransactionID], [StampsTransactionID], [Memo], [OriginalStampsAccountID], [ScanFormBatchID], [RequestedLabelFormat],
	RateShop) 
SELECT [ShipmentID], [StampsAccountID], [HidePostage], [RequireFullAddressValidation], [IntegratorTransactionID], [StampsTransactionID], [Memo], [OriginalStampsAccountID], [ScanFormBatchID], [RequestedLabelFormat],
	0
FROM [dbo].[StampsShipment]
GO
DROP TABLE [dbo].[StampsShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_StampsShipment]', N'StampsShipment'
GO
PRINT N'Creating primary key [PK_StampsShipment] on [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [PK_StampsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [FK_StampsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [FK_StampsShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
GO