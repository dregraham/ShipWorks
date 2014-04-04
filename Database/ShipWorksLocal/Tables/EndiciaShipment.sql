CREATE TABLE [dbo].[EndiciaShipment] (
    [ShipmentID]               BIGINT         NOT NULL,
    [EndiciaAccountID]         BIGINT         NOT NULL,
    [OriginalEndiciaAccountID] BIGINT         NULL,
    [StealthPostage]           BIT            NOT NULL,
    [NoPostage]                BIT            NOT NULL,
    [ReferenceID]              NVARCHAR (300) NOT NULL,
    [RubberStamp1]             NVARCHAR (300) NOT NULL,
    [RubberStamp2]             NVARCHAR (300) NOT NULL,
    [RubberStamp3]             NVARCHAR (300) NOT NULL,
    [TransactionID]            INT            NULL,
    [RefundFormID]             INT            NULL,
    [ScanFormBatchID]          BIGINT         NULL,
    [ScanBasedReturn]          BIT            NOT NULL,
    CONSTRAINT [PK_EndiciaShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_EndiciaShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EndiciaShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
);


GO
CREATE TRIGGER [dbo].[EndiciaShipmentAuditTrigger]
    ON [dbo].[EndiciaShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[EndiciaShipmentAuditTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EndiciaShipment', @level2type = N'COLUMN', @level2name = N'EndiciaAccountID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EndiciaShipment', @level2type = N'COLUMN', @level2name = N'TransactionID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EndiciaShipment', @level2type = N'COLUMN', @level2name = N'RefundFormID';

