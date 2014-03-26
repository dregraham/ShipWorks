CREATE TABLE [dbo].[ServerMessageSignoff] (
    [ServerMessageSignoffID] BIGINT     IDENTITY (1037, 1000) NOT NULL,
    [RowVersion]             ROWVERSION NOT NULL,
    [ServerMessageID]        BIGINT     NOT NULL,
    [UserID]                 BIGINT     NOT NULL,
    [ComputerID]             BIGINT     NOT NULL,
    [Dismissed]              DATETIME   NOT NULL,
    CONSTRAINT [PK_ServerMessageSignoff] PRIMARY KEY CLUSTERED ([ServerMessageSignoffID] ASC),
    CONSTRAINT [FK_ServerMessageSignoff_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ServerMessageSignoff_DashboardMessage] FOREIGN KEY ([ServerMessageID]) REFERENCES [dbo].[ServerMessage] ([ServerMessageID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessageSignoff]
    ON [dbo].[ServerMessageSignoff]([UserID] ASC, [ComputerID] ASC, [ServerMessageID] ASC);

