SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[UserSettings]'
GO
IF EXISTS(SELECT *
	FROM sys.all_columns
		WHERE [object_id] = OBJECT_ID('UserSettings')
			AND [name] = 'NextGlobalPostNotificationDate')
BEGIN
	ALTER TABLE UserSettings
		DROP COLUMN NextGlobalPostNotificationDate
END
