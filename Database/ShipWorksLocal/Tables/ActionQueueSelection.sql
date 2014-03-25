CREATE TABLE [dbo].[ActionQueueSelection] (
    [ActionQueueSelectionID] BIGINT IDENTITY (1097, 1000) NOT NULL,
    [ActionQueueID]          BIGINT NOT NULL,
    [ObjectID]               BIGINT NOT NULL,
    CONSTRAINT [PK_ActionQueueSelection] PRIMARY KEY CLUSTERED ([ActionQueueSelectionID] ASC),
    CONSTRAINT [FK_ActionQueueSelection_ActionQueue] FOREIGN KEY ([ActionQueueID]) REFERENCES [dbo].[ActionQueue] ([ActionQueueID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ActionQueueSelection_ActionQueueID]
    ON [dbo].[ActionQueueSelection]([ActionQueueID] ASC);

