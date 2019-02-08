
PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'AutoUpdateDayOfWeek') IS NULL
	ALTER TABLE dbo.[Configuration] ADD AutoUpdateDayOfWeek INT NOT NULL CONSTRAINT [DF_Configuration_AutoUpdateDayOfWeek] DEFAULT(4)
GO

IF EXISTS(SELECT * FROM sys.default_constraints WHERE name = 'DF_Configuration_AutoUpdateDayOfWeek')
	ALTER TABLE dbo.[Configuration] DROP CONSTRAINT [DF_Configuration_AutoUpdateDayOfWeek]
GO

IF COL_LENGTH(N'[dbo].[Configuration]', N'AutoUpdateHourOfDay') IS NULL
	ALTER TABLE dbo.[Configuration] ADD AutoUpdateHourOfDay INT NOT NULL CONSTRAINT [DF_Configuration_AutoUpdateHourOfDay] DEFAULT(23)
GO

IF EXISTS(SELECT * FROM sys.default_constraints WHERE name = 'DF_Configuration_AutoUpdateHourOfDay')
	ALTER TABLE dbo.[Configuration] DROP CONSTRAINT [DF_Configuration_AutoUpdateHourOfDay]
GO