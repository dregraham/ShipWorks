PRINT N'Altering [dbo].[UserSettings]'
GO
IF COL_LENGTH(N'[dbo].[UserSettings]', N'SingleScanConfirmationMode') IS NULL
ALTER TABLE [dbo].[UserSettings] ADD [SingleScanConfirmationMode] [int] NOT NULL CONSTRAINT [DF_UserSettings_SingleScanConfirmationMode] DEFAULT ((0))
GO