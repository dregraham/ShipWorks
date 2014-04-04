CREATE TABLE [dbo].[EndiciaScanForm] (
    [EndiciaScanFormID]    BIGINT         IDENTITY (1067, 1000) NOT NULL,
    [EndiciaAccountID]     BIGINT         NOT NULL,
    [EndiciaAccountNumber] NVARCHAR (50)  NOT NULL,
    [SubmissionID]         VARCHAR (100)  NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [ScanFormBatchID]      BIGINT         NOT NULL,
    [Description]          NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EndiciaScanForm] PRIMARY KEY CLUSTERED ([EndiciaScanFormID] ASC),
    CONSTRAINT [FK_EndiciaScanForm_EndiciaScanForm] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
);

