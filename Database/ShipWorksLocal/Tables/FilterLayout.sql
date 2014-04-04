CREATE TABLE [dbo].[FilterLayout] (
    [FilterLayoutID] BIGINT     IDENTITY (1011, 1000) NOT NULL,
    [RowVersion]     ROWVERSION NOT NULL,
    [UserID]         BIGINT     NULL,
    [FilterTarget]   INT        NOT NULL,
    [FilterNodeID]   BIGINT     NOT NULL,
    CONSTRAINT [PK_FilterLayout] PRIMARY KEY CLUSTERED ([FilterLayoutID] ASC),
    CONSTRAINT [FK_FilterLayout_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID]),
    CONSTRAINT [FK_FilterLayout_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterLayout]
    ON [dbo].[FilterLayout]([UserID] ASC, [FilterTarget] ASC);

