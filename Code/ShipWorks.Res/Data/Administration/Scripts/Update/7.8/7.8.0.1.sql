PRINT N'Altering [dbo].[UpsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UpsShipment]', N'ShipEngineLabelID') IS NULL
ALTER TABLE [dbo].[UpsShipment] ADD [ShipEngineLabelID] [nvarchar] (50) NULL
GO
PRINT N'Altering [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] 
ALTER COLUMN [ShipEngineCarrierId] [nvarchar] (50)
GO
PRINT N'Altering [dbo].[AmazonSWAAccount]'
GO
ALTER TABLE [dbo].[AmazonSWAAccount] 
ALTER COLUMN [ShipEngineCarrierId] [nvarchar] (50)
GO
PRINT N'Altering [dbo].[DhlExpressAccount]'
GO
ALTER TABLE [dbo].[DhlExpressAccount]
ALTER COLUMN [ShipEngineCarrierId] [nvarchar] (50)
GO
PRINT N'Altering [dbo].[AsendiaAccount]'
GO
ALTER TABLE [dbo].[AsendiaAccount]
ALTER COLUMN [ShipEngineCarrierId] [nvarchar] (50)
GO
PRINT N'Altering [dbo].[AmazonSWAShipment]'
GO
ALTER TABLE [dbo].[AmazonSWAShipment]
ALTER COLUMN [ShipEngineLabelID] [nvarchar] (50)
GO
PRINT N'Altering [dbo].[DhlExpressShipment]'
GO
ALTER TABLE [dbo].[DhlExpressShipment]
ALTER COLUMN [ShipEngineLabelID] [nvarchar] (50)
GO
PRINT N'Altering [dbo].[AsendiaShipment]'
GO
ALTER TABLE [dbo].[AsendiaShipment]
ALTER COLUMN [ShipEngineLabelID] [nvarchar] (50)
GO