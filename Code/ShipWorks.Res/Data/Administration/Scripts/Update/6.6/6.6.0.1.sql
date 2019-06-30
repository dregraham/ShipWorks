PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'WarehouseID') IS NULL
	ALTER TABLE [dbo].[Configuration] ADD [WarehouseID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Configuration_WarehouseID] DEFAULT ('')

IF COL_LENGTH(N'[dbo].[Configuration]', N'WarehouseName') IS NULL
	ALTER TABLE [dbo].[Configuration] ADD [WarehouseName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Configuration_WarehouseName] DEFAULT ('')
GO


