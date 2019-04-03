

PRINT N'Altering [dbo].[UserSettings]'
GO
IF COL_LENGTH(N'[dbo].[UserSettings]', N'LastReleaseNotesSeen') IS NULL
	ALTER TABLE dbo.[UserSettings] ADD LastReleaseNotesSeen VARCHAR(25) NOT NULL CONSTRAINT [DF_UserSettings_LastReleaseNotesSeen] DEFAULT('0.0.0.0')
GO
