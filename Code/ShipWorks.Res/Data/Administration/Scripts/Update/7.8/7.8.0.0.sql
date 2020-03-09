PRINT N'Altering [dbo].[UspsAccount]'
GO
IF COL_LENGTH(N'[dbo].[UspsAccount]', N'ShipEngineCarrierId') IS NULL
ALTER TABLE [dbo].[UspsAccount] ADD [ShipEngineCarrierId] [nvarchar] (12) NULL
GO

PRINT N'Altering [dbo].[UpsAccount]'
GO
IF COL_LENGTH(N'[dbo].[UpsAccount]', N'ShipEngineCarrierId') IS NULL
ALTER TABLE [dbo].[UpsAccount] ADD [ShipEngineCarrierId] [nvarchar] (12) NULL
GO