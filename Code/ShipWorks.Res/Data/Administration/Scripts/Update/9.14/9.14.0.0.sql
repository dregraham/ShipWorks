PRINT N'ALTERING [dbo].[FedExAccount]'
GO
IF COL_LENGTH(N'[dbo].[FedExAccount]', N'ShipEngineCarrierID') IS NULL
	ALTER TABLE [dbo].[FedExAccount] ADD [ShipEngineCarrierID] [nvarchar](50) NULL
GO
PRINT N'ALTERING [dbo].[FedExShipment]'
IF COL_LENGTH(N'[dbo].[FedExShipment]', N'PayorCountryCode') IS NULL
	ALTER TABLE [dbo].[FedExShipment] ADD [PayorCountryCode] [nvarchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF COL_LENGTH(N'[dbo].[FedExShipment]', N'PayorPostalCode') IS NULL
	ALTER TABLE [dbo].[FedExShipment] ADD [PayorPostalCode] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO