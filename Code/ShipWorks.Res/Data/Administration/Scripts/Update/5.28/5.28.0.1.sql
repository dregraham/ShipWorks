PRINT N'Adding column IncludeMilliseconds to [dbo].[GenericModuleStore]'
GO

IF COL_LENGTH(N'[dbo].[GenericModuleStore]', N'IncludeMilliseconds') IS NULL
ALTER TABLE [dbo].[GenericModuleStore] ADD [IncludeMilliseconds] [bit] NOT NULL CONSTRAINT DF_GenericModuleStore_IncludeMilliseconds DEFAULT 0
GO 
PRINT N'Removing constraints'
GO
IF OBJECT_ID('[dbo].[DF_GenericModuleStore_IncludeMilliseconds]', 'D') IS NOT NULL
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT DF_GenericModuleStore_IncludeMilliseconds
GO