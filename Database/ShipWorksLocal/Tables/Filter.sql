CREATE TABLE [dbo].[Filter] (
    [FilterID]     BIGINT        IDENTITY (1010, 1000) NOT NULL,
    [RowVersion]   ROWVERSION    NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [FilterTarget] INT           NOT NULL,
    [IsFolder]     BIT           NOT NULL,
    [Definition]   XML           NULL,
    CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED ([FilterID] ASC)
);

