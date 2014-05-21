CREATE TABLE [dbo].[FilterNodeContentDirty] (
    [FilterNodeContentDirtyID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [RowVersion]               ROWVERSION     NOT NULL,
    [ObjectID]                 BIGINT         NOT NULL,
    [ParentID]                 BIGINT         NULL,
    [ObjectType]               INT            NOT NULL,
    [ComputerID]               BIGINT         NOT NULL,
    [ColumnsUpdated]           VARBINARY (100) NOT NULL,
    CONSTRAINT [PK_FilterNodeContentDirty] PRIMARY KEY CLUSTERED ([FilterNodeContentDirtyID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeCountDirty]
    ON [dbo].[FilterNodeContentDirty]([RowVersion] ASC);

