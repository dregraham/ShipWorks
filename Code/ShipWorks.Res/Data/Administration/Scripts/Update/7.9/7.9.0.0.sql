PRINT N'Altering [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] ADD
[SingleScanConfirmationMode] [int] NOT NULL CONSTRAINT [DF_UserSettings_SingleScanConfirmationMode] DEFAULT ((0))
GO