CREATE TABLE [dbo].[TemplateComputerSettings] (
    [TemplateComputerSettingsID] BIGINT         IDENTITY (1029, 1000) NOT NULL,
    [TemplateID]                 BIGINT         NOT NULL,
    [ComputerID]                 BIGINT         NOT NULL,
    [PrinterName]                NVARCHAR (350) NOT NULL,
    [PaperSource]                INT            NOT NULL,
    CONSTRAINT [PK_TemplateComputerSettings] PRIMARY KEY CLUSTERED ([TemplateComputerSettingsID] ASC),
    CONSTRAINT [FK_TemplateComputerSettings_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TemplateComputerSettings_Template] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID]) ON DELETE CASCADE
);

