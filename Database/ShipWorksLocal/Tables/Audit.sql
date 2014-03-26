CREATE TABLE [dbo].[Audit] (
    [AuditID]       BIGINT        IDENTITY (1048, 1000) NOT NULL,
    [RowVersion]    ROWVERSION    NOT NULL,
    [TransactionID] BIGINT        NOT NULL,
    [UserID]        BIGINT        NOT NULL,
    [ComputerID]    BIGINT        NOT NULL,
    [Reason]        INT           NOT NULL,
    [ReasonDetail]  VARCHAR (100) NULL,
    [Date]          DATETIME      NOT NULL,
    [Action]        INT           NOT NULL,
    [ObjectID]      BIGINT        NULL,
    [HasEvents]     BIT           NOT NULL,
    CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED ([AuditID] ASC),
    CONSTRAINT [FK_Audit_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]),
    CONSTRAINT [FK_Audit_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
ALTER TABLE [dbo].[Audit] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Audit_TransactionID]
    ON [dbo].[Audit]([TransactionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Audit_Action]
    ON [dbo].[Audit]([Action] ASC);

