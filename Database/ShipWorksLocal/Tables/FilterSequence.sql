CREATE TABLE [dbo].[FilterSequence] (
    [FilterSequenceID] BIGINT     IDENTITY (1009, 1000) NOT NULL,
    [RowVersion]       ROWVERSION NOT NULL,
    [ParentFilterID]   BIGINT     NULL,
    [FilterID]         BIGINT     NOT NULL,
    [Position]         INT        NOT NULL,
    CONSTRAINT [PK_FilterFolderContent] PRIMARY KEY CLUSTERED ([FilterSequenceID] ASC),
    CONSTRAINT [FK_FilterSequence_Filter] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filter] ([FilterID]),
    CONSTRAINT [FK_FilterSequence_Folder] FOREIGN KEY ([ParentFilterID]) REFERENCES [dbo].[Filter] ([FilterID])
);


GO
CREATE NONCLUSTERED INDEX [IX_FilterChild_ParentFilterID]
    ON [dbo].[FilterSequence]([ParentFilterID] ASC);


GO
CREATE TRIGGER [dbo].[FilterSequenceLayoutDirty]
    ON [dbo].[FilterSequence]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterSequenceLayoutDirty]

