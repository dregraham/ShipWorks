CREATE TABLE [dbo].[FilterNodeUpdatePending] (
    [FilterNodeContentID] BIGINT         NOT NULL,
    [FilterTarget]        INT            NOT NULL,
    [ColumnMask]          VARBINARY (75) NOT NULL,
    [JoinMask]            INT            NOT NULL,
    [Position]            INT            NOT NULL
);

