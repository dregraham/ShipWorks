PRINT N'Altering [dbo].[UserSettings]'
GO
ALTER TABLE UserSettings ADD ShowRibbon BIT NOT NULL DEFAULT 1
GO
ALTER TABLE UserSettings ADD ShowQuickAccessToolbar BIT NOT NULL DEFAULT 1
GO