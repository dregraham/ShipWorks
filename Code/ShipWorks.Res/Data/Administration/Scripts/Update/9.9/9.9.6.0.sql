PRINT N'ALTERING [dbo].[FedExAccount]'
GO
IF COL_LENGTH(N'[dbo].[FedExAccount]', N'ShipEngineCarrierID') IS NULL
	ALTER TABLE [dbo].[FedExAccount] ADD [ShipEngineCarrierID] [nvarchar](50) NULL
GO