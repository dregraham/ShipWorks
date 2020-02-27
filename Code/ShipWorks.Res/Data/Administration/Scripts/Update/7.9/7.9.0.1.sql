PRINT N'Altering [dbo].[UpsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'ShipEngineLabelID') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [ShipEngineLabelID] [nvarchar] (12) NULL
GO
