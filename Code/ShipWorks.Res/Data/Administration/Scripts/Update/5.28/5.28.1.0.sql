PRINT N'Adding column IncludeMilliseconds to [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD [IncludeMilliseconds] [bit] NOT NULL CONSTRAINT DF_GenericModuleStore_IncludeMilliseconds DEFAULT 0
GO 
PRINT N'Removing constraints'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT DF_GenericModuleStore_IncludeMilliseconds
GO