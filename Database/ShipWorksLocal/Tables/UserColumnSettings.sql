CREATE TABLE [dbo].[UserColumnSettings] (
    [UserColumnSettingsID] BIGINT           IDENTITY (1039, 1000) NOT NULL,
    [SettingsKey]          UNIQUEIDENTIFIER NOT NULL,
    [UserID]               BIGINT           NOT NULL,
    [InitialSortType]      INT              NOT NULL,
    [GridColumnLayoutID]   BIGINT           NOT NULL,
    CONSTRAINT [PK_UserColumnSettings] PRIMARY KEY CLUSTERED ([UserColumnSettingsID] ASC),
    CONSTRAINT [FK_UserColumnSettings_GridColumnLayout] FOREIGN KEY ([GridColumnLayoutID]) REFERENCES [dbo].[GridColumnLayout] ([GridColumnLayoutID]),
    CONSTRAINT [FK_UserColumnSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserColumnSettings]
    ON [dbo].[UserColumnSettings]([UserID] ASC, [SettingsKey] ASC);

