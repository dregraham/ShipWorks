﻿PRINT N'ALTERING [dbo].[FedExShipment]'
GO

IF COL_LENGTH(N'[dbo].[FedExShipment]', N'ShipEngineCarrierID') IS NULL
	ALTER TABLE [dbo].[FedExShipment] ADD [ShipEngineLabelId] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO