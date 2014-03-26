CREATE TABLE [dbo].[GridColumnPosition] (
    [GridColumnPositionID] BIGINT           IDENTITY (1017, 1000) NOT NULL,
    [GridColumnLayoutID]   BIGINT           NOT NULL,
    [ColumnGuid]           UNIQUEIDENTIFIER NOT NULL,
    [Visible]              BIT              NOT NULL,
    [Width]                INT              NOT NULL,
    [Position]             INT              NOT NULL,
    CONSTRAINT [PK_GridColumnLayout] PRIMARY KEY CLUSTERED ([GridColumnPositionID] ASC),
    CONSTRAINT [FK_GridLayoutColumn_GridLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GridLayoutColumn]
    ON [dbo].[GridColumnPosition]([GridColumnLayoutID] ASC, [ColumnGuid] ASC);

