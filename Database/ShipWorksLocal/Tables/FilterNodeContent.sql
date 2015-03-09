CREATE TABLE [dbo].[FilterNodeContent] (
    [FilterNodeContentID] BIGINT         IDENTITY (1014, 1000) NOT NULL,
    [RowVersion]          ROWVERSION     NOT NULL,
    [CountVersion]        BIGINT         NOT NULL,
    [Status]              SMALLINT       NOT NULL,
    [InitialCalculation]  NVARCHAR (MAX) NOT NULL,
    [UpdateCalculation]   NVARCHAR (MAX) NOT NULL,
    [ColumnMask]          VARBINARY (100) NOT NULL,
    [JoinMask]            INT            NOT NULL,
    [Cost]                INT            NOT NULL,
    [Count]               INT            NOT NULL,
    CONSTRAINT [PK_FilterNodeContent] PRIMARY KEY CLUSTERED ([FilterNodeContentID] ASC)
);

