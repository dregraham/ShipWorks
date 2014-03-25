CREATE TABLE [dbo].[Template] (
    [TemplateID]              BIGINT         IDENTITY (1025, 1000) NOT NULL,
    [RowVersion]              ROWVERSION     NOT NULL,
    [ParentFolderID]          BIGINT         NOT NULL,
    [Name]                    NVARCHAR (100) NOT NULL,
    [Xsl]                     NVARCHAR (MAX) NOT NULL,
    [Type]                    INT            NOT NULL,
    [Context]                 INT            NOT NULL,
    [OutputFormat]            INT            NOT NULL,
    [OutputEncoding]          NVARCHAR (20)  NOT NULL,
    [PageMarginLeft]          FLOAT (53)     NOT NULL,
    [PageMarginRight]         FLOAT (53)     NOT NULL,
    [PageMarginBottom]        FLOAT (53)     NOT NULL,
    [PageMarginTop]           FLOAT (53)     NOT NULL,
    [PageWidth]               FLOAT (53)     NOT NULL,
    [PageHeight]              FLOAT (53)     NOT NULL,
    [LabelSheetID]            BIGINT         NOT NULL,
    [PrintCopies]             INT            NOT NULL,
    [PrintCollate]            BIT            NOT NULL,
    [SaveFileName]            NVARCHAR (500) NOT NULL,
    [SaveFileFolder]          NVARCHAR (500) NOT NULL,
    [SaveFilePrompt]          INT            NOT NULL,
    [SaveFileBOM]             BIT            NOT NULL,
    [SaveFileOnlineResources] BIT            NOT NULL,
    CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([TemplateID] ASC),
    CONSTRAINT [FK_Template_TemplateFolder] FOREIGN KEY ([ParentFolderID]) REFERENCES [dbo].[TemplateFolder] ([TemplateFolderID])
);


GO
ALTER TABLE [dbo].[Template] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[TemplateDeleteTrigger]
    ON [dbo].[Template]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[TemplateDeleteTrigger]


GO
CREATE TRIGGER [dbo].[TemplateLabelTrigger]
    ON [dbo].[Template]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[TemplateLabelTrigger]

