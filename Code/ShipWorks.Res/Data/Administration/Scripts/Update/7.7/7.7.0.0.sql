PRINT N'Dropping constraints from [dbo].[UserSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ScanToShipAutoAdvance' AND object_id = OBJECT_ID(N'[dbo].[UserSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_UserSettings_ScanToShipAutoAdvance]', 'D'))
ALTER TABLE [dbo].[UserSettings] DROP CONSTRAINT [DF_UserSettings_ScanToShipAutoAdvance]
GO

PRINT N'Altering [dbo].[UserSettings]'
GO
IF COL_LENGTH(N'[dbo].[UserSettings]', N'ScanToShipAutoAdvance') IS NOT NULL
ALTER TABLE [dbo].[UserSettings] DROP COLUMN [ScanToShipAutoAdvance]
GO

PRINT N'Renaming column in [dbo].[UserSettings]'
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'AutoPrintRequireValidation' AND object_id = OBJECT_ID(N'[dbo].[UserSettings]', 'U'))
    AND NOT EXISTS ( SELECT 1 FROM sys.columns WHERE name = N'RequireVerificationToShip' AND object_id = OBJECT_ID(N'[dbo].[UserSettings]', 'U'))
        EXEC sp_RENAME N'[dbo].[UserSettings].[AutoPrintRequireValidation]', N'RequireVerificationToShip', 'COLUMN';