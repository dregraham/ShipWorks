PRINT N'ALTERING [dbo].[AmazonSFPShipment]'
GO
IF COL_LENGTH(N'[dbo].[AmazonSFPShipment]', N'ShipEngineLabelID') IS NULL
	ALTER TABLE [dbo].[AmazonSFPShipment] ADD [ShipEngineLabelID] [nvarchar](50) NULL
GO