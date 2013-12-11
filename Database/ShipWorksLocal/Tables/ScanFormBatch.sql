CREATE TABLE [dbo].[ScanFormBatch] (
    [ScanFormBatchID] BIGINT   IDENTITY (1095, 1000) NOT NULL,
    [ShipmentType]    INT      NOT NULL,
    [CreatedDate]     DATETIME NOT NULL,
    [ShipmentCount]   INT      NOT NULL,
    CONSTRAINT [PK_ScanFormBatch] PRIMARY KEY CLUSTERED ([ScanFormBatchID] ASC)
);

