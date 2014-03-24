CREATE TABLE [dbo].[UserSettings] (
    [UserID]                     BIGINT          NOT NULL,
    [DisplayColorScheme]         INT             NOT NULL,
    [DisplaySystemTray]          BIT             NOT NULL,
    [WindowLayout]               VARBINARY (MAX) NOT NULL,
    [GridMenuLayout]             XML             NULL,
    [FilterInitialUseLastActive] BIT             NOT NULL,
    [FilterInitialSpecified]     BIGINT          NOT NULL,
    [FilterInitialSortType]      INT             NOT NULL,
    [FilterLastActive]           BIGINT          NOT NULL,
    [FilterExpandedFolders]      XML             NULL,
    [ShippingWeightFormat]       INT             NOT NULL,
    [TemplateExpandedFolders]    XML             NULL,
    [TemplateLastSelected]       BIGINT          NOT NULL,
    CONSTRAINT [PK_UserSetting_1] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_UserSetting_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);

