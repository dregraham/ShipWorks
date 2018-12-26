PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'DefaultPickListTemplateID') IS NULL
ALTER TABLE [dbo].[Configuration] ADD [DefaultPickListTemplateID] [bigint] NULL
GO
PRINT N'Adding foreign keys to [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] ADD CONSTRAINT [FK_Configuration_DefaultPickListTemplate] FOREIGN KEY ([DefaultPickListTemplateID]) REFERENCES [dbo].[Template] ([TemplateID])
GO

