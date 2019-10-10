PRINT N'Altering [dbo].[UserSettings]'
GO
If(select COL_LENGTH('UserSettings','MinimizeRibbon'))IS NULL
ALTER TABLE UserSettings ADD MinimizeRibbon BIT NOT NULL DEFAULT 0

If(select COL_LENGTH('UserSettings','ShowQAToolbarBelowRibbon')) IS NULL
ALTER TABLE UserSettings ADD ShowQAToolbarBelowRibbon BIT NOT NULL DEFAULT 0
GO