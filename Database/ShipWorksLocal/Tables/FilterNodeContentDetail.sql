CREATE TABLE [dbo].[FilterNodeContentDetail] (
    [FilterNodeContentID] BIGINT NOT NULL,
    [ObjectID]            BIGINT NOT NULL,
    CONSTRAINT [FK_FilterNodeContentDetail_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeCountDetail]
    ON [dbo].[FilterNodeContentDetail]([FilterNodeContentID] ASC, [ObjectID] ASC) WITH (IGNORE_DUP_KEY = ON);

