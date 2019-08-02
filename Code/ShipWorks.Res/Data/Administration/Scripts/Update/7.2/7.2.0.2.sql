PRINT N'Altering [dbo].[UserSettings]'
GO
IF COL_LENGTH(N'[dbo].[UserSettings]', N'AutoPrintRequireValidation') IS NULL
ALTER TABLE [dbo].[UserSettings] ADD[AutoPrintRequireValidation] [bit] NOT NULL CONSTRAINT [DF_UserSettings_AutoPrintRequireValidation] DEFAULT ((0))
GO

PRINT N'Dropping constraints from [dbo].[UserSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'AutoPrintRequireValidation' AND object_id = OBJECT_ID(N'[dbo].[UserSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_UserSettings_AutoPrintRequireValidation]', 'D'))
ALTER TABLE [dbo].[UserSettings] DROP CONSTRAINT [DF_UserSettings_AutoPrintRequireValidation]
GO