CREATE TABLE [dbo].[PrintResult] (
    [PrintResultID]     BIGINT           IDENTITY (1045, 1000) NOT NULL,
    [RowVersion]        ROWVERSION       NOT NULL,
    [JobIdentifier]     UNIQUEIDENTIFIER NOT NULL,
    [RelatedObjectID]   BIGINT           NOT NULL,
    [ContextObjectID]   BIGINT           NOT NULL,
    [TemplateID]        BIGINT           NULL,
    [TemplateType]      INT              NULL,
    [OutputFormat]      INT              NULL,
    [LabelSheetID]      BIGINT           NULL,
    [ComputerID]        BIGINT           NOT NULL,
    [ContentResourceID] BIGINT           NOT NULL,
    [PrintDate]         DATETIME         NOT NULL,
    [PrinterName]       NVARCHAR (350)   NOT NULL,
    [PaperSource]       INT              NOT NULL,
    [PaperSourceName]   NVARCHAR (100)   NOT NULL,
    [Copies]            INT              NOT NULL,
    [Collated]          BIT              NOT NULL,
    [PageMarginLeft]    FLOAT (53)       NOT NULL,
    [PageMarginRight]   FLOAT (53)       NOT NULL,
    [PageMarginBottom]  FLOAT (53)       NOT NULL,
    [PageMarginTop]     FLOAT (53)       NOT NULL,
    [PageWidth]         FLOAT (53)       NOT NULL,
    [PageHeight]        FLOAT (53)       NOT NULL,
    CONSTRAINT [PK_PrintResult] PRIMARY KEY CLUSTERED ([PrintResultID] ASC),
    CONSTRAINT [FK_PrintResult_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
);


GO
ALTER TABLE [dbo].[PrintResult] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_PrintResult_RelatedObjectID]
    ON [dbo].[PrintResult]([RelatedObjectID] ASC);


GO
CREATE TRIGGER [dbo].[FilterDirtyPrintResult]
    ON [dbo].[PrintResult]
    AFTER INSERT, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyPrintResult]


GO
CREATE TRIGGER [dbo].[PrintResultDeleteTrigger]
    ON [dbo].[PrintResult]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[PrintResultDeleteTrigger]

