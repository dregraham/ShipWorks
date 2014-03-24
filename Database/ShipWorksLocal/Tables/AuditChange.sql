CREATE TABLE [dbo].[AuditChange] (
    [AuditChangeID] BIGINT IDENTITY (1003, 1000) NOT NULL,
    [AuditID]       BIGINT NOT NULL,
    [ChangeType]    INT    NOT NULL,
    [ObjectID]      BIGINT NOT NULL,
    CONSTRAINT [PK_AuditChange] PRIMARY KEY CLUSTERED ([AuditChangeID] ASC),
    CONSTRAINT [FK_AuditChange_Audit] FOREIGN KEY ([AuditID]) REFERENCES [dbo].[Audit] ([AuditID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditChange]
    ON [dbo].[AuditChange]([AuditID] ASC);

