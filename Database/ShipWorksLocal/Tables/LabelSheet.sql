CREATE TABLE [dbo].[LabelSheet] (
    [LabelSheetID]      BIGINT         IDENTITY (1027, 1000) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    [Name]              NVARCHAR (100) NOT NULL,
    [PaperSizeHeight]   FLOAT (53)     NOT NULL,
    [PaperSizeWidth]    FLOAT (53)     NOT NULL,
    [MarginTop]         FLOAT (53)     NOT NULL,
    [MarginLeft]        FLOAT (53)     NOT NULL,
    [LabelHeight]       FLOAT (53)     NOT NULL,
    [LabelWidth]        FLOAT (53)     NOT NULL,
    [VerticalSpacing]   FLOAT (53)     NOT NULL,
    [HorizontalSpacing] FLOAT (53)     NOT NULL,
    [Rows]              INT            NOT NULL,
    [Columns]           INT            NOT NULL,
    CONSTRAINT [PK_LabelSheet] PRIMARY KEY CLUSTERED ([LabelSheetID] ASC)
);


GO
ALTER TABLE [dbo].[LabelSheet] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LabelSheet_Name]
    ON [dbo].[LabelSheet]([Name] ASC);

