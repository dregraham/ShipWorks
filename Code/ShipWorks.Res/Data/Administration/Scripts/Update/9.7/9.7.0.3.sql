PRINT N'ALTERING [dbo].[DhlEcommerceShipment]'
GO

DECLARE @ObjectName NVARCHAR(100)
SELECT @ObjectName = NAME FROM SYS.OBJECTS WHERE TYPE = 'D' AND NAME LIKE 'DF__DhlEcomme__Ancil__%'

IF @ObjectName IS NOT NULL
	EXEC('ALTER TABLE [dbo].[DhlEcommerceShipment] DROP CONSTRAINT ' + @ObjectName)
GO

IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'AncillaryEndorsement') IS NOT NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] DROP COLUMN [AncillaryEndorsement]
GO

PRINT N'ALTERING [dbo].[DhlEcommerceProfile]'
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceProfile]', N'AncillaryEndorsement') IS NOT NULL
	ALTER TABLE [dbo].[DhlEcommerceProfile] DROP COLUMN [AncillaryEndorsement]
GO