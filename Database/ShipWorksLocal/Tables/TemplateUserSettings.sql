CREATE TABLE [dbo].[TemplateUserSettings] (
    [TemplateUserSettingsID] BIGINT        IDENTITY (1028, 1000) NOT NULL,
    [TemplateID]             BIGINT        NOT NULL,
    [UserID]                 BIGINT        NOT NULL,
    [PreviewSource]          INT           NOT NULL,
    [PreviewCount]           INT           NOT NULL,
    [PreviewFilterNodeID]    BIGINT        NULL,
    [PreviewZoom]            NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_TemplateUserSettings] PRIMARY KEY CLUSTERED ([TemplateUserSettingsID] ASC),
    CONSTRAINT [FK_TemplateUserSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TemplateUserSettings_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[TemplateUserSettingsTrigger]
    ON [dbo].[TemplateUserSettings]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[TemplateUserSettingsTrigger]

