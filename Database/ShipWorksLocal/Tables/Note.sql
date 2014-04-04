CREATE TABLE [dbo].[Note] (
    [NoteID]     BIGINT         IDENTITY (1044, 1000) NOT NULL,
    [RowVersion] ROWVERSION     NOT NULL,
    [ObjectID]   BIGINT         NOT NULL,
    [UserID]     BIGINT         NULL,
    [Edited]     DATETIME       NOT NULL,
    [Text]       NVARCHAR (MAX) NOT NULL,
    [Source]     INT            NOT NULL,
    [Visibility] INT            NOT NULL,
    CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED ([NoteID] ASC),
    CONSTRAINT [FK_Note_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
ALTER TABLE [dbo].[Note] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_OrderNote_ObjectID]
    ON [dbo].[Note]([ObjectID] ASC);


GO
CREATE TRIGGER [dbo].[FilterDirtyNote]
    ON [dbo].[Note]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyNote]


GO
CREATE TRIGGER [dbo].[NoteAuditTrigger]
    ON [dbo].[Note]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[NoteAuditTrigger]


GO
CREATE TRIGGER [dbo].[NoteLabelTrigger]
    ON [dbo].[Note]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[NoteLabelTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Note', @level2type = N'COLUMN', @level2name = N'ObjectID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'RelatedTo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Note', @level2type = N'COLUMN', @level2name = N'ObjectID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Note', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Note', @level2type = N'COLUMN', @level2name = N'Text';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'101', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Note', @level2type = N'COLUMN', @level2name = N'Source';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'102', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Note', @level2type = N'COLUMN', @level2name = N'Visibility';

