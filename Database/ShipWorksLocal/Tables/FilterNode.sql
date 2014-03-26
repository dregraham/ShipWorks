CREATE TABLE [dbo].[FilterNode] (
    [FilterNodeID]        BIGINT     IDENTITY (1007, 1000) NOT NULL,
    [RowVersion]          ROWVERSION NOT NULL,
    [ParentFilterNodeID]  BIGINT     NULL,
    [FilterSequenceID]    BIGINT     NOT NULL,
    [FilterNodeContentID] BIGINT     NOT NULL,
    [Created]             DATETIME   NOT NULL,
    [Purpose]             INT        NOT NULL,
    CONSTRAINT [PK_FilterNode] PRIMARY KEY CLUSTERED ([FilterNodeID] ASC),
    CONSTRAINT [FK_FilterNode_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID]),
    CONSTRAINT [FK_FilterNode_FilterSequence] FOREIGN KEY ([FilterSequenceID]) REFERENCES [dbo].[FilterSequence] ([FilterSequenceID]),
    CONSTRAINT [FK_FilterNode_Parent] FOREIGN KEY ([ParentFilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID])
);


GO
CREATE NONCLUSTERED INDEX [IX_FilterNode_ParentFilterNodeID]
    ON [dbo].[FilterNode]([ParentFilterNodeID] ASC);


GO
CREATE TRIGGER [dbo].[FilterNodeLayoutDirty]
    ON [dbo].[FilterNode]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterNodeLayoutDirty]

