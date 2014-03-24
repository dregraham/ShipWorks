CREATE TABLE [dbo].[TemplateStoreSettings] (
    [TemplateStoreSettingsID] BIGINT         IDENTITY (1033, 1000) NOT NULL,
    [TemplateID]              BIGINT         NOT NULL,
    [StoreID]                 BIGINT         NULL,
    [EmailUseDefault]         BIT            NOT NULL,
    [EmailAccountID]          BIGINT         NOT NULL,
    [EmailTo]                 NVARCHAR (MAX) NOT NULL,
    [EmailCc]                 NVARCHAR (MAX) NOT NULL,
    [EmailBcc]                NVARCHAR (MAX) NOT NULL,
    [EmailSubject]            NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_TemplateStoreSettings] PRIMARY KEY CLUSTERED ([TemplateStoreSettingsID] ASC),
    CONSTRAINT [FK_TemplateStoreSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TemplateStoreSettings]
    ON [dbo].[TemplateStoreSettings]([TemplateID] ASC, [StoreID] ASC);

