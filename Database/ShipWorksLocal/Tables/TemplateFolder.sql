CREATE TABLE [dbo].[TemplateFolder] (
    [TemplateFolderID] BIGINT         IDENTITY (1024, 1000) NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    [ParentFolderID]   BIGINT         NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TemplateFolder] PRIMARY KEY CLUSTERED ([TemplateFolderID] ASC),
    CONSTRAINT [FK_TemplateFolder_TemplateFolder] FOREIGN KEY ([ParentFolderID]) REFERENCES [dbo].[TemplateFolder] ([TemplateFolderID])
);


GO
ALTER TABLE [dbo].[TemplateFolder] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[TemplateFolderLabelTrigger]
    ON [dbo].[TemplateFolder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[TemplateFolderLabelTrigger]

