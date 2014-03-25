CREATE TABLE [dbo].[StampsScanForm] (
    [StampsScanFormID]      BIGINT         IDENTITY (1072, 1000) NOT NULL,
    [StampsAccountID]       BIGINT         NOT NULL,
    [ScanFormTransactionID] VARCHAR (100)  NOT NULL,
    [ScanFormUrl]           VARCHAR (2048) NOT NULL,
    [CreatedDate]           DATETIME       NOT NULL,
    [ScanFormBatchID]       BIGINT         NOT NULL,
    [Description]           NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_StampsScanForm] PRIMARY KEY CLUSTERED ([StampsScanFormID] ASC),
    CONSTRAINT [FK_StampsScanForm_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
);

