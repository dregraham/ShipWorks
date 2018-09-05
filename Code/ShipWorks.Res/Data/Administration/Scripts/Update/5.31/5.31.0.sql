IF COL_LENGTH('UserSettings', 'UIMode') IS NULL
BEGIN
	ALTER TABLE [dbo].[UserSettings] ADD
	[UIMode] [int] NOT NULL CONSTRAINT [DF_UserSettings_UIMode] DEFAULT ((0))

	ALTER TABLE [dbo].[UserSettings] DROP CONSTRAINT [DF_UserSettings_UIMode]
END
