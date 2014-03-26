CREATE TABLE [dbo].[GridColumnFormat] (
    [GridColumnFormatID] BIGINT           IDENTITY (1015, 1000) NOT NULL,
    [UserID]             BIGINT           NOT NULL,
    [ColumnGuid]         UNIQUEIDENTIFIER NOT NULL,
    [Settings]           XML              NOT NULL,
    CONSTRAINT [PK_GridColumnFormat] PRIMARY KEY CLUSTERED ([GridColumnFormatID] ASC),
    CONSTRAINT [FK_GridColumnFormat_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GridColumnDisplay]
    ON [dbo].[GridColumnFormat]([UserID] ASC, [ColumnGuid] ASC);

