CREATE TABLE [dbo].[FilterNodeColumnSettings] (
    [FilterNodeColumnSettingsID] BIGINT IDENTITY (1032, 1000) NOT NULL,
    [UserID]                     BIGINT NULL,
    [FilterNodeID]               BIGINT NOT NULL,
    [Inherit]                    BIT    NOT NULL,
    [GridColumnLayoutID]         BIGINT NOT NULL,
    CONSTRAINT [PK_FilterNodeColumnSettings] PRIMARY KEY CLUSTERED ([FilterNodeColumnSettingsID] ASC),
    CONSTRAINT [FK_FilterNodeColumnSettings_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_FilterNodeColumnSettings_GridColumnLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID]),
    CONSTRAINT [FK_FilterNodeColumnSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeColumnSettings]
    ON [dbo].[FilterNodeColumnSettings]([UserID] ASC, [FilterNodeID] ASC);


GO
CREATE TRIGGER [dbo].[FilterNodeColumnSettingsDeleted]
    ON [dbo].[FilterNodeColumnSettings]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterNodeColumnSettingsDeleted]

