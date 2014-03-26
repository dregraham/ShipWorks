CREATE TABLE [dbo].[User] (
    [UserID]     BIGINT         IDENTITY (1002, 1000) NOT NULL,
    [RowVersion] ROWVERSION     NOT NULL,
    [Username]   NVARCHAR (30)  NOT NULL,
    [Password]   NVARCHAR (32)  NOT NULL,
    [Email]      NVARCHAR (255) NOT NULL,
    [IsAdmin]    BIT            NOT NULL,
    [IsDeleted]  BIT            NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC)
);


GO
ALTER TABLE [dbo].[User] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Username]
    ON [dbo].[User]([Username] ASC);

