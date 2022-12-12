PRINT N'Altering [dbo].[Store]'
GO
IF COL_LENGTH(N'[dbo].[Store]', N'ShouldMigrate') IS NULL
ALTER TABLE [dbo].[Store] ADD[ShouldMigrate] [bit] NOT NULL CONSTRAINT [DF_Store_ShouldMigrate] DEFAULT ((1))
GO
PRINT N'Dropping constraints from [dbo].[Store]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ShouldMigrate' AND object_id = OBJECT_ID(N'[dbo].[Store]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_Store_ShouldMigrate]', 'D'))
ALTER TABLE [dbo].[Store] DROP CONSTRAINT [DF_Store_ShouldMigrate]
GO
