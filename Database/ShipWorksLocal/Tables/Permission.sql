CREATE TABLE [dbo].[Permission] (
    [PermissionID]   BIGINT IDENTITY (1004, 1000) NOT NULL,
    [UserID]         BIGINT NOT NULL,
    [PermissionType] INT    NOT NULL,
    [ObjectID]       BIGINT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([PermissionID] ASC),
    CONSTRAINT [FK_Permission_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Permission]
    ON [dbo].[Permission]([UserID] ASC, [PermissionType] ASC, [ObjectID] ASC);

