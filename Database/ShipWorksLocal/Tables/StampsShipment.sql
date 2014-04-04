CREATE TABLE [dbo].[StampsShipment] (
    [ShipmentID]                   BIGINT           NOT NULL,
    [StampsAccountID]              BIGINT           NOT NULL,
    [HidePostage]                  BIT              NOT NULL,
    [RequireFullAddressValidation] BIT              NOT NULL,
    [IntegratorTransactionID]      UNIQUEIDENTIFIER NOT NULL,
    [StampsTransactionID]          UNIQUEIDENTIFIER NOT NULL,
    [Memo]                         NVARCHAR (200)   NOT NULL,
    [OriginalStampsAccountID]      BIGINT           NULL,
    [ScanFormBatchID]              BIGINT           NULL,
    CONSTRAINT [PK_StampsShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_StampsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE,
    CONSTRAINT [FK_StampsShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
);


GO
CREATE TRIGGER [dbo].[StampsShipmentAuditTrigger]
    ON [dbo].[StampsShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[StampsShipmentAuditTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StampsShipment', @level2type = N'COLUMN', @level2name = N'StampsAccountID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StampsShipment', @level2type = N'COLUMN', @level2name = N'IntegratorTransactionID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StampsShipment', @level2type = N'COLUMN', @level2name = N'StampsTransactionID';

