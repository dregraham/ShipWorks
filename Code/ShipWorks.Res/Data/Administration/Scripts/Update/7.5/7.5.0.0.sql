PRINT N'Altering [dbo].[UserSettings]'
GO
IF COL_LENGTH(N'[dbo].[UserSettings]', N'ScanToShipAutoAdvance') IS NULL
ALTER TABLE [dbo].[UserSettings] ADD[ScanToShipAutoAdvance] [bit] NOT NULL CONSTRAINT [DF_UserSettings_ScanToShipAutoAdvance] DEFAULT ((0))
GO

PRINT N'Dropping constraints from [dbo].[UserSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ScanToShipAutoAdvance' AND object_id = OBJECT_ID(N'[dbo].[UserSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_UserSettings_ScanToShipAutoAdvance]', 'D'))
ALTER TABLE [dbo].[UserSettings] DROP CONSTRAINT [DF_UserSettings_ScanToShipAutoAdvance]
GO