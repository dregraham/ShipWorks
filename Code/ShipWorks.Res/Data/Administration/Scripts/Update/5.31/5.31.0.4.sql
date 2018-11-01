PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'AllowUIModeToggle') IS NULL
ALTER TABLE [dbo].[Configuration] ADD[AllowUIModeToggle] [bit] NOT NULL CONSTRAINT [DF_Configuration_AllowUIModeToggle] DEFAULT ((0))
GO

PRINT N'Dropping constraints from [dbo].[Configuration]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'AllowUIModeToggle' AND object_id = OBJECT_ID(N'[dbo].[Configuration]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_Configuration_AllowUIModeToggle]', 'D'))
ALTER TABLE [dbo].[Configuration] DROP CONSTRAINT [DF_Configuration_AllowUIModeToggle]
GO