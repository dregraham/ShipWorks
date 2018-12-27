PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'DefaultPickListTemplateID') IS NULL
ALTER TABLE [dbo].[Configuration] ADD [DefaultPickListTemplateID] [bigint] NULL
GO

