PRINT N'ALTERING [dbo].[DhlEcommerceShipment]'
GO

DECLARE @ObjectName NVARCHAR(100)
SELECT @ObjectName = NAME FROM SYS.OBJECTS WHERE TYPE = 'D' AND NAME = 'DF_DhlEcommerceShipment_Insurance'

IF @ObjectName IS NOT NULL
	EXEC('ALTER TABLE [dbo].[DhlEcommerceShipment] DROP CONSTRAINT ' + @ObjectName)
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'Insurance') IS NOT NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] DROP COLUMN [Insurance]
GO